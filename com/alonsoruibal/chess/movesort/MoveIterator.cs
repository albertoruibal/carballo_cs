using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movesort
{
	/// <summary>The Move Iterator generates moves as needed.</summary>
	/// <remarks>
	/// The Move Iterator generates moves as needed. It is separated into phases.
	/// It sets the check flag on moves. It also checks if the move is legal before generating it.
	/// </remarks>
	public class MoveIterator
	{
		public const int GenerateAll = 0;

		public const int GenerateCapturesPromos = 1;

		public const int GenerateCapturesPromosChecks = 2;

		public const int PhaseTt = 0;

		public const int PhaseGenCaptures = 1;

		public const int PhaseGoodCapturesAndPromos = 2;

		public const int PhaseEqualCaptures = 3;

		public const int PhaseGenNonCaptures = 4;

		public const int PhaseKiller1 = 5;

		public const int PhaseKiller2 = 6;

		public const int PhaseKiller3 = 7;

		public const int PhaseKiller4 = 8;

		public const int PhaseNonCaptures = 9;

		public const int PhaseBadCaptures = 10;

		public const int PhaseEnd = 11;

		private static readonly int[] VictimPieceValues = new int[] { 0, 100, 325, 330, 500
			, 975, 10000 };

		private static readonly int[] AggressorPieceValues = new int[] { 0, 10, 32, 33, 50
			, 97, 99 };

		private const int ScorePromotionQueen = 975;

		private const int ScoreUnderpromotion = int.MinValue + 1;

		private const int ScoreLowest = int.MinValue;

		public const int SeeNotCalculated = short.MaxValue;

		private Board board;

		private AttacksInfo ai;

		private int ttMove;

		private int movesToGenerate;

		private int move;

		private int lastMoveSee;

		private int killer1;

		private int killer2;

		private int killer3;

		private int killer4;

		private bool foundKiller1;

		private bool foundKiller2;

		private bool foundKiller3;

		private bool foundKiller4;

		public bool checkEvasion;

		private int us;

		private int them;

		private bool turn;

		private long all;

		private long mines;

		private long others;

		private int goodCaptureIndex;

		private int equalCaptureIndex;

		private int badCaptureIndex;

		private int nonCaptureIndex;

		private int[] goodCaptures = new int[256];

		private int[] goodCapturesSee = new int[256];

		private int[] goodCapturesScores = new int[256];

		private int[] badCaptures = new int[256];

		private int[] badCapturesScores = new int[256];

		private int[] equalCaptures = new int[256];

		private int[] equalCapturesSee = new int[256];

		private int[] equalCapturesScores = new int[256];

		private int[] nonCaptures = new int[256];

		private int[] nonCapturesSee = new int[256];

		private int[] nonCapturesScores = new int[256];

		private int depth;

		private SortInfo sortInfo;

		private int phase;

		private BitboardAttacks bbAttacks;

		//
		// Kind of moves to generate
		// In check evasions all moves are always generated
		// Generates only good/equal captures and queen promotions
		// Generates only good/equal captures, queen promotions and checks
		//
		// Move generation phases
		//
		// Stores captures and queen promotions
		// Stores captures and queen promotions
		// Stores captures and queen promotions
		// Stores non captures and underpromotions
		public virtual int GetLastMoveSee()
		{
			return lastMoveSee != SeeNotCalculated ? lastMoveSee : board.See(move, ai);
		}

		public virtual void GenMoves(int ttMove)
		{
			GenMoves(ttMove, GenerateAll);
		}

		public virtual void GenMoves(int ttMove, int movesToGenerate)
		{
			this.ttMove = ttMove;
			this.movesToGenerate = movesToGenerate;
			phase = PhaseTt;
			checkEvasion = board.GetCheck();
			lastMoveSee = 0;
		}

		private void InitMoveGen()
		{
			ai.Build(board);
			killer1 = sortInfo.killerMove1[depth];
			killer2 = sortInfo.killerMove2[depth];
			killer3 = depth < 2 ? Move.None : sortInfo.killerMove1[depth - 2];
			killer4 = depth < 2 ? Move.None : sortInfo.killerMove2[depth - 2];
			foundKiller1 = false;
			foundKiller2 = false;
			foundKiller3 = false;
			foundKiller4 = false;
			goodCaptureIndex = 0;
			badCaptureIndex = 0;
			equalCaptureIndex = 0;
			nonCaptureIndex = 0;
			// Only for clarity
			turn = board.GetTurn();
			us = turn ? 0 : 1;
			them = turn ? 1 : 0;
			all = board.GetAll();
			mines = board.GetMines();
			others = board.GetOthers();
		}

		public virtual int Next()
		{
			switch (phase)
			{
				case PhaseTt:
				{
					phase++;
					if (ttMove != Move.None)
					{
						lastMoveSee = Move.IsCapture(ttMove) || Move.IsCheck(ttMove) ? board.See(ttMove) : 
							0;
						if (checkEvasion || movesToGenerate == GenerateAll || Move.GetMoveType(ttMove) ==
							 Move.TypePromotionQueen || (movesToGenerate == GenerateCapturesPromos && Move.IsCapture
							(ttMove) && lastMoveSee >= 0) || (movesToGenerate == GenerateCapturesPromosChecks
							 && (Move.IsCapture(ttMove) || Move.IsCheck(ttMove)) && lastMoveSee >= 0))
						{
							//
							//
							//
							return ttMove;
						}
					}
					goto case PhaseGenCaptures;
				}

				case PhaseGenCaptures:
				{
					InitMoveGen();
					if (checkEvasion)
					{
						GenerateCheckEvasionCaptures();
					}
					else
					{
						GenerateCaptures();
					}
					phase++;
					goto case PhaseGoodCapturesAndPromos;
				}

				case PhaseGoodCapturesAndPromos:
				{
					move = PickMoveFromArray(goodCaptureIndex, goodCaptures, goodCapturesScores, goodCapturesSee
						);
					if (move != Move.None)
					{
						return move;
					}
					phase++;
					goto case PhaseEqualCaptures;
				}

				case PhaseEqualCaptures:
				{
					move = PickMoveFromArray(equalCaptureIndex, equalCaptures, equalCapturesScores, equalCapturesSee
						);
					if (move != Move.None)
					{
						return move;
					}
					phase++;
					goto case PhaseGenNonCaptures;
				}

				case PhaseGenNonCaptures:
				{
					if (checkEvasion)
					{
						GenerateCheckEvasionsNonCaptures();
					}
					else
					{
						if (movesToGenerate == GenerateCapturesPromos)
						{
							phase = PhaseEnd;
							return Move.None;
						}
						GenerateNonCaptures();
					}
					phase++;
					goto case PhaseKiller1;
				}

				case PhaseKiller1:
				{
					lastMoveSee = SeeNotCalculated;
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
					goto case PhaseKiller3;
				}

				case PhaseKiller3:
				{
					phase++;
					if (foundKiller3)
					{
						return killer3;
					}
					goto case PhaseKiller4;
				}

				case PhaseKiller4:
				{
					phase++;
					if (foundKiller4)
					{
						return killer4;
					}
					goto case PhaseNonCaptures;
				}

				case PhaseNonCaptures:
				{
					move = PickMoveFromArray(nonCaptureIndex, nonCaptures, nonCapturesScores, nonCapturesSee
						);
					if (move != Move.None)
					{
						return move;
					}
					phase++;
					goto case PhaseBadCaptures;
				}

				case PhaseBadCaptures:
				{
					move = PickMoveFromArray(badCaptureIndex, badCaptures, badCapturesScores, badCapturesScores
						);
					if (move != Move.None)
					{
						return move;
					}
					phase = PhaseEnd;
					return Move.None;
				}
			}
			return Move.None;
		}

		private int PickMoveFromArray(int arrayLength, int[] arrayMoves, int[] arrayScores
			, int[] arraySee)
		{
			if (arrayLength == 0)
			{
				return Move.None;
			}
			int maxScore = ScoreLowest;
			int bestIndex = -1;
			for (int i = 0; i < arrayLength; i++)
			{
				if (arrayScores[i] > maxScore)
				{
					maxScore = arrayScores[i];
					bestIndex = i;
				}
			}
			if (bestIndex != -1)
			{
				int move = arrayMoves[bestIndex];
				lastMoveSee = arraySee[bestIndex];
				arrayScores[bestIndex] = ScoreLowest;
				return move;
			}
			else
			{
				return Move.None;
			}
		}

		public MoveIterator(Board board, AttacksInfo ai, SortInfo sortInfo, int depth)
		{
			this.board = board;
			this.ai = ai;
			this.sortInfo = sortInfo;
			this.depth = depth;
			bbAttacks = BitboardAttacks.GetInstance();
		}

		public virtual void SetBoard(Board board)
		{
			this.board = board;
		}

		/// <summary>Generates captures and good promos</summary>
		public virtual void GenerateCaptures()
		{
			long square = unchecked((long)(0x1L));
			for (int index = 0; index < 64; index++)
			{
				if ((square & mines) != 0)
				{
					if ((square & board.rooks) != 0)
					{
						// Rook
						GenerateMovesFromAttacks(Piece.Rook, index, square, ai.attacksFromSquare[index] &
							 others, true);
					}
					else
					{
						if ((square & board.bishops) != 0)
						{
							// Bishop
							GenerateMovesFromAttacks(Piece.Bishop, index, square, ai.attacksFromSquare[index]
								 & others, true);
						}
						else
						{
							if ((square & board.queens) != 0)
							{
								// Queen
								GenerateMovesFromAttacks(Piece.Queen, index, square, ai.attacksFromSquare[index] 
									& others, true);
							}
							else
							{
								if ((square & board.kings) != 0)
								{
									// King
									GenerateMovesFromAttacks(Piece.King, index, square, ai.attacksFromSquare[index] &
										 others & ~ai.attackedSquaresAlsoPinned[them], true);
								}
								else
								{
									if ((square & board.knights) != 0)
									{
										// Knight
										GenerateMovesFromAttacks(Piece.Knight, index, square, ai.attacksFromSquare[index]
											 & others, true);
									}
									else
									{
										if ((square & board.pawns) != 0)
										{
											// Pawns
											if (turn)
											{
												GeneratePawnCapturesOrGoodPromos(index, square, (ai.attacksFromSquare[index] & (others
													 | board.GetPassantSquare())) | (((square & BitboardUtils.b2_u) != 0) && (((square
													 << 8) & all) == 0) ? (square << 8) : 0), board.GetPassantSquare());
											}
											else
											{
												//
												//
												// Pushes only if promotion
												GeneratePawnCapturesOrGoodPromos(index, square, (ai.attacksFromSquare[index] & (others
													 | board.GetPassantSquare())) | (((square & BitboardUtils.b2_d) != 0) && ((((long
													)(((ulong)square) >> 8)) & all) == 0) ? ((long)(((ulong)square) >> 8)) : 0), board
													.GetPassantSquare());
											}
										}
									}
								}
							}
						}
					}
				}
				//
				//
				// Pushes only if promotion
				square <<= 1;
			}
		}

		/// <summary>Generates non tactical moves</summary>
		public virtual void GenerateNonCaptures()
		{
			long square = unchecked((long)(0x1L));
			for (int index = 0; index < 64; index++)
			{
				if ((square & mines) != 0)
				{
					if ((square & board.rooks) != 0)
					{
						// Rook
						GenerateMovesFromAttacks(Piece.Rook, index, square, ai.attacksFromSquare[index] &
							 ~all, false);
					}
					else
					{
						if ((square & board.bishops) != 0)
						{
							// Bishop
							GenerateMovesFromAttacks(Piece.Bishop, index, square, ai.attacksFromSquare[index]
								 & ~all, false);
						}
						else
						{
							if ((square & board.queens) != 0)
							{
								// Queen
								GenerateMovesFromAttacks(Piece.Queen, index, square, ai.attacksFromSquare[index] 
									& ~all, false);
							}
							else
							{
								if ((square & board.kings) != 0)
								{
									// King
									GenerateMovesFromAttacks(Piece.King, index, square, ai.attacksFromSquare[index] &
										 ~all & ~ai.attackedSquaresAlsoPinned[them], false);
								}
								else
								{
									if ((square & board.knights) != 0)
									{
										// Knight
										GenerateMovesFromAttacks(Piece.Knight, index, square, ai.attacksFromSquare[index]
											 & ~all, false);
									}
								}
							}
						}
					}
					if ((square & board.pawns) != 0)
					{
						// Pawns excluding the already generated promos
						if (turn)
						{
							GeneratePawnNonCapturesAndBadPromos(index, square, (((square << 8) & all) == 0 ? 
								(square << 8) : 0) | ((square & BitboardUtils.b2_d) != 0 && (((square << 8) | (square
								 << 16)) & all) == 0 ? (square << 16) : 0));
						}
						else
						{
							GeneratePawnNonCapturesAndBadPromos(index, square, ((((long)(((ulong)square) >> 8
								)) & all) == 0 ? ((long)(((ulong)square) >> 8)) : 0) | ((square & BitboardUtils.
								b2_u) != 0 && ((((long)(((ulong)square) >> 8)) | ((long)(((ulong)square) >> 16))
								) & all) == 0 ? ((long)(((ulong)square) >> 16)) : 0));
						}
					}
				}
				square <<= 1;
			}
			// Castling: disabled when in check or king route attacked
			if (!board.GetCheck())
			{
				if (turn ? board.GetWhiteKingsideCastling() : board.GetBlackKingsideCastling())
				{
					long rookOrigin = board.castlingRooks[turn ? 0 : 2];
					long rookDestiny = Board.CastlingRookDestinySquare[turn ? 0 : 2];
					long rookRoute = BitboardUtils.GetHorizontalLine(rookDestiny, rookOrigin) & ~rookOrigin;
					long kingOrigin = board.kings & mines;
					long kingDestiny = Board.CastlingKingDestinySquare[turn ? 0 : 2];
					long kingRoute = BitboardUtils.GetHorizontalLine(kingOrigin, kingDestiny) & ~kingOrigin;
					if ((all & (kingRoute | rookRoute) & ~rookOrigin & ~kingOrigin) == 0 && (ai.attackedSquaresAlsoPinned
						[them] & kingRoute) == 0)
					{
						//
						AddMove(Piece.King, ai.kingIndex[us], kingOrigin, board.chess960 ? rookOrigin : kingDestiny
							, false, Move.TypeKingsideCastling);
					}
				}
				if (turn ? board.GetWhiteQueensideCastling() : board.GetBlackQueensideCastling())
				{
					long rookOrigin = board.castlingRooks[turn ? 1 : 3];
					long rookDestiny = Board.CastlingRookDestinySquare[turn ? 1 : 3];
					long rookRoute = BitboardUtils.GetHorizontalLine(rookOrigin, rookDestiny) & ~rookOrigin;
					long kingOrigin = board.kings & mines;
					long kingDestiny = Board.CastlingKingDestinySquare[turn ? 1 : 3];
					long kingRoute = BitboardUtils.GetHorizontalLine(kingDestiny, kingOrigin) & ~kingOrigin;
					if ((all & (kingRoute | rookRoute) & ~rookOrigin & ~kingOrigin) == 0 && (ai.attackedSquaresAlsoPinned
						[them] & kingRoute) == 0)
					{
						//
						AddMove(Piece.King, ai.kingIndex[us], kingOrigin, board.chess960 ? rookOrigin : kingDestiny
							, false, Move.TypeQueensideCastling);
					}
				}
			}
		}

		public virtual void GenerateCheckEvasionCaptures()
		{
			// King can capture one of the checking pieces if two pieces giving check
			GenerateMovesFromAttacks(Piece.King, ai.kingIndex[us], board.kings & mines, others
				 & ai.attacksFromSquare[ai.kingIndex[us]] & ~ai.attackedSquaresAlsoPinned[them], 
				true);
			if (BitboardUtils.PopCount(ai.piecesGivingCheck) == 1)
			{
				long square = 1;
				for (int index = 0; index < 64; index++)
				{
					if ((square & mines) != 0 && (square & board.kings) == 0)
					{
						if ((square & board.pawns) != 0)
						{
							// Pawns
							long destinySquares = 0;
							// Good promotion interposes to the check
							if ((square & (turn ? BitboardUtils.b2_u : BitboardUtils.b2_d)) != 0)
							{
								// Pawn about to promote
								destinySquares = ai.interposeCheckSquares & (turn ? (((square << 8) & all) == 0 ? 
									(square << 8) : 0) : ((((long)(((ulong)square) >> 8)) & all) == 0 ? ((long)(((ulong
									)square) >> 8)) : 0));
							}
							// Pawn captures the checking piece
							destinySquares |= (ai.attacksFromSquare[index] & ai.piecesGivingCheck);
							if (destinySquares != 0)
							{
								GeneratePawnCapturesOrGoodPromos(index, square, destinySquares, board.GetPassantSquare
									());
							}
							else
							{
								if (board.GetPassantSquare() != 0 && (ai.attacksFromSquare[index] & board.GetPassantSquare
									()) != 0)
								{
									// This pawn can capture to the passant square
									long testPassantSquare = (turn ? ai.piecesGivingCheck << 8 : (long)(((ulong)ai.piecesGivingCheck
										) >> 8));
									if (testPassantSquare == board.GetPassantSquare() || (board.GetPassantSquare() & 
										ai.interposeCheckSquares) != 0)
									{
										// En-passant capture target giving check
										// En passant capture to interpose
										AddMove(Piece.Pawn, index, square, board.GetPassantSquare(), true, Move.TypePassant
											);
									}
								}
							}
						}
						else
						{
							if (((ai.attacksFromSquare[index] & ai.piecesGivingCheck)) != 0)
							{
								if ((square & board.rooks) != 0)
								{
									// Rook
									GenerateMovesFromAttacks(Piece.Rook, index, square, ai.piecesGivingCheck, true);
								}
								else
								{
									if ((square & board.bishops) != 0)
									{
										// Bishop
										GenerateMovesFromAttacks(Piece.Bishop, index, square, ai.piecesGivingCheck, true);
									}
									else
									{
										if ((square & board.queens) != 0)
										{
											// Queen
											GenerateMovesFromAttacks(Piece.Queen, index, square, ai.piecesGivingCheck, true);
										}
										else
										{
											if ((square & board.knights) != 0)
											{
												// Knight
												GenerateMovesFromAttacks(Piece.Knight, index, square, ai.piecesGivingCheck, true);
											}
										}
									}
								}
							}
						}
					}
					square <<= 1;
				}
			}
		}

		public virtual void GenerateCheckEvasionsNonCaptures()
		{
			// Moving king (without captures)
			GenerateMovesFromAttacks(Piece.King, ai.kingIndex[us], board.kings & mines, ai.attacksFromSquare
				[ai.kingIndex[us]] & ~all & ~ai.attackedSquaresAlsoPinned[them], false);
			// Interpose: Cannot interpose with more than one piece giving check
			if (BitboardUtils.PopCount(ai.piecesGivingCheck) == 1)
			{
				long square = 1;
				for (int index = 0; index < 64; index++)
				{
					if ((square & mines) != 0 && (square & board.kings) == 0)
					{
						if ((square & board.pawns) != 0)
						{
							long destinySquares;
							if (turn)
							{
								destinySquares = ai.interposeCheckSquares & ((((square << 8) & all) == 0 ? (square
									 << 8) : 0) | ((square & BitboardUtils.b2_d) != 0 && (((square << 8) | (square <<
									 16)) & all) == 0 ? (square << 16) : 0));
							}
							else
							{
								destinySquares = ai.interposeCheckSquares & (((((long)(((ulong)square) >> 8)) & all
									) == 0 ? ((long)(((ulong)square) >> 8)) : 0) | ((square & BitboardUtils.b2_u) !=
									 0 && ((((long)(((ulong)square) >> 8)) | ((long)(((ulong)square) >> 16))) & all)
									 == 0 ? ((long)(((ulong)square) >> 16)) : 0));
							}
							if (destinySquares != 0)
							{
								GeneratePawnNonCapturesAndBadPromos(index, square, destinySquares);
							}
						}
						else
						{
							long destinySquares = ai.attacksFromSquare[index] & ai.interposeCheckSquares & ~all;
							if (destinySquares != 0)
							{
								if ((square & board.rooks) != 0)
								{
									// Rook
									GenerateMovesFromAttacks(Piece.Rook, index, square, destinySquares, false);
								}
								else
								{
									if ((square & board.bishops) != 0)
									{
										// Bishop
										GenerateMovesFromAttacks(Piece.Bishop, index, square, destinySquares, false);
									}
									else
									{
										if ((square & board.queens) != 0)
										{
											// Queen
											GenerateMovesFromAttacks(Piece.Queen, index, square, destinySquares, false);
										}
										else
										{
											if ((square & board.knights) != 0)
											{
												// Knight
												GenerateMovesFromAttacks(Piece.Knight, index, square, destinySquares, false);
											}
										}
									}
								}
							}
						}
					}
					square <<= 1;
				}
			}
		}

		/// <summary>Generates moves from an attack mask</summary>
		private void GenerateMovesFromAttacks(int pieceMoved, int fromIndex, long from, long
			 attacks, bool capture)
		{
			while (attacks != 0)
			{
				long to = turn ? BitboardUtils.Msb(attacks) : BitboardUtils.Lsb(attacks);
				AddMove(pieceMoved, fromIndex, from, to, capture, 0);
				attacks ^= to;
			}
		}

		private void GeneratePawnCapturesOrGoodPromos(int fromIndex, long from, long attacks
			, long passant)
		{
			if ((ai.pinnedPieces & from) != 0)
			{
				attacks &= ai.pinnedMobility[fromIndex];
			}
			// Be careful with pawn advance moves, the pawn may be pinned
			while (attacks != 0)
			{
				long to = turn ? BitboardUtils.Msb(attacks) : BitboardUtils.Lsb(attacks);
				if ((to & passant) != 0)
				{
					AddMove(Piece.Pawn, fromIndex, from, to, true, Move.TypePassant);
				}
				else
				{
					bool capture = (to & others) != 0;
					if ((to & (BitboardUtils.b_u | BitboardUtils.b_d)) != 0)
					{
						AddMove(Piece.Pawn, fromIndex, from, to, capture, Move.TypePromotionQueen);
						// If it is a capture, we must add the underpromotions
						if (capture)
						{
							AddMove(Piece.Pawn, fromIndex, from, to, true, Move.TypePromotionKnight);
							AddMove(Piece.Pawn, fromIndex, from, to, true, Move.TypePromotionRook);
							AddMove(Piece.Pawn, fromIndex, from, to, true, Move.TypePromotionBishop);
						}
					}
					else
					{
						if (capture)
						{
							AddMove(Piece.Pawn, fromIndex, from, to, true, 0);
						}
					}
				}
				attacks ^= to;
			}
		}

		private void GeneratePawnNonCapturesAndBadPromos(int fromIndex, long from, long attacks
			)
		{
			if ((ai.pinnedPieces & from) != 0)
			{
				attacks &= ai.pinnedMobility[fromIndex];
			}
			// Be careful with pawn advance moves, the pawn may be pinned
			while (attacks != 0)
			{
				long to = turn ? BitboardUtils.Msb(attacks) : BitboardUtils.Lsb(attacks);
				if ((to & (BitboardUtils.b_u | BitboardUtils.b_d)) != 0)
				{
					AddMove(Piece.Pawn, fromIndex, from, to, false, Move.TypePromotionKnight);
					AddMove(Piece.Pawn, fromIndex, from, to, false, Move.TypePromotionRook);
					AddMove(Piece.Pawn, fromIndex, from, to, false, Move.TypePromotionBishop);
				}
				else
				{
					AddMove(Piece.Pawn, fromIndex, from, to, false, 0);
				}
				attacks ^= to;
			}
		}

		private void AddMove(int pieceMoved, int fromIndex, long from, long to, bool capture
			, int moveType)
		{
			int toIndex = BitboardUtils.Square2Index(to);
			//
			// Verify check and legality
			//
			bool check = false;
			int newMyKingIndex;
			long rookSlidersAfterMove;
			long allAfterMove;
			long minesAfterMove;
			long bishopSlidersAfterMove = (board.bishops | board.queens) & ~from & ~to;
			long squaresForDiscovery = from;
			if (moveType == Move.TypeKingsideCastling || moveType == Move.TypeQueensideCastling)
			{
				// {White Kingside, White Queenside, Black Kingside, Black Queenside}
				int j = (turn ? 0 : 2) + (moveType == Move.TypeQueensideCastling ? 1 : 0);
				newMyKingIndex = Board.CastlingKingDestinyIndex[j];
				// Castling has a special "to" in Chess960 where the destiny square is the rook
				long kingTo = Board.CastlingKingDestinySquare[j];
				long rookTo = Board.CastlingRookDestinySquare[j];
				long rookMoveMask = board.castlingRooks[j] ^ rookTo;
				rookSlidersAfterMove = (board.rooks ^ rookMoveMask) | board.queens;
				allAfterMove = ((all ^ rookMoveMask) | kingTo) & ~from;
				minesAfterMove = ((mines ^ rookMoveMask) | kingTo) & ~from;
				// Direct check by rook
				check |= (rookTo & ai.rookAttacksKing[them]) != 0;
			}
			else
			{
				if (pieceMoved == Piece.King)
				{
					newMyKingIndex = toIndex;
				}
				else
				{
					newMyKingIndex = ai.kingIndex[us];
				}
				rookSlidersAfterMove = (board.rooks | board.queens) & ~from & ~to;
				allAfterMove = (all | to) & ~from;
				minesAfterMove = (mines | to) & ~from;
				squaresForDiscovery = from;
				if (moveType == Move.TypePassant)
				{
					squaresForDiscovery |= (turn ? (long)(((ulong)to) >> 8) : to << 8);
					allAfterMove &= ~squaresForDiscovery;
				}
				// Direct checks
				if (pieceMoved == Piece.Knight || moveType == Move.TypePromotionKnight)
				{
					check = (to & bbAttacks.knight[ai.kingIndex[them]]) != 0;
				}
				else
				{
					if (pieceMoved == Piece.Bishop || moveType == Move.TypePromotionBishop)
					{
						check = (to & ai.bishopAttacksKing[them]) != 0;
						bishopSlidersAfterMove |= to;
					}
					else
					{
						if (pieceMoved == Piece.Rook || moveType == Move.TypePromotionRook)
						{
							check = (to & ai.rookAttacksKing[them]) != 0;
							rookSlidersAfterMove |= to;
						}
						else
						{
							if (pieceMoved == Piece.Queen || moveType == Move.TypePromotionQueen)
							{
								check = (to & (ai.bishopAttacksKing[them] | ai.rookAttacksKing[them])) != 0;
								bishopSlidersAfterMove |= to;
								rookSlidersAfterMove |= to;
							}
							else
							{
								if (pieceMoved == Piece.Pawn)
								{
									check = (to & bbAttacks.pawn[them][ai.kingIndex[them]]) != 0;
								}
							}
						}
					}
				}
			}
			if ((squaresForDiscovery & ai.mayPin[them]) != 0 && (moveType == Move.TypePassant
				 || ((ai.piecesGivingCheck & (board.rooks | board.bishops | board.queens)) != 0 
				&& pieceMoved == Piece.King)))
			{
				// Candidates to leave the king in check after moving
				if (((squaresForDiscovery & ai.bishopAttacksKing[us]) != 0) || ((ai.piecesGivingCheck
					 & (board.bishops | board.queens)) != 0 && pieceMoved == Piece.King))
				{
					// Moving the king when the king is in check by a slider
					// Regenerate bishop attacks to my king
					long newBishopAttacks = bbAttacks.GetBishopAttacks(newMyKingIndex, allAfterMove);
					if ((newBishopAttacks & bishopSlidersAfterMove & ~minesAfterMove) != 0)
					{
						return;
					}
				}
				// Illegal move
				if ((squaresForDiscovery & ai.rookAttacksKing[us]) != 0 || ((ai.piecesGivingCheck
					 & (board.rooks | board.queens)) != 0 && pieceMoved == Piece.King))
				{
					// Regenerate rook attacks to my king
					long newRookAttacks = bbAttacks.GetRookAttacks(newMyKingIndex, allAfterMove);
					if ((newRookAttacks & rookSlidersAfterMove & ~minesAfterMove) != 0)
					{
						return;
					}
				}
			}
			// Illegal move
			// After a promotion to queen or rook there are new sliders transversing the origin square, so mayPin is not valid
			if (!check && ((squaresForDiscovery & ai.mayPin[us]) != 0 || moveType == Move.TypePromotionQueen
				 || moveType == Move.TypePromotionRook || moveType == Move.TypePromotionBishop))
			{
				// Discovered checks
				if ((squaresForDiscovery & ai.bishopAttacksKing[them]) != 0)
				{
					// Regenerate bishop attacks to the other king
					long newBishopAttacks = bbAttacks.GetBishopAttacks(ai.kingIndex[them], allAfterMove
						);
					if ((newBishopAttacks & bishopSlidersAfterMove & minesAfterMove) != 0)
					{
						check = true;
					}
				}
				if ((squaresForDiscovery & ai.rookAttacksKing[them]) != 0)
				{
					// Regenerate rook attacks to the other king
					long newRookAttacks = bbAttacks.GetRookAttacks(ai.kingIndex[them], allAfterMove);
					if ((newRookAttacks & rookSlidersAfterMove & minesAfterMove) != 0)
					{
						check = true;
					}
				}
			}
			// Generating checks, if the move is not a check, skip it
			if (movesToGenerate == GenerateCapturesPromosChecks && !checkEvasion && !check &&
				 !capture && moveType != Move.TypePromotionQueen)
			{
				return;
			}
			// Now, with legality verified and the check flag, generate the move
			int move = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, check, moveType);
			if (move == ttMove)
			{
				return;
			}
			if (!capture)
			{
				if (move == killer1)
				{
					foundKiller1 = true;
					return;
				}
				else
				{
					if (move == killer2)
					{
						foundKiller2 = true;
						return;
					}
					else
					{
						if (move == killer3)
						{
							foundKiller3 = true;
							return;
						}
						else
						{
							if (move == killer4)
							{
								foundKiller4 = true;
								return;
							}
						}
					}
				}
			}
			int pieceCaptured = capture ? Move.GetPieceCaptured(board, move) : 0;
			int see = SeeNotCalculated;
			if (capture || (movesToGenerate == GenerateCapturesPromosChecks && check))
			{
				// If there aren't pieces attacking the destiny square
				// and the piece cannot pin an attack to the see square,
				// the see will be the captured piece value
				if ((ai.attackedSquares[them] & to) == 0 && (ai.mayPin[them] & from) == 0)
				{
					see = capture ? Board.SeePieceValues[pieceCaptured] : 0;
				}
				else
				{
					see = board.See(fromIndex, toIndex, pieceMoved, pieceCaptured);
				}
			}
			if (movesToGenerate != GenerateAll && !checkEvasion && see < 0)
			{
				return;
			}
			if (capture && see < 0)
			{
				badCaptures[badCaptureIndex] = move;
				badCapturesScores[badCaptureIndex] = see;
				badCaptureIndex++;
				return;
			}
			bool underPromotion = moveType == Move.TypePromotionKnight || moveType == Move.TypePromotionRook
				 || moveType == Move.TypePromotionBishop;
			if ((capture || moveType == Move.TypePromotionQueen) && !underPromotion)
			{
				// Order GOOD captures by MVV/LVA (Hyatt dixit)
				int score = 0;
				if (capture)
				{
					score = VictimPieceValues[pieceCaptured] - AggressorPieceValues[pieceMoved];
				}
				if (moveType == Move.TypePromotionQueen)
				{
					score += ScorePromotionQueen;
				}
				if (see > 0 || moveType == Move.TypePromotionQueen)
				{
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
				nonCaptures[nonCaptureIndex] = move;
				nonCapturesSee[nonCaptureIndex] = see;
				nonCapturesScores[nonCaptureIndex] = underPromotion ? ScoreUnderpromotion : sortInfo
					.GetMoveScore(move);
				nonCaptureIndex++;
			}
		}
	}
}
