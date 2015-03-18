using Com.Alonsoruibal.Chess;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movegen
{
	public class LegalMoveGenerator : MagicMoveGenerator
	{
		/// <summary>
		/// Get only LEGAL moves testing with doMove
		/// The moves are returned with the check flag set
		/// </summary>
		public override int GenerateMoves(Board board, int[] moves, int index)
		{
			int lastIndex = base.GenerateMoves(board, moves, index);
			int j = index;
			for (int i = 0; i < lastIndex; i++)
			{
				if (board.DoMove(moves[i], true, false))
				{
					moves[j++] = board.GetCheck() ? moves[i] | Move.CheckMask : moves[i];
					board.UndoMove();
				}
			}
			return j;
		}
	}
}
