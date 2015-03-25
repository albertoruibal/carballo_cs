using Sharpen;

namespace Com.Alonsoruibal.Chess
{
	/// <summary>Holds configuration parameters</summary>
	/// <author>rui</author>
	public class Config
	{
		public const bool DefaultUseBook = true;

		public const bool DefaultPonder = true;

		public const int DefaultBookKnowgledge = 100;

		public const string DefaultEvaluator = "experimental";

		public const bool DefaultNullMove = true;

		public const bool DefaultStaticNullMove = true;

		public const bool DefaultIid = true;

		public const int DefaultIidMargin = 300;

		public const bool DefaultLmr = true;

		public const int DefaultExtensionsCheck = 2;

		public const int DefaultExtensionsMateThreat = 2;

		public const int DefaultExtensionsPawnPush = 0;

		public const int DefaultExtensionsPassedPawn = 0;

		public const int DefaultExtensionsSingular = 2;

		public const int DefaultSingularExtensionMargin = 50;

		public const bool DefaultAspirationWindow = true;

		public const string DefaultAspirationWindowSizes = "10,25,150,400,550,1025";

		public const int DefaultTranspositionTableSize = 64;

		public const bool DefaultFutility = true;

		public const int DefaultFutilityMarginQs = 80;

		public const int DefaultFutilityMargin = 100;

		public const int DefaultFutilityMarginAggressive = 200;

		public const bool DefaultRazoring = true;

		public const int DefaultRazoringMargin = 400;

		public const int DefaultContemptFactor = 90;

		public const int DefaultEvalCenter = 100;

		public const int DefaultEvalPositional = 100;

		public const int DefaultEvalAttacks = 100;

		public const int DefaultEvalMobility = 100;

		public const int DefaultEvalPawnStructure = 100;

		public const int DefaultEvalPassedPawns = 100;

		public const int DefaultEvalKingSafety = 100;

		public const int DefaultRand = 0;

		public const bool DefaultUciChess960 = false;

		private bool ponder = DefaultPonder;

		private bool useBook = DefaultUseBook;

		private Com.Alonsoruibal.Chess.Book.Book book;

		private int bookKnowledge = DefaultBookKnowgledge;

		private string evaluator = DefaultEvaluator;

		private bool nullMove = DefaultNullMove;

		private bool staticNullMove = DefaultStaticNullMove;

		private bool iid = DefaultIid;

		private int iidMargin = DefaultIidMargin;

		private bool lmr = DefaultLmr;

		private int extensionsCheck = DefaultExtensionsCheck;

		private int extensionsMateThreat = DefaultExtensionsMateThreat;

		private int extensionsPawnPush = DefaultExtensionsPawnPush;

		private int extensionsPassedPawn = DefaultExtensionsPassedPawn;

		private int extensionsSingular = DefaultExtensionsSingular;

		private int singularExtensionMargin = DefaultSingularExtensionMargin;

		private bool aspirationWindow = DefaultAspirationWindow;

		private int[] aspirationWindowSizes;

		private int transpositionTableSize = DefaultTranspositionTableSize;

		private bool futility = DefaultFutility;

		private int futilityMarginQS = DefaultFutilityMarginQs;

		private int futilityMargin = DefaultFutilityMargin;

		private int futilityMarginAggressive = DefaultFutilityMarginAggressive;

		private bool razoring = DefaultRazoring;

		private int razoringMargin = DefaultRazoringMargin;

		private int contemptFactor = DefaultContemptFactor;

		private int evalCenter = DefaultEvalCenter;

		private int evalPositional = DefaultEvalPositional;

		private int evalAttacks = DefaultEvalAttacks;

		private int evalMobility = DefaultEvalMobility;

		private int evalPawnStructure = DefaultEvalPawnStructure;

		private int evalPassedPawns = DefaultEvalPassedPawns;

		private int evalKingSafety = DefaultEvalKingSafety;

		private int rand = DefaultRand;

		private bool uciChess960 = DefaultUciChess960;

		public Config()
		{
			// Default values are static fields used also from UCIEngine
			// 2 = 1 PLY
			// >0 refuses draw <0 looks for draw
			// It is initialized in the constructor
			SetAspirationWindowSizes(DefaultAspirationWindowSizes);
		}

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

