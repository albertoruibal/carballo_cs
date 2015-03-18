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
		public abstract int Evaluate(Board board, AttacksInfo attacksInfo);

		/// <summary>Merges two short Opening - Ending values in one int</summary>
		public static int Oe(int opening, int endgame)
		{
			return (opening << 16) | (endgame & unchecked((int)(0xffff)));
		}

		/// <summary>Multiply with negative numbers (in the factor or in one of the oe components) cannot be done directly
		/// 	</summary>
		public static int OeMul(int factor, int oeValue)
		{
			return (((oeValue >> 16) * factor) << 16) | ((oeValue & unchecked((int)(0xffff)))
				 * factor) & unchecked((int)(0xffff));
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
