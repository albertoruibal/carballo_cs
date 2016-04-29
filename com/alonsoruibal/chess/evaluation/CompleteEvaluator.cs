using System;
using System.Text;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Log;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	/// <summary>Evaluation is done in centipawns</summary>
	/// <author>rui</author>
	public class CompleteEvaluator : Evaluator
	{
		private static readonly Logger logger = Logger.GetLogger("CompleteEvaluator");

		private static readonly int[][] Mobility = new int[][] { new int[] {  }, new int[
			] {  }, new int[] { Oe(0, 0), Oe(12, 16), Oe(18, 24), Oe(21, 28), Oe(24, 32) }, 
			new int[] { Oe(0, 0), Oe(11, 11), Oe(18, 18), Oe(22, 22), Oe(25, 25), Oe(28, 28)
			, Oe(30, 30), Oe(32, 32) }, new int[] { Oe(0, 0), Oe(6, 9), Oe(10, 15), Oe(13, 20
			), Oe(15, 23), Oe(17, 26), Oe(19, 29), Oe(21, 31), Oe(22, 33), Oe(23, 35), Oe(24
			, 37), Oe(25, 38), Oe(26, 39), Oe(27, 41), Oe(28, 42) }, new int[] { Oe(0, 0), Oe
			(7, 7), Oe(12, 12), Oe(16, 16), Oe(20, 20), Oe(23, 23), Oe(26, 26), Oe(28, 28), 
			Oe(30, 30), Oe(33, 33), Oe(34, 34), Oe(36, 36), Oe(38, 38), Oe(39, 39), Oe(41, 41
			), Oe(42, 42), Oe(43, 43), Oe(44, 44), Oe(46, 46), Oe(47, 47), Oe(48, 48), Oe(49
			, 49), Oe(50, 50), Oe(51, 51), Oe(52, 52), Oe(52, 52), Oe(53, 53), Oe(54, 54) } };

		private static readonly int[] PawnAttacks = new int[] { 0, 0, Oe(11, 15), Oe(12, 
			16), Oe(17, 23), Oe(19, 25), 0 };

		private static readonly int[] MinorAttacks = new int[] { 0, Oe(3, 5), Oe(7, 9), Oe
			(7, 9), Oe(10, 14), Oe(11, 15), 0 };

		private static readonly int[] MajorAttacks = new int[] { 0, Oe(2, 2), Oe(3, 4), Oe
			(3, 4), Oe(5, 6), Oe(5, 7), 0 };

		private static readonly int HungPieces = Oe(16, 25);

		private static readonly int PinnedPiece = Oe(7, 15);

		private static readonly int[] PawnBackwards = new int[] { Oe(20, 15), Oe(10, 15) };

		private static readonly int[] PawnIsolated = new int[] { Oe(20, 20), Oe(10, 20) };

		private static readonly int[] PawnDoubled = new int[] { Oe(8, 16), Oe(10, 20) };

		private static readonly int PawnUnsupported = Oe(2, 4);

		private static readonly int[] PawnCandidate = new int[] { 0, Oe(8, 13), Oe(8, 13)
			, Oe(13, 20), Oe(24, 36), Oe(39, 59), Oe(60, 90), 0 };

		private static readonly int[] PawnPasser = new int[] { 0, Oe(17, 25), Oe(17, 25), 
			Oe(27, 41), Oe(48, 72), Oe(79, 118), Oe(120, 180), 0 };

		private static readonly int[] PawnPasserOutside = new int[] { 0, Oe(3, 5), Oe(3, 
			5), Oe(5, 8), Oe(10, 14), Oe(16, 24), Oe(24, 36), 0 };

		private static readonly int[] PawnPasserConnected = new int[] { 0, 0, 0, Oe(5, 7)
			, Oe(14, 21), Oe(28, 42), Oe(47, 70), 0 };

		private static readonly int[] PawnPasserSupported = new int[] { 0, 0, 0, Oe(5, 8)
			, Oe(16, 24), Oe(32, 48), Oe(53, 80), 0 };

		private static readonly int[] PawnPasserMobile = new int[] { 0, 0, 0, Oe(3, 5), Oe
			(9, 14), Oe(18, 27), Oe(30, 45), 0 };

		private static readonly int[] PawnPasserRunner = new int[] { 0, 0, 0, Oe(4, 6), Oe
			(12, 18), Oe(24, 36), Oe(40, 60), 0 };

		private static readonly int PawnPasserUnstoppable = Oe(750, 750);

		private static readonly int[] PawnShield = new int[] { 0, Oe(32, 0), Oe(24, 0), Oe
			(16, 0), Oe(8, 0), 0, 0, 0 };

		private static readonly int[] PawnStorm = new int[] { 0, 0, 0, Oe(12, 0), Oe(25, 
			0), Oe(50, 0), 0, 0 };

		private static readonly int[] KnightOutpost = new int[] { Oe(15, 10), Oe(25, 15) };

		private static readonly int[] BishopOutpost = new int[] { Oe(7, 4), Oe(12, 7) };

		private static readonly int BishopMyPawnsInColorPenalty = Oe(2, 4);

		private static readonly int BishopTrappedPenalty = Oe(40, 40);

		private static readonly long[] BishopTrapping = new long[] { 0, Square.F2, 0, 0, 
			0, 0, Square.C2, 0, Square.G3, 0, 0, 0, 0, 0, 0, Square.B3, Square.G4, 0, 0, 0, 
			0, 0, 0, Square.B4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Square.G5, 0
			, 0, 0, 0, 0, 0, Square.B5, Square.G6, 0, 0, 0, 0, 0, 0, Square.B6, 0, Square.F7
			, 0, 0, 0, 0, Square.C7, 0 };

		private static readonly int[] RookOutpost = new int[] { Oe(2, 1), Oe(3, 2) };

		private static readonly int[] RookFile = new int[] { Oe(15, 10), Oe(7, 5) };

		private static readonly int Rook7 = Oe(7, 10);

		private static readonly int[] PieceAttacksKing = new int[] { 0, 0, Oe(30, 0), Oe(
			20, 0), Oe(40, 0), Oe(80, 0) };

		private static readonly int[] KingSafetyPonder = new int[] { 0, 0, 32, 48, 56, 60
			, 62, 63, 64, 64, 64, 64, 64, 64, 64, 64 };

		public static readonly int Tempo = Oe(15, 5);

		private static readonly long[] OutpostMask = new long[] { unchecked((long)(0x00007e7e7e000000L
			)), unchecked((long)(0x0000007e7e7e0000L)) };

		private static readonly int[] pawnPcsq = new int[] { Oe(80, 96), Oe(92, 94), Oe(98
			, 92), Oe(105, 90), Oe(105, 90), Oe(98, 92), Oe(92, 94), Oe(80, 96), Oe(77, 93), 
			Oe(89, 91), Oe(95, 89), Oe(102, 87), Oe(102, 87), Oe(95, 89), Oe(89, 91), Oe(77, 
			93), Oe(78, 93), Oe(90, 91), Oe(96, 89), Oe(113, 87), Oe(113, 87), Oe(96, 89), Oe
			(90, 91), Oe(78, 93), Oe(79, 94), Oe(91, 92), Oe(97, 90), Oe(124, 88), Oe(124, 88
			), Oe(97, 90), Oe(91, 92), Oe(79, 94), Oe(81, 95), Oe(93, 93), Oe(99, 91), Oe(116
			, 89), Oe(116, 89), Oe(99, 91), Oe(93, 93), Oe(81, 95), Oe(82, 96), Oe(94, 94), 
			Oe(100, 92), Oe(107, 90), Oe(107, 90), Oe(100, 92), Oe(94, 94), Oe(82, 96), Oe(83
			, 98), Oe(95, 96), Oe(101, 94), Oe(108, 92), Oe(108, 92), Oe(101, 94), Oe(95, 96
			), Oe(83, 98), Oe(80, 96), Oe(92, 94), Oe(98, 92), Oe(105, 90), Oe(105, 90), Oe(
			98, 92), Oe(92, 94), Oe(80, 96) };

		private static readonly int[] knightPcsq = new int[] { Oe(267, 303), Oe(283, 308)
			, Oe(294, 313), Oe(298, 316), Oe(298, 316), Oe(294, 313), Oe(283, 308), Oe(267, 
			303), Oe(289, 310), Oe(305, 317), Oe(316, 321), Oe(320, 323), Oe(320, 323), Oe(316
			, 321), Oe(305, 317), Oe(289, 310), Oe(305, 315), Oe(321, 321), Oe(332, 326), Oe
			(336, 328), Oe(336, 328), Oe(332, 326), Oe(321, 321), Oe(305, 315), Oe(314, 319)
			, Oe(330, 324), Oe(341, 329), Oe(345, 333), Oe(345, 333), Oe(341, 329), Oe(330, 
			324), Oe(314, 319), Oe(320, 321), Oe(336, 326), Oe(347, 331), Oe(351, 335), Oe(351
			, 335), Oe(347, 331), Oe(336, 326), Oe(320, 321), Oe(318, 322), Oe(334, 328), Oe
			(345, 333), Oe(349, 335), Oe(349, 335), Oe(345, 333), Oe(334, 328), Oe(318, 322)
			, Oe(309, 317), Oe(325, 324), Oe(336, 328), Oe(340, 330), Oe(340, 330), Oe(336, 
			328), Oe(325, 324), Oe(309, 317), Oe(288, 310), Oe(304, 315), Oe(315, 320), Oe(319
			, 323), Oe(319, 323), Oe(315, 320), Oe(304, 315), Oe(288, 310) };

		private static readonly int[] bishopPcsq = new int[] { Oe(318, 325), Oe(317, 324)
			, Oe(314, 323), Oe(312, 323), Oe(312, 323), Oe(314, 323), Oe(317, 324), Oe(318, 
			325), Oe(322, 324), Oe(328, 326), Oe(325, 325), Oe(323, 325), Oe(323, 325), Oe(325
			, 325), Oe(328, 326), Oe(322, 324), Oe(319, 323), Oe(325, 325), Oe(332, 328), Oe
			(331, 327), Oe(331, 327), Oe(332, 328), Oe(325, 325), Oe(319, 323), Oe(317, 323)
			, Oe(323, 325), Oe(331, 327), Oe(340, 330), Oe(340, 330), Oe(331, 327), Oe(323, 
			325), Oe(317, 323), Oe(317, 323), Oe(323, 325), Oe(331, 327), Oe(340, 330), Oe(340
			, 330), Oe(331, 327), Oe(323, 325), Oe(317, 323), Oe(319, 323), Oe(325, 325), Oe
			(332, 328), Oe(331, 327), Oe(331, 327), Oe(332, 328), Oe(325, 325), Oe(319, 323)
			, Oe(322, 324), Oe(328, 326), Oe(325, 325), Oe(323, 325), Oe(323, 325), Oe(325, 
			325), Oe(328, 326), Oe(322, 324), Oe(323, 325), Oe(322, 324), Oe(319, 323), Oe(317
			, 323), Oe(317, 323), Oe(319, 323), Oe(322, 324), Oe(323, 325) };

		private static readonly int[] rookPcsq = new int[] { Oe(496, 500), Oe(500, 500), 
			Oe(504, 500), Oe(508, 500), Oe(508, 500), Oe(504, 500), Oe(500, 500), Oe(496, 500
			), Oe(496, 500), Oe(500, 500), Oe(504, 500), Oe(508, 500), Oe(508, 500), Oe(504, 
			500), Oe(500, 500), Oe(496, 500), Oe(496, 500), Oe(500, 500), Oe(504, 500), Oe(508
			, 500), Oe(508, 500), Oe(504, 500), Oe(500, 500), Oe(496, 500), Oe(496, 500), Oe
			(500, 500), Oe(504, 500), Oe(508, 500), Oe(508, 500), Oe(504, 500), Oe(500, 500)
			, Oe(496, 500), Oe(496, 501), Oe(500, 501), Oe(504, 501), Oe(508, 501), Oe(508, 
			501), Oe(504, 501), Oe(500, 501), Oe(496, 501), Oe(496, 501), Oe(500, 501), Oe(504
			, 501), Oe(508, 501), Oe(508, 501), Oe(504, 501), Oe(500, 501), Oe(496, 501), Oe
			(496, 501), Oe(500, 501), Oe(504, 501), Oe(508, 501), Oe(508, 501), Oe(504, 501)
			, Oe(500, 501), Oe(496, 501), Oe(496, 498), Oe(500, 498), Oe(504, 498), Oe(508, 
			498), Oe(508, 498), Oe(504, 498), Oe(500, 498), Oe(496, 498) };

		private static readonly int[] queenPcsq = new int[] { Oe(964, 960), Oe(968, 965), 
			Oe(971, 967), Oe(973, 968), Oe(973, 968), Oe(971, 967), Oe(968, 965), Oe(964, 960
			), Oe(968, 965), Oe(974, 970), Oe(976, 972), Oe(978, 973), Oe(978, 973), Oe(976, 
			972), Oe(974, 970), Oe(968, 965), Oe(971, 967), Oe(976, 972), Oe(980, 975), Oe(981
			, 977), Oe(981, 977), Oe(980, 975), Oe(976, 972), Oe(971, 967), Oe(973, 968), Oe
			(978, 973), Oe(981, 977), Oe(984, 980), Oe(984, 980), Oe(981, 977), Oe(978, 973)
			, Oe(973, 968), Oe(973, 968), Oe(978, 973), Oe(981, 977), Oe(984, 980), Oe(984, 
			980), Oe(981, 977), Oe(978, 973), Oe(973, 968), Oe(971, 967), Oe(976, 972), Oe(980
			, 975), Oe(981, 977), Oe(981, 977), Oe(980, 975), Oe(976, 972), Oe(971, 967), Oe
			(968, 965), Oe(974, 970), Oe(976, 972), Oe(978, 973), Oe(978, 973), Oe(976, 972)
			, Oe(974, 970), Oe(968, 965), Oe(964, 960), Oe(968, 965), Oe(971, 967), Oe(973, 
			968), Oe(973, 968), Oe(971, 967), Oe(968, 965), Oe(964, 960) };

		private static readonly int[] kingPcsq = new int[] { Oe(1044, 942), Oe(1049, 965)
			, Oe(1019, 981), Oe(999, 987), Oe(999, 987), Oe(1019, 981), Oe(1049, 965), Oe(1044
			, 942), Oe(1041, 965), Oe(1046, 990), Oe(1016, 1002), Oe(996, 1008), Oe(996, 1008
			), Oe(1016, 1002), Oe(1046, 990), Oe(1041, 965), Oe(1038, 981), Oe(1043, 1002), 
			Oe(1013, 1017), Oe(993, 1023), Oe(993, 1023), Oe(1013, 1017), Oe(1043, 1002), Oe
			(1038, 981), Oe(1035, 987), Oe(1040, 1008), Oe(1010, 1023), Oe(990, 1032), Oe(990
			, 1032), Oe(1010, 1023), Oe(1040, 1008), Oe(1035, 987), Oe(1030, 987), Oe(1035, 
			1008), Oe(1005, 1023), Oe(985, 1032), Oe(985, 1032), Oe(1005, 1023), Oe(1035, 1008
			), Oe(1030, 987), Oe(1025, 981), Oe(1030, 1002), Oe(1000, 1017), Oe(980, 1023), 
			Oe(980, 1023), Oe(1000, 1017), Oe(1030, 1002), Oe(1025, 981), Oe(1015, 965), Oe(
			1020, 990), Oe(990, 1002), Oe(970, 1008), Oe(970, 1008), Oe(990, 1002), Oe(1020, 
			990), Oe(1015, 965), Oe(1005, 942), Oe(1010, 965), Oe(980, 981), Oe(960, 987), Oe
			(960, 987), Oe(980, 981), Oe(1010, 965), Oe(1005, 942) };

		public bool debug = false;

		public bool debugPawns = false;

		public StringBuilder debugSB;

		private int[] pcsq = new int[] { 0, 0 };

		private int[] positional = new int[] { 0, 0 };

		private int[] mobility = new int[] { 0, 0 };

		private int[] attacks = new int[] { 0, 0 };

		private int[] kingAttackersCount = new int[] { 0, 0 };

		private int[] kingSafety = new int[] { 0, 0 };

		private int[] pawnStructure = new int[] { 0, 0 };

		private int[] passedPawns = new int[] { 0, 0 };

		private long[] pawnCanAttack = new long[] { 0, 0 };

		private long[] mobilitySquares = new long[] { 0, 0 };

		private long[] kingZone = new long[] { 0, 0 };

		// Mobility units: this value is added for the number of destination square not occupied by one of our pieces or attacked by opposite pawns
		// Attacks
		// Minor piece attacks to pawn undefended pieces
		// Major piece attacks to pawn undefended pieces
		// Two or more pieces of the other side attacked by inferior pieces
		// Pawns
		// Those are all penalties. Array is {not opposed, opposed}: If not opposed, backwards and isolated pawns can be easily attacked
		// Not opposed is worse in the opening
		// Not opposed is worse in the opening
		// Not opposed is better, opening is better
		// Not backwards or isolated
		// And now the bonuses. Array by relative rank
		// Knights
		// Array is Not defended by pawn, defended by pawn
		// Bishops
		// Penalty for each of my pawns in the bishop color (Capablanca rule)
		// Rooks
		// Array is Not defended by pawn, defended by pawn
		// Open / Semi open
		// Rook 5, 6 or 7th rank attacking a pawn in the same rank not defended by pawn
		// King
		// Sums for each piece attacking an square near the king
		// Ponder kings attacks by the number of attackers (not pawns)
		// Tempo
		// Add to moving side score
		// Squares surrounding King
		public override int Evaluate(Board board, AttacksInfo ai)
		{
			if (debug)
			{
				debugSB = new StringBuilder();
				debugSB.Append("\n");
				debugSB.Append(board.ToString());
				debugSB.Append("\n");
			}
			int whitePawns = BitboardUtils.PopCount(board.pawns & board.whites);
			int blackPawns = BitboardUtils.PopCount(board.pawns & board.blacks);
			int whiteKnights = BitboardUtils.PopCount(board.knights & board.whites);
			int blackKnights = BitboardUtils.PopCount(board.knights & board.blacks);
			int whiteBishops = BitboardUtils.PopCount(board.bishops & board.whites);
			int blackBishops = BitboardUtils.PopCount(board.bishops & board.blacks);
			int whiteRooks = BitboardUtils.PopCount(board.rooks & board.whites);
			int blackRooks = BitboardUtils.PopCount(board.rooks & board.blacks);
			int whiteQueens = BitboardUtils.PopCount(board.queens & board.whites);
			int blackQueens = BitboardUtils.PopCount(board.queens & board.blacks);
			int endGameValue = Endgame.EndGameValue(board, whitePawns, blackPawns, whiteKnights
				, blackKnights, whiteBishops, blackBishops, whiteRooks, blackRooks, whiteQueens, 
				blackQueens);
			if (endGameValue != NoValue)
			{
				return endGameValue;
			}
			pcsq[W] = ((board.whites & board.bishops & Square.Whites) != 0 && (board.whites &
				 board.bishops & Square.Blacks) != 0 ? BishopPair : 0);
			//
			pcsq[B] = ((board.blacks & board.bishops & Square.Whites) != 0 && (board.blacks &
				 board.bishops & Square.Blacks) != 0 ? BishopPair : 0);
			//
			positional[W] = 0;
			positional[B] = 0;
			mobility[W] = 0;
			mobility[B] = 0;
			kingAttackersCount[W] = 0;
			kingAttackersCount[B] = 0;
			kingSafety[W] = 0;
			kingSafety[B] = 0;
			pawnStructure[W] = 0;
			pawnStructure[B] = 0;
			passedPawns[W] = 0;
			passedPawns[B] = 0;
			mobilitySquares[W] = ~board.whites;
			mobilitySquares[B] = ~board.blacks;
			ai.Build(board);
			// Squares that pawns attack or can attack by advancing
			pawnCanAttack[W] = ai.pawnAttacks[W];
			pawnCanAttack[B] = ai.pawnAttacks[B];
			long whitePawnsAux = board.pawns & board.whites;
			long blackPawnsAux = board.pawns & board.blacks;
			for (int i = 0; i < 5; i++)
			{
				whitePawnsAux = whitePawnsAux << 8;
				whitePawnsAux &= ~((board.pawns & board.blacks) | ai.pawnAttacks[B]);
				// Cannot advance because of a blocking pawn or a opposite pawn attack
				blackPawnsAux = (long)(((ulong)blackPawnsAux) >> 8);
				blackPawnsAux &= ~((board.pawns & board.whites) | ai.pawnAttacks[W]);
				// Cannot advance because of a blocking pawn or a opposite pawn attack
				if (whitePawnsAux == 0 || blackPawnsAux == 0)
				{
					break;
				}
				pawnCanAttack[W] |= ((whitePawnsAux & ~BitboardUtils.b_l) << 9) | ((whitePawnsAux
					 & ~BitboardUtils.b_r) << 7);
				pawnCanAttack[B] |= ((long)(((ulong)(blackPawnsAux & ~BitboardUtils.b_r)) >> 9)) 
					| ((long)(((ulong)(whitePawnsAux & ~BitboardUtils.b_l)) >> 7));
			}
			// Calculate attacks
			attacks[W] = EvalAttacks(board, ai, W, board.blacks);
			attacks[B] = EvalAttacks(board, ai, B, board.whites);
			// Squares surrounding King and three squares towards thew other side
			kingZone[W] = bbAttacks.king[ai.kingIndex[W]];
			kingZone[W] |= (kingZone[W] << 8);
			kingZone[B] = bbAttacks.king[ai.kingIndex[B]];
			kingZone[B] |= (kingZone[B] >> 8);
			long all = board.GetAll();
			long pieceAttacks;
			long safeAttacks;
			long kingAttacks;
			bool onlyKingsAndPawns = (board.knights | board.bishops | board.rooks | board.queens
				) == 0;
			long square = 1;
			for (int index = 0; index < 64; index++)
			{
				if ((square & all) != 0)
				{
					bool isWhite = ((board.whites & square) != 0);
					int us = (isWhite ? W : B);
					int them = (isWhite ? B : W);
					long mines = (isWhite ? board.whites : board.blacks);
					long others = (isWhite ? board.blacks : board.whites);
					int pcsqIndex = (isWhite ? index : 63 - index);
					int rank = index >> 3;
					int file = 7 - index & 7;
					pieceAttacks = ai.attacksFromSquare[index];
					if ((square & board.pawns) != 0)
					{
						pcsq[us] += pawnPcsq[pcsqIndex];
						int relativeRank = isWhite ? rank : 7 - rank;
						long myPawns = board.pawns & mines;
						long otherPawns = board.pawns & others;
						long adjacentFiles = BitboardUtils.FilesAdjacent[file];
						long ranksForward = BitboardUtils.RanksForward[us][rank];
						long pawnFile = BitboardUtils.File[file];
						long routeToPromotion = pawnFile & ranksForward;
						long otherPawnsAheadAdjacent = ranksForward & adjacentFiles & otherPawns;
						bool supported = (square & ai.pawnAttacks[us]) != 0;
						bool doubled = (myPawns & routeToPromotion) != 0;
						bool opposed = (otherPawns & routeToPromotion) != 0;
						bool passed = !doubled && !opposed && otherPawnsAheadAdjacent == 0;
						if (!passed)
						{
							long myPawnsAheadAdjacent = ranksForward & adjacentFiles & myPawns;
							long myPawnsBesideAndBehindAdjacent = BitboardUtils.RankAndBackward[us][rank] & adjacentFiles
								 & myPawns;
							bool isolated = (myPawns & adjacentFiles) == 0;
							bool candidate = !doubled && !opposed && (((otherPawnsAheadAdjacent & ~pieceAttacks
								) == 0) || (BitboardUtils.PopCount(myPawnsBesideAndBehindAdjacent) >= BitboardUtils
								.PopCount(otherPawnsAheadAdjacent & ~pieceAttacks)));
							// Can become passer advancing
							// Has more friend pawns beside and behind than opposed pawns controlling his route to promotion
							bool backward = !isolated && !candidate && myPawnsBesideAndBehindAdjacent == 0 &&
								 (pieceAttacks & otherPawns) == 0 && (BitboardUtils.RankAndBackward[us][isWhite ? 
								BitboardUtils.GetRankLsb(myPawnsAheadAdjacent) : BitboardUtils.GetRankMsb(myPawnsAheadAdjacent
								)] & routeToPromotion & (board.pawns | ai.pawnAttacks[them])) != 0;
							// No backwards if it can capture
							// Other pawns stopping it from advance, opposing or capturing it before reaching my pawns
							if (debugPawns)
							{
								bool connected = ((bbAttacks.king[index] & adjacentFiles & myPawns) != 0);
								debugSB.Append("PAWN " + BitboardUtils.SquareNames[index] + (isWhite ? " WHITE " : 
									" BLACK ") + (isolated ? "isolated " : string.Empty) + (supported ? "supported "
									 : string.Empty) + (connected ? "connected " : string.Empty) + (doubled ? "doubled "
									 : string.Empty) + (opposed ? "opposed " : string.Empty) + (candidate ? "candidate "
									 : string.Empty) + (backward ? "backward " : string.Empty) + "\n");
							}
							if (backward)
							{
								pawnStructure[us] -= PawnBackwards[opposed ? 1 : 0];
							}
							if (isolated)
							{
								pawnStructure[us] -= PawnIsolated[opposed ? 1 : 0];
							}
							if (doubled)
							{
								pawnStructure[us] -= PawnDoubled[opposed ? 1 : 0];
							}
							if (!supported && !isolated && !backward)
							{
								pawnStructure[us] -= PawnUnsupported;
							}
							if (candidate)
							{
								passedPawns[us] += PawnCandidate[relativeRank];
							}
							// Pawn Storm: It can open a file near the king
							if ((routeToPromotion & kingZone[them]) != 0)
							{
								pawnStructure[us] += PawnStorm[relativeRank];
							}
						}
						else
						{
							//
							// Passed Pawn
							//
							long backFile = BitboardUtils.File[file] & BitboardUtils.RanksBackward[us][rank];
							// If it has a rook or queen behind consider all the route to promotion attacked or defended
							long attackedAndNotDefendedRoute = ((routeToPromotion & ai.attackedSquares[them])
								 | ((backFile & (board.rooks | board.queens) & others) != 0 ? routeToPromotion : 
								0)) & ~((routeToPromotion & ai.attackedSquares[us]) | ((backFile & (board.rooks 
								| board.queens) & mines) != 0 ? routeToPromotion : 0));
							//
							long pushSquare = isWhite ? square << 8 : (long)(((ulong)square) >> 8);
							bool connected = (bbAttacks.king[index] & adjacentFiles & myPawns) != 0;
							bool outside = otherPawns != 0 && (((square & BitboardUtils.FilesLeft[3]) != 0 &&
								 (board.pawns & BitboardUtils.FilesLeft[file]) == 0) || ((square & BitboardUtils
								.FilesRight[4]) != 0 && (board.pawns & BitboardUtils.FilesRight[file]) == 0));
							bool mobile = (pushSquare & (all | attackedAndNotDefendedRoute)) == 0;
							bool runner = mobile && (routeToPromotion & all) == 0 && attackedAndNotDefendedRoute
								 == 0;
							if (debug)
							{
								debugSB.Append("PAWN " + BitboardUtils.SquareNames[index] + (isWhite ? " WHITE " : 
									" BLACK ") + "passed " + (outside ? "outside " : string.Empty) + (connected ? "connected "
									 : string.Empty) + (supported ? "supported " : string.Empty) + (mobile ? "mobile "
									 : string.Empty) + (runner ? "runner " : string.Empty) + "\n");
							}
							passedPawns[us] += PawnPasser[relativeRank];
							if (outside)
							{
								passedPawns[us] += PawnPasserOutside[relativeRank];
							}
							if (supported)
							{
								passedPawns[us] += PawnPasserSupported[relativeRank];
							}
							else
							{
								if (connected)
								{
									passedPawns[us] += PawnPasserConnected[relativeRank];
								}
							}
							if (runner)
							{
								passedPawns[us] += PawnPasserRunner[relativeRank];
							}
							else
							{
								if (mobile)
								{
									passedPawns[us] += PawnPasserMobile[relativeRank];
								}
							}
							if (onlyKingsAndPawns && runner)
							{
								long promotionSquare = routeToPromotion & (isWhite ? BitboardUtils.Rank[7] : BitboardUtils
									.Rank[0]);
								if ((ai.kingAttacks[us] & promotionSquare) != 0 && (ai.kingAttacks[us] & square) 
									!= 0)
								{
									// The king controls the promotion square
									passedPawns[us] += PawnPasserUnstoppable;
								}
								else
								{
									// Simple pawn square rule implementation
									int ranksToPromo = 7 - relativeRank + (relativeRank == 1 ? -1 : 0);
									// The pawn can advance two squares
									int kingToPromo = BitboardUtils.Distance(BitboardUtils.Square2Index(promotionSquare
										), ai.kingIndex[them]) + (isWhite != board.GetTurn() ? -1 : 0);
									// The other king can move first
									if (kingToPromo > ranksToPromo)
									{
										passedPawns[us] += PawnPasserUnstoppable;
									}
								}
							}
						}
						// Pawn is part of the king shield
						if ((pawnFile & ~ranksForward & kingZone[us]) != 0)
						{
							pawnStructure[us] += PawnShield[relativeRank];
						}
					}
					else
					{
						if ((square & board.knights) != 0)
						{
							pcsq[us] += knightPcsq[pcsqIndex];
							safeAttacks = pieceAttacks & ~ai.pawnAttacks[them];
							mobility[us] += Mobility[Piece.Knight][BitboardUtils.PopCount(safeAttacks & mobilitySquares
								[us] & BitboardUtils.RanksForward[us][rank])];
							kingAttacks = safeAttacks & kingZone[them];
							if (kingAttacks != 0)
							{
								kingSafety[us] += PieceAttacksKing[Piece.Knight] * BitboardUtils.PopCount(kingAttacks
									);
								kingAttackersCount[us]++;
							}
							// Knight outpost: no opposite pawns can attack the square
							if ((square & OutpostMask[us] & ~pawnCanAttack[them]) != 0)
							{
								positional[us] += KnightOutpost[(square & ai.pawnAttacks[us]) != 0 ? 1 : 0];
							}
						}
						else
						{
							if ((square & board.bishops) != 0)
							{
								pcsq[us] += bishopPcsq[pcsqIndex];
								safeAttacks = pieceAttacks & ~ai.pawnAttacks[them];
								mobility[us] += Mobility[Piece.Bishop][BitboardUtils.PopCount(safeAttacks & mobilitySquares
									[us] & BitboardUtils.RanksForward[us][rank])];
								kingAttacks = safeAttacks & kingZone[them];
								if (kingAttacks != 0)
								{
									kingSafety[us] += PieceAttacksKing[Piece.Bishop] * BitboardUtils.PopCount(kingAttacks
										);
									kingAttackersCount[us]++;
								}
								// Bishop Outpost
								if ((square & OutpostMask[us] & ~pawnCanAttack[them]) != 0)
								{
									positional[us] += BishopOutpost[(square & ai.pawnAttacks[us]) != 0 ? 1 : 0];
								}
								positional[us] -= BishopMyPawnsInColorPenalty * BitboardUtils.PopCount(board.pawns
									 & mines & BitboardUtils.GetSameColorSquares(square));
								if ((BishopTrapping[index] & board.pawns & others) != 0)
								{
									mobility[us] -= BishopTrappedPenalty;
								}
							}
							else
							{
								if ((square & board.rooks) != 0)
								{
									pcsq[us] += rookPcsq[pcsqIndex];
									safeAttacks = pieceAttacks & ~ai.pawnAttacks[them] & ~ai.knightAttacks[them] & ~ai
										.bishopAttacks[them];
									mobility[us] += Mobility[Piece.Rook][BitboardUtils.PopCount(safeAttacks & mobilitySquares
										[us])];
									kingAttacks = safeAttacks & kingZone[them];
									if (kingAttacks != 0)
									{
										kingSafety[us] += PieceAttacksKing[Piece.Rook] * BitboardUtils.PopCount(kingAttacks
											);
										kingAttackersCount[us]++;
									}
									if ((square & OutpostMask[us] & ~pawnCanAttack[them]) != 0)
									{
										positional[us] += RookOutpost[(square & ai.pawnAttacks[us]) != 0 ? 1 : 0];
									}
									long rookFile = BitboardUtils.File[file];
									if ((rookFile & board.pawns & mines) == 0)
									{
										positional[us] += RookFile[(rookFile & board.pawns) == 0 ? 0 : 1];
									}
									int relativeRank = isWhite ? rank : 7 - rank;
									if (relativeRank >= 4)
									{
										long pawnsAttacked = pieceAttacks & BitboardUtils.Rank[rank] & board.pawns & others
											 & ~ai.pawnAttacks[them];
										if (pawnsAttacked != 0)
										{
											positional[us] += Rook7 * BitboardUtils.PopCount(pawnsAttacked);
										}
									}
								}
								else
								{
									if ((square & board.queens) != 0)
									{
										pcsq[us] += queenPcsq[pcsqIndex];
										safeAttacks = pieceAttacks & ~ai.pawnAttacks[them] & ~ai.knightAttacks[them] & ~ai
											.bishopAttacks[them] & ~ai.rookAttacks[them];
										mobility[us] += Mobility[Piece.Queen][BitboardUtils.PopCount(safeAttacks & mobilitySquares
											[us])];
										kingAttacks = safeAttacks & kingZone[them];
										if (kingAttacks != 0)
										{
											kingSafety[us] += PieceAttacksKing[Piece.Queen] * BitboardUtils.PopCount(kingAttacks
												);
											kingAttackersCount[us]++;
										}
									}
									else
									{
										if ((square & board.kings) != 0)
										{
											pcsq[us] += kingPcsq[pcsqIndex];
										}
									}
								}
							}
						}
					}
				}
				square <<= 1;
			}
			int oe = (board.GetTurn() ? Tempo : -Tempo) + pcsq[W] - pcsq[B] + positional[W] -
				 positional[B] + attacks[W] - attacks[B] + mobility[W] - mobility[B] + pawnStructure
				[W] - pawnStructure[B] + passedPawns[W] - passedPawns[B] + OeShr(6, KingSafetyPonder
				[kingAttackersCount[W]] * kingSafety[W] - KingSafetyPonder[kingAttackersCount[B]
				] * kingSafety[B]);
			// Ponder opening and Endgame value depending of the non-pawn pieces:
			// opening=> gamephase = 256 / ending => gamephase = 0
			int nonPawnMaterial = (whiteKnights + blackKnights) * Knight + (whiteBishops + blackBishops
				) * Bishop + (whiteRooks + blackRooks) * Rook + (whiteQueens + blackQueens) * Queen;
			int gamePhase = nonPawnMaterial >= NonPawnMaterialMidgameMax ? 256 : nonPawnMaterial
				 <= NonPawnMaterialEndgameMin ? 0 : ((nonPawnMaterial - NonPawnMaterialEndgameMin
				) << 8) / (NonPawnMaterialMidgameMax - NonPawnMaterialEndgameMin);
			int value = (gamePhase * O(oe) + (256 - gamePhase) * E(oe)) >> 8;
			// divide by 256
			if (debug)
			{
				logger.Debug(debugSB);
				logger.Debug("                    WOpening WEndgame BOpening BEndgame");
				logger.Debug("pcsq              = " + FormatOE(pcsq[W]) + " " + FormatOE(pcsq[B])
					);
				logger.Debug("mobility          = " + FormatOE(mobility[W]) + " " + FormatOE(mobility
					[B]));
				logger.Debug("positional        = " + FormatOE(positional[W]) + " " + FormatOE(positional
					[B]));
				logger.Debug("pawnStructure     = " + FormatOE(pawnStructure[W]) + " " + FormatOE
					(pawnStructure[B]));
				logger.Debug("passedPawns       = " + FormatOE(passedPawns[W]) + " " + FormatOE(passedPawns
					[B]));
				logger.Debug("attacks           = " + FormatOE(attacks[W]) + " " + FormatOE(attacks
					[B]));
				logger.Debug("kingSafety        = " + FormatOE(OeShr(6, KingSafetyPonder[kingAttackersCount
					[W]] * kingSafety[W])) + " " + FormatOE(OeShr(6, KingSafetyPonder[kingAttackersCount
					[B]] * kingSafety[B])));
				logger.Debug("tempo             = " + FormatOE(board.GetTurn() ? Tempo : -Tempo));
				logger.Debug("                    -----------------");
				logger.Debug("TOTAL:              " + FormatOE(oe));
				logger.Debug("gamePhase = " + gamePhase + " => value = " + value);
			}
			System.Diagnostics.Debug.Assert(Math.Abs(value) < KnownWin, "Eval is outside limits"
				);
			return value;
		}

		private int EvalAttacks(Board board, AttacksInfo ai, int us, long others)
		{
			int attacks = 0;
			long attackedByPawn = ai.pawnAttacks[us] & others & ~board.pawns;
			while (attackedByPawn != 0)
			{
				long lsb = BitboardUtils.Lsb(attackedByPawn);
				attacks += PawnAttacks[board.GetPieceIntAt(lsb)];
				attackedByPawn &= ~lsb;
			}
			long otherWeak = ai.attackedSquares[us] & others & ~ai.pawnAttacks[1 - us];
			if (otherWeak != 0)
			{
				long attackedByMinor = (ai.knightAttacks[us] | ai.bishopAttacks[us]) & otherWeak;
				while (attackedByMinor != 0)
				{
					long lsb = BitboardUtils.Lsb(attackedByMinor);
					attacks += MinorAttacks[board.GetPieceIntAt(lsb)];
					attackedByMinor &= ~lsb;
				}
				long attackedByMajor = (ai.rookAttacks[us] | ai.queenAttacks[us]) & otherWeak;
				while (attackedByMajor != 0)
				{
					long lsb = BitboardUtils.Lsb(attackedByMajor);
					attacks += MajorAttacks[board.GetPieceIntAt(lsb)];
					attackedByMajor &= ~lsb;
				}
			}
			long superiorAttacks = ai.pawnAttacks[us] & others & ~board.pawns | (ai.knightAttacks
				[us] | ai.bishopAttacks[us]) & others & (board.rooks | board.queens) | ai.rookAttacks
				[us] & others & board.queens;
			int superiorAttacksCount = BitboardUtils.PopCount(superiorAttacks);
			if (superiorAttacksCount >= 2)
			{
				attacks += superiorAttacksCount * HungPieces;
			}
			long pinnedNotPawn = ai.pinnedPieces & ~board.pawns & others;
			if (pinnedNotPawn != 0)
			{
				attacks += PinnedPiece * BitboardUtils.PopCount(pinnedNotPawn);
			}
			return attacks;
		}
	}
}