		public virtual bool GetNullMove()
		{
			return nullMove;
		}

		public virtual void SetNullMove(bool nullMove)
		{
			this.nullMove = nullMove;
		}

		public virtual bool GetStaticNullMove()
		{
			return staticNullMove;
		}

		public virtual void SetStaticNullMove(bool staticNullMove)
		{
			this.staticNullMove = staticNullMove;
		}

		public virtual bool GetIid()
		{
			return iid;
		}

		public virtual void SetIid(bool iid)
		{
			this.iid = iid;
		}

		public virtual int GetIidMargin()
		{
			return iidMargin;
		}

		public virtual void SetIidMargin(int iidMargin)
		{
			this.iidMargin = iidMargin;
		}

		public virtual bool GetLmr()
		{
			return lmr;
		}

		public virtual void SetLmr(bool lmr)
		{
			this.lmr = lmr;
		}

		public virtual bool GetFutility()
		{
			return futility;
		}

		public virtual void SetFutility(bool futility)
		{
			this.futility = futility;
		}

		public virtual int GetFutilityMarginQS()
		{
			return futilityMarginQS;
		}

		public virtual void SetFutilityMarginQS(int futilityMarginQS)
		{
			this.futilityMarginQS = futilityMarginQS;
		}

		public virtual int GetFutilityMargin()
		{
			return futilityMargin;
		}

		public virtual void SetFutilityMargin(int futilityMargin)
		{
			this.futilityMargin = futilityMargin;
		}

		public virtual int GetFutilityMarginAggressive()
		{
			return futilityMarginAggressive;
		}

		public virtual void SetFutilityMarginAggressive(int futilityMarginAggressive)
		{
			this.futilityMarginAggressive = futilityMarginAggressive;
		}

		public virtual bool GetAspirationWindow()
		{
			return aspirationWindow;
		}

		public virtual void SetAspirationWindow(bool aspirationWindow)
		{
			this.aspirationWindow = aspirationWindow;
		}

		public virtual int[] GetAspirationWindowSizes()
		{
			return aspirationWindowSizes;
		}

		public virtual void SetAspirationWindowSizes(string aspirationWindowSizesString)
		{
			string[] aux = aspirationWindowSizesString.Split(",");
			aspirationWindowSizes = new int[aux.Length];
			for (int i = 0; i < aux.Length; i++)
			{
				aspirationWindowSizes[i] = System.Convert.ToInt32(aux[i]);
			}
		}

		public virtual int GetTranspositionTableSize()
		{
			return transpositionTableSize;
		}

		public virtual void SetTranspositionTableSize(int transpositionTableSize)
		{
			this.transpositionTableSize = transpositionTableSize;
		}

		public virtual int GetExtensionsCheck()
		{
			return extensionsCheck;
		}

		public virtual void SetExtensionsCheck(int extensionsCheck)
		{
			this.extensionsCheck = extensionsCheck;
		}

		public virtual int GetExtensionsMateThreat()
		{
			return extensionsMateThreat;
		}

		public virtual void SetExtensionsMateThreat(int extensionsMateThreat)
		{
			this.extensionsMateThreat = extensionsMateThreat;
		}

		public virtual int GetExtensionsPawnPush()
		{
			return extensionsPawnPush;
		}

		public virtual void SetExtensionsPawnPush(int extensionsPawnPush)
		{
			this.extensionsPawnPush = extensionsPawnPush;
		}

		public virtual int GetExtensionsPassedPawn()
		{
			return extensionsPassedPawn;
		}

		public virtual void SetExtensionsPassedPawn(int extensionsPassedPawn)
		{
			this.extensionsPassedPawn = extensionsPassedPawn;
		}

		public virtual int GetExtensionsSingular()
		{
			return extensionsSingular;
		}

		public virtual void SetExtensionsSingular(int extensionsSingular)
		{
			this.extensionsSingular = extensionsSingular;
		}

		public virtual int GetSingularExtensionMargin()
		{
			return singularExtensionMargin;
		}

		public virtual void SetSingularExtensionMargin(int singularExtensionMargin)
		{
			this.singularExtensionMargin = singularExtensionMargin;
		}

		public virtual int GetContemptFactor()
		{
			return contemptFactor;
		}

