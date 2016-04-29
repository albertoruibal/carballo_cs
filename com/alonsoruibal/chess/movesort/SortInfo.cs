using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Search;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Movesort
{
	public class SortInfo
	{
		public const int HistoryMax = int.MaxValue - 1;

		public int[] killerMove1;

		public int[] killerMove2;

		private int[][] history;

		public SortInfo()
		{
			// Two killer move slots
			// By piece type and destiny square
			killerMove1 = new int[SearchEngine.MaxDepth];
			killerMove2 = new int[SearchEngine.MaxDepth];
			history = new int[][] { new int[64], new int[64], new int[64], new int[64], new int
				[64], new int[64] };
		}

		public virtual void Clear()
		{
			Arrays.Fill(killerMove1, 0);
			Arrays.Fill(killerMove2, 0);
			for (int i = 0; i < 6; i++)
			{
				Arrays.Fill(history[i], 0);
			}
		}

		/// <summary>we are informed of the score produced by the move at any level</summary>
		public virtual void BetaCutoff(int move, int depth)
		{
			// removes captures and promotions from killers
			if (move == Move.None || Move.IsTactical(move))
			{
				return;
			}
			if (move != killerMove1[depth])
			{
				killerMove2[depth] = killerMove1[depth];
				killerMove1[depth] = move;
			}
			history[Move.GetPieceMoved(move) - 1][Move.GetToIndex(move)]++;
			// Detect history overflows and divide all values by two
			if (history[Move.GetPieceMoved(move) - 1][Move.GetToIndex(move)] >= HistoryMax)
			{
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 64; j++)
					{
						history[i][j] = (int)(((uint)history[i][j]) >> 1);
					}
				}
			}
		}

		public virtual int GetMoveScore(int move)
		{
			return history[Move.GetPieceMoved(move) - 1][Move.GetToIndex(move)];
		}
	}
}
