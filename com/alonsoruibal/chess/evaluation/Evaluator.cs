using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Util;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	public abstract class Evaluator
	{
		public const int W = Color.W;

		public const int B = Color.B;

		public const int NoValue = short.MaxValue;

		public const int Mate = 30000;

		public const int KnownWin = 20000;

		public const int Draw = 0;

		public const int Pawn = 100;

		public const int Knight = 325;

		public const int Bishop = 325;

		public const int Rook = 500;

		public const int Queen = 975;

		public static readonly int[] PieceValues = new int[] { 0, Pawn, Knight, Bishop, Rook
			, Queen };

		public static readonly int BishopPair = Com.Alonsoruibal.Chess.Evaluation.Evaluator
			.Oe(50, 50);

		public const int NonPawnMaterialEndgameMin = Queen + Rook;

		public const int NonPawnMaterialMidgameMax = 2 * Knight + 2 * Bishop + 4 * Rook +
			 2 * Queen;

		public BitboardAttacks bbAttacks;

		public Evaluator()
		{
			// Bonus by having two bishops in different colors
			bbAttacks = BitboardAttacks.GetInstance();
		}

		/// <summary>Board evaluator</summary>
		public abstract int Evaluate(Board board, AttacksInfo attacksInfo);

		/// <summary>Merges two short Opening - Ending values in one int</summary>
		public static int Oe(int opening, int endgame)
		{
			return ((opening < 0 ? opening - 1 : opening) << 16) | (endgame & unchecked((int)
				(0xffff)));
		}

		/// <summary>Get the "Opening" part</summary>
		public static int O(int oe)
		{
			int i = oe >> 16;
			return i < 0 ? i + 1 : i;
		}

		/// <summary>Get the "Endgame" part</summary>
		public static int E(int oe)
		{
			return (short)(oe & unchecked((int)(0xffff)));
		}

		/// <summary>Shift right each part by factor positions</summary>
		public static int OeShr(int factor, int oeValue)
		{
			return Oe(O(oeValue) >> factor, E(oeValue) >> factor);
		}

		internal virtual string FormatOE(int value)
		{
			return StringUtils.PadLeft(O(value).ToString(), 8) + " " + StringUtils.PadLeft(E(
				value).ToString(), 8);
		}
	}
}