		public virtual void SetContemptFactor(int contemptFactor)
		{
			this.contemptFactor = contemptFactor;
		}

		public virtual int GetEvalCenter()
		{
			return evalCenter;
		}

		public virtual void SetEvalCenter(int evalCenter)
		{
			this.evalCenter = evalCenter;
		}

		public virtual int GetEvalPositional()
		{
			return evalPositional;
		}

		public virtual void SetEvalPositional(int evalPositional)
		{
			this.evalPositional = evalPositional;
		}

		public virtual int GetEvalAttacks()
		{
			return evalAttacks;
		}

		public virtual void SetEvalAttacks(int evalAttacks)
		{
			this.evalAttacks = evalAttacks;
		}

		public virtual int GetEvalMobility()
		{
			return evalMobility;
		}

		public virtual void SetEvalMobility(int evalMobility)
		{
			this.evalMobility = evalMobility;
		}

		public virtual int GetEvalPawnStructure()
		{
			return evalPawnStructure;
		}

		public virtual void SetEvalPawnStructure(int evalPawnStructure)
		{
			this.evalPawnStructure = evalPawnStructure;
		}

		public virtual int GetEvalPassedPawns()
		{
			return evalPassedPawns;
		}

		public virtual void SetEvalPassedPawns(int evalPassedPawns)
		{
			this.evalPassedPawns = evalPassedPawns;
		}

		public virtual int GetEvalKingSafety()
		{
			return evalKingSafety;
		}

		public virtual void SetEvalKingSafety(int evalKingSafety)
		{
			this.evalKingSafety = evalKingSafety;
		}

		public virtual int GetRand()
		{
			return rand;
		}

		public virtual void SetRand(int rand)
		{
			this.rand = rand;
		}

		public virtual bool GetRazoring()
		{
			return razoring;
		}

		public virtual void SetRazoring(bool razoring)
		{
			this.razoring = razoring;
		}

		public virtual int GetRazoringMargin()
		{
			return razoringMargin;
		}

		public virtual void SetRazoringMargin(int razoringMargin)
		{
			this.razoringMargin = razoringMargin;
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
			int kPercentage = ((engineElo - 500) * 100) / 1600;
			// knowledge percentage
			int bookPercentage = ((engineElo - 500) * 100) / 1600;
			// book knowledge percentage
			int ePercentage = 88 - ((engineElo - 500) * 88) / 1600;
			// percentage of errors
			SetRand(ePercentage);
			SetUseBook(true);
			SetBookKnowledge(bookPercentage);
			SetEvalPawnStructure(kPercentage);
			SetEvalPassedPawns(kPercentage);
			SetEvalKingSafety(kPercentage);
			SetEvalMobility(kPercentage);
			SetEvalPositional(kPercentage);
			SetEvalCenter(kPercentage);
		}

		public override string ToString()
		{
			return "------------------ Config ---------------------------------------------------------------------\n"
				 + "Book              " + useBook + " (" + book + ")\n" + "TT Size           " +
				 transpositionTableSize + "\n" + "Aspiration Window " + aspirationWindow + " " +
				 Arrays.ToString(aspirationWindowSizes) + "\n" + "Extensions        Check=" + extensionsCheck
				 + " MateThreat=" + extensionsMateThreat + " PawnPush=" + extensionsPawnPush + " PassedPawn="
				 + extensionsPassedPawn + " Singular=" + extensionsSingular + " (" + singularExtensionMargin
				 + ")\n" + "Razoring          " + razoring + " (" + razoringMargin + ")\n" + "Null Move         "
				 + nullMove + "\n" + "Futility Pruning  " + futility + " (" + futilityMarginQS +
				 ", " + futilityMargin + ", " + futilityMarginAggressive + ")\n" + "Static Null Move  "
				 + staticNullMove + "\n" + "IID               " + iid + " (" + iidMargin + ")\n"
				 + "LMR               " + lmr + "\n" + "Evaluator         " + evaluator + " KingSafety="
				 + evalKingSafety + " Mobility=" + evalMobility + " PassedPawns=" + evalPassedPawns
				 + " PawnStructure=" + evalPawnStructure + "\n" + "Contempt Factor   " + contemptFactor;
		}
	}
}
