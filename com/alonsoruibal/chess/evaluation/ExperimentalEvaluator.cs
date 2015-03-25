using System;
using System.Text;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Log;
using Com.Alonsoruibal.Chess.Util;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	/// <summary>
	/// Evaluation is done in centipawns
	/// <p/>
	/// TODO: bishop / knights / rook traps: revise
	/// </summary>
	/// <author>rui</author>
	public class ExperimentalEvaluator : Evaluator
	{
		private static readonly Logger logger = Logger.GetLogger("ExperimentalEvaluator");

		public const int Pawn = 100;

		public const int Knight = 325;

		public const int Bishop = 325;

		public const int BishopPair = 50;

		public const int Rook = 500;

		public const int Queen = 975;

		public static readonly int[] PieceValues = new int[] { 0, Pawn, Knight, Bishop, Rook
			, Queen, 9999 };

		private static readonly int BishopM = Oe(5, 5);

		private static readonly int BishopAttacksKing = Oe(2, 1);

		private static readonly int BishopDefendsKing = Oe(2, 1);

		private static readonly int BishopAttacksPuP = Oe(3, 4);

		private static readonly int BishopAttacksPuK = Oe(5, 5);

		private static readonly int BishopAttacksRq = Oe(7, 10);

		private static readonly int BishopPawnInColor = Oe(1, 1);

		private static readonly int BishopForwardPPu = Oe(0, 2);

		private static readonly int BishopOutpost = Oe(1, 2);

		private static readonly int BishopOutpostAttNkPu = Oe(3, 4);

		private static readonly int BishopTrapped = Oe(-40, -40);

		private static readonly int KnightM = Oe(6, 8);

		private static readonly int KnightAttacksKing = Oe(4, 2);

		private static readonly int KnightDefendsKing = Oe(4, 2);

		private static readonly int KnightAttacksPuP = Oe(3, 4);

		private static readonly int KnightAttacksPuB = Oe(5, 5);

		private static readonly int KnightAttacksRq = Oe(7, 10);

		private static readonly int KnightOutpost = Oe(2, 3);

		private static readonly int RookM = Oe(2, 3);

		private static readonly int RookAttacksKing = Oe(3, 1);

		private static readonly int RookDefendsKing = Oe(3, 1);

		private static readonly int RookAttacksPuP = Oe(2, 3);

		private static readonly int RookAttacksPuBk = Oe(4, 5);

		private static readonly int RookAttacksQ = Oe(5, 5);

		private static readonly int RookColumnOpenNoMg = Oe(20, 10);

		private static readonly int RookColumnOpenMgNp = Oe(10, 0);

		private static readonly int RookColumnOpenMgP = Oe(15, 5);

		private static readonly int RookColumnSemiopen = Oe(3, 6);

		private static readonly int RookColumnSemiopenBp = Oe(15, 5);

		private static readonly int RookColumnSemiopenK = Oe(3, 6);

		private static readonly int Rook8King8 = Oe(5, 10);

		private static readonly int Rook7Kp78 = Oe(10, 30);

		private static readonly int Rook7P78K8Rq7 = Oe(10, 20);

		private static readonly int Rook6Kp678 = Oe(5, 15);

		private static readonly int RookOutpost = Oe(1, 2);

		private static readonly int RookOutpostAttNkPu = Oe(3, 4);

		private static readonly int QueenM = Oe(2, 2);

		private static readonly int QueenAttacksKing = Oe(5, 2);

		private static readonly int QueenDefendsKing = Oe(5, 2);

		private static readonly int QueenAttacksPu = Oe(4, 4);

		private static readonly int Queen7Kp78 = Oe(5, 25);

		private static readonly int Queen7P78K8R7 = Oe(10, 15);

		private static readonly int KingPawnShield = Oe(5, 0);

		private static readonly int[] KingSafetyPonder = new int[] { 0, 1, 2, 4, 8, 8, 8, 
			8, 8, 8, 8, 8, 8, 8, 8, 8 };

		private static readonly int PawnAttacksKing = Oe(1, 0);

		private static readonly int PawnAttacksKnight = Oe(5, 7);

		private static readonly int PawnAttacksBishop = Oe(5, 7);

		private static readonly int PawnAttacksRook = Oe(7, 10);

		private static readonly int PawnAttacksQueen = Oe(8, 12);

		private static readonly int PawnUnsupported = Oe(-2, 4);

		private static readonly int PawnBackwards = Oe(-10, -15);

		private static readonly int[] PawnIsolated = new int[] { Oe(-15, -20), Oe(-12, -16
			) };

		private static readonly int[] PawnDoubled = new int[] { Oe(-2, -4), Oe(-4, -8) };

		private static readonly int[] PawnCandidate = new int[] { 0, 0, 0, Oe(5, 5), Oe(10
			, 12), Oe(20, 25), 0, 0 };

		private static readonly int[] PawnPasser = new int[] { 0, 0, 0, Oe(10, 10), Oe(20
			, 25), Oe(40, 50), Oe(60, 75), 0 };

		private static readonly int[] PawnPasserOutside = new int[] { 0, 0, 0, 0, Oe(2, 5
			), Oe(5, 10), Oe(10, 20), 0 };

		private static readonly int[] PawnPasserConnected = new int[] { 0, 0, 0, 0, Oe(5, 
			10), Oe(10, 15), Oe(20, 30), 0 };

		private static readonly int[] PawnPasserSupported = new int[] { 0, 0, 0, 0, Oe(5, 
			10), Oe(10, 15), Oe(15, 25), 0 };

		private static readonly int[] PawnPasserMobile = new int[] { 0, 0, 0, Oe(1, 2), Oe
			(2, 3), Oe(3, 5), Oe(5, 10), 0 };

		private static readonly int[] PawnPasserRunner = new int[] { 0, 0, 0, 0, Oe(5, 10
			), Oe(10, 20), Oe(20, 40), 0 };

		private static readonly int HungPieces = Oe(16, 25);

		private static readonly int PinnedPiece = Oe(25, 35);

		public const int Tempo = 9;

		private static readonly long[] OutpostMask = new long[] { unchecked((long)(0x00007e7e7e000000L
			)), unchecked((long)(0x0000007e7e7e0000L)) };

		private static readonly int[] KnightOutpostAttacksNkPu = new int[] { 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Oe(7, 7), Oe(7, 7
			), Oe(10, 10), Oe(10, 10), Oe(7, 7), Oe(7, 7), 0, 0, Oe(5, 5), Oe(5, 5), Oe(8, 8
			), Oe(8, 8), Oe(5, 5), Oe(5, 5), 0, 0, 0, Oe(5, 5), Oe(8, 8), Oe(8, 8), Oe(5, 5)
			, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		private static readonly long[] BishopTrapping = new long[] { 0, 1L << 10, 0, 0, 0
			, 0, 1L << 13, 0, 1L << 17, 0, 0, 0, 0, 0, 0, 1L << 22, 1L << 25, 0, 0, 0, 0, 0, 
			0, 1L << 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1L << 33, 0, 0, 0, 
			0, 0, 0, 1L << 38, 1L << 41, 0, 0, 0, 0, 0, 0, 1L << 46, 0, 1L << 50, 0, 0, 0, 0
			, 1L << 53, 0 };

		private static readonly int[] pawnPcsq = new int[] { Oe(-21, -4), Oe(-9, -6), Oe(
			-3, -8), Oe(4, -10), Oe(4, -10), Oe(-3, -8), Oe(-9, -6), Oe(-21, -4), Oe(-24, -7
			), Oe(-12, -9), Oe(-6, -11), Oe(1, -13), Oe(1, -13), Oe(-6, -11), Oe(-12, -9), Oe
			(-24, -7), Oe(-23, -7), Oe(-11, -9), Oe(-5, -11), Oe(12, -13), Oe(12, -13), Oe(-
			5, -11), Oe(-11, -9), Oe(-23, -7), Oe(-22, -6), Oe(-10, -8), Oe(-4, -10), Oe(23, 
			-12), Oe(23, -12), Oe(-4, -10), Oe(-10, -8), Oe(-22, -6), Oe(-20, -5), Oe(-8, -7
			), Oe(-2, -9), Oe(15, -11), Oe(15, -11), Oe(-2, -9), Oe(-8, -7), Oe(-20, -5), Oe
			(-19, -4), Oe(-7, -6), Oe(-1, -8), Oe(6, -10), Oe(6, -10), Oe(-1, -8), Oe(-7, -6
			), Oe(-19, -4), Oe(-18, -2), Oe(-6, -4), Oe(0, -6), Oe(7, -8), Oe(7, -8), Oe(0, 
			-6), Oe(-6, -4), Oe(-18, -2), Oe(-21, -4), Oe(-9, -6), Oe(-3, -8), Oe(4, -10), Oe
			(4, -10), Oe(-3, -8), Oe(-9, -6), Oe(-21, -4) };

		private static readonly int[] knightPcsq = new int[] { Oe(-59, -22), Oe(-43, -17)
			, Oe(-32, -12), Oe(-28, -9), Oe(-28, -9), Oe(-32, -12), Oe(-43, -17), Oe(-59, -22
			), Oe(-37, -15), Oe(-21, -8), Oe(-10, -4), Oe(-6, -2), Oe(-6, -2), Oe(-10, -4), 
			Oe(-21, -8), Oe(-37, -15), Oe(-21, -10), Oe(-5, -4), Oe(7, 1), Oe(11, 3), Oe(11, 
			3), Oe(7, 1), Oe(-5, -4), Oe(-21, -10), Oe(-12, -6), Oe(4, -1), Oe(16, 4), Oe(20
			, 8), Oe(20, 8), Oe(16, 4), Oe(4, -1), Oe(-12, -6), Oe(-6, -4), Oe(11, 1), Oe(22
			, 6), Oe(26, 10), Oe(26, 10), Oe(22, 6), Oe(11, 1), Oe(-6, -4), Oe(-8, -3), Oe(9
			, 3), Oe(20, 8), Oe(24, 10), Oe(24, 10), Oe(20, 8), Oe(9, 3), Oe(-8, -3), Oe(-17
			, -8), Oe(-1, -1), Oe(11, 3), Oe(15, 5), Oe(15, 5), Oe(11, 3), Oe(-1, -1), Oe(-17
			, -8), Oe(-38, -15), Oe(-22, -10), Oe(-11, -5), Oe(-7, -2), Oe(-7, -2), Oe(-11, 
			-5), Oe(-22, -10), Oe(-38, -15) };

		private static readonly int[] bishopPcsq = new int[] { Oe(-7, 0), Oe(-9, -1), Oe(
			-12, -2), Oe(-14, -2), Oe(-14, -2), Oe(-12, -2), Oe(-9, -1), Oe(-7, 0), Oe(-4, -
			1), Oe(3, 1), Oe(0, 0), Oe(-2, 0), Oe(-2, 0), Oe(0, 0), Oe(3, 1), Oe(-4, -1), Oe
			(-7, -2), Oe(0, 0), Oe(7, 3), Oe(6, 2), Oe(6, 2), Oe(7, 3), Oe(0, 0), Oe(-7, -2)
			, Oe(-9, -2), Oe(-2, 0), Oe(6, 2), Oe(15, 5), Oe(15, 5), Oe(6, 2), Oe(-2, 0), Oe
			(-9, -2), Oe(-9, -2), Oe(-2, 0), Oe(6, 2), Oe(15, 5), Oe(15, 5), Oe(6, 2), Oe(-2
			, 0), Oe(-9, -2), Oe(-7, -2), Oe(0, 0), Oe(7, 3), Oe(6, 2), Oe(6, 2), Oe(7, 3), 
			Oe(0, 0), Oe(-7, -2), Oe(-4, -1), Oe(3, 1), Oe(0, 0), Oe(-2, 0), Oe(-2, 0), Oe(0
			, 0), Oe(3, 1), Oe(-4, -1), Oe(-2, 0), Oe(-4, -1), Oe(-7, -2), Oe(-9, -2), Oe(-9
			, -2), Oe(-7, -2), Oe(-4, -1), Oe(-2, 0) };

		private static readonly int[] rookPcsq = new int[] { Oe(-4, 0), Oe(0, 0), Oe(4, 0
			), Oe(8, 0), Oe(8, 0), Oe(4, 0), Oe(0, 0), Oe(-4, 0), Oe(-4, 0), Oe(0, 0), Oe(4, 
			0), Oe(8, 0), Oe(8, 0), Oe(4, 0), Oe(0, 0), Oe(-4, 0), Oe(-4, 0), Oe(0, 0), Oe(4
			, 0), Oe(8, 0), Oe(8, 0), Oe(4, 0), Oe(0, 0), Oe(-4, 0), Oe(-4, 0), Oe(0, 0), Oe
			(4, 0), Oe(8, 0), Oe(8, 0), Oe(4, 0), Oe(0, 0), Oe(-4, 0), Oe(-4, 1), Oe(0, 1), 
			Oe(4, 1), Oe(8, 1), Oe(8, 1), Oe(4, 1), Oe(0, 1), Oe(-4, 1), Oe(-4, 1), Oe(0, 1)
			, Oe(4, 1), Oe(8, 1), Oe(8, 1), Oe(4, 1), Oe(0, 1), Oe(-4, 1), Oe(-4, 1), Oe(0, 
			1), Oe(4, 1), Oe(8, 1), Oe(8, 1), Oe(4, 1), Oe(0, 1), Oe(-4, 1), Oe(-5, -2), Oe(
			-1, -2), Oe(3, -2), Oe(7, -2), Oe(7, -2), Oe(3, -2), Oe(-1, -2), Oe(-5, -2) };

		private static readonly int[] queenPcsq = new int[] { Oe(-12, -15), Oe(-8, -10), 
			Oe(-5, -8), Oe(-3, -7), Oe(-3, -7), Oe(-5, -8), Oe(-8, -10), Oe(-12, -15), Oe(-8
			, -10), Oe(-2, -5), Oe(0, -3), Oe(2, -2), Oe(2, -2), Oe(0, -3), Oe(-2, -5), Oe(-
			8, -10), Oe(-5, -8), Oe(0, -3), Oe(5, 0), Oe(6, 2), Oe(6, 2), Oe(5, 0), Oe(0, -3
			), Oe(-5, -8), Oe(-3, -7), Oe(2, -2), Oe(6, 2), Oe(9, 5), Oe(9, 5), Oe(6, 2), Oe
			(2, -2), Oe(-3, -7), Oe(-3, -7), Oe(2, -2), Oe(6, 2), Oe(9, 5), Oe(9, 5), Oe(6, 
			2), Oe(2, -2), Oe(-3, -7), Oe(-5, -8), Oe(0, -3), Oe(5, 0), Oe(6, 2), Oe(6, 2), 
			Oe(5, 0), Oe(0, -3), Oe(-5, -8), Oe(-8, -10), Oe(-2, -5), Oe(0, -3), Oe(2, -2), 
			Oe(2, -2), Oe(0, -3), Oe(-2, -5), Oe(-8, -10), Oe(-12, -15), Oe(-8, -10), Oe(-5, 
			-8), Oe(-3, -7), Oe(-3, -7), Oe(-5, -8), Oe(-8, -10), Oe(-12, -15) };

		private static readonly int[] kingPcsq = new int[] { Oe(43, -58), Oe(48, -35), Oe
			(18, -19), Oe(-2, -13), Oe(-2, -13), Oe(18, -19), Oe(48, -35), Oe(43, -58), Oe(40
			, -35), Oe(45, -10), Oe(16, 2), Oe(-4, 8), Oe(-4, 8), Oe(16, 2), Oe(45, -10), Oe
			(40, -35), Oe(37, -19), Oe(43, 2), Oe(13, 17), Oe(-7, 23), Oe(-7, 23), Oe(13, 17
			), Oe(43, 2), Oe(37, -19), Oe(34, -13), Oe(40, 8), Oe(10, 23), Oe(-10, 32), Oe(-
			10, 32), Oe(10, 23), Oe(40, 8), Oe(34, -13), Oe(29, -13), Oe(35, 8), Oe(5, 23), 
			Oe(-15, 32), Oe(-15, 32), Oe(5, 23), Oe(35, 8), Oe(29, -13), Oe(24, -19), Oe(30, 
			2), Oe(0, 17), Oe(-20, 23), Oe(-20, 23), Oe(0, 17), Oe(30, 2), Oe(24, -19), Oe(14
			, -35), Oe(19, -10), Oe(-10, 2), Oe(-30, 8), Oe(-30, 8), Oe(-10, 2), Oe(19, -10)
			, Oe(14, -35), Oe(4, -58), Oe(9, -35), Oe(-21, -19), Oe(-41, -13), Oe(-41, -13), 
			Oe(-21, -19), Oe(9, -35), Oe(4, -58) };

		private Config config;

		public bool debug = false;

		public StringBuilder debugSB;

		private int[] pawnMaterial = new int[] { 0, 0 };

		private int[] material = new int[] { 0, 0 };

		private int[] center = new int[] { 0, 0 };

		private int[] positional = new int[] { 0, 0 };

		private int[] mobility = new int[] { 0, 0 };

		private int[] attacks = new int[] { 0, 0 };

		private int[] kingAttackersCount = new int[] { 0, 0 };

		private int[] kingSafety = new int[] { 0, 0 };

		private int[] kingDefense = new int[] { 0, 0 };

		private int[] pawnStructure = new int[] { 0, 0 };

		private int[] passedPawns = new int[] { 0, 0 };

		private long[] superiorPieceAttacked = new long[] { 0, 0 };

		private long[] pawnAttacks = new long[] { 0, 0 };

		private long[] pawnCanAttack = new long[] { 0, 0 };

		private long[] minorPiecesDefendedByPawns = new long[] { 0, 0 };

		private long[] squaresNearKing = new long[] { 0, 0 };

		public ExperimentalEvaluator(Config config)
		{
			// Bonus by having two bishops in different colors
			// Bishops
			// Mobility units: this value is added for each destination square not occupied by one of our pieces or attacked by opposite pawns
			// Sums for each of our pawns (or opposite/2) in the bishop color
			// Sums for each of the undefended opposite pawns forward
			// Only if defended by pawn
			// attacks squares Near King or other opposite pieces Pawn Undefended
			// Knights
			// Adds one time if no opposite can can attack out knight and twice if it is defended by one of our pawns
			// Rooks
			// Attacks pawn not defended by pawn (PU=Pawn Undefended)
			// Attacks bishop or knight not defended by pawn
			// Attacks queen
			// No pawns in rook column and no minor guarded
			// No pawns in rook column and minor guarded, my pawn can attack
			// No pawns in rook column and minor guarded, my pawn can attack
			// No pawns mines in column
			// And attacks a backward pawn
			// No pawns mines in column and opposite king
			// Rook in 8th rank and opposite king in 8th rank
			// Rook in 8th rank and opposite king/pawn in 7/8th rank
			// Rook in 7th rank and opposite king in 8th and attacked opposite queen/rook on 7th
			// Rook in 7th rank and opposite king/pawn in 6/7/8th
			// Only if defended by pawn
			// Also attacks other piece not defended by pawn or a square near king
			// Queen
			// Queen in 8th rank and opposite king/pawn in 7/8th rank
			// Queen in 7th my root in 7th defending queen and opposite king in 8th
			// King
			// Protection: sums for each pawn near king
			// Ponder kings attacks by the number of attackers (not pawns)
			// Pawns
			// Sums for each pawn attacking an square near the king or the king
			// Sums for each pawn attacking a KNIGHT
			// Array is not opposed, opposed
			// Array by relative rank
			// Candidates to pawn passer
			// no opposite pawns at left or at right
			// defended by pawn
			// two or more pieces of the other side attacked by inferior pieces
			// Tempo
			// Add to moving side score
			// Knight outpost attacks squares Near King or other opposite pieces Pawn Undefended
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			//
			// Squares attackeds by pawns
			// Squares surrounding King
			this.config = config;
		}

		public override int Evaluate(Board board, AttacksInfo attacksInfo)
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
			int endGameValue = EndgameEvaluator.EndGameValue(board, whitePawns, blackPawns, whiteKnights
				, blackKnights, whiteBishops, blackBishops, whiteRooks, blackRooks, whiteQueens, 
				blackQueens);
			if (endGameValue != Evaluator.NoValue)
			{
				return endGameValue;
			}
			pawnMaterial[0] = Pawn * whitePawns;
			pawnMaterial[1] = Pawn * blackPawns;
			material[0] = Knight * whiteKnights + Bishop * whiteBishops + Rook * whiteRooks +
				 Queen * whiteQueens + ((board.whites & board.bishops & BitboardUtils.WhiteSquares
				) != 0 && (board.whites & board.bishops & BitboardUtils.BlackSquares) != 0 ? BishopPair
				 : 0);
			//
			//
			material[1] = Knight * blackKnights + Bishop * blackBishops + Rook * blackRooks +
				 Queen * blackQueens + ((board.blacks & board.bishops & BitboardUtils.WhiteSquares
				) != 0 && (board.blacks & board.bishops & BitboardUtils.BlackSquares) != 0 ? BishopPair
				 : 0);
			//
			//
			center[0] = 0;
			center[1] = 0;
			positional[0] = 0;
			positional[1] = 0;
			mobility[0] = 0;
			mobility[1] = 0;
			kingAttackersCount[0] = 0;
			kingAttackersCount[1] = 0;
			kingSafety[0] = 0;
			kingSafety[1] = 0;
			kingDefense[0] = 0;
			kingDefense[1] = 0;
			pawnStructure[0] = 0;
			pawnStructure[1] = 0;
			passedPawns[0] = 0;
			passedPawns[1] = 0;
			superiorPieceAttacked[0] = 0;
			superiorPieceAttacked[1] = 0;
			// Squares attacked by pawns
			pawnAttacks[0] = ((board.pawns & board.whites & ~BitboardUtils.b_l) << 9) | ((board
				.pawns & board.whites & ~BitboardUtils.b_r) << 7);
			pawnAttacks[1] = ((long)(((ulong)(board.pawns & board.blacks & ~BitboardUtils.b_r
				)) >> 9)) | ((long)(((ulong)(board.pawns & board.blacks & ~BitboardUtils.b_l)) >>
				 7));
			// Squares that pawns attack or can attack by advancing
			pawnCanAttack[0] = pawnAttacks[0] | pawnAttacks[0] << 8 | pawnAttacks[0] << 16 | 
				pawnAttacks[0] << 24 | pawnAttacks[0] << 32 | pawnAttacks[0] << 40;
			pawnCanAttack[1] = pawnAttacks[1] | (long)(((ulong)pawnAttacks[1]) >> 8) | (long)
				(((ulong)pawnAttacks[1]) >> 16) | (long)(((ulong)pawnAttacks[1]) >> 24) | (long)
				(((ulong)pawnAttacks[1]) >> 32) | (long)(((ulong)pawnAttacks[1]) >> 40);
			// Initialize attacks with pawn attacks
			attacks[0] = PawnAttacksKnight * BitboardUtils.PopCount(pawnAttacks[0] & board.knights
				 & board.blacks) + PawnAttacksBishop * BitboardUtils.PopCount(pawnAttacks[0] & board
				.bishops & board.blacks) + PawnAttacksRook * BitboardUtils.PopCount(pawnAttacks[
				0] & board.rooks & board.blacks) + PawnAttacksQueen * BitboardUtils.PopCount(pawnAttacks
				[0] & board.queens & board.blacks);
			//
			//
			//
			attacks[1] = PawnAttacksKnight * BitboardUtils.PopCount(pawnAttacks[1] & board.knights
				 & board.whites) + PawnAttacksBishop * BitboardUtils.PopCount(pawnAttacks[1] & board
				.bishops & board.whites) + PawnAttacksRook * BitboardUtils.PopCount(pawnAttacks[
				1] & board.rooks & board.whites) + PawnAttacksQueen * BitboardUtils.PopCount(pawnAttacks
				[1] & board.queens & board.whites);
			//
			//
			//
			minorPiecesDefendedByPawns[0] = board.whites & (board.bishops | board.knights) & 
				pawnAttacks[0];
			minorPiecesDefendedByPawns[1] = board.blacks & (board.bishops | board.knights) & 
				pawnAttacks[1];
			// Squares surrounding King
			squaresNearKing[0] = bbAttacks.king[BitboardUtils.Square2Index(board.whites & board
				.kings)] | board.whites & board.kings;
			squaresNearKing[1] = bbAttacks.king[BitboardUtils.Square2Index(board.blacks & board
				.kings)] | board.blacks & board.kings;
			attacksInfo.Build(board);
			long all = board.GetAll();
			long pieceAttacks;
			long pieceAttacksXray;
			long auxLong;
			long auxLong2;
			long square = 1;
			for (int index = 0; index < 64; index++)
			{
				if ((square & all) != 0)
				{
					bool isWhite = ((board.whites & square) != 0);
					int color = (isWhite ? 0 : 1);
					long mines = (isWhite ? board.whites : board.blacks);
					long others = (isWhite ? board.blacks : board.whites);
					long otherPawnAttacks = (isWhite ? pawnAttacks[1] : pawnAttacks[0]);
					int pcsqIndex = (isWhite ? index : 63 - index);
					int rank = index >> 3;
					int column = 7 - index & 7;
					pieceAttacks = attacksInfo.attacksFromSquare[index];
					if ((square & board.pawns) != 0)
					{
						center[color] += pawnPcsq[pcsqIndex];
						if ((pieceAttacks & squaresNearKing[1 - color] & ~otherPawnAttacks) != 0)
						{
							kingSafety[color] += PawnAttacksKing;
						}
						superiorPieceAttacked[color] |= pieceAttacks & others & (board.knights | board.bishops
							 | board.rooks | board.queens);
						long myPawns = board.pawns & mines;
						long otherPawns = board.pawns & others;
						long adjacentColumns = BitboardUtils.ColumnsAdjacents[column];
						long ranksForward = BitboardUtils.RanksForward[color][rank];
						long routeToPromotion = BitboardUtils.Column[column] & ranksForward;
						long myPawnsBesideAndBehindAdjacent = BitboardUtils.RankAndBackward[color][rank] 
							& adjacentColumns & myPawns;
						long myPawnsAheadAdjacent = ranksForward & adjacentColumns & myPawns;
						long otherPawnsAheadAdjacent = ranksForward & adjacentColumns & otherPawns;
						long myPawnAttacks = isWhite ? pawnAttacks[0] : pawnAttacks[1];
						bool isolated = (myPawns & adjacentColumns) == 0;
						bool supported = (square & myPawnAttacks) != 0;
						bool doubled = (myPawns & routeToPromotion) != 0;
						bool opposed = (otherPawns & routeToPromotion) != 0;
						bool passed = !doubled && !opposed && (otherPawnsAheadAdjacent == 0);
						bool candidate = !doubled && !opposed && !passed && (((otherPawnsAheadAdjacent & 
							~pieceAttacks) == 0) || (BitboardUtils.PopCount(myPawnsBesideAndBehindAdjacent) 
							>= BitboardUtils.PopCount(otherPawnsAheadAdjacent)));
						// Can become passer advancing
						// Has more friend pawns beside and behind than opposed pawns controlling his route to promotion
						bool backwards = !isolated && !passed && !candidate && myPawnsBesideAndBehindAdjacent
							 == 0 && (pieceAttacks & otherPawns) == 0 && (BitboardUtils.RankAndBackward[color
							][isWhite ? BitboardUtils.GetRankLsb(myPawnsAheadAdjacent) : BitboardUtils.GetRankMsb
							(myPawnsAheadAdjacent)] & routeToPromotion & (board.pawns | otherPawnAttacks)) !=
							 0;
						// No backwards if it can capture
						// Other pawns stopping it from advance, opposing or capturing it before reaching my pawns
						if (debug)
						{
							bool connected = ((bbAttacks.king[index] & adjacentColumns & myPawns) != 0);
							debugSB.Append("PAWN " + index + (color == 0 ? " WHITE " : " BLACK ") + BitboardUtils
								.PopCount(myPawnsBesideAndBehindAdjacent) + " " + BitboardUtils.PopCount(otherPawnsAheadAdjacent
								) + " " + (isolated ? "isolated " : string.Empty) + (supported ? "supported " : 
								string.Empty) + (connected ? "connected " : string.Empty) + (doubled ? "doubled "
								 : string.Empty) + (opposed ? "opposed " : string.Empty) + (passed ? "passed " : 
								string.Empty) + (candidate ? "candidate " : string.Empty) + (backwards ? "backwards "
								 : string.Empty) + "\n");
						}
						//
						//
						//
						//
						//
						//
						//
						//
						//
						//
						//
						//
						if (!supported && !isolated)
						{
							pawnStructure[color] += PawnUnsupported;
						}
						if (doubled)
						{
							pawnStructure[color] += PawnDoubled[opposed ? 1 : 0];
						}
						if (isolated)
						{
							pawnStructure[color] += PawnIsolated[opposed ? 1 : 0];
						}
						if (backwards)
						{
							pawnStructure[color] += PawnBackwards;
						}
						if (candidate)
						{
							passedPawns[color] += PawnCandidate[(isWhite ? rank : 7 - rank)];
						}
						if (passed)
						{
							int relativeRank = isWhite ? rank : 7 - rank;
							long backColumn = BitboardUtils.Column[column] & BitboardUtils.RanksBackward[color
								][rank];
							// If has has root/queen behind consider all the route to promotion attacked or defended
							long attackedAndNotDefendedRoute = ((routeToPromotion & attacksInfo.attackedSquares
								[1 - color]) | ((backColumn & (board.rooks | board.queens) & others) != 0 ? routeToPromotion
								 : 0)) & ~((routeToPromotion & attacksInfo.attackedSquares[color]) | ((backColumn
								 & (board.rooks | board.queens) & mines) != 0 ? routeToPromotion : 0));
							//
							long pushSquare = isWhite ? square << 8 : (long)(((ulong)square) >> 8);
							long pawnsLeft = BitboardUtils.RowsLeft[column] & board.pawns;
							long pawnsRight = BitboardUtils.RowsRight[column] & board.pawns;
							bool connected = ((bbAttacks.king[index] & adjacentColumns & myPawns) != 0);
							bool outside = ((pawnsLeft != 0) && (pawnsRight == 0)) || ((pawnsLeft == 0) && (pawnsRight
								 != 0));
							bool mobile = ((pushSquare & (all | attackedAndNotDefendedRoute)) == 0);
							bool runner = mobile && ((routeToPromotion & all) == 0) && (attackedAndNotDefendedRoute
								 == 0);
							if (debug)
							{
								debugSB.Append("        PASSER " + (outside ? "outside " : string.Empty) + (mobile
									 ? "mobile " : string.Empty) + (runner ? "runner " : string.Empty) + "\n");
							}
							//
							//
							//
							//
							passedPawns[color] += PawnPasser[relativeRank];
							if (supported)
							{
								passedPawns[color] += PawnPasserSupported[relativeRank];
							}
							if (connected)
							{
								passedPawns[color] += PawnPasserConnected[relativeRank];
							}
							if (outside)
							{
								passedPawns[color] += PawnPasserOutside[relativeRank];
							}
							if (mobile)
							{
								passedPawns[color] += PawnPasserMobile[relativeRank];
							}
							if (runner)
							{
								passedPawns[color] += PawnPasserRunner[relativeRank];
							}
						}
					}
					else
					{
						if ((square & board.knights) != 0)
						{
							center[color] += knightPcsq[pcsqIndex];
							// Only mobility forward
							mobility[color] += KnightM * BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
								 & BitboardUtils.RanksForward[color][rank]);
							if ((pieceAttacks & squaresNearKing[1 - color] & ~otherPawnAttacks) != 0)
							{
								kingSafety[color] += KnightAttacksKing;
								kingAttackersCount[color]++;
							}
							if ((pieceAttacks & squaresNearKing[color]) != 0)
							{
								kingDefense[color] += KnightDefendsKing;
							}
							if ((pieceAttacks & board.pawns & others & ~otherPawnAttacks) != 0)
							{
								attacks[color] += KnightAttacksPuP;
							}
							if ((pieceAttacks & board.bishops & others & ~otherPawnAttacks) != 0)
							{
								attacks[color] += KnightAttacksPuB;
							}
							if ((pieceAttacks & (board.rooks | board.queens) & others) != 0)
							{
								attacks[color] += KnightAttacksRq;
							}
							superiorPieceAttacked[color] |= pieceAttacks & others & (board.rooks | board.queens
								);
							// Knight outpost: no opposite pawns can attack the square
							if ((square & OutpostMask[color] & ~pawnCanAttack[1 - color]) != 0)
							{
								positional[color] += KnightOutpost;
								// Defended by one of our pawns
								if ((square & pawnAttacks[color]) != 0)
								{
									positional[color] += KnightOutpost;
									// Attacks squares near king or other pieces pawn undefended
									if ((pieceAttacks & (squaresNearKing[1 - color] | others) & ~otherPawnAttacks) !=
										 0)
									{
										positional[color] += KnightOutpostAttacksNkPu[pcsqIndex];
									}
								}
							}
						}
						else
						{
							if ((square & board.bishops) != 0)
							{
								center[color] += bishopPcsq[pcsqIndex];
								mobility[color] += BishopM * BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
									 & BitboardUtils.RanksForward[color][rank]);
								if ((pieceAttacks & squaresNearKing[1 - color] & ~otherPawnAttacks) != 0)
								{
									kingSafety[color] += BishopAttacksKing;
									kingAttackersCount[color]++;
								}
								if ((pieceAttacks & squaresNearKing[color]) != 0)
								{
									kingDefense[color] += BishopDefendsKing;
								}
								if ((pieceAttacks & board.pawns & others & ~otherPawnAttacks) != 0)
								{
									attacks[color] += BishopAttacksPuP;
								}
								if ((pieceAttacks & board.knights & others & ~otherPawnAttacks) != 0)
								{
									attacks[color] += BishopAttacksPuK;
								}
								if ((pieceAttacks & (board.rooks | board.queens) & others) != 0)
								{
									attacks[color] += BishopAttacksRq;
								}
								superiorPieceAttacked[color] |= pieceAttacks & others & (board.rooks | board.queens
									);
								pieceAttacksXray = bbAttacks.GetBishopAttacks(index, all & ~(pieceAttacks & others
									 & ~board.pawns)) & ~pieceAttacks;
								if ((pieceAttacksXray & (board.rooks | board.queens | board.kings) & others) != 0)
								{
									attacks[color] += PinnedPiece;
								}
								// Bishop Outpost: no opposite pawns can attack the square and defended by one of our pawns
								if ((square & OutpostMask[color] & ~pawnCanAttack[1 - color] & pawnAttacks[color]
									) != 0)
								{
									positional[color] += BishopOutpost;
									// Attacks squares near king or other pieces pawn undefended
									if ((pieceAttacks & (squaresNearKing[1 - color] | others) & ~otherPawnAttacks) !=
										 0)
									{
										positional[color] += BishopOutpostAttNkPu;
									}
								}
								// auxLong = pawns in bishop color
								if ((square & BitboardUtils.WhiteSquares) != 0)
								{
									auxLong = BitboardUtils.WhiteSquares & board.pawns;
								}
								else
								{
									auxLong = BitboardUtils.BlackSquares & board.pawns;
								}
								positional[color] += BishopPawnInColor * ((int)(((uint)BitboardUtils.PopCount(auxLong
									 & mines) + BitboardUtils.PopCount(auxLong & others)) >> 1)) + BishopForwardPPu 
									* ((int)(((uint)BitboardUtils.PopCount(auxLong & others & BitboardUtils.RanksForward
									[color][rank])) >> 1));
								if ((BishopTrapping[index] & board.pawns & others) != 0)
								{
									mobility[color] += BishopTrapped;
								}
							}
							else
							{
								// TODO protection
								if ((square & board.rooks) != 0)
								{
									center[color] += rookPcsq[pcsqIndex];
									mobility[color] += RookM * BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
										);
									if ((pieceAttacks & squaresNearKing[1 - color] & ~otherPawnAttacks) != 0)
									{
										kingSafety[color] += RookAttacksKing;
										kingAttackersCount[color]++;
									}
									if ((pieceAttacks & squaresNearKing[color]) != 0)
									{
										kingDefense[color] += RookDefendsKing;
									}
									if ((pieceAttacks & board.pawns & others & ~otherPawnAttacks) != 0)
									{
										attacks[color] += RookAttacksPuP;
									}
									if ((pieceAttacks & (board.bishops | board.knights) & others & ~otherPawnAttacks)
										 != 0)
									{
										attacks[color] += RookAttacksPuBk;
									}
									if ((pieceAttacks & board.queens & others) != 0)
									{
										attacks[color] += RookAttacksQ;
									}
									superiorPieceAttacked[color] |= pieceAttacks & others & board.queens;
									pieceAttacksXray = bbAttacks.GetRookAttacks(index, all & ~(pieceAttacks & others 
										& ~board.pawns)) & ~pieceAttacks;
									if ((pieceAttacksXray & (board.queens | board.kings) & others) != 0)
									{
										attacks[color] += PinnedPiece;
									}
									auxLong = (isWhite ? BitboardUtils.b_u : BitboardUtils.b_d);
									if ((square & auxLong) != 0 && (others & board.kings & auxLong) != 0)
									{
										positional[color] += Rook8King8;
									}
									if ((square & (isWhite ? BitboardUtils.r2_u : BitboardUtils.r2_d)) != 0 & (others
										 & (board.kings | board.pawns) & (isWhite ? BitboardUtils.b2_u : BitboardUtils.b2_d
										)) != 0)
									{
										positional[color] += Rook7Kp78;
										if ((others & board.kings & auxLong) != 0 && (pieceAttacks & others & (board.queens
											 | board.rooks) & (isWhite ? BitboardUtils.r2_u : BitboardUtils.r2_d)) != 0)
										{
											positional[color] += Rook7P78K8Rq7;
										}
									}
									if ((square & (isWhite ? BitboardUtils.r3_u : BitboardUtils.r3_d)) != 0 & (others
										 & (board.kings | board.pawns) & (isWhite ? BitboardUtils.b3_u : BitboardUtils.b3_d
										)) != 0)
									{
										positional[color] += Rook6Kp678;
									}
									auxLong = BitboardUtils.Column[column] & BitboardUtils.RanksForward[color][rank];
									if ((auxLong & board.pawns & mines) == 0)
									{
										positional[color] += RookColumnSemiopen;
										if ((auxLong & board.pawns) == 0)
										{
											if ((auxLong & minorPiecesDefendedByPawns[1 - color]) == 0)
											{
												positional[color] += RookColumnOpenNoMg;
											}
											else
											{
												if ((auxLong & minorPiecesDefendedByPawns[1 - color] & pawnCanAttack[color]) == 0)
												{
													positional[color] += RookColumnOpenMgNp;
												}
												else
												{
													positional[color] += RookColumnOpenMgP;
												}
											}
										}
										else
										{
											// There is an opposite backward pawn
											if ((auxLong & board.pawns & others & pawnCanAttack[1 - color]) == 0)
											{
												positional[color] += RookColumnSemiopenBp;
											}
										}
										if ((auxLong & board.kings & others) != 0)
										{
											positional[color] += RookColumnSemiopenK;
										}
									}
									// Rook Outpost: no opposite pawns can attack the square and defended by one of our pawns
									if ((square & OutpostMask[color] & ~pawnCanAttack[1 - color] & pawnAttacks[color]
										) != 0)
									{
										positional[color] += RookOutpost;
										// Attacks squares near king or other pieces pawn undefended
										if ((pieceAttacks & (squaresNearKing[1 - color] | others) & ~otherPawnAttacks) !=
											 0)
										{
											positional[color] += RookOutpostAttNkPu;
										}
									}
								}
								else
								{
									if ((square & board.queens) != 0)
									{
										center[color] += queenPcsq[pcsqIndex];
										mobility[color] += QueenM * BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
											);
										if ((pieceAttacks & squaresNearKing[1 - color] & ~otherPawnAttacks) != 0)
										{
											kingSafety[color] += QueenAttacksKing;
											kingAttackersCount[color]++;
										}
										if ((pieceAttacks & squaresNearKing[color]) != 0)
										{
											kingDefense[color] += QueenDefendsKing;
										}
										if ((pieceAttacks & others & ~otherPawnAttacks) != 0)
										{
											attacks[color] += QueenAttacksPu;
										}
										pieceAttacksXray = (bbAttacks.GetRookAttacks(index, all & ~(pieceAttacks & others
											 & ~board.pawns)) | bbAttacks.GetBishopAttacks(index, all & ~(pieceAttacks & others
											 & ~board.pawns))) & ~pieceAttacks;
										if ((pieceAttacksXray & board.kings & others) != 0)
										{
											attacks[color] += PinnedPiece;
										}
										auxLong = (isWhite ? BitboardUtils.b_u : BitboardUtils.b_d);
										auxLong2 = (isWhite ? BitboardUtils.r2_u : BitboardUtils.r2_d);
										if ((square & auxLong2) != 0 && (others & (board.kings | board.pawns) & (auxLong 
											| auxLong2)) != 0)
										{
											attacks[color] += Queen7Kp78;
											if ((board.rooks & mines & auxLong2 & pieceAttacks) != 0 && (board.kings & others
												 & auxLong) != 0)
											{
												positional[color] += Queen7P78K8R7;
											}
										}
									}
									else
									{
										if ((square & board.kings) != 0)
										{
											center[color] += kingPcsq[pcsqIndex];
											// If king is in the first rank, we add the pawn shield
											if ((square & (isWhite ? BitboardUtils.Rank[0] : BitboardUtils.Rank[7])) != 0)
											{
												kingDefense[color] += KingPawnShield * BitboardUtils.PopCount(pieceAttacks & mines
													 & board.pawns);
											}
										}
									}
								}
							}
						}
					}
				}
				square <<= 1;
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
			int supAttWhite = BitboardUtils.PopCount(superiorPieceAttacked[0]);
			int supAttBlack = BitboardUtils.PopCount(superiorPieceAttacked[1]);
			int hungPieces = (supAttWhite >= 2 ? supAttWhite * HungPieces : 0) - (supAttBlack
				 >= 2 ? supAttBlack * HungPieces : 0);
			int oe = OeMul(config.GetEvalCenter(), center[0] - center[1]) + OeMul(config.GetEvalPositional
				(), positional[0] - positional[1]) + OeMul(config.GetEvalAttacks(), attacks[0] -
				 attacks[1] + hungPieces) + OeMul(config.GetEvalMobility(), mobility[0] - mobility
				[1]) + OeMul(config.GetEvalPawnStructure(), pawnStructure[0] - pawnStructure[1])
				 + OeMul(config.GetEvalPassedPawns(), passedPawns[0] - passedPawns[1]) + OeMul(config
				.GetEvalKingSafety(), kingDefense[0] - kingDefense[1] + (KingSafetyPonder[kingAttackersCount
				[0]] * kingSafety[0] - KingSafetyPonder[kingAttackersCount[1]] * kingSafety[1]));
			value += (gamePhase * O(oe)) / (256 * 100);
			// divide by 256
			value += ((256 - gamePhase) * E(oe)) / (256 * 100);
			if (debug)
			{
				logger.Debug(debugSB);
				logger.Debug("material          = " + (material[0] - material[1]));
				logger.Debug("pawnMaterial      = " + (pawnMaterial[0] - pawnMaterial[1]));
				logger.Debug("tempo             = " + (board.GetTurn() ? Tempo : -Tempo));
				logger.Debug("gamePhase         = " + gamePhase);
				logger.Debug("                     Opening  Endgame");
				logger.Debug("center            = " + FormatOE(center[0] - center[1]));
				logger.Debug("positional        = " + FormatOE(positional[0] - positional[1]));
				logger.Debug("attacks           = " + FormatOE(attacks[0] - attacks[1]));
				logger.Debug("mobility          = " + FormatOE(mobility[0] - mobility[1]));
				logger.Debug("pawnStructure     = " + FormatOE(pawnStructure[0] - pawnStructure[1
					]));
				logger.Debug("passedPawns       = " + FormatOE(passedPawns[0] - passedPawns[1]));
				logger.Debug("kingSafety        = " + FormatOE(KingSafetyPonder[kingAttackersCount
					[0]] * kingSafety[0] - KingSafetyPonder[kingAttackersCount[1]] * kingSafety[1]));
				logger.Debug("kingDefense       = " + FormatOE(kingDefense[0] - kingDefense[1]));
				logger.Debug("value             = " + value);
			}
			System.Diagnostics.Debug.Assert(Math.Abs(value) < Evaluator.KnownWin, "Eval is outside limits"
				);
			return value;
		}

		private string FormatOE(int value)
		{
			return StringUtils.PadLeft(O(value).ToString(), 8) + " " + StringUtils.PadLeft(E(
				value).ToString(), 8);
		}
	}
}
