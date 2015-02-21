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
	public class TranspositionTableNoEval
	{
		private static readonly Logger logger = Logger.GetLogger("MultiprobeTranspositionTable"
			);

		public const int TypeExactScore = 1;

		public const int TypeFailLow = 2;

		public const int TypeFailHigh = 3;

		public const int TypeEval = 4;

		private const int MaxProbes = 4;

		public long[] keys;

		public long[] infos;

		private int index;

		private int size;

		private long info;

		private byte generation;

		private int score;

		private int sizeBits;

		public TranspositionTableNoEval(int sizeMb)
		{
			sizeBits = BitboardUtils.Square2Index(sizeMb) + 16;
			size = 1 << sizeBits;
			keys = new long[size];
			infos = new long[size];
			generation = 0;
			index = -1;
			logger.Debug("Created Multiprobe transposition table, size = " + size + " slots "
				 + size * 18.0 / (1024 * 1024) + " MBytes");
		}

		public virtual void Clear()
		{
			Arrays.Fill(keys, 0);
		}

		public virtual bool Search(Board board, int distanceToInitialPly, bool exclusion)
		{
			info = 0;
			score = 0;
			int startIndex = (int)((long)(((ulong)(exclusion ? board.GetExclusionKey() : board
				.GetKey())) >> (64 - sizeBits)));
			// Verifies that it is really this board
			for (index = startIndex; index < startIndex + MaxProbes && index < size; index++)
			{
				if (keys[index] == board.GetKey2())
				{
					info = infos[index];
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

		public virtual void NewGeneration()
		{
			generation++;
		}

		public virtual bool IsMyGeneration()
		{
			return GetGeneration() == generation;
		}

		public virtual void Save(Board board, int distanceToInitialPly, int depthAnalyzed
			, int bestMove, int score, int lowerBound, int upperBound, bool exclusion)
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
			if (fixedScore > Evaluator.Victory || fixedScore < -Evaluator.Victory)
			{
				System.Console.Out.WriteLine("The TT score fixed is outside the limits: " + fixedScore
					);
				try
				{
					throw new Exception();
				}
				catch (Exception e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
				System.Environment.Exit(-1);
			}
			if (score <= lowerBound)
			{
				Set(board, TypeFailLow, bestMove, fixedScore, depthAnalyzed, exclusion);
			}
			else
			{
				if (score >= upperBound)
				{
					Set(board, TypeFailHigh, bestMove, fixedScore, depthAnalyzed, exclusion);
				}
				else
				{
					Set(board, TypeExactScore, bestMove, fixedScore, depthAnalyzed, exclusion);
				}
			}
		}

		/// <summary>In case of collision overwrites the eldest.</summary>
		/// <remarks>In case of collision overwrites the eldest. It must keep PV nodes</remarks>
		public virtual void Set(Board board, int nodeType, int bestMove, int score, int depthAnalyzed
			, bool exclusion)
		{
			long key2 = board.GetKey2();
			int startIndex = (int)((long)(((ulong)(exclusion ? board.GetExclusionKey() : board
				.GetKey())) >> (64 - sizeBits)));
			// Verifies that it is really this board
			int oldGenerationIndex = -1;
			// first index of an old generation entry
			int notPvIndex = -1;
			// first index of an not PV entry
			index = -1;
			for (int i = startIndex; i < startIndex + MaxProbes && i < size; i++)
			{
				info = infos[i];
				// Replace an empty TT position or the existing position
				if (keys[i] == 0 || keys[i] == key2)
				{
					index = i;
					break;
				}
				if (oldGenerationIndex == -1 && GetGeneration() != generation)
				{
					oldGenerationIndex = i;
				}
				if (notPvIndex == -1 && GetNodeType() != TypeExactScore)
				{
					notPvIndex = i;
				}
			}
			if (index == -1 && notPvIndex != -1)
			{
				index = notPvIndex;
			}
			else
			{
				if (index == -1 && oldGenerationIndex != -1)
				{
					index = oldGenerationIndex;
				}
				else
				{
					if (index == -1)
					{
						// TT FULL
						return;
					}
				}
			}
			keys[index] = key2;
			info = (bestMove & unchecked((int)(0x1fffff))) | ((nodeType & unchecked((int)(0xf
				))) << 21) | (((long)(generation & unchecked((int)(0xff)))) << 32) | (((long)(depthAnalyzed
				 & unchecked((int)(0xff)))) << 40) | (((long)(score & unchecked((int)(0xffff))))
				 << 48);
			infos[index] = info;
		}
	}
}
