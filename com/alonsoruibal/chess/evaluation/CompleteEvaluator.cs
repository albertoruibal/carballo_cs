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

		static CompleteEvaluator()
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
			// Array is not opposed, opposed
			// Array by relative rank
			// Candidates to pawn passer
			// no opposite pawns at left or at right
			// defended by pawn
			// Ponder kings attacks by the number of attackers (not pawns) later divided by 8
			// two or more pieces of the other side attacked by inferior pieces
			// Tempo
			// Add to moving side score
			// The pair of values are {opening, endgame}
			// Values are rotated for whites, so when white is playing is like shown in the code
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

		private int[] pawnStructure = new int[] { 0, 0 };

		private int[] passedPawns = new int[] { 0, 0 };

		private long[] attacksFromSquare = new long[64];

		private long[] superiorPieceAttacked = new long[] { 0, 0 };

		private long[] attackedSquares = new long[] { 0, 0 };

		private long[] pawnAttacks = new long[] { 0, 0 };

		private long[] squaresNearKing = new long[] { 0, 0 };

		public CompleteEvaluator(Config config)
		{
			// D5
			//	int kingDefense[] = {0,0};
			// Squares attackeds by pawns
			// Squares surrounding King
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
			attacks[0] = 0;
			attacks[1] = 0;
			// Squares surrounding King
			squaresNearKing[0] = bbAttacks.king[BitboardUtils.Square2Index(board.whites & board
				.kings)];
			squaresNearKing[1] = bbAttacks.king[BitboardUtils.Square2Index(board.blacks & board
				.kings)];
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
							mobility[color] += KnightM * (BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
								) - KnightMUnits);
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
								center[color] += bishopIndexValue[pcsqIndex];
								mobility[color] += BishopM * (BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
									) - BishopMUnits);
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
									center[color] += rookIndexValue[pcsqIndex];
									mobility[color] += RookM * (BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
										) - RookMUnits);
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
									long columnAttacks = BitboardUtils.Column[column];
									if ((columnAttacks & board.pawns) == 0)
									{
										positional[color] += RookColumnOpen;
									}
									else
									{
										if ((columnAttacks & board.pawns & mines) == 0)
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
										mobility[color] += QueenM * (BitboardUtils.PopCount(pieceAttacks & ~mines & ~otherPawnAttacks
											) - QueenMUnits);
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
											center[color] += kingIndexValue[pcsqIndex];
											// TODO
											if ((square & (isWhite ? BitboardUtils.Rank[0] : BitboardUtils.Rank[7])) != 0)
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
