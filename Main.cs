using System;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Search;
using Com.Alonsoruibal.Chess.Book;

namespace CarballoChessEngine
{

	class MainClass
	{

		public static void Main (string[] args)
		{
			Config config = new Config();
			config.SetBook(new FileBook("book_small.bin"));
			SearchParameters searchParams = new SearchParameters();
			searchParams.SetMoveTime(30000);
			SearchEngine searchEngine = new SearchEngine(config);
			//searchEngine.GetBoard().SetFen("rq2r1k1/5pp1/p7/4bNP1/1p2P2P/5Q2/PP4K1/5R1R w - -");
			searchEngine.Go(searchParams);
		}
	}
}