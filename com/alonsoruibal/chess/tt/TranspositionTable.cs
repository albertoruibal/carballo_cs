using System;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Evaluation;
using Com.Alonsoruibal.Chess.Log;
using Com.Alonsoruibal.Chess.Search;
using Sharpen;

namespace Com.Alonsoruibal.Chess.TT
{
	/// <summary>
	/// Transposition table using two keys and multiprobe
	/// <p/>
	/// Uses part of the board's zobrish key (shifted) as the index
	/// </summary>
	/// <author>rui</author>
	public class TranspositionTable
	{
		private static readonly Logger logger = Logger.GetLogger("MultiprobeTranspositionTable"
			);

		public const int DepthQsChecks = 1;

		public const int DepthQsNoChecks = 0;

		public const int TypeEval = 0;

		public const int TypeExactScore = 1;

		public const int TypeFailLow = 2;

		public const int TypeFailHigh = 3;

		private const int MaxProbes = 4;

		public long[] keys;

		public long[] infos;

		public short[] evals;

		private int size;

		private long info;

		private short eval;

		private byte generation;

		private int entriesOccupied;

		private int score;

		private int sizeBits;

		public TranspositionTable(int sizeMb)
		{
			sizeBits = BitboardUtils.Square2Index(sizeMb) + 16;
			size = 1 << sizeBits;
			keys = new long[size];
			infos = new long[size];
			evals = new short[size];
			entriesOccupied = 0;
			generation = 0;
			logger.Debug("Created transposition table, size = " + size + " slots " + size * 18.0
				 / (1024 * 1024) + " MBytes");
		}

		public virtual void Clear()
		{
			entriesOccupied = 0;
			Arrays.Fill(keys, 0);
		}

		public virtual bool Search(Board board, int distanceToInitialPly, bool exclusion)
		{
			info = 0;
			score = 0;
			int startIndex = (int)((long)(((ulong)(exclusion ? board.GetExclusionKey() : board
				.GetKey())) >> (64 - sizeBits)));
			// Verifies that it is really this board
			for (int i = startIndex; i < startIndex + MaxProbes && i < size; i++)
			{
				if (keys[i] == board.GetKey2())
				{
					info = infos[i];
					eval = evals[i];
					score = (short)(((long)(((ulong)info) >> 48)) & unchecked((int)(0xffff)));
					// Fix mate score with the real distance to the initial PLY
					if (score >= SearchEngine.ValueIsMate)
					{
						score -= distanceToInitialPly;
					}
					else
					{
						if (score <= -SearchEngine.ValueIsMate)
						{
							score += distanceToInitialPly;
						}
					}
					return true;
				}
			}
			return false;
		}

		public virtual int GetBestMove()
		{
			return (int)(info & unchecked((int)(0x1fffff)));
		}

		public virtual int GetNodeType()
		{
			return (int)(((long)(((ulong)info) >> 21)) & unchecked((int)(0xf)));
		}

		public virtual byte GetGeneration()
		{
			return unchecked((byte)(((long)(((ulong)info) >> 32)) & unchecked((int)(0xff))));
		}

		public virtual byte GetDepthAnalyzed()
		{
			return unchecked((byte)(((long)(((ulong)info) >> 40)) & unchecked((int)(0xff))));
		}

		public virtual int GetScore()
		{
			return score;
		}

		public virtual int GetEval()
		{
			return eval;
		}

		public virtual void NewGeneration()
		{
			generation++;
		}

		public virtual bool IsMyGeneration()
		{
			return GetGeneration() == generation;
		}

		public virtual void Save(Board board, int distanceToInitialPly, int depthAnalyzed
			, int bestMove, int score, int lowerBound, int upperBound, int eval, bool exclusion
			)
		{
			// Fix mate score with the real distance to mate from the current PLY, not from the initial PLY
			int fixedScore = score;
			if (score >= SearchEngine.ValueIsMate)
			{
				fixedScore += distanceToInitialPly;
			}
			else
			{
				if (score <= -SearchEngine.ValueIsMate)
				{
					fixedScore -= distanceToInitialPly;
				}
			}
			System.Diagnostics.Debug.Assert(fixedScore >= -Evaluator.Victory && fixedScore <=
				 Evaluator.Victory, "Fixed TT score is outside limits");
			System.Diagnostics.Debug.Assert(Math.Abs(eval) < SearchEngine.ValueIsMate || Math
				.Abs(eval) == Evaluator.Victory || eval == Evaluator.NoValue, "Storing a eval value in the TT outside limits"
				);
			if (score <= lowerBound)
			{
				Set(board, TypeFailLow, bestMove, fixedScore, depthAnalyzed, eval, exclusion);
			}
			else
			{
				if (score >= upperBound)
				{
					Set(board, TypeFailHigh, bestMove, fixedScore, depthAnalyzed, eval, exclusion);
				}
				else
				{
					Set(board, TypeExactScore, bestMove, fixedScore, depthAnalyzed, eval, exclusion);
				}
			}
		}

		public virtual void Set(Board board, int nodeType, int bestMove, int score, int depthAnalyzed
			, int eval, bool exclusion)
		{
			long key2 = board.GetKey2();
			int startIndex = (int)((long)(((ulong)(exclusion ? board.GetExclusionKey() : board
				.GetKey())) >> (64 - sizeBits)));
			int replaceIndex = startIndex;
			int replaceImportance = int.MaxValue;
			// A higher value, so the first entry will be the default
			for (int i = startIndex; i < startIndex + MaxProbes && i < size; i++)
			{
				info = infos[i];
				if (keys[i] == 0)
				{
					// Replace an empty TT position
					entriesOccupied++;
					replaceIndex = i;
					break;
				}
				else
				{
					if (keys[i] == key2)
					{
						// Replace the same position
						replaceIndex = i;
						if (bestMove == Move.None)
						{
							// Keep previous best move
							bestMove = GetBestMove();
						}
						break;
					}
				}
				// Calculates a value with this TT entry importance
				int entryImportance = (GetNodeType() == TypeExactScore ? 10 : 0) + 255 - GetGenerationDelta
					() + GetDepthAnalyzed();
				// Bonus for the PV entries
				// The older the generation, the less importance
				// The more depth, the more importance
				// We will replace the less important entry
				if (entryImportance < replaceImportance)
				{
					replaceImportance = entryImportance;
					replaceIndex = i;
				}
			}
			keys[replaceIndex] = key2;
			info = (bestMove & unchecked((int)(0x1fffff))) | ((nodeType & unchecked((int)(0xf
				))) << 21) | (((long)(generation & unchecked((int)(0xff)))) << 32) | (((long)(depthAnalyzed
				 & unchecked((int)(0xff)))) << 40) | (((long)(score & unchecked((int)(0xffff))))
				 << 48);
			infos[replaceIndex] = info;
			evals[replaceIndex] = (short)eval;
		}

		/// <summary>Returns the difference between the current generation and the entry generation (max 255)
		/// 	</summary>
		private int GetGenerationDelta()
		{
			byte entryGeneration = unchecked((byte)(((long)(((ulong)info) >> 32)) & unchecked(
				(int)(0xff))));
			return (generation >= entryGeneration ? generation - entryGeneration : 256 + generation
				 - entryGeneration);
		}

		public virtual int GetHashFull()
		{
			return (int)(1000L * entriesOccupied / size);
		}
	}
}
