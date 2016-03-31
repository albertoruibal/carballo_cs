using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movegen
{
	/// <summary>
	/// Magic move generator
	/// Generate pseudo-legal moves because can leave the king in check.
	/// </summary>
	/// <remarks>
	/// Magic move generator
	/// Generate pseudo-legal moves because can leave the king in check.
	/// It does not set the check flag.
	/// </remarks>
	/// <author>Alberto Alonso Ruibal</author>
	public class MagicMoveGenerator : MoveGenerator
	{
		private int[] moves;

		private int moveIndex;

		private long all;

		private long mines;

		private long others;

		internal BitboardAttacks bbAttacks;

		public virtual int GenerateMoves(Board board, int[] moves, int startIndex)
		{
			this.moves = moves;
			bbAttacks = BitboardAttacks.GetInstance();
			moveIndex = startIndex;
			all = board.GetAll();
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
						GenerateMovesFromAttacks(Piece.Rook, index, bbAttacks.GetRookAttacks(index, all) 
							& ~mines);
					}
					else
					{
						if ((square & board.bishops) != 0)
						{
							// Bishop
							GenerateMovesFromAttacks(Piece.Bishop, index, bbAttacks.GetBishopAttacks(index, all
								) & ~mines);
						}
						else
						{
							if ((square & board.queens) != 0)
							{
								// Queen
								GenerateMovesFromAttacks(Piece.Queen, index, (bbAttacks.GetRookAttacks(index, all
									) | bbAttacks.GetBishopAttacks(index, all)) & ~mines);
							}
							else
							{
								if ((square & board.kings) != 0)
								{
									// King
									GenerateMovesFromAttacks(Piece.King, index, bbAttacks.king[index] & ~mines);
								}
								else
								{
									if ((square & board.knights) != 0)
									{
										// Knight
										GenerateMovesFromAttacks(Piece.Knight, index, bbAttacks.knight[index] & ~mines);
									}
									else
									{
										if ((square & board.pawns) != 0)
										{
											// Pawns
											if ((square & board.whites) != 0)
											{
												if (((square << 8) & all) == 0)
												{
													AddMoves(Piece.Pawn, index, index + 8, false, 0);
													// Two squares if it is in he first row	
													if (((square & BitboardUtils.b2_d) != 0) && (((square << 16) & all) == 0))
													{
														AddMoves(Piece.Pawn, index, index + 16, false, 0);
													}
												}
												GeneratePawnCapturesFromAttacks(index, bbAttacks.pawn[Color.W][index], board.GetPassantSquare
													());
											}
											else
											{
												if ((((long)(((ulong)square) >> 8)) & all) == 0)
												{
													AddMoves(Piece.Pawn, index, index - 8, false, 0);
													// Two squares if it is in he first row	
													if (((square & BitboardUtils.b2_u) != 0) && ((((long)(((ulong)square) >> 16)) & all
														) == 0))
													{
														AddMoves(Piece.Pawn, index, index - 16, false, 0);
													}
												}
												GeneratePawnCapturesFromAttacks(index, bbAttacks.pawn[Color.B][index], board.GetPassantSquare
													());
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
			// Castling: disabled when in check or king route attacked
			if (!board.GetCheck())
			{
				if (board.GetTurn() ? board.GetWhiteKingsideCastling() : board.GetBlackKingsideCastling
					())
				{
					long rookOrigin = board.castlingRooks[board.GetTurn() ? 0 : 2];
					long rookDestiny = Board.CastlingRookDestinySquare[board.GetTurn() ? 0 : 2];
					long rookRoute = BitboardUtils.GetHorizontalLine(rookDestiny, rookOrigin) & ~rookOrigin;
					long kingOrigin = board.kings & mines;
					long kingDestiny = Board.CastlingKingDestinySquare[board.GetTurn() ? 0 : 2];
					long kingRoute = BitboardUtils.GetHorizontalLine(kingOrigin, kingDestiny) & ~kingOrigin;
					if ((all & (kingRoute | rookRoute) & ~rookOrigin & ~kingOrigin) == 0 && !bbAttacks
						.AreSquaresAttacked(board, kingRoute, board.GetTurn()))
					{
						//
						AddMoves(Piece.King, BitboardUtils.Square2Index(kingOrigin), BitboardUtils.Square2Index
							(board.chess960 ? rookOrigin : kingDestiny), false, Move.TypeKingsideCastling);
					}
				}
				if (board.GetTurn() ? board.GetWhiteQueensideCastling() : board.GetBlackQueensideCastling
					())
				{
					long rookOrigin = board.castlingRooks[board.GetTurn() ? 1 : 3];
					long rookDestiny = Board.CastlingRookDestinySquare[board.GetTurn() ? 1 : 3];
					long rookRoute = BitboardUtils.GetHorizontalLine(rookOrigin, rookDestiny) & ~rookOrigin;
					long kingOrigin = board.kings & mines;
					long kingDestiny = Board.CastlingKingDestinySquare[board.GetTurn() ? 1 : 3];
					long kingRoute = BitboardUtils.GetHorizontalLine(kingDestiny, kingOrigin) & ~kingOrigin;
					if ((all & (kingRoute | rookRoute) & ~rookOrigin & ~kingOrigin) == 0 && !bbAttacks
						.AreSquaresAttacked(board, kingRoute, board.GetTurn()))
					{
						//
						AddMoves(Piece.King, BitboardUtils.Square2Index(kingOrigin), BitboardUtils.Square2Index
							(board.chess960 ? rookOrigin : kingDestiny), false, Move.TypeQueensideCastling);
					}
				}
			}
			return moveIndex;
		}

		/// <summary>Generates moves from an attack mask</summary>
		private void GenerateMovesFromAttacks(int pieceMoved, int fromIndex, long attacks
			)
		{
			while (attacks != 0)
			{
				long to = BitboardUtils.Lsb(attacks);
				AddMoves(pieceMoved, fromIndex, BitboardUtils.Square2Index(to), ((to & others) !=
					 0), 0);
				attacks ^= to;
			}
		}

		private void GeneratePawnCapturesFromAttacks(int fromIndex, long attacks, long passant
			)
		{
			while (attacks != 0)
			{
				long to = BitboardUtils.Lsb(attacks);
				if ((to & others) != 0)
				{
					AddMoves(Piece.Pawn, fromIndex, BitboardUtils.Square2Index(to), true, 0);
				}
				else
				{
					if ((to & passant) != 0)
					{
						AddMoves(Piece.Pawn, fromIndex, BitboardUtils.Square2Index(to), true, Move.TypePassant
							);
					}
				}
				attacks ^= to;
			}
		}

		/// <summary>Adds a move</summary>
		private void AddMoves(int pieceMoved, int fromIndex, int toIndex, bool capture, int
			 moveType)
		{
			if (pieceMoved == Piece.Pawn && (toIndex < 8 || toIndex >= 56))
			{
				moves[moveIndex++] = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, Move.TypePromotionQueen
					);
				moves[moveIndex++] = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, Move.TypePromotionKnight
					);
				moves[moveIndex++] = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, Move.TypePromotionRook
					);
				moves[moveIndex++] = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, Move.TypePromotionBishop
					);
			}
			else
			{
				moves[moveIndex++] = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, moveType
					);
			}
		}
	}
}
