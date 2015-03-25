using Com.Alonsoruibal.Chess;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movegen
{
	public interface MoveGenerator
	{
		int GenerateMoves(Board board, int[] moves, int index);
	}
}
