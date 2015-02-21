using Com.Alonsoruibal.Chess;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movegen
{
	/// <author>rui</author>
	public interface MoveGenerator
	{
		int GenerateMoves(Board board, int[] moves, int index);
	}
}
