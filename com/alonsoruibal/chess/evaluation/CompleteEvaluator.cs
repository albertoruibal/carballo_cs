using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Log;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	/// <summary>
	/// Evaluation is done in centipawns
	/// <p/>
	/// Material imbalances from Larry KaufMan:
	/// http://home.comcast.net/~danheisman/Articles/evaluation_of_material_imbalance.htm
	/// <p/>
	/// Piece/square values like Fruit/Toga
	/// <p/>
	/// TODO: pawn races
	/// TODO: pawn storm
	/// TODO: pinned pieces
	/// </summary>
	/// <author>rui</author>
	public class CompleteEvaluator : Evaluator
	{
		private static readonly Logger logger = Logger.GetLogger("CompleteEvaluator");

		public const int Pawn = 100;

		public const int Knight = 325;

		public const int Bishop = 325;

		public const int BishopPair = 50;

		public const int Rook = 500;

		public const int Queen = 975;

		private const int BishopMUnits = 6;

		private static readonly int BishopM = Oe(5, 5);

		private static readonly int BishopTrapped = Oe(-100, -100);

		private const int KnightMUnits = 4;

		private static readonly int KnightM = Oe(4, 4);

		private const int KnightKaufBonus = 7;

		private const int RookMUnits = 7;

		private static readonly int RookM = Oe(2, 4);

		private static readonly int RookColumnOpen = Oe(25, 20);

		private static readonly int RookColumnSemiopen = Oe(15, 10);

		private static readonly int RookConnect = Oe(20, 10);

		private const int RookKaufBonus = -12;

		private const int QueenMUnits = 13;

		private static readonly int QueenM = Oe(2, 4);

		private static readonly int KingPawnNear = Oe(5, 0);

		private static readonly int PawnAttacksKing = Oe(1, 0);

		private static readonly int KnightAttacksKing = Oe(4, 0);

		private static readonly int BishopAttacksKing = Oe(2, 0);

		private static readonly int RookAttacksKing = Oe(3, 0);

		private static readonly int QueenAttacksKing = Oe(5, 0);

		private static readonly int PawnIsolated = Oe(-10, -20);

		private static readonly int PawnDoubled = Oe(-10, -20);

		private static readonly int PawnWeak = Oe(-10, -15);

		private static readonly int[] PawnPasser = new int[] { 0, Oe(5, 10), Oe(10, 20), 
			Oe(20, 40), Oe(30, 60), Oe(50, 100), Oe(75, 150), 0 };

		private static readonly int[] PawnPasserSupport = new int[] { 0, 0, Oe(5, 10), Oe
			(10, 20), Oe(15, 30), Oe(25, 50), Oe(37, 75), 0 };

		private static readonly int[] PawnPasserKingD = new int[] { 0, 0, Oe(1, 2), Oe(2, 
			4), Oe(3, 6), Oe(5, 10), Oe(7, 15), 0 };

		private static readonly int[] KingSafetyPonder = new int[] { 0, 1, 4, 8, 16, 25, 
			36, 49, 50, 50, 50, 50, 50, 50, 50, 50 };

		private static readonly int HungPieces = Oe(16, 25);

		private static readonly int PinnedPiece = Oe(25, 35);

		public const int Tempo = 10;

		private static readonly int[] KnigthOutpost = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Oe(7, 7), Oe(9, 9), Oe(9, 9), Oe(7, 7), 0, 0, 0, Oe
			(5, 5), Oe(10, 10), Oe(20, 20), Oe(20, 20), Oe(10, 10), Oe(5, 5), 0, 0, Oe(5, 5)
			, Oe(10, 10), Oe(20, 20), Oe(20, 20), Oe(10, 10), Oe(5, 5), 0, 0, 0, Oe(7, 7), Oe
			(9, 9), Oe(9, 9), Oe(7, 7), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
			 };

		private static readonly long[] BishopTrapping = new long[] { unchecked((int)(0x00
			)), 1L << 10, unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00
			)), unchecked((int)(0x00)), 1L << 13, unchecked((int)(0x00)), 1L << 17, unchecked(
			(int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00
			)), unchecked((int)(0x00)), unchecked((int)(0x00)), 1L << 22, 1L << 25, unchecked(
			(int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00
			)), unchecked((int)(0x00)), unchecked((int)(0x00)), 1L << 30, unchecked((int)(0x00
			)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked(
			(int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00
			)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked(
			(int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00
			)), unchecked((int)(0x00)), 1L << 33, unchecked((int)(0x00)), unchecked((int)(0x00
			)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked(
			(int)(0x00)), 1L << 38, 1L << 41, unchecked((int)(0x00)), unchecked((int)(0x00))
			, unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked(
			(int)(0x00)), 1L << 46, unchecked((int)(0x00)), 1L << 50, unchecked((int)(0x00))
			, unchecked((int)(0x00)), unchecked((int)(0x00)), unchecked((int)(0x00)), 1L << 
			53, unchecked((int)(0x00)) };

		private static readonly int PawnColumnValue = Oe(5, 0);

		private static readonly int KnightCenterValue = Oe(5, 5);

		private static readonly int KnightRankValue = Oe(5, 0);

		private static readonly int KnightBackRankValue = Oe(0, 0);

		private static readonly int KnightTrappedValue = Oe(-100, 0);

		private static readonly int BishopCenterValue = Oe(2, 3);

		private static readonly int BishopBackRankValue = Oe(-10, 0);

		private static readonly int BishopDiagonalValue = Oe(4, 0);

		private static readonly int RookColumnValue = Oe(3, 0);

		private static readonly int QueenCenterValue = Oe(0, 4);

		private static readonly int QueenBackRankValue = Oe(-5, 0);

		private static readonly int KingCenterValue = Oe(0, 12);

		private static readonly int KingColumnValue = Oe(10, 0);

		private static readonly int KingRankValue = Oe(10, 0);

		private static readonly int[] PawnColumn = new int[] { -3, -1, +0, +1, +1, +0, -1
			, -3 };

		private static readonly int[] KnightLine = new int[] { -4, -2, +0, +1, +1, +0, -2
			, -4 };

		private static readonly int[] KnightRank = new int[] { -2, -1, +0, +1, +2, +3, +2
			, +1 };

		private static readonly int[] BishopLine = new int[] { -3, -1, +0, +1, +1, +0, -1
			, -3 };

		private static readonly int[] RookColumn = new int[] { -2, -1, +0, +1, +1, +0, -1
			, -2 };

		private static readonly int[] QueenLine = new int[] { -3, -1, +0, +1, +1, +0, -1, 
			-3 };

		private static readonly int[] KingLine = new int[] { -3, -1, +0, +1, +1, +0, -1, 
			-3 };

		private static readonly int[] KingColumn = new int[] { +3, +4, +2, +0, +0, +2, +4
			, +3 };

		private static readonly int[] KingRank = new int[] { +1, +0, -2, -3, -4, -5, -6, 
			-7 };

		public static readonly int[] pawnIndexValue = new int[64];

		public static readonly int[] knightIndexValue = new int[64];

		public static readonly int[] bishopIndexValue = new int[64];

		public static readonly int[] rookIndexValue = new int[64];

		public static readonly int[] queenIndexValue = new int[64];

		public static readonly int[] kingIndexValue = new int[64];

		internal Config config;

		public bool debug = false;

		public CompleteEvaluator(Config config)
		{
			// Bonus by having two bishops in different colors
			// Bishops
			// Mobility units: this value is added for each destination square not occupied by one of our pieces
			// Bishops
			// Rooks
			// No pawns in rook column
			// Only opposite pawns in rook column
			// Rook connects with other rook TODO???
			// Queen
			// Protection: sums for each pawn near king (opening)
			// King Safety: not in endgame!!!
			// Pawns
			// Penalty for each pawn in a doubled rank
			//	private final static int PAWN_BACKWARD         = oe(-8,-10);
			//	private final static int PAWN_BLOCKED          = oe(0,0); //-20; // Pawn blocked by opposite pawn
			// Weak pawn
			// Depends of the rank
			// Depends of the rank
			// Sums by each square away of the other opposite king
			// Ponder kings attacks by the number of attackers (not pawns) later divided by 8
			// two or more pieces of the other side attacked by inferior pieces
			// Tempo
			// Add to moving side score
			// The pair of values are {opening, endgame}
			// Values are rotated for whites, so when white is playing is like shown in the code
			this.config = config;
		}

		static CompleteEvaluator()
		{
			// Initialize Piece square values Fruit/Toga style 
			int i;
			for (i = 0; i < 64; i++)
			{
				int rank = i >> 3;
				int column = 7 - i & 7;
				pawnIndexValue[i] = PawnColumn[column] * PawnColumnValue;
				knightIndexValue[i] = KnightLine[column] * KnightCenterValue + KnightLine[rank] *
					 KnightCenterValue + KnightRank[rank] * KnightRankValue;
				bishopIndexValue[i] = BishopLine[column] * BishopCenterValue + BishopLine[rank] *
					 BishopCenterValue;
				rookIndexValue[i] = RookColumn[column] * RookColumnValue;
				queenIndexValue[i] = QueenLine[column] * QueenCenterValue + QueenLine[rank] * QueenCenterValue;
				kingIndexValue[i] = KingColumn[column] * KingColumnValue + KingRank[rank] * KingRankValue
					 + KingLine[column] * KingCenterValue + KingLine[rank] * KingCenterValue;
			}
			knightIndexValue[56] += KnightTrappedValue;
			// H8 
			knightIndexValue[63] += KnightTrappedValue;
			// A8
			for (i = 0; i < 8; i++)
			{
				queenIndexValue[i] += QueenBackRankValue;
				knightIndexValue[i] += KnightBackRankValue;
				bishopIndexValue[i] += BishopBackRankValue;
				bishopIndexValue[(i << 3) | i] += BishopDiagonalValue;
				bishopIndexValue[((i << 3) | i) ^ unchecked((int)(0x38))] += BishopDiagonalValue;
			}
			// Pawn opening corrections
			pawnIndexValue[19] += Oe(10, 0);
			// E3
			pawnIndexValue[20] += Oe(10, 0);
			// D3
			pawnIndexValue[27] += Oe(25, 0);
			// E4
			pawnIndexValue[28] += Oe(25, 0);
			// D4
			pawnIndexValue[35] += Oe(10, 0);
			// E5
			pawnIndexValue[36] += Oe(10, 0);
		}

		private int[] bishopCount = new int[] { 0, 0 };

		private long[] superiorPieceAttacked = new long[] { 0, 0 };

		private int[] material = new int[] { 0, 0 };

		private int[] pawnMaterial = new int[] { 0, 0 };

		private int[] mobility = new int[] { 0, 0 };

		private int[] attacks = new int[] { 0, 0 };

		private int[] center = new int[] { 0, 0 };

		private int[] positional = new int[] { 0, 0 };

		private int[] kingAttackersCount = new int[] { 0, 0 };

		private int[] kingSafety = new int[] { 0, 0 };

		private int[] pawnStructure = new int[] { 0, 0 };

		private int[] passedPawns = new int[] { 0, 0 };

		private long[] pawnAttacks = new long[] { 0, 0 };

		private long[] squaresNearKing = new long[] { 0, 0 };

		private int[] knightKaufBonus = new int[] { 0, 0 };

		private int[] rookKaufBonus = new int[] { 0, 0 };

		private long all;

		private long pieceAttacks;

		private long pieceAttacksXray;

		private long mines;

		private long others;

		private long square;

		private int auxInt;

		private int pcsqIndex;

		private int color;

		private int index;

		private bool isWhite;

		// D5
		//	int kingDefense[] = {0,0};
		// Squares attackeds by pawns
		// Squares surrounding King
		public override int Evaluate(Board board)
		{
			all = board.GetAll();
			Arrays.Fill(bishopCount, 0);
			Arrays.Fill(superiorPieceAttacked, 0);
			Arrays.Fill(material, 0);
			Arrays.Fill(pawnMaterial, 0);
			Arrays.Fill(mobility, 0);
			Arrays.Fill(attacks, 0);
			Arrays.Fill(center, 0);
			Arrays.Fill(positional, 0);
			Arrays.Fill(kingAttackersCount, 0);
			Arrays.Fill(kingSafety, 0);
			Arrays.Fill(pawnStructure, 0);
			Arrays.Fill(passedPawns, 0);
			// Squares attackeds by pawns
			pawnAttacks[0] = ((board.pawns & board.whites & ~BitboardUtils.b_l) << 9) | ((board
				.pawns & board.whites & ~BitboardUtils.b_r) << 7);
			pawnAttacks[1] = ((long)(((ulong)(board.pawns & board.blacks & ~BitboardUtils.b_r
				)) >> 9)) | ((long)(((ulong)(board.pawns & board.blacks & ~BitboardUtils.b_l)) >>
				 7));
			// Squares surrounding King
			squaresNearKing[0] = bbAttacks.king[BitboardUtils.Square2Index(board.whites & board
				.kings)];
			squaresNearKing[1] = bbAttacks.king[BitboardUtils.Square2Index(board.blacks & board
				.kings)];
			// From material imbalances (Larry Kaufmann):
			// A further refinement would be to raise the knight's value by 1/16 and lower the rook's value by 1/8
			// for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five
			int whitePawnsCount = BitboardUtils.PopCount(board.pawns & board.whites);
			int blackPawnsCount = BitboardUtils.PopCount(board.pawns & board.blacks);
			knightKaufBonus[0] = KnightKaufBonus * (whitePawnsCount - 5);
			knightKaufBonus[1] = KnightKaufBonus * (blackPawnsCount - 5);
			rookKaufBonus[0] = RookKaufBonus * (whitePawnsCount - 5);
			rookKaufBonus[1] = RookKaufBonus * (blackPawnsCount - 5);
			square = 1;
			index = 0;
			while (square != 0)
			{
				isWhite = ((board.whites & square) != 0);
				color = (isWhite ? 0 : 1);
				mines = (isWhite ? board.whites : board.blacks);
				others = (isWhite ? board.blacks : board.whites);
				pcsqIndex = (isWhite ? index : 63 - index);
				if ((square & all) != 0)
				{
					int rank = index >> 3;
					int column = 7 - index & 7;
					if ((square & board.pawns) != 0)
					{
						pawnMaterial[color] += Pawn;
						center[color] += pawnIndexValue[pcsqIndex];
						pieceAttacks = (isWhite ? bbAttacks.pawnUpwards[index] : bbAttacks.pawnDownwards[
							index]);
						superiorPieceAttacked[color] |= pieceAttacks & others & (board.knights | board.bishops
							 | board.rooks | board.queens);
						if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
						{
							kingSafety[color] += PawnAttacksKing;
						}
						// Doubled pawn detection
						if ((BitboardUtils.Column[column] & BitboardUtils.RanksForward[color][rank] & board
							.pawns & mines) != square)
						{
							pawnStructure[color] += PawnDoubled;
						}
						// Blocked Pawn
						//					boolean blocked = ((isWhite ? (square<< 8)  : (square >>> 8)) & others) != 0;
						//					if (blocked) pawnStructure[color] += PAWN_BLOCKED;
						// Backwards Pawn
						//					if (((BitboardUtils.COLUMN[column] | BitboardUtils.COLUMNS_ADJACENTS[column]) & ~BitboardUtils.RANKS_FORWARD[color][rank] & board.pawns & mines) == 0)
						//						pawnStructure[color] += PAWN_BACKWARD;
						// Passed Pawn
						if (((BitboardUtils.Column[column] | BitboardUtils.ColumnsAdjacents[column]) & (BitboardUtils
							.RanksForward[color][rank]) & board.pawns & others) == 0)
						{
							passedPawns[color] += PawnPasser[(isWhite ? rank : 7 - rank)];
							if ((square & pawnAttacks[color]) != 0)
							{
								passedPawns[color] += PawnPasserSupport[(isWhite ? rank : 7 - rank)];
							}
							passedPawns[color] += PawnPasserKingD[(isWhite ? rank : 7 - rank)] * BitboardUtils
								.Distance(index, BitboardUtils.Square2Index(board.kings & others));
						}
						// Isolated pawn
						bool isolated = (BitboardUtils.ColumnsAdjacents[column] & board.pawns & mines) ==
							 0;
						if (isolated)
						{
							pawnStructure[color] += PawnIsolated;
						}
						long auxLong;
						long auxLong2;
						bool weak = !isolated && (pawnAttacks[color] & square) == 0;
						//						&& pcsqIndex >= 24
						// not defended is weak and only if over rank 2
						if (weak)
						{
							// Can be defended advancing one square
							auxLong = (isWhite ? bbAttacks.pawnDownwards[color] : bbAttacks.pawnUpwards[color
								]) & ~pawnAttacks[1 - color] & ~all;
							while (auxLong != 0)
							{
								// Not attacked by other pawn and empty
								auxLong2 = BitboardUtils.Lsb(auxLong);
								auxLong &= ~auxLong2;
								auxLong2 = isWhite ? (long)(((ulong)auxLong2) >> 8) : auxLong2 << 8;
								if ((auxLong2 & mines & board.pawns) != 0)
								{
									weak = false;
								}
								else
								{
									// Defended advancing one pawn two squares
									if ((auxLong2 & all) == 0)
									{
										// empty square								
										auxLong2 = (isWhite ? (long)(((ulong)auxLong2) >> 8) : auxLong2 << 8);
										if (((isWhite ? BitboardUtils.Rank[1] : BitboardUtils.Rank[6]) & auxLong2 & board
											.pawns & mines) != 0)
										{
											weak = false;
										}
									}
								}
							}
							if (weak)
							{
								// Can advance to be supported
								auxLong = (isWhite ? square << 8 : (long)(((ulong)square) >> 8)) & ~pawnAttacks[1
									 - color] & ~all;
								if (auxLong != 0)
								{
									if ((auxLong & pawnAttacks[color]) != 0)
									{
										weak = false;
									}
									else
									{
										// Checks advancing two squares if in initial position
										if (((isWhite ? BitboardUtils.Rank[1] : BitboardUtils.Rank[6]) & square) != 0)
										{
											auxLong = (isWhite ? square << 16 : (long)(((ulong)square) >> 16)) & ~pawnAttacks
												[1 - color] & ~all;
											if ((auxLong & pawnAttacks[color]) != 0)
											{
												weak = false;
											}
										}
									}
								}
							}
						}
						if (weak)
						{
							pawnStructure[color] += PawnWeak;
						}
					}
					else
					{
						//					if (weak) {
						//						System.out.println("weak pawn: \n" + board.toString());
						//						System.out.println("square: \n" + BitboardUtils.toString(square));
						//					}
						if ((square & board.knights) != 0)
						{
							material[color] += Knight + knightKaufBonus[color];
							center[color] += knightIndexValue[pcsqIndex];
							pieceAttacks = bbAttacks.knight[index];
							auxInt = BitboardUtils.PopCount(pieceAttacks & ~mines & ~pawnAttacks[1 - color]) 
								- KnightMUnits;
							mobility[color] += KnightM * auxInt;
							if ((pieceAttacks & squaresNearKing[color]) != 0)
							{
								kingSafety[color] += KnightAttacksKing;
								kingAttackersCount[color]++;
							}
							superiorPieceAttacked[color] |= pieceAttacks & others & (board.rooks | board.queens
								);
							// Knight outpost: no opposite pawns can attack the square and is defended by one of our pawns
							if (((BitboardUtils.ColumnsAdjacents[column] & BitboardUtils.RanksForward[color][
								rank] & board.pawns & others) == 0) && (((isWhite ? bbAttacks.pawnDownwards[index
								] : bbAttacks.pawnUpwards[index]) & board.pawns & mines) != 0))
							{
								positional[color] += KnigthOutpost[pcsqIndex];
							}
						}
						else
						{
							if ((square & board.bishops) != 0)
							{
								material[color] += Bishop;
								if (++bishopCount[color] == 2)
								{
									material[color] += BishopPair;
								}
								center[color] += bishopIndexValue[pcsqIndex];
								pieceAttacks = bbAttacks.GetBishopAttacks(index, all);
								auxInt = BitboardUtils.PopCount(pieceAttacks & ~mines & ~pawnAttacks[1 - color]) 
									- BishopMUnits;
								mobility[color] += BishopM * auxInt;
								if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
								{
									kingSafety[color] += BishopAttacksKing;
									kingAttackersCount[color]++;
								}
								pieceAttacksXray = bbAttacks.GetBishopAttacks(index, all & ~(pieceAttacks & others
									)) & ~pieceAttacks;
								if ((pieceAttacksXray & (board.rooks | board.queens | board.kings) & others) != 0)
								{
									attacks[color] += PinnedPiece;
								}
								superiorPieceAttacked[color] |= pieceAttacks & others & (board.rooks | board.queens
									);
								if ((BishopTrapping[index] & board.pawns & others) != 0)
								{
									mobility[color] += BishopTrapped;
								}
							}
							else
							{
								if ((square & board.rooks) != 0)
								{
									material[color] += Rook + rookKaufBonus[color];
									center[color] += rookIndexValue[pcsqIndex];
									pieceAttacks = bbAttacks.GetRookAttacks(index, all);
									auxInt = BitboardUtils.PopCount(pieceAttacks & ~mines & ~pawnAttacks[1 - color]) 
										- RookMUnits;
									mobility[color] += RookM * auxInt;
									pieceAttacksXray = bbAttacks.GetRookAttacks(index, all & ~(pieceAttacks & others)
										) & ~pieceAttacks;
									if ((pieceAttacksXray & (board.queens | board.kings) & others) != 0)
									{
										attacks[color] += PinnedPiece;
									}
									if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
									{
										kingSafety[color] += RookAttacksKing;
										kingAttackersCount[color]++;
									}
									superiorPieceAttacked[color] |= pieceAttacks & others & board.queens;
									if ((pieceAttacks & mines & (board.rooks)) != 0)
									{
										positional[color] += RookConnect;
									}
									pieceAttacks = BitboardUtils.Column[column];
									if ((pieceAttacks & board.pawns) == 0)
									{
										positional[color] += RookColumnOpen;
									}
									else
									{
										if ((pieceAttacks & board.pawns & mines) == 0)
										{
											positional[color] += RookColumnSemiopen;
										}
									}
								}
								else
								{
									if ((square & board.queens) != 0)
									{
										center[color] += queenIndexValue[pcsqIndex];
										material[color] += Queen;
										pieceAttacks = bbAttacks.GetRookAttacks(index, all) | bbAttacks.GetBishopAttacks(
											index, all);
										auxInt = BitboardUtils.PopCount(pieceAttacks & ~mines & ~pawnAttacks[1 - color]) 
											- QueenMUnits;
										mobility[color] += QueenM * auxInt;
										if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
										{
											kingSafety[color] += QueenAttacksKing;
											kingAttackersCount[color]++;
										}
										pieceAttacksXray = (bbAttacks.GetRookAttacks(index, all & ~(pieceAttacks & others
											)) | bbAttacks.GetBishopAttacks(index, all & ~(pieceAttacks & others))) & ~pieceAttacks;
										if ((pieceAttacksXray & board.kings & others) != 0)
										{
											attacks[color] += PinnedPiece;
										}
									}
									else
									{
										if ((square & board.kings) != 0)
										{
											pieceAttacks = bbAttacks.king[index];
											center[color] += kingIndexValue[pcsqIndex];
											// TODO
											if ((square & (isWhite ? BitboardUtils.Rank[1] : BitboardUtils.Rank[7])) != 0)
											{
												positional[color] += KingPawnNear * BitboardUtils.PopCount(pieceAttacks & mines &
													 board.pawns);
											}
										}
									}
								}
							}
						}
					}
				}
				square <<= 1;
				index++;
			}
			// Ponder opening and Endgame value depending of the non-pawn pieces:
			// opening=> gamephase = 255 / ending => gamephase ~= 0
			int gamePhase = ((material[0] + material[1]) << 8) / 5000;
			if (gamePhase > 256)
			{
				gamePhase = 256;
			}
			// Security
			int value = 0;
			// First Material
			value += pawnMaterial[0] - pawnMaterial[1] + material[0] - material[1];
			// Tempo
			value += (board.GetTurn() ? Tempo : -Tempo);
			int oe = config.GetEvalCenter() * (center[0] - center[1]) + config.GetEvalPositional
				() * (positional[0] - positional[1]) + config.GetEvalAttacks() * (attacks[0] - attacks
				[1]) + config.GetEvalMobility() * (mobility[0] - mobility[1]) + config.GetEvalPawnStructure
				() * (pawnStructure[0] - pawnStructure[1]) + config.GetEvalPassedPawns() * (passedPawns
				[0] - passedPawns[1]) + (config.GetEvalKingSafety() / 8) * ((KingSafetyPonder[kingAttackersCount
				[0]] * kingSafety[0] - KingSafetyPonder[kingAttackersCount[1]] * kingSafety[1]))
				 + config.GetEvalAttacks() * ((BitboardUtils.PopCount(superiorPieceAttacked[0]) 
				>= 2 ? HungPieces : 0) - (BitboardUtils.PopCount(superiorPieceAttacked[1]) >= 2 ? 
				HungPieces : 0));
			// Divide by eight
			value += (gamePhase * O(oe)) / (256 * 100);
			// divide by 256
			value += ((256 - gamePhase) * E(oe)) / (256 * 100);
			if (debug)
			{
				logger.Debug("\n" + board.ToString());
				logger.Debug(board.GetFen());
				logger.Debug("materialValue          = " + (material[0] - material[1]));
				logger.Debug("pawnMaterialValue      = " + (pawnMaterial[0] - pawnMaterial[1]));
				logger.Debug("centerOpening          = " + O(center[0] - center[1]));
				logger.Debug("centerEndgame          = " + E(center[0] - center[1]));
				logger.Debug("positionalOpening      = " + O(positional[0] - positional[1]));
				logger.Debug("positionalEndgame      = " + E(positional[0] - positional[1]));
				logger.Debug("attacksO 				 = " + O(attacks[0] - attacks[1]));
				logger.Debug("attacksE 				 = " + E(attacks[0] - attacks[1]));
				logger.Debug("mobilityO              = " + O(mobility[0] - mobility[1]));
				logger.Debug("mobilityE              = " + E(mobility[0] - mobility[1]));
				logger.Debug("pawnsO                 = " + O(pawnStructure[0] - pawnStructure[1])
					);
				logger.Debug("pawnsE                 = " + E(pawnStructure[0] - pawnStructure[1])
					);
				logger.Debug("passedPawnsO           = " + O(passedPawns[0] - passedPawns[1]));
				logger.Debug("passedPawnsE           = " + E(passedPawns[0] - passedPawns[1]));
				logger.Debug("kingSafetyValueO       = " + O(KingSafetyPonder[kingAttackersCount[
					0]] * kingSafety[0] - KingSafetyPonder[kingAttackersCount[1]] * kingSafety[1]));
				logger.Debug("kingSafetyValueE       = " + E(KingSafetyPonder[kingAttackersCount[
					0]] * kingSafety[0] - KingSafetyPonder[kingAttackersCount[1]] * kingSafety[1]));
				logger.Debug("HungPiecesO 		     = " + O((BitboardUtils.PopCount(superiorPieceAttacked
					[0]) >= 2 ? HungPieces : 0) - (BitboardUtils.PopCount(superiorPieceAttacked[1]) 
					>= 2 ? HungPieces : 0)));
				logger.Debug("HungPiecesE 		     = " + O((BitboardUtils.PopCount(superiorPieceAttacked
					[0]) >= 2 ? HungPieces : 0) - (BitboardUtils.PopCount(superiorPieceAttacked[1]) 
					>= 2 ? HungPieces : 0)));
				logger.Debug("gamePhase              = " + gamePhase);
				logger.Debug("tempo                  = " + (board.GetTurn() ? Tempo : -Tempo));
				logger.Debug("value                  = " + value);
			}
			return value;
		}
	}
}
