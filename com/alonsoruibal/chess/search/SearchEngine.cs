using System;
using System.Collections.Generic;
using System.Text;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Evaluation;
using Com.Alonsoruibal.Chess.Log;
using Com.Alonsoruibal.Chess.Movesort;
using Com.Alonsoruibal.Chess.TT;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Search
{
	/// <summary>Search engine</summary>
	/// <author>Alberto Alonso Ruibal</author>
	public class SearchEngine : Runnable
	{
		private static readonly Logger logger = Logger.GetLogger("SearchEngine");

		public const int MaxDepth = 64;

		public const int ValueIsMate = Evaluator.Victory - MaxDepth;

		private const int Ply = 2;

		private const int LmrDepthsNotReduced = 3 * Ply;

		private const int RazorDepth = 4 * Ply;

		private static readonly int[] SingularMoveDepth = new int[] { 6 * Ply, 6 * Ply, 8
			 * Ply };

		private static readonly int[] IidDepth = new int[] { 5 * Ply, 5 * Ply, 8 * Ply };

		public const int NodeRoot = 0;

		public const int NodePv = 1;

		public const int NodeNull = 2;

		public bool debug = false;

		private SearchParameters searchParameters;

		private bool searching = false;

		private bool foundOneMove;

		private Config config;

		private long thinkToTime = 0;

		private int thinkToNodes = 0;

		private int thinkToDepth = 0;

		private Board board;

		private SearchObserver observer;

		private Evaluator evaluator;

		private TranspositionTable tt;

		private SortInfo sortInfo;

		private AttacksInfo[] attacksInfos;

		private MoveIterator[] moveIterators;

		private int bestMoveScore;

		private int globalBestMove;

		private int ponderMove;

		private string pv;

		private int initialPly;

		private int depth;

		private int selDepth;

		private int rootScore;

		private int[] aspWindows;

		private bool panicTime;

		private bool engineIsWhite;

		public long startTime;

		private long positionCounter;

		private long pvPositionCounter;

		private long qsPositionCounter;

		private long pvCutNodes;

		private long pvAllNodes;

		private long nullCutNodes;

		private long nullAllNodes;

		private static long aspirationWindowProbe = 0;

		private static long aspirationWindowHit = 0;

		private static long futilityHit = 0;

		private static long aggressiveFutilityHit = 0;

		private static long razoringProbe = 0;

		private static long razoringHit = 0;

		private static long singularExtensionProbe = 0;

		private static long singularExtensionHit = 0;

		private static long nullMoveProbe = 0;

		private static long nullMoveHit = 0;

		private static long ttProbe = 0;

		private static long ttPvHit = 0;

		private static long ttLBHit = 0;

		private static long ttUBHit = 0;

		private static long ttEvalHit = 0;

		private static long ttEvalProbe = 0;

		private bool initialized;

		private Random random;

		private int[][] pvReductionMatrix;

		private int[][] nonPvReductionMatrix;

		public SearchEngine(Config config)
		{
			// Think limits
			// Initial Ply for search
			// For performance benching
			// Aspiration window
			// Futility pruning
			// Aggresive Futility pruning
			// Razoring
			// Singular Extension
			// Null Move
			// Transposition Table
			this.config = config;
			random = new Random();
			board = new Board();
			sortInfo = new SortInfo();
			attacksInfos = new AttacksInfo[MaxDepth];
			moveIterators = new MoveIterator[MaxDepth];
			for (int i = 0; i < MaxDepth; i++)
			{
				attacksInfos[i] = new AttacksInfo();
				moveIterators[i] = new MoveIterator(board, attacksInfos[i], sortInfo, i);
			}
			pvReductionMatrix = new int[][] { new int[64], new int[64], new int[64], new int[
				64], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64
				], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], 
				new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new 
				int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int
				[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64
				], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], 
				new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new 
				int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int
				[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64
				], new int[64], new int[64], new int[64], new int[64] };
			nonPvReductionMatrix = new int[][] { new int[64], new int[64], new int[64], new int
				[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64
				], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], 
				new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new 
				int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int
				[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64
				], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], 
				new int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new 
				int[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int
				[64], new int[64], new int[64], new int[64], new int[64], new int[64], new int[64
				], new int[64], new int[64], new int[64], new int[64] };
			// Init our reduction lookup tables
			for (int depthRemaining = 1; depthRemaining < 64; depthRemaining++)
			{
				// OnePly = 2
				for (int moveNumber = 1; moveNumber < 64; moveNumber++)
				{
					double pvRed = 0.5 + Math.Log(depthRemaining) * Math.Log(moveNumber) / 6.0;
					double nonPVRed = 0.5 + Math.Log(depthRemaining) * Math.Log(moveNumber) / 3.0;
					pvReductionMatrix[depthRemaining][moveNumber] = (int)(pvRed >= 1.0 ? Math.Floor(pvRed
						 * Ply) : 0);
					nonPvReductionMatrix[depthRemaining][moveNumber] = (int)(nonPVRed >= 1.0 ? Math.Floor
						(nonPVRed * Ply) : 0);
				}
			}
			// System.out.println(depthRemaining + " " + moveNumber + " " + pvReductionMatrix[depthRemaining][moveNumber] + " " + nonPvReductionMatrix[depthRemaining][moveNumber]);
			Init();
		}

		public virtual void Init()
		{
			initialized = false;
			board.StartPosition();
			sortInfo.Clear();
			string evaluatorName = config.GetEvaluator();
			if ("simplified".Equals(evaluatorName))
			{
				evaluator = new SimplifiedEvaluator();
			}
			else
			{
				if ("complete".Equals(evaluatorName))
				{
					evaluator = new CompleteEvaluator(config);
				}
				else
				{
					if ("experimental".Equals(evaluatorName))
					{
						evaluator = new ExperimentalEvaluator(config);
					}
				}
			}
			tt = new TranspositionTable(config.GetTranspositionTableSize());
			initialized = true;
			if (debug)
			{
				logger.Debug(config.ToString());
			}
		}

		public virtual void Clear()
		{
			sortInfo.Clear();
			tt.Clear();
		}

		public virtual void Destroy()
		{
			config = null;
			observer = null;
			tt = null;
			evaluator = null;
			sortInfo = null;
			if (moveIterators != null)
			{
				for (int i = 0; i < MaxDepth; i++)
				{
					moveIterators[i] = null;
				}
			}
			System.GC.Collect();
		}

		private int GetReduction(int nodeType, int depth, int movecount)
		{
			return nodeType == NodePv || nodeType == NodeRoot ? pvReductionMatrix[Math.Min(depth
				 / Ply, 63)][Math.Min(movecount, 63)] : nonPvReductionMatrix[Math.Min(depth / Ply
				, 63)][Math.Min(movecount, 63)];
		}

		//
		//
		public virtual void SetObserver(SearchObserver observer)
		{
			this.observer = observer;
		}

		public virtual Board GetBoard()
		{
			return board;
		}

		public virtual int GetBestMove()
		{
			return globalBestMove;
		}

		public virtual long GetBestMoveScore()
		{
			return bestMoveScore;
		}

		public virtual long GetNodes()
		{
			return positionCounter + pvPositionCounter + qsPositionCounter;
		}

		public virtual Config GetConfig()
		{
			return config;
		}

		public virtual void SetConfig(Config config)
		{
			this.config = config;
		}

		/// <summary>Decides when we are going to allow null move.</summary>
		/// <remarks>Decides when we are going to allow null move. Don't do null move in king and pawn endings
		/// 	</remarks>
		private bool BoardAllowsNullMove()
		{
			return (board.GetMines() & (board.knights | board.bishops | board.rooks | board.queens
				)) != 0;
		}

		/// <summary>
		/// Calculates the extension of a move in the actual position
		/// Now the move is not done
		/// </summary>
		private int Extensions(int move, bool mateThreat, int moveSee)
		{
			int ext = 0;
			if (Move.IsCheck(move) && moveSee >= 0)
			{
				ext += config.GetExtensionsCheck();
				if (ext >= Ply)
				{
					return Ply;
				}
			}
			if (Move.GetPieceMoved(move) == Move.Pawn)
			{
				if (Move.IsPawnPush678(move))
				{
					ext += config.GetExtensionsPawnPush();
				}
				if (board.IsPassedPawn(Move.GetToIndex(move)))
				{
					ext += config.GetExtensionsPassedPawn();
				}
				if (ext >= Ply)
				{
					return Ply;
				}
			}
			if (mateThreat)
			{
				ext += config.GetExtensionsMateThreat();
				if (ext >= Ply)
				{
					return Ply;
				}
			}
			return ext;
		}

		/// <summary>Returns true if we can use the value stored on the TT to return from search
		/// 	</summary>
		private bool CanUseTT(int depthRemaining, int alpha, int beta)
		{
			if (tt.GetDepthAnalyzed() >= depthRemaining && tt.IsMyGeneration())
			{
				switch (tt.GetNodeType())
				{
					case TranspositionTable.TypeExactScore:
					{
						ttPvHit++;
						return true;
					}

					case TranspositionTable.TypeFailLow:
					{
						ttLBHit++;
						if (tt.GetScore() <= alpha)
						{
							return true;
						}
						break;
					}

					case TranspositionTable.TypeFailHigh:
					{
						ttUBHit++;
						if (tt.GetScore() >= beta)
						{
							return true;
						}
						break;
					}
				}
			}
			return false;
		}

		/// <summary>It also changes the sign to the score depending of the turn</summary>
		public virtual int Evaluate(bool foundTT, int distanceToInitialPly)
		{
			ttEvalProbe++;
			int eval;
			if (foundTT)
			{
				ttEvalHit++;
				return tt.GetEval();
			}
			eval = evaluator.Evaluate(board, attacksInfos[distanceToInitialPly]);
			if (!board.GetTurn())
			{
				eval = -eval;
			}
			tt.Set(board, TranspositionTable.TypeEval, Move.None, 0, 0, eval, false);
			return eval;
		}

		public virtual int RefineEval(bool foundTT, int eval)
		{
			if (foundTT && ((tt.GetNodeType() == TranspositionTable.TypeExactScore) || ((tt.GetNodeType
				() == TranspositionTable.TypeFailLow) && (tt.GetScore() < eval)) || ((tt.GetNodeType
				() == TranspositionTable.TypeFailHigh) && (tt.GetScore() > eval))))
			{
				return tt.GetScore();
			}
			return eval;
		}

		/// <exception cref="Com.Alonsoruibal.Chess.Search.SearchFinishedException"/>
		public virtual int QuiescentSearch(int qsdepth, int alpha, int beta)
		{
			qsPositionCounter++;
			int distanceToInitialPly = board.GetMoveNumber() - initialPly;
			// It checks draw by three fold repetition, fifty moves rule and no material to mate
			if (board.IsDraw())
			{
				return EvaluateDraw(distanceToInitialPly);
			}
			// Mate distance pruning
			alpha = Math.Max(ValueMatedIn(distanceToInitialPly), alpha);
			beta = Math.Min(ValueMateIn(distanceToInitialPly + 1), beta);
			if (alpha >= beta)
			{
				return alpha;
			}
			bool isPv = beta - alpha > 1;
			int ttMove = Move.None;
			// Generate checks for PV on PLY 0
			bool generateChecks = isPv && (qsdepth == 0);
			// If we generate check, the entry in the TT has depthAnalyzed=1, because is better than without checks (depthAnalyzed=0)
			int ttDepth = generateChecks ? TranspositionTable.DepthQsChecks : TranspositionTable
				.DepthQsNoChecks;
			ttProbe++;
			bool foundTT = tt.Search(board, distanceToInitialPly, false);
			if (foundTT)
			{
				if (!isPv && CanUseTT(ttDepth, alpha, beta))
				{
					return tt.GetScore();
				}
				ttMove = tt.GetBestMove();
			}
			int bestScore = alpha;
			int bestMove = Move.None;
			int staticEval = Evaluator.NoValue;
			int eval = -Evaluator.Victory;
			int futilityBase = -Evaluator.Victory;
			// Do not allow stand pat when in check
			if (!board.GetCheck())
			{
				staticEval = Evaluate(foundTT, distanceToInitialPly);
				eval = RefineEval(foundTT, staticEval);
				// Evaluation functions increase alpha and can originate beta cutoffs
				bestScore = Math.Max(bestScore, eval);
				if (bestScore >= beta)
				{
					if (!foundTT)
					{
						tt.Set(board, TranspositionTable.TypeFailHigh, Move.None, bestScore, TranspositionTable
							.DepthQsChecks, staticEval, false);
					}
					return bestScore;
				}
				futilityBase = eval + config.GetFutilityMarginQS();
			}
			// If we have more depths than possible...
			if (distanceToInitialPly >= MaxDepth - 1)
			{
				return board.GetCheck() ? EvaluateDraw(distanceToInitialPly) : eval;
			}
			// Return a drawish score if we are in check
			bool validOperations = false;
			MoveIterator moveIterator = moveIterators[distanceToInitialPly];
			moveIterator.GenMoves(ttMove, (generateChecks ? MoveIterator.GenerateCapturesPromosChecks
				 : MoveIterator.GenerateCapturesPromos));
			int move;
			while ((move = moveIterator.Next()) != Move.None)
			{
				validOperations = true;
				// Futility pruning
				if (config.GetFutility() && !moveIterator.checkEvasion && !Move.IsCheck(move) && 
					!isPv && move != ttMove && !Move.IsPawnPush678(move) && futilityBase > -Evaluator
					.KnownWin)
				{
					//
					//
					//
					//
					//
					//
					int futilityValue = futilityBase + ExperimentalEvaluator.PieceValues[Move.GetPieceCaptured
						(board, move)];
					if (futilityValue < beta)
					{
						bestScore = Math.Max(bestScore, futilityValue);
						continue;
					}
					if (futilityBase < beta && moveIterator.GetLastMoveSee() <= 0)
					{
						bestScore = Math.Max(bestScore, futilityBase);
						continue;
					}
				}
				board.DoMove(move, false, false);
				System.Diagnostics.Debug.Assert(board.GetCheck() == Move.IsCheck(move), "Check flag not generated properly"
					);
				int score = -QuiescentSearch(qsdepth + 1, -beta, -bestScore);
				board.UndoMove();
				if (score > bestScore)
				{
					bestScore = score;
					bestMove = move;
					if (score >= beta)
					{
						break;
					}
				}
			}
			if (board.GetCheck() && !validOperations)
			{
				return ValueMatedIn(distanceToInitialPly);
			}
			tt.Save(board, distanceToInitialPly, ttDepth, bestMove, bestScore, alpha, beta, staticEval
				, false);
			return bestScore;
		}

		/// <summary>Search Root, PV and null window</summary>
		/// <exception cref="Com.Alonsoruibal.Chess.Search.SearchFinishedException"/>
		public virtual int Search(int nodeType, int depthRemaining, int alpha, int beta, 
			bool allowNullMove, int excludedMove)
		{
			if (nodeType != NodeRoot && foundOneMove && (Runtime.CurrentTimeMillis() > thinkToTime
				 || (positionCounter + pvPositionCounter + qsPositionCounter) > thinkToNodes))
			{
				FinishRun();
			}
			int distanceToInitialPly = board.GetMoveNumber() - initialPly;
			if (nodeType == NodePv || nodeType == NodeRoot)
			{
				pvPositionCounter++;
				if (distanceToInitialPly > selDepth)
				{
					selDepth = distanceToInitialPly;
				}
			}
			else
			{
				positionCounter++;
			}
			// It checks draw by three fold repetition, fifty moves rule and no material to mate
			if (board.IsDraw())
			{
				return EvaluateDraw(distanceToInitialPly);
			}
			// Mate distance pruning
			alpha = Math.Max(ValueMatedIn(distanceToInitialPly), alpha);
			beta = Math.Min(ValueMateIn(distanceToInitialPly + 1), beta);
			if (alpha >= beta)
			{
				return alpha;
			}
			int ttMove = Move.None;
			int ttScore = 0;
			int ttNodeType = 0;
			int ttDepthAnalyzed = 0;
			int score = 0;
			ttProbe++;
			bool foundTT = tt.Search(board, distanceToInitialPly, excludedMove != 0);
			if (foundTT)
			{
				if (nodeType != NodeRoot && CanUseTT(depthRemaining, alpha, beta))
				{
					return tt.GetScore();
				}
				ttMove = tt.GetBestMove();
				ttScore = tt.GetScore();
				ttNodeType = tt.GetNodeType();
				ttDepthAnalyzed = tt.GetDepthAnalyzed();
			}
			bool mateThreat = false;
			bool futilityPrune = false;
			int futilityValue = -Evaluator.Victory;
			int staticEval = -Evaluator.Victory;
			int eval = -Evaluator.Victory;
			if (!board.GetCheck())
			{
				// Do a static eval, in case of exclusion and not found in the TT, search again with the normal key
				bool evalTT = excludedMove == 0 || foundTT ? foundTT : tt.Search(board, distanceToInitialPly
					, false);
				staticEval = Evaluate(evalTT, distanceToInitialPly);
				eval = RefineEval(foundTT, staticEval);
			}
			// If we have more depths than possible...
			if (distanceToInitialPly >= MaxDepth - 1)
			{
				return board.GetCheck() ? EvaluateDraw(distanceToInitialPly) : eval;
			}
			// Return a drawish score if we are in check
			if (!board.GetCheck())
			{
				// Hyatt's Razoring http://chessprogramming.wikispaces.com/Razoring
				if (nodeType == NodeNull && config.GetRazoring() && ttMove == 0 && allowNullMove 
					&& depthRemaining < RazorDepth && Math.Abs(beta) < ValueIsMate && eval + config.
					GetRazoringMargin() < beta && (board.pawns & ((board.whites & BitboardUtils.b2_u
					) | (board.blacks & BitboardUtils.b2_d))) == 0)
				{
					//
					//
					//
					// Not when last was a null move
					//
					//
					//
					// No pawns on 7TH
					razoringProbe++;
					if (depthRemaining <= Ply)
					{
						razoringHit++;
						return QuiescentSearch(0, alpha, beta);
					}
					int rbeta = beta - config.GetRazoringMargin();
					int v = QuiescentSearch(0, rbeta - 1, rbeta);
					if (v < rbeta)
					{
						razoringHit++;
						return v;
					}
				}
				// Static null move pruning or futility pruning in parent node
				if (nodeType == NodeNull && config.GetStaticNullMove() && allowNullMove && depthRemaining
					 < RazorDepth && Math.Abs(beta) < ValueIsMate && Math.Abs(eval) < Evaluator.KnownWin
					 && eval - config.GetFutilityMargin() >= beta && BoardAllowsNullMove())
				{
					//
					//
					//
					//
					//
					//
					//
					return eval - config.GetFutilityMargin();
				}
				// Null move pruning and mate threat detection
				if (nodeType == NodeNull && config.GetNullMove() && allowNullMove && depthRemaining
					 > 3 * Ply && Math.Abs(beta) < ValueIsMate && eval > beta - (depthRemaining >= 4
					 * Ply ? config.GetNullMoveMargin() : 0) && BoardAllowsNullMove())
				{
					//
					//
					//
					//
					//
					//
					nullMoveProbe++;
					board.DoMove(0, false, false);
					int R = 3 * Ply + (depthRemaining >= 5 * Ply ? depthRemaining / (4 * Ply) : 0);
					if (eval - beta > ExperimentalEvaluator.Pawn)
					{
						R++;
					}
					// TODO TEST adding PLY
					score = -Search(NodeNull, depthRemaining - R, -beta, -beta + 1, false, 0);
					board.UndoMove();
					if (score >= beta)
					{
						if (score >= ValueIsMate)
						{
							score = beta;
						}
						// Verification search on initial depths
						if (depthRemaining < 6 * Ply || Search(NodeNull, depthRemaining - 5 * Ply, beta -
							 1, beta, false, 0) >= beta)
						{
							//
							nullMoveHit++;
							return score;
						}
					}
					else
					{
						// Detect mate threat
						if (score <= -ValueIsMate)
						{
							mateThreat = true;
						}
					}
				}
				// Internal Iterative Deepening (IID)
				// Do a reduced move to search for a ttMove that will improve sorting
				if (config.GetIid() && ttMove == 0 && depthRemaining >= IidDepth[nodeType] && allowNullMove
					 && (nodeType != NodeNull || staticEval + config.GetIidMargin() > beta) && excludedMove
					 == 0)
				{
					//
					//
					//
					//
					//
					int d = (nodeType == NodePv ? depthRemaining - 2 * Ply : depthRemaining >> 1);
					Search(nodeType, d, alpha, beta, false, 0);
					if (tt.Search(board, distanceToInitialPly, false))
					{
						ttMove = tt.GetBestMove();
					}
				}
				// Futility pruning
				if (nodeType == NodeNull && config.GetFutility())
				{
					if (depthRemaining <= Ply)
					{
						// at frontier nodes
						futilityValue = staticEval + config.GetFutilityMargin();
						if (futilityValue < beta)
						{
							futilityHit++;
							futilityPrune = true;
						}
					}
					else
					{
						if (depthRemaining <= 2 * Ply)
						{
							// at pre-frontier nodes
							futilityValue = staticEval + config.GetFutilityMarginAggressive();
							if (futilityValue < beta)
							{
								aggressiveFutilityHit++;
								futilityPrune = true;
							}
						}
					}
				}
			}
			MoveIterator moveIterator = moveIterators[distanceToInitialPly];
			moveIterator.GenMoves(ttMove);
			int movesDone = 0;
			bool validOperations = false;
			int bestScore = -Evaluator.Victory;
			int move;
			int bestMove = Move.None;
			while ((move = moveIterator.Next()) != Move.None)
			{
				validOperations = true;
				if (move == excludedMove)
				{
					continue;
				}
				int extension = Extensions(move, mateThreat, moveIterator.GetLastMoveSee());
				// Check singular move extension
				// It also detects singular replies
				if (nodeType != NodeRoot && move == ttMove && extension < Ply && excludedMove == 
					0 && config.GetExtensionsSingular() > 0 && depthRemaining >= SingularMoveDepth[nodeType
					] && ttNodeType == TranspositionTable.TypeFailHigh && ttDepthAnalyzed >= depthRemaining
					 - 3 * Ply && Math.Abs(ttScore) < Evaluator.KnownWin)
				{
					//
					//
					//
					//
					//
					//
					//
					//
					singularExtensionProbe++;
					int seBeta = ttScore - config.GetSingularExtensionMargin();
					int excScore = depthRemaining >> 1 < Ply ? QuiescentSearch(0, seBeta - 1, seBeta)
						 : Search(nodeType, depthRemaining >> 1, seBeta - 1, seBeta, false, move);
					if (excScore < seBeta)
					{
						singularExtensionHit++;
						extension += config.GetExtensionsSingular();
						if (extension > Ply)
						{
							extension = Ply;
						}
					}
				}
				bool importantMove = nodeType == NodeRoot || extension != 0 || moveIterator.checkEvasion
					 || Move.IsCheck(move) || Move.IsCapture(move) || Move.IsPawnPush678(move) || Move
					.IsCastling(move) || move == ttMove || sortInfo.IsKiller(move, distanceToInitialPly
					 + 1);
				//
				//
				//
				//
				// Include ALL captures
				// Includes promotions
				//
				if (futilityPrune && bestScore > -Evaluator.KnownWin && !importantMove)
				{
					//
					//
					if (futilityValue <= alpha)
					{
						if (futilityValue > bestScore)
						{
							bestScore = futilityValue;
						}
					}
					continue;
				}
				board.DoMove(move, false, false);
				System.Diagnostics.Debug.Assert(board.GetCheck() == Move.IsCheck(move), "Check flag not generated properly"
					);
				movesDone++;
				int lowBound = (alpha > bestScore ? alpha : bestScore);
				if ((nodeType == NodePv || nodeType == NodeRoot) && movesDone == 1)
				{
					// PV move not null searched
					score = depthRemaining + extension - Ply < Ply ? -QuiescentSearch(0, -beta, -lowBound
						) : -Search(NodePv, depthRemaining + extension - Ply, -beta, -lowBound, true, 0);
				}
				else
				{
					// Try searching null window
					bool doFullSearch = true;
					// Late move reductions (LMR)
					int reduction = 0;
					if (config.GetLmr() && depthRemaining >= LmrDepthsNotReduced && !importantMove)
					{
						//
						//
						reduction += GetReduction(nodeType, depthRemaining, movesDone);
					}
					if (reduction > 0)
					{
						score = depthRemaining - reduction - Ply < Ply ? -QuiescentSearch(0, -lowBound - 
							1, -lowBound) : -Search(NodeNull, depthRemaining - reduction - Ply, -lowBound - 
							1, -lowBound, true, 0);
						doFullSearch = (score > lowBound);
					}
					if (doFullSearch)
					{
						score = depthRemaining + extension - Ply < Ply ? -QuiescentSearch(0, -lowBound - 
							1, -lowBound) : -Search(NodeNull, depthRemaining + extension - Ply, -lowBound - 
							1, -lowBound, true, 0);
						// Finally search as PV if score on window
						if ((nodeType == NodePv || nodeType == NodeRoot) && score > lowBound && (nodeType
							 == NodeRoot || score < beta))
						{
							//
							//
							score = depthRemaining + extension - Ply < Ply ? -QuiescentSearch(0, -beta, -lowBound
								) : -Search(NodePv, depthRemaining + extension - Ply, -beta, -lowBound, true, 0);
						}
					}
				}
				board.UndoMove();
				// It tracks the best move and it also insert errors on the root node
				if (score > bestScore && (nodeType != NodeRoot || config.GetRand() == 0 || (random
					.Next(100) > config.GetRand())))
				{
					bestMove = move;
					bestScore = score;
					if (nodeType == NodeRoot)
					{
						globalBestMove = move;
						bestMoveScore = score;
						foundOneMove = true;
						if (depthRemaining > 6 * Ply)
						{
							NotifyMoveFound(move, score, alpha, beta);
						}
					}
				}
				// alpha/beta cut (fail high)
				if (score >= beta)
				{
					break;
				}
			}
			// Checkmate or stalemate
			if (excludedMove == 0 && !validOperations)
			{
				bestScore = EvaluateEndgame(distanceToInitialPly);
			}
			// Fix score for excluded moves
			if (bestScore == -Evaluator.Victory)
			{
				bestScore = ValueMatedIn(distanceToInitialPly);
			}
			// Tells MoveSorter the move score
			if (bestScore >= beta)
			{
				if (excludedMove == 0 && validOperations)
				{
					sortInfo.BetaCutoff(bestMove, distanceToInitialPly);
				}
				if (nodeType == NodeNull)
				{
					nullCutNodes++;
				}
				else
				{
					pvCutNodes++;
				}
			}
			else
			{
				if (nodeType == NodeNull)
				{
					nullAllNodes++;
				}
				else
				{
					pvAllNodes++;
				}
			}
			// Save in the transposition table
			tt.Save(board, distanceToInitialPly, depthRemaining, bestMove, bestScore, alpha, 
				beta, staticEval, excludedMove != 0);
			return bestScore;
		}

		/// <summary>Notifies the best move to the SearchObserver filling a SearchStatusInfo object
		/// 	</summary>
		private void NotifyMoveFound(int move, int score, int alpha, int beta)
		{
			long time = Runtime.CurrentTimeMillis();
			GetPv(move);
			SearchStatusInfo info = new SearchStatusInfo();
			info.SetDepth(depth);
			info.SetSelDepth(selDepth);
			info.SetTime(time - startTime);
			info.SetPv(pv);
			info.SetScore(score, alpha, beta);
			info.SetNodes(positionCounter + pvPositionCounter + qsPositionCounter);
			info.SetHashFull(tt.GetHashFull());
			info.SetNps((int)(1000 * (positionCounter + pvPositionCounter + qsPositionCounter
				) / ((time - startTime + 1))));
			if (observer != null)
			{
				observer.Info(info);
			}
			else
			{
				logger.Debug(info.ToString());
			}
		}

		/// <summary>It searches for the best movement</summary>
		public virtual void Go(SearchParameters searchParameters)
		{
			if (!initialized)
			{
				return;
			}
			if (!searching)
			{
				this.searchParameters = searchParameters;
				try
				{
					PrepareRun();
					Run();
				}
				catch (Exception)
				{
				}
			}
		}

		private void SearchStats()
		{
			if ((positionCounter + pvPositionCounter + qsPositionCounter) > 0)
			{
				logger.Debug("Positions PV      = " + pvPositionCounter + " " + (100 * pvPositionCounter
					 / (positionCounter + pvPositionCounter + qsPositionCounter)) + "%");
				//
				logger.Debug("Positions QS      = " + qsPositionCounter + " " + (100 * qsPositionCounter
					 / (positionCounter + pvPositionCounter + qsPositionCounter)) + "%");
				//
				logger.Debug("Positions Null    = " + positionCounter + " " + (100 * positionCounter
					 / (positionCounter + pvPositionCounter + qsPositionCounter)) + "%");
			}
			//
			logger.Debug("PV Cut            = " + pvCutNodes + " " + (100 * pvCutNodes / (pvCutNodes
				 + pvAllNodes + 1)) + "%");
			logger.Debug("PV All            = " + pvAllNodes);
			logger.Debug("Null Cut          = " + nullCutNodes + " " + (100 * nullCutNodes / 
				(nullCutNodes + nullAllNodes + 1)) + "%");
			logger.Debug("Null All          = " + nullAllNodes);
			if (aspirationWindowProbe > 0)
			{
				logger.Debug("Asp Win      Hits = " + (100 * aspirationWindowHit / aspirationWindowProbe
					) + "%");
			}
			if (ttEvalProbe > 0)
			{
				logger.Debug("TT Eval      Hits = " + ttEvalHit + " " + (100 * ttEvalHit / ttEvalProbe
					) + "%");
			}
			if (ttProbe > 0)
			{
				logger.Debug("TT PV        Hits = " + ttPvHit + " " + (1000000 * ttPvHit / ttProbe
					) + " per 10^6");
				logger.Debug("TT LB        Hits = " + ttProbe + " " + (100 * ttLBHit / ttProbe) +
					 "%");
				logger.Debug("TT UB        Hits = " + ttUBHit + " " + (100 * ttUBHit / ttProbe) +
					 "%");
			}
			logger.Debug("Futility     Hits = " + futilityHit);
			logger.Debug("Agg.Futility Hits = " + aggressiveFutilityHit);
			if (nullMoveProbe > 0)
			{
				logger.Debug("Null Move    Hits = " + nullMoveHit + " " + (100 * nullMoveHit / nullMoveProbe
					) + "%");
			}
			if (razoringProbe > 0)
			{
				logger.Debug("Razoring     Hits = " + razoringHit + " " + (100 * razoringHit / razoringProbe
					) + "%");
			}
			if (singularExtensionProbe > 0)
			{
				logger.Debug("S.Extensions Hits = " + singularExtensionHit + " " + (100 * singularExtensionHit
					 / singularExtensionProbe) + "%");
			}
		}

		/// <exception cref="Com.Alonsoruibal.Chess.Search.SearchFinishedException"/>
		public virtual void PrepareRun()
		{
			startTime = Runtime.CurrentTimeMillis();
			SetSearchLimits(searchParameters, false);
			panicTime = false;
			engineIsWhite = board.GetTurn();
			foundOneMove = false;
			searching = true;
			logger.Debug("Board\n" + board);
			positionCounter = 0;
			pvPositionCounter = 0;
			qsPositionCounter = 0;
			globalBestMove = Move.None;
			ponderMove = Move.None;
			pv = null;
			initialPly = board.GetMoveNumber();
			if (config.GetUseBook() && config.GetBook() != null && board.IsUsingBook() && (config
				.GetBookKnowledge() == 100 || ((random.NextDouble() * 100) < config.GetBookKnowledge
				())))
			{
				logger.Debug("Searching move in book");
				int bookMove = config.GetBook().GetMove(board);
				if (bookMove != 0)
				{
					globalBestMove = bookMove;
					logger.Debug("Move found in book");
					FinishRun();
				}
				else
				{
					logger.Debug("Move NOT found in book");
					board.SetOutBookMove(board.GetMoveNumber());
				}
			}
			depth = 1;
			rootScore = Evaluate(tt.Search(board, 0, false), 0);
			tt.NewGeneration();
			aspWindows = config.GetAspirationWindowSizes();
		}

		/// <exception cref="Com.Alonsoruibal.Chess.Search.SearchFinishedException"/>
		private void RunStepped()
		{
			selDepth = 0;
			int failHighCount = 0;
			int failLowCount = 0;
			int initialScore = rootScore;
			int alpha = (initialScore - aspWindows[failLowCount] > -Evaluator.Victory ? initialScore
				 - aspWindows[failLowCount] : -Evaluator.Victory);
			int beta = (initialScore + aspWindows[failHighCount] < Evaluator.Victory ? initialScore
				 + aspWindows[failHighCount] : Evaluator.Victory);
			int previousRootScore = rootScore;
			long time1 = Runtime.CurrentTimeMillis();
			// Iterate aspiration windows
			while (true)
			{
				aspirationWindowProbe++;
				rootScore = Search(NodeRoot, depth * Ply, alpha, beta, false, 0);
				// logger.debug("alpha = " + alpha + ", beta = " + beta + ", rootScore=" + rootScore);
				if (rootScore <= alpha)
				{
					failLowCount++;
					alpha = (failLowCount < aspWindows.Length && (initialScore - aspWindows[failLowCount
						] > -Evaluator.Victory) ? initialScore - aspWindows[failLowCount] : -Evaluator.Victory
						);
				}
				else
				{
					if (rootScore >= beta)
					{
						failHighCount++;
						beta = (failHighCount < aspWindows.Length && (initialScore + aspWindows[failHighCount
							] < Evaluator.Victory) ? initialScore + aspWindows[failHighCount] : Evaluator.Victory
							);
					}
					else
					{
						aspirationWindowHit++;
						break;
					}
				}
			}
			long time2 = Runtime.CurrentTimeMillis();
			if (depth <= 6)
			{
				NotifyMoveFound(globalBestMove, bestMoveScore, alpha, beta);
			}
			else
			{
				if (!panicTime && rootScore < previousRootScore - 100)
				{
					panicTime = true;
					SetSearchLimits(searchParameters, true);
				}
			}
			if ((searchParameters.ManageTime() && (Math.Abs(rootScore) > ValueIsMate || (time2
				 + ((time2 - time1) << 1)) > thinkToTime)) || depth == MaxDepth || depth > thinkToDepth)
			{
				// Under time restrictions and...
				// Mate found or
				// It will not likely finish the next iteration
				// Search limit reached
				FinishRun();
			}
			depth++;
		}

		public virtual void SetSearchLimits(SearchParameters searchParameters, bool panicTime
			)
		{
			thinkToNodes = searchParameters.GetNodes();
			thinkToDepth = searchParameters.GetDepth();
			thinkToTime = searchParameters.CalculateMoveTime(engineIsWhite, startTime, panicTime
				);
		}

		/// <exception cref="Com.Alonsoruibal.Chess.Search.SearchFinishedException"/>
		public virtual void FinishRun()
		{
			// Go back the board to the initial position
			board.UndoMove(initialPly);
			searching = false;
			if (observer != null)
			{
				observer.BestMove(globalBestMove, ponderMove);
			}
			if (debug)
			{
				SearchStats();
			}
			throw new SearchFinishedException();
		}

		public virtual void Run()
		{
			try
			{
				while (true)
				{
					RunStepped();
				}
			}
			catch (SearchFinishedException)
			{
			}
		}

		/// <summary>Gets the principal variation from the transposition table</summary>
		private void GetPv(int firstMove)
		{
			StringBuilder sb = new StringBuilder();
			IList<long> keys = new AList<long>();
			// To not repeat keys
			sb.Append(Move.ToString(firstMove));
			board.DoMove(firstMove);
			int i = 1;
			while (i < 256)
			{
				if (tt.Search(board, i, false))
				{
					if (tt.GetBestMove() == 0 || keys.Contains(board.GetKey()))
					{
						break;
					}
					keys.AddItem(board.GetKey());
					if (i == 1)
					{
						ponderMove = tt.GetBestMove();
					}
					sb.Append(" ");
					sb.Append(Move.ToString(tt.GetBestMove()));
					board.DoMove(tt.GetBestMove(), true, false);
					i++;
					if (board.IsMate())
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			// Now undo moves
			for (int j = 0; j < i; j++)
			{
				board.UndoMove();
			}
			pv = sb.ToString();
		}

		public virtual void Stop()
		{
			thinkToTime = 0;
			thinkToNodes = 0;
			thinkToDepth = 0;
		}

		/// <summary>Is better to end before.</summary>
		/// <remarks>Is better to end before. Not necessary to change sign</remarks>
		public virtual int EvaluateEndgame(int distanceToInitialPly)
		{
			if (board.GetCheck())
			{
				return ValueMatedIn(distanceToInitialPly);
			}
			else
			{
				return EvaluateDraw(distanceToInitialPly);
			}
		}

		public virtual int EvaluateDraw(int distanceToInitialPly)
		{
			return (distanceToInitialPly & 1) == 0 ? -config.GetContemptFactor() : config.GetContemptFactor
				();
		}

		private int ValueMatedIn(int distanceToInitialPly)
		{
			return -Evaluator.Victory + distanceToInitialPly;
		}

		private int ValueMateIn(int distanceToInitialPly)
		{
			return Evaluator.Victory - distanceToInitialPly;
		}

		public virtual TranspositionTable GetTT()
		{
			return tt;
		}

		public virtual bool IsInitialized()
		{
			return initialized;
		}

		public virtual bool IsSearching()
		{
			return searching;
		}

		public virtual void SetSearchParameters(SearchParameters searchParameters)
		{
			this.searchParameters = searchParameters;
		}
	}
}
