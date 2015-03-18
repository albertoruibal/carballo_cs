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
	/// Material imbalances from Larry KaufMan:
	/// http://home.comcast.net/~danheisman/Articles/evaluation_of_material_imbalance.htm
	/// <p/>
	/// Piece/square values like Fruit/Toga
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

		private static readonly int PawnAttacksKing = Oe(1, 0);

		private static readonly int KnightAttacksKing = Oe(4, 0);

		private static readonly int BishopAttacksKing = Oe(2, 0);

		private static readonly int RookAttacksKing = Oe(3, 0);

		private static readonly int QueenAttacksKing = Oe(5, 0);

		private static readonly int KingPawnShield = Oe(5, 0);

		private static readonly int[] KingSafetyPonder = new int[] { 0, 1, 4, 8, 16, 25, 
			36, 49, 50, 50, 50, 50, 50, 50, 50, 50 };

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

		public const int Tempo = 10;

		private static readonly int[] KnigthOutpost = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Oe(7, 7), Oe(9, 9), Oe(9, 9), Oe(7, 7), 0, 0, 0, Oe
			(5, 5), Oe(10, 10), Oe(20, 20), Oe(20, 20), Oe(10, 10), Oe(5, 5), 0, 0, Oe(5, 5)
			, Oe(10, 10), Oe(20, 20), Oe(20, 20), Oe(10, 10), Oe(5, 5), 0, 0, 0, Oe(7, 7), Oe
			(9, 9), Oe(9, 9), Oe(7, 7), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
			 };

		private static readonly long[] BishopTrapping = new long[] { 0, 1L << 10, 0, 0, 0
			, 0, 1L << 13, 0, 1L << 17, 0, 0, 0, 0, 0, 0, 1L << 22, 1L << 25, 0, 0, 0, 0, 0, 
			0, 1L << 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1L << 33, 0, 0, 0, 
			0, 0, 0, 1L << 38, 1L << 41, 0, 0, 0, 0, 0, 0, 1L << 46, 0, 1L << 50, 0, 0, 0, 0
			, 1L << 53, 0 };

		private static readonly int[] pawnPcsq = new int[] { Oe(-15, 0), Oe(-5, 0), Oe(0, 
			0), Oe(5, 0), Oe(5, 0), Oe(0, 0), Oe(-5, 0), Oe(-15, 0), Oe(-15, 0), Oe(-5, 0), 
			Oe(0, 0), Oe(5, 0), Oe(5, 0), Oe(0, 0), Oe(-5, 0), Oe(-15, 0), Oe(-15, 0), Oe(-5
			, 0), Oe(0, 0), Oe(15, 0), Oe(15, 0), Oe(0, 0), Oe(-5, 0), Oe(-15, 0), Oe(-15, 0
			), Oe(-5, 0), Oe(0, 0), Oe(25, 0), Oe(25, 0), Oe(0, 0), Oe(-5, 0), Oe(-15, 0), Oe
			(-15, 0), Oe(-5, 0), Oe(0, 0), Oe(15, 0), Oe(15, 0), Oe(0, 0), Oe(-5, 0), Oe(-15
			, 0), Oe(-15, 0), Oe(-5, 0), Oe(0, 0), Oe(5, 0), Oe(5, 0), Oe(0, 0), Oe(-5, 0), 
			Oe(-15, 0), Oe(-15, 0), Oe(-5, 0), Oe(0, 0), Oe(5, 0), Oe(5, 0), Oe(0, 0), Oe(-5
			, 0), Oe(-15, 0), Oe(-15, 0), Oe(-5, 0), Oe(0, 0), Oe(5, 0), Oe(5, 0), Oe(0, 0), 
			Oe(-5, 0), Oe(-15, 0) };

		private static readonly int[] knightPcsq = new int[] { Oe(-49, -40), Oe(-39, -30)
			, Oe(-30, -20), Oe(-25, -15), Oe(-25, -15), Oe(-30, -20), Oe(-39, -30), Oe(-49, 
			-40), Oe(-34, -30), Oe(-24, -20), Oe(-15, -10), Oe(-10, -5), Oe(-10, -5), Oe(-15
			, -10), Oe(-24, -20), Oe(-34, -30), Oe(-20, -20), Oe(-10, -10), Oe(0, 0), Oe(5, 
			5), Oe(5, 5), Oe(0, 0), Oe(-10, -10), Oe(-20, -20), Oe(-10, -15), Oe(0, -5), Oe(
			10, 5), Oe(15, 10), Oe(15, 10), Oe(10, 5), Oe(0, -5), Oe(-10, -15), Oe(-5, -15), 
			Oe(5, -5), Oe(15, 5), Oe(20, 10), Oe(20, 10), Oe(15, 5), Oe(5, -5), Oe(-5, -15), 
			Oe(-5, -20), Oe(5, -10), Oe(15, 0), Oe(20, 5), Oe(20, 5), Oe(15, 0), Oe(5, -10), 
			Oe(-5, -20), Oe(-19, -30), Oe(-9, -20), Oe(0, -10), Oe(5, -5), Oe(5, -5), Oe(0, 
			-10), Oe(-9, -20), Oe(-19, -30), Oe(-134, -40), Oe(-24, -30), Oe(-15, -20), Oe(-
			10, -15), Oe(-10, -15), Oe(-15, -20), Oe(-24, -30), Oe(-134, -40) };

		private static readonly int[] bishopPcsq = new int[] { Oe(-17, -18), Oe(-17, -12)
			, Oe(-16, -9), Oe(-14, -6), Oe(-14, -6), Oe(-16, -9), Oe(-17, -12), Oe(-17, -18)
			, Oe(-7, -12), Oe(1, -6), Oe(-2, -3), Oe(1, 0), Oe(1, 0), Oe(-2, -3), Oe(1, -6), 
			Oe(-7, -12), Oe(-6, -9), Oe(-2, -3), Oe(4, 0), Oe(2, 3), Oe(2, 3), Oe(4, 0), Oe(
			-2, -3), Oe(-6, -9), Oe(-4, -6), Oe(1, 0), Oe(2, 3), Oe(8, 6), Oe(8, 6), Oe(2, 3
			), Oe(1, 0), Oe(-4, -6), Oe(-4, -6), Oe(1, 0), Oe(2, 3), Oe(8, 6), Oe(8, 6), Oe(
			2, 3), Oe(1, 0), Oe(-4, -6), Oe(-6, -9), Oe(-2, -3), Oe(4, 0), Oe(2, 3), Oe(2, 3
			), Oe(4, 0), Oe(-2, -3), Oe(-6, -9), Oe(-7, -12), Oe(1, -6), Oe(-2, -3), Oe(1, 0
			), Oe(1, 0), Oe(-2, -3), Oe(1, -6), Oe(-7, -12), Oe(-7, -18), Oe(-7, -12), Oe(-6
			, -9), Oe(-4, -6), Oe(-4, -6), Oe(-6, -9), Oe(-7, -12), Oe(-7, -18) };

		private static readonly int[] rookPcsq = new int[] { Oe(-6, 0), Oe(-3, 0), Oe(0, 
			0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0), Oe(-6, 0), Oe(-3, 0), Oe
			(0, 0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0), Oe(-6, 0), Oe(-3, 0)
			, Oe(0, 0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0), Oe(-6, 0), Oe(-3
			, 0), Oe(0, 0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0), Oe(-6, 0), Oe
			(-3, 0), Oe(0, 0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0), Oe(-6, 0)
			, Oe(-3, 0), Oe(0, 0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0), Oe(-6
			, 0), Oe(-3, 0), Oe(0, 0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0), Oe
			(-6, 0), Oe(-3, 0), Oe(0, 0), Oe(3, 0), Oe(3, 0), Oe(0, 0), Oe(-3, 0), Oe(-6, 0)
			 };

		private static readonly int[] queenPcsq = new int[] { Oe(-4, -24), Oe(-4, -16), Oe
			(-5, -12), Oe(-5, -8), Oe(-5, -8), Oe(-5, -12), Oe(-4, -16), Oe(-4, -24), Oe(1, 
			-16), Oe(1, -8), Oe(0, -4), Oe(1, 0), Oe(1, 0), Oe(0, -4), Oe(1, -8), Oe(1, -16)
			, Oe(0, -12), Oe(0, -4), Oe(0, 0), Oe(0, 4), Oe(0, 4), Oe(0, 0), Oe(0, -4), Oe(0
			, -12), Oe(0, -8), Oe(1, 0), Oe(0, 4), Oe(0, 8), Oe(0, 8), Oe(0, 4), Oe(1, 0), Oe
			(0, -8), Oe(0, -8), Oe(1, 0), Oe(0, 4), Oe(0, 8), Oe(0, 8), Oe(0, 4), Oe(1, 0), 
			Oe(0, -8), Oe(0, -12), Oe(0, -4), Oe(0, 0), Oe(0, 4), Oe(0, 4), Oe(0, 0), Oe(0, 
			-4), Oe(0, -12), Oe(1, -16), Oe(1, -8), Oe(0, -4), Oe(1, 0), Oe(1, 0), Oe(0, -4)
			, Oe(1, -8), Oe(1, -16), Oe(1, -24), Oe(1, -16), Oe(0, -12), Oe(0, -8), Oe(0, -8
			), Oe(0, -12), Oe(1, -16), Oe(1, -24) };

		private static readonly int[] kingPcsq = new int[] { Oe(41, -72), Oe(51, -48), Oe
			(30, -36), Oe(10, -24), Oe(10, -24), Oe(30, -36), Oe(51, -48), Oe(41, -72), Oe(31
			, -48), Oe(41, -24), Oe(20, -12), Oe(1, 0), Oe(1, 0), Oe(20, -12), Oe(41, -24), 
			Oe(31, -48), Oe(10, -36), Oe(20, -12), Oe(0, 0), Oe(-20, 12), Oe(-20, 12), Oe(0, 
			0), Oe(20, -12), Oe(10, -36), Oe(0, -24), Oe(11, 0), Oe(-10, 12), Oe(-30, 24), Oe
			(-30, 24), Oe(-10, 12), Oe(11, 0), Oe(0, -24), Oe(-10, -24), Oe(1, 0), Oe(-20, 12
			), Oe(-40, 24), Oe(-40, 24), Oe(-20, 12), Oe(1, 0), Oe(-10, -24), Oe(-20, -36), 
			Oe(-10, -12), Oe(-30, 0), Oe(-50, 12), Oe(-50, 12), Oe(-30, 0), Oe(-10, -12), Oe
			(-20, -36), Oe(-29, -48), Oe(-19, -24), Oe(-40, -12), Oe(-59, 0), Oe(-59, 0), Oe
			(-40, -12), Oe(-19, -24), Oe(-29, -48), Oe(-39, -72), Oe(-29, -48), Oe(-50, -36)
			, Oe(-70, -24), Oe(-70, -24), Oe(-50, -36), Oe(-29, -48), Oe(-39, -72) };

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

		private long[] squaresNearKing = new long[] { 0, 0 };

		public CompleteEvaluator(Config config)
		{
			// Bonus by having two bishops in different colors
			// Bishops
			// Mobility units: this value is added for each destination square not occupied by one of our pieces
			// Knights
			// Rooks
			// No pawns in rook column
			// Only opposite pawns in rook column
			// Rook connects with other rook x 2
			// Queen
			// King Safety: not in endgame!!!
			// Protection: sums for each pawn near king (opening)
			// Ponder kings attacks by the number of attackers (not pawns) later divided by 8
			// Pawns
			// Array is not opposed, opposed
			// Array by relative rank
			// Candidates to pawn passer
			// no opposite pawns at left or at right
			// defended by pawn
			// two or more pieces of the other side attacked by inferior pieces
			// Tempo
			// Add to moving side score
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
			// From material imbalances (Larry Kaufmann):
			// A further refinement would be to raise the knight's value by 1/16 and lower the rook's value by 1/8
			// for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five
			int knightKaufBonusWhite = KnightKaufBonus * (whitePawns - 5);
			int knightKaufBonusBlack = KnightKaufBonus * (blackPawns - 5);
			int rookKaufBonusWhite = RookKaufBonus * (whitePawns - 5);
			int rookKaufBonusBlack = RookKaufBonus * (blackPawns - 5);
			pawnMaterial[0] = Pawn * whitePawns;
			pawnMaterial[1] = Pawn * blackPawns;
			material[0] = (Knight + knightKaufBonusWhite) * whiteKnights + Bishop * whiteBishops
				 + (Rook + rookKaufBonusWhite) * whiteRooks + Queen * whiteQueens + ((board.whites
				 & board.bishops & BitboardUtils.WhiteSquares) != 0 && (board.whites & board.bishops
				 & BitboardUtils.BlackSquares) != 0 ? BishopPair : 0);
			//
			//
			material[1] = (Knight + knightKaufBonusBlack) * blackKnights + Bishop * blackBishops
				 + (Rook + rookKaufBonusBlack) * blackRooks + Queen * blackQueens + ((board.blacks
				 & board.bishops & BitboardUtils.WhiteSquares) != 0 && (board.blacks & board.bishops
				 & BitboardUtils.BlackSquares) != 0 ? BishopPair : 0);
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
			attacks[0] = 0;
			attacks[1] = 0;
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
						if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
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
						long myPawnAttacks = (isWhite ? pawnAttacks[0] : pawnAttacks[1]);
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
						bool backwards = !isolated && !passed && !candidate && (myPawnsBesideAndBehindAdjacent
							 == 0) && ((pieceAttacks & otherPawns) == 0) && ((BitboardUtils.RankAndBackward[
							color][isWhite ? BitboardUtils.GetRankLsb(myPawnsAheadAdjacent) : BitboardUtils.
							GetRankMsb(myPawnsAheadAdjacent)] & routeToPromotion & (board.pawns | otherPawnAttacks
							)) != 0);
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
							int relativeRank = (isWhite ? rank : 7 - rank);
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
							mobility[color] += OeMul(BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
								) - KnightMUnits, KnightM);
							if ((pieceAttacks & squaresNearKing[color]) != 0)
							{
								kingSafety[color] += KnightAttacksKing;
								kingAttackersCount[color]++;
							}
							superiorPieceAttacked[color] |= pieceAttacks & others & (board.rooks | board.queens
								);
							// Knight outpost: no opposite pawns can attack the square and it is defended by one of our pawns
							if ((square & ~pawnCanAttack[1 - color] & pawnAttacks[color]) != 0)
							{
								positional[color] += KnigthOutpost[pcsqIndex];
							}
						}
						else
						{
							if ((square & board.bishops) != 0)
							{
								center[color] += bishopPcsq[pcsqIndex];
								mobility[color] += OeMul(BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
									) - BishopMUnits, BishopM);
								if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
								{
									kingSafety[color] += BishopAttacksKing;
									kingAttackersCount[color]++;
								}
								superiorPieceAttacked[color] |= pieceAttacks & others & (board.rooks | board.queens
									);
								pieceAttacksXray = bbAttacks.GetBishopAttacks(index, all & ~(pieceAttacks & others
									 & ~board.pawns)) & ~pieceAttacks;
								if ((pieceAttacksXray & (board.rooks | board.queens | board.kings) & others) != 0)
								{
									attacks[color] += PinnedPiece;
								}
								if ((BishopTrapping[index] & board.pawns & others) != 0)
								{
									mobility[color] += BishopTrapped;
								}
							}
							else
							{
								if ((square & board.rooks) != 0)
								{
									center[color] += rookPcsq[pcsqIndex];
									mobility[color] += OeMul(BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
										) - RookMUnits, RookM);
									if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
									{
										kingSafety[color] += RookAttacksKing;
										kingAttackersCount[color]++;
									}
									superiorPieceAttacked[color] |= pieceAttacks & others & board.queens;
									pieceAttacksXray = bbAttacks.GetRookAttacks(index, all & ~(pieceAttacks & others 
										& ~board.pawns)) & ~pieceAttacks;
									if ((pieceAttacksXray & (board.queens | board.kings) & others) != 0)
									{
										attacks[color] += PinnedPiece;
									}
									if ((pieceAttacks & mines & board.rooks) != 0)
									{
										positional[color] += RookConnect;
									}
									auxLong = BitboardUtils.Column[column];
									if ((auxLong & board.pawns) == 0)
									{
										positional[color] += RookColumnOpen;
									}
									else
									{
										if ((auxLong & board.pawns & mines) == 0)
										{
											positional[color] += RookColumnSemiopen;
										}
									}
								}
								else
								{
									if ((square & board.queens) != 0)
									{
										center[color] += queenPcsq[pcsqIndex];
										mobility[color] += OeMul(BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
											) - QueenMUnits, QueenM);
										if ((pieceAttacks & squaresNearKing[1 - color]) != 0)
										{
											kingSafety[color] += QueenAttacksKing;
											kingAttackersCount[color]++;
										}
										pieceAttacksXray = (bbAttacks.GetRookAttacks(index, all & ~(pieceAttacks & others
											 & ~board.pawns)) | bbAttacks.GetBishopAttacks(index, all & ~(pieceAttacks & others
											 & ~board.pawns))) & ~pieceAttacks;
										if ((pieceAttacksXray & board.kings & others) != 0)
										{
											attacks[color] += PinnedPiece;
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
				.GetEvalKingSafety(), kingDefense[0] - kingDefense[1]) + OeMul((int)(((uint)config
				.GetEvalKingSafety()) >> 3), (KingSafetyPonder[kingAttackersCount[0]] * kingSafety
				[0] - KingSafetyPonder[kingAttackersCount[1]] * kingSafety[1]));
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
				logger.Debug("kingSafety x8     = " + FormatOE(KingSafetyPonder[kingAttackersCount
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
