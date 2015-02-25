using System;
using System.Text;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Log;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	/// <summary>
	/// Evaluation is done in centipawns
	/// <p/>
	/// TODO: hung pieces & x-ray attacks: revise
	/// TODO: bishop / knights / rook traps: revise
	/// TODO: test knights and bishops only forward mobility
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

		private const int Opening = 0;

		private const int Endgame = 1;

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

		private static readonly int[] KingSafetyPonder = new int[] { 0, 1, 2, 4, 8, 8, 8, 
			8, 8, 8, 8, 8, 8, 8, 8, 8 };

		private static readonly int[][] PawnColumn = new int[][] { new int[] { -20, -8, -
			2, 5, 5, -2, -8, -20 }, new int[] { -4, -6, -8, -10, -10, -8, -6, -4 } };

		private static readonly int[][] PawnRank = new int[][] { new int[] { 0, -3, -2, -
			1, 1, 2, 3, 0 }, new int[] { 0, -3, -3, -2, -1, 0, 2, 0 } };

		private static readonly int[][] KnightColumn = new int[][] { new int[] { -26, -10
			, 1, 5, 5, 1, -10, -26 }, new int[] { -4, -1, 2, 4, 4, 2, -1, -4 } };

		private static readonly int[][] KnightRank = new int[][] { new int[] { -32, -10, 
			6, 15, 21, 19, 10, -11 }, new int[] { -10, -5, -2, 1, 3, 5, 2, -3 } };

		private static readonly int[][] KnightLine = new int[][] { new int[] { 0, 0, 0, 0
			, 0, 0, 0, 0 }, new int[] { 2, 1, 0, -1, -2, -4, -7, -10 } };

		private static readonly int[][] BishopLine = new int[][] { new int[] { 10, 5, 1, 
			-3, -5, -7, -8, -12 }, new int[] { 3, 2, 0, 0, -2, -2, -3, -3 } };

		private static readonly int[][] BishopRank = new int[][] { new int[] { -5, 0, 0, 
			0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0 } };

		private static readonly int[][] RookColumn = new int[][] { new int[] { -4, 0, 4, 
			8, 8, 4, 0, -4 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0 } };

		private static readonly int[][] RookRank = new int[][] { new int[] { 0, 0, 0, 0, 
			0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 1, 1, 1, -2 } };

		private static readonly int[][] QueenColumn = new int[][] { new int[] { -2, 0, 1, 
			2, 2, 1, 0, -2 }, new int[] { -2, 0, 1, 2, 2, 1, 0, -2 } };

		private static readonly int[][] QueenRank = new int[][] { new int[] { -2, 0, 1, 2
			, 2, 1, 0, -2 }, new int[] { -2, 0, 1, 2, 2, 1, 0, -2 } };

		private static readonly int[][] QueenLine = new int[][] { new int[] { 3, 2, 1, 0, 
			-2, -4, -7, -10 }, new int[] { 1, 0, -1, -3, -4, -6, -8, -12 } };

		private static readonly int[][] KingColumn = new int[][] { new int[] { 40, 45, 15
			, -5, -5, 15, 45, 40 }, new int[] { -15, 0, 10, 15, 15, 10, 0, -15 } };

		private static readonly int[][] KingRank = new int[][] { new int[] { 4, 1, -2, -5
			, -10, -15, -25, -35 }, new int[] { -15, 0, 10, 15, 15, 10, 0, -15 } };

		private static readonly int[][] KingLine = new int[][] { new int[] { 0, 0, 0, 0, 
			0, 0, 0, 0 }, new int[] { 2, 0, -2, -5, -8, -12, -20, -30 } };

		public static readonly int[] pawnIndexValue = new int[64];

		public static readonly int[] knightIndexValue = new int[64];

		public static readonly int[] bishopIndexValue = new int[64];

		public static readonly int[] rookIndexValue = new int[64];

		public static readonly int[] queenIndexValue = new int[64];

		public static readonly int[] kingIndexValue = new int[64];

		static ExperimentalEvaluator()
		{
			// Bonus by having two bishops in different colors
			// Bishops
			// Mobility units: this value is added for each destination square not occupied by one of our pieces or attacked by opposite pawns
			// Sums for each of our pawns (or oppsite/2) in the bishop color
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
			// Ponder kings attacks by the number of attackers (not pawns)
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
			// Values are rotated for whites, so when white is playing is like shown in the code
			// Initialize Piece square values
			int i;
			for (i = 0; i < 64; i++)
			{
				int rank = i >> 3;
				int column = 7 - i & 7;
				int d = (((column - rank) >= 0) ? (column - rank) : -(column - rank));
				int e = (((column + rank - 7) >= 0) ? (column + rank - 7) : -(column + rank - 7));
				pawnIndexValue[i] = Oe(PawnColumn[Opening][column] + PawnRank[Opening][rank], PawnColumn
					[Endgame][column] + PawnRank[Endgame][rank]);
				knightIndexValue[i] = Oe(KnightColumn[Opening][column] + KnightRank[Opening][rank
					] + KnightLine[Opening][d] + KnightLine[Opening][e], KnightColumn[Endgame][column
					] + KnightRank[Endgame][rank] + KnightLine[Endgame][d] + KnightLine[Endgame][e]);
				bishopIndexValue[i] = Oe(BishopRank[Opening][rank] + BishopLine[Opening][d] + BishopLine
					[Opening][e], BishopRank[Endgame][rank] + BishopLine[Endgame][d] + BishopLine[Endgame
					][e]);
				rookIndexValue[i] = Oe(RookColumn[Opening][column] + RookRank[Opening][rank], RookColumn
					[Endgame][column] + RookRank[Endgame][rank]);
				queenIndexValue[i] = Oe(QueenColumn[Opening][column] + QueenRank[Opening][rank] +
					 QueenLine[Opening][d] + QueenLine[Opening][e], QueenColumn[Endgame][column] + QueenRank
					[Endgame][rank] + QueenLine[Endgame][d] + QueenLine[Endgame][e]);
				kingIndexValue[i] = Oe(KingColumn[Opening][column] + KingRank[Opening][rank] + KingLine
					[Opening][d] + KingLine[Opening][e], KingColumn[Endgame][column] + KingRank[Endgame
					][rank] + KingLine[Endgame][d] + KingLine[Endgame][e]);
			}
		}

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

		private long[] attacksFromSquare = new long[64];

		private long[] superiorPieceAttacked = new long[] { 0, 0 };

		private long[] attackedSquares = new long[] { 0, 0 };

		private long[] pawnAttacks = new long[] { 0, 0 };

		private long[] pawnCanAttack = new long[] { 0, 0 };

		private long[] minorPiecesDefendedByPawns = new long[] { 0, 0 };

		private long[] squaresNearKing = new long[] { 0, 0 };

		public ExperimentalEvaluator(Config config)
		{
			// Squares attackeds by pawns
			this.config = config;
		}

		public override int Evaluate(Board board)
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
			long all = board.GetAll();
			long pieceAttacks;
			long pieceAttacksXray;
			long auxLong;
			long auxLong2;
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
			attackedSquares[0] = 0;
			attackedSquares[1] = 0;
			// Squares attacked by pawns
			pawnAttacks[0] = ((board.pawns & board.whites & ~BitboardUtils.b_l) << 9) | ((board
				.pawns & board.whites & ~BitboardUtils.b_r) << 7);
			pawnAttacks[1] = ((long)(((ulong)(board.pawns & board.blacks & ~BitboardUtils.b_r
				)) >> 9)) | ((long)(((ulong)(board.pawns & board.blacks & ~BitboardUtils.b_l)) >>
				 7));
			// Square that pawn attacks or can attack by advancing
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
			// first build attacks info
			int index;
			long square = 1;
			for (index = 0; index < 64; index++)
			{
				if ((square & all) != 0)
				{
					bool isWhite = ((board.whites & square) != 0);
					int color = (isWhite ? 0 : 1);
					if ((square & board.pawns) != 0)
					{
						pieceAttacks = (isWhite ? bbAttacks.pawnUpwards[index] : bbAttacks.pawnDownwards[
							index]);
					}
					else
					{
						if ((square & board.knights) != 0)
						{
							pieceAttacks = bbAttacks.knight[index];
						}
						else
						{
							if ((square & board.bishops) != 0)
							{
								pieceAttacks = bbAttacks.GetBishopAttacks(index, all);
							}
							else
							{
								if ((square & board.rooks) != 0)
								{
									pieceAttacks = bbAttacks.GetRookAttacks(index, all);
								}
								else
								{
									if ((square & board.queens) != 0)
									{
										pieceAttacks = bbAttacks.GetRookAttacks(index, all) | bbAttacks.GetBishopAttacks(
											index, all);
									}
									else
									{
										if ((square & board.kings) != 0)
										{
											pieceAttacks = bbAttacks.king[index];
										}
										else
										{
											pieceAttacks = 0;
										}
									}
								}
							}
						}
					}
					attackedSquares[color] |= pieceAttacks;
					attacksFromSquare[index] = pieceAttacks;
				}
				square <<= 1;
			}
			square = 1;
			index = 0;
			while (square != 0)
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
					pieceAttacks = attacksFromSquare[index];
					if ((square & board.pawns) != 0)
					{
						center[color] += pawnIndexValue[pcsqIndex];
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
							long attackedAndNotDefendedRoute = ((routeToPromotion & attackedSquares[1 - color
								]) | ((backColumn & (board.rooks | board.queens) & others) != 0 ? routeToPromotion
								 : 0)) & ~((routeToPromotion & attackedSquares[color]) | ((backColumn & (board.rooks
								 | board.queens) & mines) != 0 ? routeToPromotion : 0));
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
							center[color] += knightIndexValue[pcsqIndex];
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
							// Knight Outpost: no opposite pawns can attack the square
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
								center[color] += bishopIndexValue[pcsqIndex];
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
									)) & ~pieceAttacks;
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
								// Pawns in our color
								if ((square & BitboardUtils.WhiteSquares) != 0)
								{
									auxLong = BitboardUtils.WhiteSquares;
								}
								else
								{
									auxLong = BitboardUtils.BlackSquares;
								}
								positional[color] += ((int)(((uint)BitboardUtils.PopCount(auxLong & board.pawns &
									 mines) + BitboardUtils.PopCount(auxLong & board.pawns & mines)) >> 1)) * BishopPawnInColor;
								positional[color] += ((int)(((uint)BitboardUtils.PopCount(auxLong & board.pawns &
									 others & BitboardUtils.RanksForward[color][rank])) >> 1)) * BishopForwardPPu;
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
									center[color] += rookIndexValue[pcsqIndex];
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
									pieceAttacksXray = bbAttacks.GetRookAttacks(index, all & ~(pieceAttacks & others)
										) & ~pieceAttacks;
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
										center[color] += queenIndexValue[pcsqIndex];
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
											)) | bbAttacks.GetBishopAttacks(index, all & ~(pieceAttacks & others))) & ~pieceAttacks;
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
											center[color] += kingIndexValue[pcsqIndex];
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
				[0] - passedPawns[1]) + config.GetEvalKingSafety() * (kingDefense[0] - kingDefense
				[1]) + config.GetEvalKingSafety() * (KingSafetyPonder[kingAttackersCount[0]] * kingSafety
				[0] - KingSafetyPonder[kingAttackersCount[1]] * kingSafety[1]) + config.GetEvalAttacks
				() * ((BitboardUtils.PopCount(superiorPieceAttacked[0]) >= 2 ? HungPieces : 0) -
				 (BitboardUtils.PopCount(superiorPieceAttacked[1]) >= 2 ? HungPieces : 0));
			value += (gamePhase * O(oe)) / (256 * 100);
			// divide by 256
			value += ((256 - gamePhase) * E(oe)) / (256 * 100);
			if (debug)
			{
				logger.Debug(debugSB);
				logger.Debug("materialValue          = " + (material[0] - material[1]));
				logger.Debug("pawnMaterialValue      = " + (pawnMaterial[0] - pawnMaterial[1]));
				logger.Debug("centerOpening          = " + O(center[0] - center[1]));
				logger.Debug("centerEndgame          = " + E(center[0] - center[1]));
				logger.Debug("positionalOpening      = " + O(positional[0] - positional[1]));
				logger.Debug("positionalEndgame      = " + E(positional[0] - positional[1]));
				logger.Debug("attacksO               = " + O(attacks[0] - attacks[1]));
				logger.Debug("attacksE               = " + E(attacks[0] - attacks[1]));
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
				logger.Debug("kingDefenseO           = " + O(kingDefense[0] - kingDefense[1]));
				logger.Debug("kingDefenseE           = " + E(kingDefense[0] - kingDefense[1]));
				logger.Debug("HungPiecesO            = " + O((BitboardUtils.PopCount(superiorPieceAttacked
					[0]) >= 2 ? HungPieces : 0) - (BitboardUtils.PopCount(superiorPieceAttacked[1]) 
					>= 2 ? HungPieces : 0)));
				logger.Debug("HungPiecesE            = " + O((BitboardUtils.PopCount(superiorPieceAttacked
					[0]) >= 2 ? HungPieces : 0) - (BitboardUtils.PopCount(superiorPieceAttacked[1]) 
					>= 2 ? HungPieces : 0)));
				logger.Debug("gamePhase              = " + gamePhase);
				logger.Debug("tempo                  = " + (board.GetTurn() ? Tempo : -Tempo));
				logger.Debug("value                  = " + value);
			}
			System.Diagnostics.Debug.Assert(Math.Abs(value) < Evaluator.Victory);
			return value;
		}
	}
}
