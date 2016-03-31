using Com.Alonsoruibal.Chess;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Bitboard
{
	/// <summary>Holds all the possible attacks for a board.</summary>
	/// <remarks>
	/// Holds all the possible attacks for a board.
	/// It is used by the evaluators and the move iterator, and also to speed the SEE calculations detecting not attacked squares.
	/// Calculates the checking pieces and the interpose squares to avoid checks.
	/// </remarks>
	public class AttacksInfo
	{
		public const int W = 0;

		public const int B = 1;

		internal BitboardAttacks bbAttacks;

		public long boardKey = 0;

		public long[] attackedSquaresAlsoPinned = new long[] { 0, 0 };

		public long[] attackedSquares = new long[] { 0, 0 };

		public long[] attacksFromSquare = new long[64];

		public long[] pawnAttacks = new long[] { 0, 0 };

		public long[] knightAttacks = new long[] { 0, 0 };

		public long[] bishopAttacks = new long[] { 0, 0 };

		public long[] rookAttacks = new long[] { 0, 0 };

		public long[] queenAttacks = new long[] { 0, 0 };

		public long[] kingAttacks = new long[] { 0, 0 };

		public int[] kingIndex = new int[] { 0, 0 };

		public long[] pinnedMobility = new long[64];

		public long[] bishopAttacksKing = new long[] { 0, 0 };

		public long[] rookAttacksKing = new long[] { 0, 0 };

		public long[] mayPin = new long[] { 0, 0 };

		public long piecesGivingCheck;

		public long interposeCheckSquares;

		public long pinnedPieces;

		public AttacksInfo()
		{
			// Includes attacks by pinned pieces that cannot move to the square, but limit king mobility
			// The other attacks do not include those from pinned pieces
			//
			// Squares with possible ray attacks to the kings: used to detect check and move legality
			//
			// both my pieces than can discover an attack and the opponent pieces pinned, that is any piece attacked by a slider
			this.bbAttacks = BitboardAttacks.GetInstance();
		}

		/// <summary>Checks for a pinned piece in each ray</summary>
		private void CheckPinnerRay(long ray, long mines, long attackerSlider)
		{
			long pinner = ray & attackerSlider;
			if (pinner != 0)
			{
				long pinned = ray & mines;
				pinnedPieces |= pinned;
				pinnedMobility[BitboardUtils.Square2Index(pinned)] = ray;
			}
		}

		private void CheckPinnerBishop(int kingIndex, long bishopSliderAttacks, long all, 
			long mines, long otherBishopsOrQueens)
		{
			if ((bishopSliderAttacks & mines) == 0 || (bbAttacks.bishop[kingIndex] & otherBishopsOrQueens
				) == 0)
			{
				return;
			}
			long xray = bbAttacks.GetBishopAttacks(kingIndex, all & ~(mines & bishopSliderAttacks
				));
			if ((xray & ~bishopSliderAttacks & otherBishopsOrQueens) != 0)
			{
				int rank = kingIndex >> 3;
				int file = 7 - kingIndex & 7;
				CheckPinnerRay(xray & BitboardUtils.RanksUpwards[rank] & BitboardUtils.FilesLeft[
					file], mines, otherBishopsOrQueens);
				CheckPinnerRay(xray & BitboardUtils.RanksUpwards[rank] & BitboardUtils.FilesRight
					[file], mines, otherBishopsOrQueens);
				CheckPinnerRay(xray & BitboardUtils.RanksDownwards[rank] & BitboardUtils.FilesLeft
					[file], mines, otherBishopsOrQueens);
				CheckPinnerRay(xray & BitboardUtils.RanksDownwards[rank] & BitboardUtils.FilesRight
					[file], mines, otherBishopsOrQueens);
			}
		}

		private void CheckPinnerRook(int kingIndex, long rookSliderAttacks, long all, long
			 mines, long otherRooksOrQueens)
		{
			if ((rookSliderAttacks & mines) == 0 || (bbAttacks.rook[kingIndex] & otherRooksOrQueens
				) == 0)
			{
				return;
			}
			long xray = bbAttacks.GetRookAttacks(kingIndex, all & ~(mines & rookSliderAttacks
				));
			if ((xray & ~rookSliderAttacks & otherRooksOrQueens) != 0)
			{
				int rank = kingIndex >> 3;
				int file = 7 - kingIndex & 7;
				CheckPinnerRay(xray & BitboardUtils.RanksUpwards[rank], mines, otherRooksOrQueens
					);
				CheckPinnerRay(xray & BitboardUtils.FilesLeft[file], mines, otherRooksOrQueens);
				CheckPinnerRay(xray & BitboardUtils.RanksDownwards[rank], mines, otherRooksOrQueens
					);
				CheckPinnerRay(xray & BitboardUtils.FilesRight[file], mines, otherRooksOrQueens);
			}
		}

		/// <summary>If we already hold the attacks for this board, do nothing</summary>
		public virtual void Build(Board board)
		{
			if (boardKey == board.GetKey())
			{
				return;
			}
			boardKey = board.GetKey();
			long all = board.GetAll();
			long mines = board.GetMines();
			long myKing = board.kings & mines;
			int us = board.GetTurn() ? 0 : 1;
			attackedSquaresAlsoPinned[W] = 0;
			attackedSquaresAlsoPinned[B] = 0;
			pawnAttacks[W] = 0;
			pawnAttacks[B] = 0;
			knightAttacks[W] = 0;
			knightAttacks[B] = 0;
			bishopAttacks[W] = 0;
			bishopAttacks[B] = 0;
			rookAttacks[W] = 0;
			rookAttacks[B] = 0;
			queenAttacks[W] = 0;
			queenAttacks[B] = 0;
			kingAttacks[W] = 0;
			kingAttacks[B] = 0;
			mayPin[W] = 0;
			mayPin[B] = 0;
			pinnedPieces = 0;
			piecesGivingCheck = 0;
			interposeCheckSquares = 0;
			kingIndex[W] = BitboardUtils.Square2Index(board.kings & board.whites);
			kingIndex[B] = BitboardUtils.Square2Index(board.kings & board.blacks);
			bishopAttacksKing[W] = bbAttacks.GetBishopAttacks(kingIndex[W], all);
			CheckPinnerBishop(kingIndex[W], bishopAttacksKing[W], all, board.whites, (board.bishops
				 | board.queens) & board.blacks);
			bishopAttacksKing[B] = bbAttacks.GetBishopAttacks(kingIndex[B], all);
			CheckPinnerBishop(kingIndex[B], bishopAttacksKing[B], all, board.blacks, (board.bishops
				 | board.queens) & board.whites);
			rookAttacksKing[W] = bbAttacks.GetRookAttacks(kingIndex[W], all);
			CheckPinnerRook(kingIndex[W], rookAttacksKing[W], all, board.whites, (board.rooks
				 | board.queens) & board.blacks);
			rookAttacksKing[B] = bbAttacks.GetRookAttacks(kingIndex[B], all);
			CheckPinnerRook(kingIndex[B], rookAttacksKing[B], all, board.blacks, (board.rooks
				 | board.queens) & board.whites);
			long pieceAttacks;
			int index;
			long square = 1;
			for (index = 0; index < 64; index++)
			{
				if ((square & all) != 0)
				{
					int color = (board.whites & square) != 0 ? W : B;
					long pinnedSquares = (square & pinnedPieces) != 0 ? pinnedMobility[index] : Square
						.All;
					pieceAttacks = 0;
					if ((square & board.pawns) != 0)
					{
						pieceAttacks = bbAttacks.pawn[color][index];
						if ((square & mines) == 0 && (pieceAttacks & myKing) != 0)
						{
							piecesGivingCheck |= square;
						}
						pawnAttacks[color] |= pieceAttacks & pinnedSquares;
					}
					else
					{
						if ((square & board.knights) != 0)
						{
							pieceAttacks = bbAttacks.knight[index];
							if ((square & mines) == 0 && (pieceAttacks & myKing) != 0)
							{
								piecesGivingCheck |= square;
							}
							knightAttacks[color] |= pieceAttacks & pinnedSquares;
						}
						else
						{
							if ((square & board.bishops) != 0)
							{
								pieceAttacks = bbAttacks.GetBishopAttacks(index, all);
								if ((square & mines) == 0 && (pieceAttacks & myKing) != 0)
								{
									piecesGivingCheck |= square;
									interposeCheckSquares |= pieceAttacks & bishopAttacksKing[us];
								}
								// And with only the diagonal attacks to the king
								bishopAttacks[color] |= pieceAttacks & pinnedSquares;
								mayPin[color] |= all & pieceAttacks;
							}
							else
							{
								if ((square & board.rooks) != 0)
								{
									pieceAttacks = bbAttacks.GetRookAttacks(index, all);
									if ((square & mines) == 0 && (pieceAttacks & myKing) != 0)
									{
										piecesGivingCheck |= square;
										interposeCheckSquares |= pieceAttacks & rookAttacksKing[us];
									}
									// And with only the rook attacks to the king
									rookAttacks[color] |= pieceAttacks & pinnedSquares;
									mayPin[color] |= all & pieceAttacks;
								}
								else
								{
									if ((square & board.queens) != 0)
									{
										long bishopSliderAttacks = bbAttacks.GetBishopAttacks(index, all);
										if ((square & mines) == 0 && (bishopSliderAttacks & myKing) != 0)
										{
											piecesGivingCheck |= square;
											interposeCheckSquares |= bishopSliderAttacks & bishopAttacksKing[us];
										}
										// And with only the diagonal attacks to the king
										long rookSliderAttacks = bbAttacks.GetRookAttacks(index, all);
										if ((square & mines) == 0 && (rookSliderAttacks & myKing) != 0)
										{
											piecesGivingCheck |= square;
											interposeCheckSquares |= rookSliderAttacks & rookAttacksKing[us];
										}
										// And with only the rook attacks to the king
										pieceAttacks = rookSliderAttacks | bishopSliderAttacks;
										queenAttacks[color] |= pieceAttacks & pinnedSquares;
										mayPin[color] |= all & pieceAttacks;
									}
									else
									{
										if ((square & board.kings) != 0)
										{
											pieceAttacks = bbAttacks.king[index];
											kingAttacks[color] |= pieceAttacks;
										}
									}
								}
							}
						}
					}
					attackedSquaresAlsoPinned[color] |= pieceAttacks;
					attacksFromSquare[index] = pieceAttacks & pinnedSquares;
				}
				else
				{
					attacksFromSquare[index] = 0;
				}
				square <<= 1;
			}
			attackedSquares[W] = pawnAttacks[W] | knightAttacks[W] | bishopAttacks[W] | rookAttacks
				[W] | queenAttacks[W] | kingAttacks[W];
			attackedSquares[B] = pawnAttacks[B] | knightAttacks[B] | bishopAttacks[B] | rookAttacks
				[B] | queenAttacks[B] | kingAttacks[B];
		}
	}
}
