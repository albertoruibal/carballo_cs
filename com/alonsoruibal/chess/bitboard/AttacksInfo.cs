using Com.Alonsoruibal.Chess;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Bitboard
{
	/// <summary>
	/// Holds all the possible attacks for a board
	/// It is used by the evaluators and the move iterator
	/// Calculates the checking pieces and the interpose squares to avoid checks
	/// </summary>
	public class AttacksInfo
	{
		internal BitboardAttacks bbAttacks;

		public long boardKey = 0;

		public long[] attacksFromSquare = new long[64];

		public long[] attackedSquares = new long[] { 0, 0 };

		public long piecesGivingCheck;

		public long interposeCheckSquares;

		public int myKingIndex;

		public int otherKingIndex;

		public long bishopAttacksMyking;

		public long rookAttacksMyking;

		public long bishopAttacksOtherking;

		public long rookAttacksOtherking;

		public AttacksInfo()
		{
			//
			// Squares with possible ray attacks to the kings: used to detect check and move legality
			//
			this.bbAttacks = BitboardAttacks.GetInstance();
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
			myKingIndex = BitboardUtils.Square2Index(myKing);
			otherKingIndex = BitboardUtils.Square2Index(board.kings & ~mines);
			bishopAttacksMyking = bbAttacks.GetBishopAttacks(myKingIndex, all);
			rookAttacksMyking = bbAttacks.GetRookAttacks(myKingIndex, all);
			bishopAttacksOtherking = bbAttacks.GetBishopAttacks(otherKingIndex, all);
			rookAttacksOtherking = bbAttacks.GetRookAttacks(otherKingIndex, all);
			attackedSquares[0] = 0;
			attackedSquares[1] = 0;
			piecesGivingCheck = 0;
			interposeCheckSquares = 0;
			long pieceAttacks;
			int index;
			long square = 1;
			for (index = 0; index < 64; index++)
			{
				if ((square & all) != 0)
				{
					bool isWhite = ((board.whites & square) != 0);
					int color = (isWhite ? 0 : 1);
					pieceAttacks = 0;
					if ((square & board.pawns) != 0)
					{
						pieceAttacks = (isWhite ? bbAttacks.pawnUpwards[index] : bbAttacks.pawnDownwards[
							index]);
					}
					else
					{
						if ((square & board.knights) != 0)
						{
							pieceAttacks = bbAttacks.knight[index];
						}
						else
						{
							if ((square & board.kings) != 0)
							{
								pieceAttacks = bbAttacks.king[index];
							}
							else
							{
								// It is a slider
								if ((square & (board.bishops | board.queens)) != 0)
								{
									long sliderAttacks = bbAttacks.GetBishopAttacks(index, all);
									if (((square & mines) == 0) && (sliderAttacks & myKing) != 0)
									{
										interposeCheckSquares |= sliderAttacks & bishopAttacksMyking;
									}
									// And with only the diagonal attacks to the king
									pieceAttacks |= sliderAttacks;
								}
								if ((square & (board.rooks | board.queens)) != 0)
								{
									long sliderAttacks = bbAttacks.GetRookAttacks(index, all);
									if (((square & mines) == 0) && (sliderAttacks & myKing) != 0)
									{
										interposeCheckSquares |= sliderAttacks & rookAttacksMyking;
									}
									// And with only the rook attacks to the king
									pieceAttacks |= sliderAttacks;
								}
							}
						}
					}
					attackedSquares[color] |= pieceAttacks;
					attacksFromSquare[index] = pieceAttacks;
					if (((square & mines) == 0) && (pieceAttacks & myKing) != 0)
					{
						piecesGivingCheck |= square;
					}
				}
				else
				{
					attacksFromSquare[index] = 0;
				}
				square <<= 1;
			}
		}
	}
}
