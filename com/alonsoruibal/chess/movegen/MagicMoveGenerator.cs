using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movegen
{
	/// <summary>
	/// Magic move generator
	/// Pseudo-legal Moves
	/// </summary>
	/// <author>Alberto Alonso Ruibal</author>
	public class MagicMoveGenerator : MoveGenerator
	{
		private int[] moves;

		private int moveIndex;

		private long all;

		private long mines;

		private long others;

		internal BitboardAttacks bbAttacks;

		public virtual int GenerateMoves(Board board, int[] moves, int mIndex)
		{
			this.moves = moves;
			bbAttacks = BitboardAttacks.GetInstance();
			moveIndex = mIndex;
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
						GenerateMovesFromAttacks(Move.Rook, index, bbAttacks.GetRookAttacks(index, all));
					}
					else
					{
						if ((square & board.bishops) != 0)
						{
							// Bishop
							GenerateMovesFromAttacks(Move.Bishop, index, bbAttacks.GetBishopAttacks(index, all
								));
						}
						else
						{
							if ((square & board.queens) != 0)
							{
								// Queen
								GenerateMovesFromAttacks(Move.Queen, index, bbAttacks.GetRookAttacks(index, all));
								GenerateMovesFromAttacks(Move.Queen, index, bbAttacks.GetBishopAttacks(index, all
									));
							}
							else
							{
								if ((square & board.kings) != 0)
								{
									// King
									GenerateMovesFromAttacks(Move.King, index, bbAttacks.king[index]);
								}
								else
								{
									if ((square & board.knights) != 0)
									{
										// Knight
										GenerateMovesFromAttacks(Move.Knight, index, bbAttacks.knight[index]);
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
													AddMoves(Move.Pawn, index, index + 8, (square << 8), false, true, 0);
													// Two squares if it is in he first row	
													if (((square & BitboardUtils.b2_d) != 0) && (((square << 16) & all) == 0))
													{
														AddMoves(Move.Pawn, index, index + 16, (square << 16), false, false, 0);
													}
												}
												GeneratePawnCapturesFromAttacks(index, bbAttacks.pawnUpwards[index], board.GetPassantSquare
													());
											}
											else
											{
												if ((((long)(((ulong)square) >> 8)) & all) == 0)
												{
													AddMoves(Move.Pawn, index, index - 8, ((long)(((ulong)square) >> 8)), false, true
														, 0);
													// Two squares if it is in he first row	
													if (((square & BitboardUtils.b2_u) != 0) && ((((long)(((ulong)square) >> 16)) & all
														) == 0))
													{
														AddMoves(Move.Pawn, index, index - 16, ((long)(((ulong)square) >> 16)), false, false
															, 0);
													}
												}
												GeneratePawnCapturesFromAttacks(index, bbAttacks.pawnDownwards[index], board.GetPassantSquare
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
					AddMoves(Move.King, myKingIndex, myKingIndex - 2, 0, false, false, Move.TypeKingsideCastling
						);
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
					AddMoves(Move.King, myKingIndex, myKingIndex + 2, 0, false, false, Move.TypeQueensideCastling
						);
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
				// If we collide with other piece (or other piece and cannot capture), this is blocking
				if ((to & mines) == 0)
				{
					// Capturing
					AddMoves(pieceMoved, fromIndex, BitboardUtils.Square2Index(to), to, ((to & others
						) != 0), true, 0);
				}
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
					AddMoves(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), to, true, true, 0);
				}
				else
				{
					if ((to & passant) != 0)
					{
						AddMoves(Move.Pawn, fromIndex, BitboardUtils.Square2Index(to), to, true, true, Move
							.TypePassant);
					}
				}
				attacks ^= to;
			}
		}

		/// <summary>
		/// Adds an operation
		/// to onlyneeded for captures
		/// </summary>
		private void AddMoves(int pieceMoved, int fromIndex, int toIndex, long to, bool capture
			, bool checkPromotion, int moveType)
		{
			if (checkPromotion && (pieceMoved == Move.Pawn) && ((to & (BitboardUtils.b_u | BitboardUtils
				.b_d)) != 0))
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
