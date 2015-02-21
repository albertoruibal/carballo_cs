using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movesort
{
	/// <summary>
	/// Sort Moves based on heuristics
	/// short first GOOD captures (a piece of less value captures other of more value)
	/// <p/>
	/// SEE captures, and move captures with SEE&lt;0 to the end
	/// </summary>
	public class MoveIterator
	{
		public const int PhaseTt = 0;

		public const int PhaseGenCaptures = 1;

		public const int PhaseGoodCapturesAndPromos = 2;

		public const int PhaseEqualCaptures = 3;

		public const int PhaseGenNoncaptures = 4;

		public const int PhaseKiller1 = 5;

		public const int PhaseKiller2 = 6;

		public const int PhaseNoncaptures = 7;

		public const int PhaseBadCaptures = 8;

		public const int PhaseEnd = 9;

		private static readonly int[] VictimPieceValues = new int[] { 0, 100, 325, 330, 500
			, 975, 10000 };

		private static readonly int[] AggressorPieceValues = new int[] { 0, 10, 32, 33, 50
			, 97, 99 };

		private const int ScorePromotionQueen = 975;

		private const int ScoreUnderpromotion = int.MinValue + 1;

		private const int ScoreLowest = int.MinValue;

		private Board board;

		private int ttMove;

		private int lastMoveSee;

		private int killer1;

		private int killer2;

		private bool foundKiller1;

		private bool foundKiller2;

		private bool quiescence;

		private bool generateChecks;

		private bool checkEvasion;

		private int nonCaptureIndex;

		private int goodCaptureIndex;

		private int equalCaptureIndex;

		private int badCaptureIndex;

		private long all;

		private long mines;

		private long others;

		private long[] attacks = new long[64];

		public int[] goodCaptures = new int[256];

		public int[] goodCapturesSee = new int[256];

		public int[] goodCapturesScores = new int[256];

		public int[] badCaptures = new int[256];

		public int[] badCapturesSee = new int[256];

		public int[] badCapturesScores = new int[256];

		public int[] equalCaptures = new int[256];

		public int[] equalCapturesSee = new int[256];

		public int[] equalCapturesScores = new int[256];

		public int[] nonCaptures = new int[256];

		public int[] nonCapturesScores = new int[256];

		private int depth;

		internal SortInfo sortInfo;

		internal int phase;

		internal BitboardAttacks bbAttacks;

		//	private static final Logger logger = Logger.getLogger(MoveIterator.class);
		// Stores slider pieces attacks
		// Stores captures and queen promotions
		// Stores captures and queen promotions
		// Stores captures and queen promotions
		// Stores non captures and underpromotions
		public virtual int GetPhase()
		{
			return phase;
		}

		public MoveIterator(Board board, SortInfo sortInfo, int depth)
		{
			this.sortInfo = sortInfo;
			this.board = board;
			this.depth = depth;
			bbAttacks = BitboardAttacks.GetInstance();
		}

		public virtual void SetBoard(Board board)
		{
			this.board = board;
		}

		/// <summary>Generates captures and tactical moves (not underpromotions)</summary>
		public virtual void GenerateCaptures()
		{
			// logger.debug(board);
			all = board.GetAll();
			// only for clearity
			mines = board.GetMines();
			others = board.GetOthers();
			byte index = 0;
			long square = unchecked((long)(0x1L));
			while (square != 0)
			{
				attacks[index] = 0;
				if (board.GetTurn() == ((square & board.whites) != 0))
				{
					if ((square & board.rooks) != 0)
					{
						// Rook
						attacks[index] = bbAttacks.GetRookAttacks(index, all);
						GenerateCapturesFromAttacks(Move.Rook, index, attacks[index] & others);
					}
					else
					{
						if ((square & board.bishops) != 0)
						{
							// Bishop
							attacks[index] = bbAttacks.GetBishopAttacks(index, all);
							GenerateCapturesFromAttacks(Move.Bishop, index, attacks[index] & others);
						}
						else
						{
							if ((square & board.queens) != 0)
							{
								// Queen
								attacks[index] = bbAttacks.GetRookAttacks(index, all) | bbAttacks.GetBishopAttacks
									(index, all);
								GenerateCapturesFromAttacks(Move.Queen, index, attacks[index] & others);
							}
							else
							{
								if ((square & board.kings) != 0)
								{
									// King
									GenerateCapturesFromAttacks(Move.King, index, bbAttacks.king[index] & others);
								}
								else
								{
									if ((square & board.knights) != 0)
									{
										// Knight
										GenerateCapturesFromAttacks(Move.Knight, index, bbAttacks.knight[index] & others);
									}
									else
									{
										if ((square & board.pawns) != 0)
										{
											// Pawns
											if ((square & board.whites) != 0)
											{
												GeneratePawnCapturesAndGoodPromos(index, (bbAttacks.pawnUpwards[index] & (others 
													| board.GetPassantSquare())) | (((square << 8) & all) == 0 ? (square << 8) : 0), 
													board.GetPassantSquare());
											}
											else
											{
												GeneratePawnCapturesAndGoodPromos(index, (bbAttacks.pawnDownwards[index] & (others
													 | board.GetPassantSquare())) | ((((long)(((ulong)square) >> 8)) & all) == 0 ? (
													(long)(((ulong)square) >> 8)) : 0), board.GetPassantSquare());
											}
										}
									}
								}
							}
						}
					}
				}
				square <<= 1;
				index++;
			}
		}

		/// <summary>Generates underpromotions and non tactical moves</summary>
		public virtual void GenerateNonCaptures()
		{
			all = board.GetAll();
			// only for clearity
			mines = board.GetMines();
			others = board.GetOthers();
			int index = 0;
			long square = unchecked((long)(0x1L));
			while (square != 0)
			{
				if (board.GetTurn() == ((square & board.whites) != 0))
				{
					if ((square & board.rooks) != 0)
					{
						// Rook
						GenerateNonCapturesFromAttacks(Move.Rook, index, attacks[index] & ~all);
					}
					else
					{
						if ((square & board.bishops) != 0)
						{
							// Bishop
							GenerateNonCapturesFromAttacks(Move.Bishop, index, attacks[index] & ~all);
						}
						else
						{
							if ((square & board.queens) != 0)
							{
								// Queen
								GenerateNonCapturesFromAttacks(Move.Queen, index, attacks[index] & ~all);
							}
							else
							{
								if ((square & board.kings) != 0)
								{
									// King
									GenerateNonCapturesFromAttacks(Move.King, index, bbAttacks.king[index] & ~all);
								}
								else
								{
									if ((square & board.knights) != 0)
									{
										// Knight
										GenerateNonCapturesFromAttacks(Move.Knight, index, bbAttacks.knight[index] & ~all
											);
									}
								}
							}
						}
					}
					if ((square & board.pawns) != 0)
					{
						// Pawns
						if ((square & board.whites) != 0)
						{
							GeneratePawnNonCapturesAndBadPromos(index, (bbAttacks.pawnUpwards[index] & others
								) | (((square << 8) & all) == 0 ? (square << 8) : 0) | ((square & BitboardUtils.
								b2_d) != 0 && (((square << 8) | (square << 16)) & all) == 0 ? (square << 16) : 0
								));
						}
						else
						{
							GeneratePawnNonCapturesAndBadPromos(index, (bbAttacks.pawnDownwards[index] & others
								) | ((((long)(((ulong)square) >> 8)) & all) == 0 ? ((long)(((ulong)square) >> 8)
								) : 0) | ((square & BitboardUtils.b2_u) != 0 && ((((long)(((ulong)square) >> 8))
								 | ((long)(((ulong)square) >> 16))) & all) == 0 ? ((long)(((ulong)square) >> 16)
								) : 0));
						}
					}
				}
				square <<= 1;
				index++;
			}
			square = board.kings & mines;
			// my king
			int myKingIndex = -1;
			// Castling: disabled when in check or squares attacked
			if ((((all & (board.GetTurn() ? unchecked((long)(0x06L)) : unchecked((long)(0x0600000000000000L
				)))) == 0 && (board.GetTurn() ? board.GetWhiteKingsideCastling() : board.GetBlackKingsideCastling
				()))))
			{
				myKingIndex = BitboardUtils.Square2Index(square);
				if (!board.GetCheck() && !bbAttacks.IsIndexAttacked(board, unchecked((byte)(myKingIndex
					 - 1)), board.GetTurn()) && !bbAttacks.IsIndexAttacked(board, unchecked((byte)(myKingIndex
					 - 2)), board.GetTurn()))
				{
					AddNonCapturesAndBadPromos(Move.King, myKingIndex, myKingIndex - 2, 0, false, Move
						.TypeKingsideCastling);
				}
			}
			if ((((all & (board.GetTurn() ? unchecked((long)(0x70L)) : unchecked((long)(0x7000000000000000L
				)))) == 0 && (board.GetTurn() ? board.GetWhiteQueensideCastling() : board.GetBlackQueensideCastling
				()))))
			{
				if (myKingIndex == -1)
				{
					myKingIndex = BitboardUtils.Square2Index(square);
				}
				if (!board.GetCheck() && !bbAttacks.IsIndexAttacked(board, unchecked((byte)(myKingIndex
					 + 1)), board.GetTurn()) && !bbAttacks.IsIndexAttacked(board, unchecked((byte)(myKingIndex
					 + 2)), board.GetTurn()))
				{
					AddNonCapturesAndBadPromos(Move.King, myKingIndex, myKingIndex + 2, 0, false, Move
						.TypeQueensideCastling);
				}
			}
		}

		/// <summary>Generates moves from an attack mask</summary>
		private void GenerateCapturesFromAttacks(int pieceMoved, int fromIndex, long attacks
			)
		{
			while (attacks != 0)
			{
				long to = BitboardUtils.Lsb(attacks);
				AddCapturesAndGoodPromos(pieceMoved, fromIndex, BitboardUtils.Square2Index(to), to
					, true, 0);
				attacks ^= to;
			}
		}

		private void GenerateNonCapturesFromAttacks(int pieceMoved, int fromIndex, long attacks
			)
		{
			while (attacks != 0)
			{
				long to = BitboardUtils.Lsb(attacks);
				AddNonCapturesAndBadPromos(pieceMoved, fromIndex, BitboardUtils.Square2Index(to), 
					to, false, 0);
				attacks ^= to;
			}
		}

		private void GeneratePawnCapturesAndGoodPromos(int fromIndex, long attacks, long 
			passant)
		{
			while (attacks != 0)
			{
				long to = BitboardUtils.Lsb(attacks);
				if ((to & passant) != 0)
				{
					AddCapturesAndGoodPromos(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), to
						, true, Move.TypePassant);
				}
				else
				{
					bool capture = (to & others) != 0;
					if ((to & (BitboardUtils.b_u | BitboardUtils.b_d)) != 0)
					{
						AddCapturesAndGoodPromos(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), to
							, capture, Move.TypePromotionQueen);
					}
					else
					{
						if (capture)
						{
							AddCapturesAndGoodPromos(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), to
								, capture, 0);
						}
					}
				}
				attacks ^= to;
			}
		}

		private void GeneratePawnNonCapturesAndBadPromos(int fromIndex, long attacks)
		{
			while (attacks != 0)
			{
				long to = BitboardUtils.Lsb(attacks);
				bool capture = (to & others) != 0;
				if ((to & (BitboardUtils.b_u | BitboardUtils.b_d)) != 0)
				{
					AddNonCapturesAndBadPromos(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), 
						to, capture, Move.TypePromotionKnight);
					AddNonCapturesAndBadPromos(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), 
						to, capture, Move.TypePromotionRook);
					AddNonCapturesAndBadPromos(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), 
						to, capture, Move.TypePromotionBishop);
				}
				else
				{
					if (!capture)
					{
						AddNonCapturesAndBadPromos(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), 
							to, capture, 0);
					}
				}
				attacks ^= to;
			}
		}

		private void AddNonCapturesAndBadPromos(int pieceMoved, int fromIndex, int toIndex
			, long to, bool capture, int moveType)
		{
			int move = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, moveType);
			if (move == killer1)
			{
				foundKiller1 = true;
			}
			else
			{
				if (move == killer2)
				{
					foundKiller2 = true;
				}
				else
				{
					if (move != ttMove)
					{
						// Score non captures
						int score = sortInfo.GetMoveScore(move);
						if (moveType == Move.TypePromotionKnight || moveType == Move.TypePromotionRook ||
							 moveType == Move.TypePromotionBishop)
						{
							score -= ScoreUnderpromotion;
						}
						nonCaptures[nonCaptureIndex] = move;
						nonCapturesScores[nonCaptureIndex] = score;
						nonCaptureIndex++;
					}
				}
			}
		}

		private void AddCapturesAndGoodPromos(int pieceMoved, int fromIndex, int toIndex, 
			long to, bool capture, int moveType)
		{
			int move = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, moveType);
			if (move != ttMove)
			{
				// Score captures
				int pieceCaptured = 0;
				if ((to & board.knights) != 0)
				{
					pieceCaptured = Move.Knight;
				}
				else
				{
					if ((to & board.bishops) != 0)
					{
						pieceCaptured = Move.Bishop;
					}
					else
					{
						if ((to & board.rooks) != 0)
						{
							pieceCaptured = Move.Rook;
						}
						else
						{
							if ((to & board.queens) != 0)
							{
								pieceCaptured = Move.Queen;
							}
							else
							{
								if (capture)
								{
									pieceCaptured = Move.Pawn;
								}
							}
						}
					}
				}
				int see = 0;
				if (capture)
				{
					see = board.See(fromIndex, toIndex, pieceMoved, pieceCaptured);
				}
				if (see >= 0)
				{
					int score = 0;
					// Order GOOD captures by MVV/LVA (Hyatt dixit)
					if (capture)
					{
						score = VictimPieceValues[pieceCaptured] - AggressorPieceValues[pieceMoved];
					}
					if (see > 0 || moveType == Move.TypePromotionQueen)
					{
						if (moveType == Move.TypePromotionQueen)
						{
							score += ScorePromotionQueen;
						}
						goodCaptures[goodCaptureIndex] = move;
						goodCapturesSee[goodCaptureIndex] = see;
						goodCapturesScores[goodCaptureIndex] = score;
						goodCaptureIndex++;
					}
					else
					{
						equalCaptures[equalCaptureIndex] = move;
						equalCapturesSee[equalCaptureIndex] = see;
						equalCapturesScores[equalCaptureIndex] = score;
						equalCaptureIndex++;
					}
				}
				else
				{
					badCaptures[badCaptureIndex] = move;
					badCapturesSee[badCaptureIndex] = see;
					badCapturesScores[badCaptureIndex] = see;
					badCaptureIndex++;
				}
			}
		}

		/// <summary>Moves are sorted ascending (best moves at the end)</summary>
		public virtual void GenMoves(int ttMove)
		{
			GenMoves(ttMove, false, true);
		}

		public virtual void GenMoves(int ttMove, bool quiescence, bool generateChecks)
		{
			this.ttMove = ttMove;
			foundKiller1 = false;
			foundKiller2 = false;
			this.quiescence = quiescence;
			this.generateChecks = generateChecks;
			this.checkEvasion = board.GetCheck();
			killer1 = sortInfo.killerMove1[depth];
			killer2 = sortInfo.killerMove2[depth];
			phase = 0;
			lastMoveSee = 0;
			goodCaptureIndex = 0;
			badCaptureIndex = 0;
			equalCaptureIndex = 0;
			nonCaptureIndex = 0;
		}

		public virtual int Next()
		{
			int maxScore;
			int bestIndex;
			switch (phase)
			{
				case PhaseTt:
				{
					phase++;
					if (ttMove != 0)
					{
						if (Move.IsCapture(ttMove))
						{
							lastMoveSee = board.See(ttMove);
						}
						return ttMove;
					}
					goto case PhaseGenCaptures;
				}

				case PhaseGenCaptures:
				{
					phase++;
					GenerateCaptures();
					goto case PhaseGoodCapturesAndPromos;
				}

				case PhaseGoodCapturesAndPromos:
				{
					maxScore = ScoreLowest;
					bestIndex = -1;
					for (int i = 0; i < goodCaptureIndex; i++)
					{
						if (goodCapturesScores[i] > maxScore)
						{
							maxScore = goodCapturesScores[i];
							bestIndex = i;
						}
					}
					if (bestIndex != -1)
					{
						goodCapturesScores[bestIndex] = ScoreLowest;
						lastMoveSee = goodCapturesSee[bestIndex];
						return goodCaptures[bestIndex];
					}
					phase++;
					goto case PhaseEqualCaptures;
				}

				case PhaseEqualCaptures:
				{
					maxScore = ScoreLowest;
					bestIndex = -1;
					for (int i_1 = 0; i_1 < equalCaptureIndex; i_1++)
					{
						if (equalCapturesScores[i_1] > maxScore)
						{
							maxScore = equalCapturesScores[i_1];
							bestIndex = i_1;
						}
					}
					if (bestIndex != -1)
					{
						equalCapturesScores[bestIndex] = ScoreLowest;
						lastMoveSee = equalCapturesSee[bestIndex];
						return equalCaptures[bestIndex];
					}
					phase++;
					goto case PhaseGenNoncaptures;
				}

				case PhaseGenNoncaptures:
				{
					phase++;
					if (quiescence && !generateChecks && !checkEvasion)
					{
						phase = PhaseEnd;
						return 0;
					}
					lastMoveSee = 0;
					GenerateNonCaptures();
					goto case PhaseKiller1;
				}

				case PhaseKiller1:
				{
					phase++;
					if (foundKiller1)
					{
						return killer1;
					}
					goto case PhaseKiller2;
				}

				case PhaseKiller2:
				{
					phase++;
					if (foundKiller2)
					{
						return killer2;
					}
					goto case PhaseNoncaptures;
				}

				case PhaseNoncaptures:
				{
					maxScore = ScoreLowest;
					bestIndex = -1;
					for (int i_2 = 0; i_2 < nonCaptureIndex; i_2++)
					{
						if (nonCapturesScores[i_2] > maxScore)
						{
							maxScore = nonCapturesScores[i_2];
							bestIndex = i_2;
						}
					}
					if (bestIndex != -1)
					{
						nonCapturesScores[bestIndex] = ScoreLowest;
						return nonCaptures[bestIndex];
					}
					phase++;
					goto case PhaseBadCaptures;
				}

				case PhaseBadCaptures:
				{
					maxScore = ScoreLowest;
					bestIndex = -1;
					for (int i_3 = 0; i_3 < badCaptureIndex; i_3++)
					{
						if (badCapturesScores[i_3] > maxScore)
						{
							maxScore = badCapturesScores[i_3];
							bestIndex = i_3;
						}
					}
					if (bestIndex != -1)
					{
						badCapturesScores[bestIndex] = ScoreLowest;
						lastMoveSee = badCapturesSee[bestIndex];
						return badCaptures[bestIndex];
					}
					break;
				}
			}
			return 0;
		}

		public virtual int GetLastMoveSee()
		{
			return lastMoveSee;
		}
	}
}
