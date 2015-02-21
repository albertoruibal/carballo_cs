using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	public abstract class Evaluator
	{
		public const int NoValue = short.MaxValue;

		public const int Victory = 30000;

		public const int KnownWin = 20000;

		public const int Draw = 0;

		public BitboardAttacks bbAttacks;

		public Evaluator()
		{
			bbAttacks = BitboardAttacks.GetInstance();
		}

		/// <summary>Board evaluator</summary>
		public abstract int Evaluate(Board board);

		public static int Oe(int opening, int endgame)
		{
			return (((short)(opening)) << 16) + (short)(endgame);
		}

		public static int O(int oe)
		{
			return oe >> 16;
		}

		public static int E(int oe)
		{
			return (short)(oe & unchecked((int)(0xffff)));
		}
	}
}
