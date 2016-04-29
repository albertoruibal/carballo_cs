using Sharpen;

namespace Com.Alonsoruibal.Chess
{
	/// <summary>Holds configuration parameters</summary>
	/// <author>rui</author>
	public class Config
	{
		public const int DefaultTranspositionTableSize = 64;

		public const bool DefaultPonder = true;

		public const bool DefaultUseBook = true;

		public const int DefaultBookKnowgledge = 100;

		public const string DefaultEvaluator = "experimental";

		public const int DefaultRand = 0;

		public const int DefaultElo = 2100;

		public const bool DefaultUciChess960 = false;

		private int transpositionTableSize = DefaultTranspositionTableSize;

		private bool ponder = DefaultPonder;

		private bool useBook = DefaultUseBook;

		private Com.Alonsoruibal.Chess.Book.Book book;

		private int bookKnowledge = DefaultBookKnowgledge;

		private string evaluator = DefaultEvaluator;

		private int rand = DefaultRand;

		private bool uciChess960 = DefaultUciChess960;

		// Default values are static fields used also from UCIEngine
		public virtual bool GetPonder()
		{
			return ponder;
		}

		public virtual void SetPonder(bool ponder)
		{
			this.ponder = ponder;
		}

		public virtual bool GetUseBook()
		{
			return useBook;
		}

		public virtual void SetUseBook(bool useBook)
		{
			this.useBook = useBook;
		}

		public virtual Com.Alonsoruibal.Chess.Book.Book GetBook()
		{
			return book;
		}

		public virtual void SetBook(Com.Alonsoruibal.Chess.Book.Book book)
		{
			this.book = book;
		}

		public virtual int GetBookKnowledge()
		{
			return bookKnowledge;
		}

		public virtual void SetBookKnowledge(int bookKnowledge)
		{
			this.bookKnowledge = bookKnowledge;
		}

		public virtual string GetEvaluator()
		{
			return evaluator;
		}

		public virtual void SetEvaluator(string evaluator)
		{
			this.evaluator = evaluator;
		}

		public virtual int GetTranspositionTableSize()
		{
			return transpositionTableSize;
		}

		public virtual void SetTranspositionTableSize(int transpositionTableSize)
		{
			this.transpositionTableSize = transpositionTableSize;
		}

		public virtual int GetRand()
		{
			return rand;
		}

		public virtual void SetRand(int rand)
		{
			this.rand = rand;
		}

		public virtual bool IsUciChess960()
		{
			return uciChess960;
		}

		public virtual void SetUciChess960(bool uciChess960)
		{
			this.uciChess960 = uciChess960;
		}

		/// <summary>2100 is the max, 500 the min</summary>
		/// <param name="engineElo"/>
		public virtual void SetElo(int engineElo)
		{
			int errorsPerMil = 900 - ((engineElo - 500) * 900) / 1600;
			SetRand(errorsPerMil);
			int kPercentage = ((engineElo - 500) * 100) / 1600;
			// knowledge percentage
			SetBookKnowledge(kPercentage);
		}
	}
}
