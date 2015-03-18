using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	public class EndgameEvaluator
	{
		public static readonly int[] closerSquares = new int[] { 0, 0, 100, 80, 60, 40, 20
			, 10 };

		private static readonly int[] toCorners = new int[] { 100, 90, 80, 70, 70, 80, 90
			, 100, 90, 70, 60, 50, 50, 60, 70, 90, 80, 60, 40, 30, 30, 40, 60, 80, 70, 50, 30
			, 20, 20, 30, 50, 70, 70, 50, 30, 20, 20, 30, 50, 70, 80, 60, 40, 30, 30, 40, 60
			, 80, 90, 70, 60, 50, 50, 60, 70, 90, 100, 90, 80, 70, 70, 80, 90, 100 };

		private static readonly int[] toColorCorners = new int[] { 200, 190, 180, 170, 160
			, 150, 140, 130, 190, 180, 170, 160, 150, 140, 130, 140, 180, 170, 155, 140, 140
			, 125, 140, 150, 170, 160, 140, 120, 110, 140, 150, 160, 160, 150, 140, 110, 120
			, 140, 160, 170, 150, 140, 125, 140, 140, 155, 170, 180, 140, 130, 140, 150, 160
			, 170, 180, 190, 130, 140, 150, 160, 170, 180, 190, 200 };

		internal static KPKBitbase kpkBitbase;

		static EndgameEvaluator()
		{
			//
			//
			//
			//
			//
			//
			//
			//
			//
			kpkBitbase = new KPKBitbase();
		}

		public static int EndGameValue(Board board, int whitePawns, int blackPawns, int whiteKnights
			, int blackKnights, int whiteBishops, int blackBishops, int whiteRooks, int blackRooks
			, int whiteQueens, int blackQueens)
		{
			// Endgame detection
			int whiteNoPawnMaterial = whiteKnights + whiteBishops + whiteRooks + whiteQueens;
			int blackNoPawnMaterial = blackKnights + blackBishops + blackRooks + blackQueens;
			int whiteMaterial = whiteNoPawnMaterial + whitePawns;
			int blackMaterial = blackNoPawnMaterial + blackPawns;
			// Endgames without pawns
			if (whitePawns == 0 && blackPawns == 0)
			{
				if ((blackMaterial == 0 && whiteMaterial == 2 && whiteKnights == 2) || (whiteMaterial
					 == 0 && blackMaterial == 2 && blackKnights == 2))
				{
					//
					return Evaluator.Draw;
				}
				// KNNk is draw
				if ((blackMaterial == 0 && whiteMaterial == 2 && whiteKnights == 1 && whiteBishops
					 == 1) || (whiteMaterial == 0 && blackMaterial == 2 && blackKnights == 1 && blackBishops
					 == 1))
				{
					//
					return EndgameEvaluator.EndgameKBNK(board, whiteMaterial > blackMaterial);
				}
				if (whiteMaterial == 1 && blackMaterial == 1 && whiteRooks == 1 && blackRooks == 
					1)
				{
					return EndgameEvaluator.EndgameKRKR(board);
				}
			}
			// Not always a draw
			if ((blackMaterial == 0 && whiteNoPawnMaterial == 0 && whitePawns == 1) || (whiteMaterial
				 == 0 && blackNoPawnMaterial == 0 && blackPawns == 1))
			{
				//
				return EndgameEvaluator.EndgameKPK(board, whiteMaterial > blackMaterial);
			}
			if (blackMaterial == 0 && (whiteBishops >= 2 || whiteRooks > 0 || whiteQueens > 0
				) || whiteMaterial == 0 && (whiteBishops >= 2 || blackRooks > 0 || blackQueens >
				 0))
			{
				//
				return EndgameEvaluator.EndgameKXK(board, whiteMaterial > blackMaterial, whiteKnights
					 + blackKnights, whiteBishops + blackBishops, whiteRooks + blackRooks, whiteQueens
					 + blackQueens);
			}
			return Evaluator.NoValue;
		}

		// One side does not have pieces, drives the king to the corners and try to approximate the kings
		private static int EndgameKXK(Board board, bool whiteDominant, int knights, int bishops
			, int rooks, int queens)
		{
			int whiteKingIndex = BitboardUtils.Square2Index(board.kings & board.whites);
			int blackKingIndex = BitboardUtils.Square2Index(board.kings & board.blacks);
			int value = Evaluator.KnownWin + knights * ExperimentalEvaluator.Knight + bishops
				 * ExperimentalEvaluator.Bishop + rooks * ExperimentalEvaluator.Rook + queens * 
				ExperimentalEvaluator.Queen + closerSquares[BitboardUtils.Distance(whiteKingIndex
				, blackKingIndex)] + (whiteDominant ? toCorners[blackKingIndex] : toCorners[whiteKingIndex
				]);
			//
			return (whiteDominant ? value : -value);
		}

		// NB vs K must drive the king to the corner of the color of the bishop
		private static int EndgameKBNK(Board board, bool whiteDominant)
		{
			int whiteKingIndex = BitboardUtils.Square2Index(board.kings & board.whites);
			int blackKingIndex = BitboardUtils.Square2Index(board.kings & board.blacks);
			if (BitboardUtils.IsBlack(board.bishops))
			{
				whiteKingIndex = BitboardUtils.FlipHorizontalIndex(whiteKingIndex);
				blackKingIndex = BitboardUtils.FlipHorizontalIndex(blackKingIndex);
			}
			int value = Evaluator.KnownWin + closerSquares[BitboardUtils.Distance(whiteKingIndex
				, blackKingIndex)] + (whiteDominant ? toColorCorners[blackKingIndex] : toColorCorners
				[whiteKingIndex]);
			//
			return (whiteDominant ? value : -value);
		}

		private static int EndgameKPK(Board board, bool whiteDominant)
		{
			if (!kpkBitbase.Probe(board))
			{
				return Evaluator.Draw;
			}
			return whiteDominant ? Evaluator.KnownWin + ExperimentalEvaluator.Pawn + BitboardUtils
				.GetRankOfIndex(BitboardUtils.Square2Index(board.pawns)) : -Evaluator.KnownWin -
				 ExperimentalEvaluator.Pawn - (7 - BitboardUtils.GetRankOfIndex(BitboardUtils.Square2Index
				(board.pawns)));
		}

		//
		private static int EndgameKRKR(Board board)
		{
			int myKingIndex = BitboardUtils.Square2Index(board.kings & board.GetMines());
			int myRookIndex = BitboardUtils.Square2Index(board.rooks & board.GetMines());
			int otherKingIndex = BitboardUtils.Square2Index(board.kings & board.GetOthers());
			int otherRookIndex = BitboardUtils.Square2Index(board.rooks & board.GetOthers());
			// The other king is too far, or my king is near the other rook, so my rook can capture the other rook
			if ((BitboardUtils.Distance(otherKingIndex, otherRookIndex) > 1 || BitboardUtils.
				Distance(myKingIndex, otherRookIndex) == 1) && (BitboardAttacks.GetInstance().GetRookAttacks
				(myRookIndex, board.GetAll()) & board.rooks) != 0)
			{
				return Evaluator.KnownWin;
			}
			// The other rook is undefended and my king can capture it
			if (BitboardUtils.Distance(otherKingIndex, otherRookIndex) > 1 && BitboardUtils.Distance
				(myKingIndex, otherRookIndex) == 1)
			{
				// Does the other king capture my rook just after my move?
				if (BitboardUtils.Distance(otherKingIndex, myRookIndex) == 1 && BitboardUtils.Distance
					(otherRookIndex, myRookIndex) > 1)
				{
					/*that's my king after after capture*/
					return Evaluator.Draw;
				}
				return Evaluator.KnownWin;
			}
			return Evaluator.Draw;
		}
	}
}
