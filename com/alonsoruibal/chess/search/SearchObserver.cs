using Sharpen;

namespace Com.Alonsoruibal.Chess.Search
{
	public interface SearchObserver
	{
		void Info(SearchStatusInfo info);

		void BestMove(int bestMove, int ponder);
	}
}
