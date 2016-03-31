using System;
using System.Collections.Generic;
using System.Text;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Hash;
using Com.Alonsoruibal.Chess.Movegen;
using Sharpen;

namespace Com.Alonsoruibal.Chess
{
	/// <summary>
	/// Stores the position and the move history
	/// TODO Other chess variants like Atomic, Suicide, etc.
	/// </summary>
	/// <author>Alberto Alonso Ruibal</author>
	public class Board
	{
		public const int MaxMoves = 1024;

		public const string FenStartPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

		public static readonly string[] Chess960StartPositions = new string[] { "QNNRKR", 
			"NQNRKR", "NNQRKR", "NNRQKR", "NNRKQR", "NNRKRQ", "QNRNKR", "NQRNKR", "NRQNKR", 
			"NRNQKR", "NRNKQR", "NRNKRQ", "QNRKNR", "NQRKNR", "NRQKNR", "NRKQNR", "NRKNQR", 
			"NRKNRQ", "QNRKRN", "NQRKRN", "NRQKRN", "NRKQRN", "NRKRQN", "NRKRNQ", "QRNNKR", 
			"RQNNKR", "RNQNKR", "RNNQKR", "RNNKQR", "RNNKRQ", "QRNKNR", "RQNKNR", "RNQKNR", 
			"RNKQNR", "RNKNQR", "RNKNRQ", "QRNKRN", "RQNKRN", "RNQKRN", "RNKQRN", "RNKRQN", 
			"RNKRNQ", "QRKNNR", "RQKNNR", "RKQNNR", "RKNQNR", "RKNNQR", "RKNNRQ", "QRKNRN", 
			"RQKNRN", "RKQNRN", "RKNQRN", "RKNRQN", "RKNRNQ", "QRKRNN", "RQKRNN", "RKQRNN", 
			"RKRQNN", "RKRNQN", "RKRNNQ" };

		public static readonly string[] Chess960StartPositionsBishops = new string[] { "BB------"
			, "B--B----", "B----B--", "B------B", "-BB-----", "--BB----", "--B--B--", "--B----B"
			, "-B--B---", "---BB---", "----BB--", "----B--B", "-B----B-", "---B--B-", "-----BB-"
			, "------BB" };

		private const long FlagTurn = unchecked((long)(0x0001L));

		private const long FlagWhiteKingsideCastling = unchecked((long)(0x0002L));

		private const long FlagWhiteQueensideCastling = unchecked((long)(0x0004L));

		private const long FlagBlackKingsideCastling = unchecked((long)(0x0008L));

		private const long FlagBlackQueensideCastling = unchecked((long)(0x0010L));

		private const long FlagCheck = unchecked((long)(0x0020L));

		private const long FlagsPassant = unchecked((long)(0x0000ff0000ff0000L));

		public static readonly int[] CastlingKingDestinyIndex = new int[] { 1, 5, 57, 61 };

		public static readonly long[] CastlingKingDestinySquare = new long[] { 1L << 1, 1L
			 << 5, 1L << 57, 1L << 61 };

		public static readonly int[] CastlingRookDestinyIndex = new int[] { 2, 4, 58, 60 };

		public static readonly long[] CastlingRookDestinySquare = new long[] { 1L << 2, 1L
			 << 4, 1L << 58, 1L << 60 };

		public static readonly int[] SeePieceValues = new int[] { 0, 100, 325, 330, 500, 
			900, 9999 };

		internal LegalMoveGenerator legalMoveGenerator = new LegalMoveGenerator();

		internal int[] legalMoves = new int[256];

		internal int legalMoveCount = -1;

		internal long[] legalMovesKey = new long[] { 0, 0 };

		public Dictionary<int, string> movesSan;

		public long whites = 0;

		public long blacks = 0;

		public long pawns = 0;

		public long rooks = 0;

		public long queens = 0;

		public long bishops = 0;

		public long knights = 0;

		public long kings = 0;

		public long flags = 0;

		public int fiftyMovesRule = 0;

		public int initialMoveNumber = 0;

		public int moveNumber = 0;

		public int outBookMove = int.MaxValue;

		public long[] key = new long[] { 0, 0 };

		public string initialFen;

		public long[][] keyHistory;

		public int[] moveHistory;

		public long[] whitesHistory;

		public long[] blacksHistory;

		public long[] pawnsHistory;

		public long[] rooksHistory;

		public long[] queensHistory;

		public long[] bishopsHistory;

		public long[] knightsHistory;

		public long[] kingsHistory;

		public long[] flagsHistory;

		public int[] fiftyMovesRuleHistory;

		public int[] seeGain;

		public long[] castlingRooks = new long[] { 0, 0, 0, 0 };

		public bool chess960;

		internal BitboardAttacks bbAttacks;

		public Board()
		{
			// Flags: must be changed only when moving
			// Position on boarch in which is captured
			// For the castlings {White Kingside, White Queenside, Black Kingside, Black Queenside}
			// For the SEE SWAP algorithm
			// if -1 then legal moves not generated
			// Bitboard arrays
			// History array indexed by moveNumber
			// to detect draw by treefold
			// Origin squares for the castling rook {White Kingside, White Queenside, Black Kingside, Black Queenside}
			// basically decides the destiny square of the castlings
			whitesHistory = new long[MaxMoves];
			blacksHistory = new long[MaxMoves];
			pawnsHistory = new long[MaxMoves];
			knightsHistory = new long[MaxMoves];
			bishopsHistory = new long[MaxMoves];
			rooksHistory = new long[MaxMoves];
			queensHistory = new long[MaxMoves];
			kingsHistory = new long[MaxMoves];
			flagsHistory = new long[MaxMoves];
			keyHistory = new long[][] { new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new 
				long[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long
				[2], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2
				], new long[2], new long[2], new long[2], new long[2], new long[2], new long[2], 
				new long[2] };
			fiftyMovesRuleHistory = new int[MaxMoves];
			seeGain = new int[32];
			moveHistory = new int[MaxMoves];
			movesSan = new Dictionary<int, string>();
			bbAttacks = BitboardAttacks.GetInstance();
		}

		/// <summary>It also computes the zobrist key</summary>
		public virtual void StartPosition()
		{
			SetFen(FenStartPosition);
		}

		/// <summary>
		/// Set a Chess960 start position
		/// http://en.wikipedia.org/wiki/Chess960_numbering_scheme
		/// </summary>
		public virtual void StartPosition(int chess960Position)
		{
			string @base = Chess960StartPositionsBishops[chess960Position & unchecked((int)(0x0f
				))];
			string otherPieces = Chess960StartPositions[(int)(((uint)chess960Position) >> 4)];
			StringBuilder oSB = new StringBuilder();
			int j = 0;
			for (int i = 0; i < 8; i++)
			{
				if (@base[i] == '-')
				{
					oSB.Append(otherPieces[j]);
					j++;
				}
				else
				{
					oSB.Append('B');
				}
			}
			string fen = oSB.ToString().ToLower() + "/pppppppp/8/8/8/8/PPPPPPPP/" + oSB.ToString
				() + " w KQkq - 0 1";
			SetFen(fen);
			chess960 = true;
		}

		public virtual long GetKey()
		{
			return key[0] ^ key[1];
		}

		public virtual long GetExclusionKey()
		{
			return key[0] ^ key[1] ^ ZobristKey.exclusionKey;
		}

		/// <summary>An alternative key to avoid collisions in the TT</summary>
		public virtual long GetKey2()
		{
			return key[0] ^ ~key[1];
		}

		public virtual int GetMoveNumber()
		{
			return moveNumber;
		}

		/// <returns>true if white moves</returns>
		public bool GetTurn()
		{
			return (flags & FlagTurn) == 0;
		}

		public virtual bool GetWhiteKingsideCastling()
		{
			return (flags & FlagWhiteKingsideCastling) != 0;
		}

		public virtual bool GetWhiteQueensideCastling()
		{
			return (flags & FlagWhiteQueensideCastling) != 0;
		}

		public virtual bool GetBlackKingsideCastling()
		{
			return (flags & FlagBlackKingsideCastling) != 0;
		}

		public virtual bool GetBlackQueensideCastling()
		{
			return (flags & FlagBlackQueensideCastling) != 0;
		}

		public virtual long GetPassantSquare()
		{
			return flags & FlagsPassant;
		}

		public virtual bool GetCheck()
		{
			return (flags & FlagCheck) != 0;
		}

		public virtual long GetAll()
		{
			return whites | blacks;
		}

		public virtual long GetMines()
		{
			return (flags & FlagTurn) == 0 ? whites : blacks;
		}

		public virtual long GetOthers()
		{
			return (flags & FlagTurn) == 0 ? blacks : whites;
		}

		public virtual int GetPieceIntAt(long square)
		{
			return ((pawns & square) != 0 ? Piece.Pawn : ((knights & square) != 0 ? Piece.Knight
				 : ((bishops & square) != 0 ? Piece.Bishop : ((rooks & square) != 0 ? Piece.Rook
				 : ((queens & square) != 0 ? Piece.Queen : ((kings & square) != 0 ? Piece.King : 
				'.'))))));
		}

		//
		//
		//
		//
		//
		public virtual char GetPieceAt(long square)
		{
			char p = ((pawns & square) != 0 ? 'p' : ((knights & square) != 0 ? 'n' : ((bishops
				 & square) != 0 ? 'b' : ((rooks & square) != 0 ? 'r' : ((queens & square) != 0 ? 
				'q' : ((kings & square) != 0 ? 'k' : '.'))))));
			//
			//
			//
			//
			//
			return ((whites & square) != 0 ? System.Char.ToUpper(p) : p);
		}

		public virtual char GetPieceUnicodeAt(long square)
		{
			if ((whites & square) != 0)
			{
				return ((pawns & square) != 0 ? '♙' : ((knights & square) != 0 ? '♘' : ((bishops 
					& square) != 0 ? '♗' : ((rooks & square) != 0 ? '♖' : ((queens & square) != 0 ? 
					'♕' : ((kings & square) != 0 ? '♔' : '.'))))));
			}
			else
			{
				//
				//
				//
				//
				//
				if ((blacks & square) != 0)
				{
					return ((pawns & square) != 0 ? '♟' : ((knights & square) != 0 ? '♞' : ((bishops 
						& square) != 0 ? '♝' : ((rooks & square) != 0 ? '♜' : ((queens & square) != 0 ? 
						'♛' : ((kings & square) != 0 ? '♚' : '.'))))));
				}
				else
				{
					//
					//
					//
					//
					//
					return '_';
				}
			}
		}

		public virtual void SetPieceAt(long square, char piece)
		{
			pawns &= ~square;
			queens &= ~square;
			rooks &= ~square;
			bishops &= ~square;
			knights &= ~square;
			kings &= ~square;
			if (piece == ' ' || piece == '.')
			{
				whites &= ~square;
				blacks &= ~square;
				return;
			}
			else
			{
				if (piece == System.Char.ToLower(piece))
				{
					whites &= ~square;
					blacks |= square;
				}
				else
				{
					whites |= square;
					blacks &= ~square;
				}
			}
			switch (System.Char.ToLower(piece))
			{
				case 'p':
				{
					pawns |= square;
					break;
				}

				case 'q':
				{
					queens |= square;
					break;
				}

				case 'r':
				{
					rooks |= square;
					break;
				}

				case 'b':
				{
					bishops |= square;
					break;
				}

				case 'n':
				{
					knights |= square;
					break;
				}

				case 'k':
				{
					kings |= square;
					break;
				}
			}
			key = ZobristKey.GetKey(this);
			SetCheckFlags();
		}

		/// <summary>Converts board to its fen notation</summary>
		public virtual string GetFen()
		{
			StringBuilder sb = new StringBuilder();
			long i = Square.A8;
			int j = 0;
			while (i != 0)
			{
				char p = GetPieceAt(i);
				if (p == '.')
				{
					j++;
				}
				if ((j != 0) && (p != '.' || ((i & BitboardUtils.b_r) != 0)))
				{
					sb.Append(j);
					j = 0;
				}
				if (p != '.')
				{
					sb.Append(p);
				}
				if ((i != 1) && (i & BitboardUtils.b_r) != 0)
				{
					sb.Append("/");
				}
				i = (long)(((ulong)i) >> 1);
			}
			sb.Append(" ");
			sb.Append((GetTurn() ? "w" : "b"));
			sb.Append(" ");
			if (GetWhiteKingsideCastling())
			{
				sb.Append("K");
			}
			if (GetWhiteQueensideCastling())
			{
				sb.Append("Q");
			}
			if (GetBlackKingsideCastling())
			{
				sb.Append("k");
			}
			if (GetBlackQueensideCastling())
			{
				sb.Append("q");
			}
			if (!GetWhiteQueensideCastling() && !GetWhiteKingsideCastling() && !GetBlackQueensideCastling
				() && !GetBlackKingsideCastling())
			{
				sb.Append("-");
			}
			sb.Append(" ");
			sb.Append((GetPassantSquare() != 0 ? BitboardUtils.Square2Algebraic(GetPassantSquare
				()) : "-"));
			sb.Append(" ");
			sb.Append(fiftyMovesRule);
			sb.Append(" ");
			sb.Append((moveNumber >> 1) + 1);
			// 0,1->1.. 2,3->2
			return sb.ToString();
		}

		/// <summary>Loads board from a fen notation</summary>
		public virtual void SetFen(string fen)
		{
			SetFenMove(fen, null);
		}

		/// <summary>Sets fen without destroying move history.</summary>
		/// <remarks>Sets fen without destroying move history. If lastMove = null destroy the move history
		/// 	</remarks>
		public virtual void SetFenMove(string fen, string lastMove)
		{
			long tmpWhites = 0;
			long tmpBlacks = 0;
			long tmpPawns = 0;
			long tmpRooks = 0;
			long tmpQueens = 0;
			long tmpBishops = 0;
			long tmpKnights = 0;
			long tmpKings = 0;
			long tmpFlags;
			int tmpFiftyMovesRule = 0;
			long[] tmpCastlingRooks = new long[] { 0, 0, 0, 0 };
			int fenMoveNumber = 0;
			int i = 0;
			long j = Square.A8;
			string[] tokens = fen.Split("[ \\t\\n\\x0B\\f\\r]+");
			string board = tokens[0];
			while ((i < board.Length) && (j != 0))
			{
				char p = board[i++];
				if (p != '/')
				{
					int number = 0;
					try
					{
						number = System.Convert.ToInt32(p.ToString());
					}
					catch (Exception)
					{
					}
					for (int k = 0; k < (number == 0 ? 1 : number); k++)
					{
						tmpWhites = (tmpWhites & ~j) | ((number == 0) && (p == System.Char.ToUpper(p)) ? 
							j : 0);
						tmpBlacks = (tmpBlacks & ~j) | ((number == 0) && (p == System.Char.ToLower(p)) ? 
							j : 0);
						tmpPawns = (tmpPawns & ~j) | (System.Char.ToUpper(p) == 'P' ? j : 0);
						tmpRooks = (tmpRooks & ~j) | (System.Char.ToUpper(p) == 'R' ? j : 0);
						tmpQueens = (tmpQueens & ~j) | (System.Char.ToUpper(p) == 'Q' ? j : 0);
						tmpBishops = (tmpBishops & ~j) | (System.Char.ToUpper(p) == 'B' ? j : 0);
						tmpKnights = (tmpKnights & ~j) | (System.Char.ToUpper(p) == 'N' ? j : 0);
						tmpKings = (tmpKings & ~j) | (System.Char.ToUpper(p) == 'K' ? j : 0);
						j = (long)(((ulong)j) >> 1);
						if (j == 0)
						{
							break;
						}
					}
				}
			}
			// security
			// Now the rest ...
			string turn = tokens[1];
			tmpFlags = 0;
			if ("b".Equals(turn))
			{
				tmpFlags |= FlagTurn;
			}
			if (tokens.Length > 2)
			{
				// Set castling rights supporting XFEN to disambiguate positions in Chess960
				string castlings = tokens[2];
				chess960 = false;
				// Squares to the sides of the kings {White Kingside, White Queenside, Black Kingside, Black Queenside}
				long[] whiteKingLateralSquares = new long[] { BitboardUtils.b_d & ((tmpKings & tmpWhites
					) - 1), BitboardUtils.b_d & ~(((tmpKings & tmpWhites) - 1) | tmpKings & tmpWhites
					), BitboardUtils.b_u & ((tmpKings & tmpBlacks) - 1), BitboardUtils.b_u & ~(((tmpKings
					 & tmpBlacks) - 1) | tmpKings & tmpBlacks) };
				// Squares where we can find a castling rook
				long[] possibleCastlingRookSquares = new long[] { 0, 0, 0, 0 };
				for (int k = 0; k < castlings.Length; k++)
				{
					char c = castlings[k];
					switch (c)
					{
						case 'K':
						{
							possibleCastlingRookSquares[0] = whiteKingLateralSquares[0];
							break;
						}

						case 'Q':
						{
							possibleCastlingRookSquares[1] = whiteKingLateralSquares[1];
							break;
						}

						case 'k':
						{
							possibleCastlingRookSquares[2] = whiteKingLateralSquares[2];
							break;
						}

						case 'q':
						{
							possibleCastlingRookSquares[3] = whiteKingLateralSquares[3];
							break;
						}

						default:
						{
							// Shredder-FEN receives the name of the file where the castling rook is
							int whiteFile = "ABCDEFGH".IndexOf(c);
							int blackFile = "abcdefgh".IndexOf(c);
							if (whiteFile >= 0)
							{
								long rookSquare = BitboardUtils.b_d & BitboardUtils.File[whiteFile];
								if ((rookSquare & whiteKingLateralSquares[0]) != 0)
								{
									possibleCastlingRookSquares[0] = rookSquare;
								}
								else
								{
									if ((rookSquare & whiteKingLateralSquares[1]) != 0)
									{
										possibleCastlingRookSquares[1] = rookSquare;
									}
								}
							}
							else
							{
								if (blackFile >= 0)
								{
									long rookSquare = BitboardUtils.b_u & BitboardUtils.File[blackFile];
									if ((rookSquare & whiteKingLateralSquares[2]) != 0)
									{
										possibleCastlingRookSquares[2] = rookSquare;
									}
									else
									{
										if ((rookSquare & whiteKingLateralSquares[3]) != 0)
										{
											possibleCastlingRookSquares[3] = rookSquare;
										}
									}
								}
							}
							break;
						}
					}
				}
				// Now store the squares of the castling rooks
				tmpCastlingRooks[0] = BitboardUtils.Lsb(tmpRooks & tmpWhites & possibleCastlingRookSquares
					[0]);
				tmpCastlingRooks[1] = BitboardUtils.Msb(tmpRooks & tmpWhites & possibleCastlingRookSquares
					[1]);
				tmpCastlingRooks[2] = BitboardUtils.Lsb(tmpRooks & tmpBlacks & possibleCastlingRookSquares
					[2]);
				tmpCastlingRooks[3] = BitboardUtils.Msb(tmpRooks & tmpBlacks & possibleCastlingRookSquares
					[3]);
				// Set the castling flags and detect Chess960
				if (tmpCastlingRooks[0] != 0)
				{
					tmpFlags |= FlagWhiteKingsideCastling;
					if ((tmpWhites & tmpKings) != 1L << 3 || tmpCastlingRooks[0] != 1L)
					{
						chess960 = true;
					}
				}
				if (tmpCastlingRooks[1] != 0)
				{
					tmpFlags |= FlagWhiteQueensideCastling;
					if ((tmpWhites & tmpKings) != 1L << 3 || tmpCastlingRooks[1] != 1L << 7)
					{
						chess960 = true;
					}
				}
				if (tmpCastlingRooks[2] != 0)
				{
					tmpFlags |= FlagBlackKingsideCastling;
					if ((tmpBlacks & tmpKings) != 1L << 59 || tmpCastlingRooks[2] != 1L << 56)
					{
						chess960 = true;
					}
				}
				if (tmpCastlingRooks[3] != 0)
				{
					tmpFlags |= FlagBlackQueensideCastling;
					if ((tmpBlacks & tmpKings) != 1L << 59 || tmpCastlingRooks[3] != 1L << 63)
					{
						chess960 = true;
					}
				}
				// END FEN castlings
				if (tokens.Length > 3)
				{
					string passant = tokens[3];
					tmpFlags |= FlagsPassant & BitboardUtils.Algebraic2Square(passant);
					if (tokens.Length > 4)
					{
						try
						{
							tmpFiftyMovesRule = System.Convert.ToInt32(tokens[4]);
						}
						catch (Exception)
						{
							tmpFiftyMovesRule = 0;
						}
						if (tokens.Length > 5)
						{
							string moveNumberString = tokens[5];
							int aux = System.Convert.ToInt32(moveNumberString);
							fenMoveNumber = ((aux > 0 ? aux - 1 : aux) << 1) + ((tmpFlags & FlagTurn) == 0 ? 
								0 : 1);
							if (fenMoveNumber < 0)
							{
								fenMoveNumber = 0;
							}
						}
					}
				}
			}
			// try to apply the last move to see if we are advancing or undoing moves
			if ((moveNumber + 1) == fenMoveNumber && lastMove != null)
			{
				DoMove(Move.GetFromString(this, lastMove, true));
			}
			else
			{
				if (fenMoveNumber < moveNumber)
				{
					for (int k = moveNumber; k > fenMoveNumber; k--)
					{
						UndoMove();
					}
				}
			}
			// Check if board changed or if we can keep the history
			if (whites != tmpWhites || blacks != tmpBlacks || pawns != tmpPawns || rooks != tmpRooks
				 || queens != tmpQueens || bishops != tmpBishops || knights != tmpKnights || kings
				 != tmpKings || (flags & FlagTurn) != (tmpFlags & FlagTurn))
			{
				//
				//
				//
				//
				//
				//
				//
				//
				// board reset
				movesSan.Clear();
				initialFen = fen;
				initialMoveNumber = fenMoveNumber;
				moveNumber = fenMoveNumber;
				outBookMove = int.MaxValue;
				whites = tmpWhites;
				blacks = tmpBlacks;
				pawns = tmpPawns;
				rooks = tmpRooks;
				queens = tmpQueens;
				bishops = tmpBishops;
				knights = tmpKnights;
				kings = tmpKings;
				fiftyMovesRule = tmpFiftyMovesRule;
				// Flags are not completed till verify, so skip checking
				flags = tmpFlags;
				castlingRooks[0] = tmpCastlingRooks[0];
				castlingRooks[1] = tmpCastlingRooks[1];
				castlingRooks[2] = tmpCastlingRooks[2];
				castlingRooks[3] = tmpCastlingRooks[3];
				// Set zobrist key and check flags
				key = ZobristKey.GetKey(this);
				SetCheckFlags();
				// and save history
				ResetHistory();
				SaveHistory(0, false);
			}
			else
			{
				if (moveNumber < outBookMove)
				{
					outBookMove = int.MaxValue;
				}
			}
		}

		/// <summary>Prints board in one string</summary>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			int j = 8;
			long i = Square.A8;
			while (i != 0)
			{
				sb.Append(GetPieceUnicodeAt(i));
				sb.Append(" ");
				if ((i & BitboardUtils.b_r) != 0)
				{
					sb.Append(j--);
					if (i == Square.H1)
					{
						sb.Append(" ");
						sb.Append(GetFen());
					}
					sb.Append("\n");
				}
				i = (long)(((ulong)i) >> 1);
			}
			sb.Append("a b c d e f g h   ");
			sb.Append((GetTurn() ? "white moves " : "black moves "));
			sb.Append((GetWhiteKingsideCastling() ? " W:0-0" : string.Empty) + (GetWhiteQueensideCastling
				() ? " W:0-0-0" : string.Empty) + (GetBlackKingsideCastling() ? " B:0-0" : string.Empty
				) + (GetBlackQueensideCastling() ? " B:0-0-0" : string.Empty));
			return sb.ToString();
		}

		/// <summary>TODO is it necessary??</summary>
		private void ResetHistory()
		{
			Arrays.Fill(whitesHistory, 0);
			Arrays.Fill(blacksHistory, 0);
			Arrays.Fill(pawnsHistory, 0);
			Arrays.Fill(knightsHistory, 0);
			Arrays.Fill(bishopsHistory, 0);
			Arrays.Fill(rooksHistory, 0);
			Arrays.Fill(queensHistory, 0);
			Arrays.Fill(kingsHistory, 0);
			Arrays.Fill(flagsHistory, 0);
			for (int i = 0; i < MaxMoves; i++)
			{
				Arrays.Fill(keyHistory[i], 0);
			}
			Arrays.Fill(fiftyMovesRuleHistory, 0);
			Arrays.Fill(moveHistory, 0);
			movesSan.Clear();
		}

		private void SaveHistory(int move, bool fillSanInfo)
		{
			if (fillSanInfo)
			{
				movesSan[moveNumber] = Move.ToSan(this, move);
			}
			moveHistory[moveNumber] = move;
			whitesHistory[moveNumber] = whites;
			blacksHistory[moveNumber] = blacks;
			pawnsHistory[moveNumber] = pawns;
			knightsHistory[moveNumber] = knights;
			bishopsHistory[moveNumber] = bishops;
			rooksHistory[moveNumber] = rooks;
			queensHistory[moveNumber] = queens;
			kingsHistory[moveNumber] = kings;
			flagsHistory[moveNumber] = flags;
			keyHistory[moveNumber][0] = key[0];
			keyHistory[moveNumber][1] = key[1];
			fiftyMovesRuleHistory[moveNumber] = fiftyMovesRule;
		}

		/// <summary>This is very inefficient because it fills the San info, so it must not be called from inside search
		/// 	</summary>
		public virtual bool DoMove(int move)
		{
			return DoMove(move, true, true);
		}

		/// <summary>
		/// Moves and also updates the board's zobrist key verify legality, if not
		/// legal undo move and return false
		/// </summary>
		public virtual bool DoMove(int move, bool verify, bool fillSanInfo)
		{
			if (move == Move.None)
			{
				return false;
			}
			// Save history
			SaveHistory(move, fillSanInfo);
			// Count consecutive moves without capture or without pawn move
			fiftyMovesRule++;
			moveNumber++;
			// Count Ply moves
			bool turn = GetTurn();
			int color = turn ? Color.W : Color.B;
			if ((flags & FlagsPassant) != 0)
			{
				// Remove passant flags: from the zobrist key
				key[1 - color] ^= ZobristKey.passantFile[BitboardUtils.GetFile(flags & FlagsPassant
					)];
				// and from the flags
				flags &= ~FlagsPassant;
			}
			if (move == Move.Null)
			{
				// Change turn
				flags ^= FlagTurn;
				key[0] ^= ZobristKey.whiteMove;
				return true;
			}
			int fromIndex = Move.GetFromIndex(move);
			long from = Move.GetFromSquare(move);
			// Check if we are applying a move in the other turn
			if ((from & GetMines()) == 0)
			{
				UndoMove();
				return false;
			}
			int toIndex = Move.GetToIndex(move);
			long to = Move.GetToSquare(move);
			long moveMask = from | to;
			// Move is as easy as xor with this mask (exceptions are promotions, captures and en-passant captures)
			int moveType = Move.GetMoveType(move);
			int pieceMoved = Move.GetPieceMoved(move);
			bool capture = Move.IsCapture(move);
			// Is it is a capture, remove pieces in destination square
			if (capture)
			{
				fiftyMovesRule = 0;
				// En-passant pawn captures remove captured pawn, put the pawn in to
				int toIndexCapture = toIndex;
				if (moveType == Move.TypePassant)
				{
					to = (GetTurn() ? ((long)(((ulong)to) >> 8)) : (to << 8));
					toIndexCapture += (GetTurn() ? -8 : 8);
				}
				key[1 - color] ^= ZobristKey.GetKeyPieceIndex(toIndexCapture, GetPieceAt(to));
				whites &= ~to;
				blacks &= ~to;
				pawns &= ~to;
				queens &= ~to;
				rooks &= ~to;
				bishops &= ~to;
				knights &= ~to;
			}
			switch (pieceMoved)
			{
				case Piece.Pawn:
				{
					// Pawn movements
					fiftyMovesRule = 0;
					// Set new passant flags if pawn is advancing two squares (marks
					// the destination square where the pawn can be captured)
					// Set only passant flags when the other side can capture
					if (((from << 16) & to) != 0 && (bbAttacks.pawn[Color.W][toIndex - 8] & pawns & GetOthers
						()) != 0)
					{
						// white
						flags |= (from << 8);
					}
					if ((((long)(((ulong)from) >> 16)) & to) != 0 && (bbAttacks.pawn[Color.B][toIndex
						 + 8] & pawns & GetOthers()) != 0)
					{
						// blask
						flags |= ((long)(((ulong)from) >> 8));
					}
					if ((flags & FlagsPassant) != 0)
					{
						key[color] ^= ZobristKey.passantFile[BitboardUtils.GetFile(flags & FlagsPassant)];
					}
					if (moveType == Move.TypePromotionQueen || moveType == Move.TypePromotionKnight ||
						 moveType == Move.TypePromotionBishop || moveType == Move.TypePromotionRook)
					{
						// Promotions:
						// change
						// the piece
						pawns &= ~from;
						key[color] ^= ZobristKey.pawn[color][fromIndex];
						switch (moveType)
						{
							case Move.TypePromotionQueen:
							{
								queens |= to;
								key[color] ^= ZobristKey.queen[color][toIndex];
								break;
							}

							case Move.TypePromotionKnight:
							{
								knights |= to;
								key[color] ^= ZobristKey.knight[color][toIndex];
								break;
							}

							case Move.TypePromotionBishop:
							{
								bishops |= to;
								key[color] ^= ZobristKey.bishop[color][toIndex];
								break;
							}

							case Move.TypePromotionRook:
							{
								rooks |= to;
								key[color] ^= ZobristKey.rook[color][toIndex];
								break;
							}
						}
					}
					else
					{
						pawns ^= moveMask;
						key[color] ^= ZobristKey.pawn[color][fromIndex] ^ ZobristKey.pawn[color][toIndex];
					}
					break;
				}

				case Piece.Rook:
				{
					rooks ^= moveMask;
					key[color] ^= ZobristKey.rook[color][fromIndex] ^ ZobristKey.rook[color][toIndex];
					break;
				}

				case Piece.Bishop:
				{
					bishops ^= moveMask;
					key[color] ^= ZobristKey.bishop[color][fromIndex] ^ ZobristKey.bishop[color][toIndex
						];
					break;
				}

				case Piece.Knight:
				{
					knights ^= moveMask;
					key[color] ^= ZobristKey.knight[color][fromIndex] ^ ZobristKey.knight[color][toIndex
						];
					break;
				}

				case Piece.Queen:
				{
					queens ^= moveMask;
					key[color] ^= ZobristKey.queen[color][fromIndex] ^ ZobristKey.queen[color][toIndex
						];
					break;
				}

				case Piece.King:
				{
					// if castling, moves rooks too
					if (moveType == Move.TypeKingsideCastling || moveType == Move.TypeQueensideCastling)
					{
						// {White Kingside, White Queenside, Black Kingside, Black Queenside}
						int j = (color << 1) + (moveType == Move.TypeQueensideCastling ? 1 : 0);
						toIndex = CastlingKingDestinyIndex[j];
						int originRookIndex = BitboardUtils.Square2Index(castlingRooks[j]);
						int destinyRookIndex = CastlingRookDestinyIndex[j];
						// Recalculate move mask for chess960 castlings
						moveMask = from ^ (1L << toIndex);
						long rookMoveMask = (1L << originRookIndex) ^ (1L << destinyRookIndex);
						key[color] ^= ZobristKey.rook[color][originRookIndex] ^ ZobristKey.rook[color][destinyRookIndex
							];
						if (GetTurn())
						{
							whites ^= rookMoveMask;
						}
						else
						{
							blacks ^= rookMoveMask;
						}
						rooks ^= rookMoveMask;
					}
					kings ^= moveMask;
					key[color] ^= ZobristKey.king[color][fromIndex] ^ ZobristKey.king[color][toIndex];
					break;
				}
			}
			// Move pieces in colour fields
			if (GetTurn())
			{
				whites ^= moveMask;
			}
			else
			{
				blacks ^= moveMask;
			}
			// Tests to disable castling
			if ((flags & FlagWhiteKingsideCastling) != 0 && ((turn && pieceMoved == Piece.King
				) || from == castlingRooks[0] || to == castlingRooks[0]))
			{
				//
				flags &= ~FlagWhiteKingsideCastling;
				key[0] ^= ZobristKey.whiteKingSideCastling;
			}
			if ((flags & FlagWhiteQueensideCastling) != 0 && ((turn && pieceMoved == Piece.King
				) || from == castlingRooks[1] || to == castlingRooks[1]))
			{
				//
				flags &= ~FlagWhiteQueensideCastling;
				key[0] ^= ZobristKey.whiteQueenSideCastling;
			}
			if ((flags & FlagBlackKingsideCastling) != 0 && ((!turn && pieceMoved == Piece.King
				) || from == castlingRooks[2] || to == castlingRooks[2]))
			{
				//
				flags &= ~FlagBlackKingsideCastling;
				key[1] ^= ZobristKey.blackKingSideCastling;
			}
			if ((flags & FlagBlackQueensideCastling) != 0 && ((!turn && pieceMoved == Piece.King
				) || from == castlingRooks[3] || to == castlingRooks[3]))
			{
				//
				flags &= ~FlagBlackQueensideCastling;
				key[1] ^= ZobristKey.blackQueenSideCastling;
			}
			// Change turn
			flags ^= FlagTurn;
			key[0] ^= ZobristKey.whiteMove;
			if (verify)
			{
				if (IsValid())
				{
					SetCheckFlags();
					if (fillSanInfo)
					{
						if (IsMate())
						{
							// Append # when mate
							movesSan[moveNumber - 1] = movesSan[moveNumber - 1].Replace("+", "#");
						}
					}
				}
				else
				{
					UndoMove();
					return false;
				}
			}
			else
			{
				// Trust move check flag
				if (Move.IsCheck(move))
				{
					flags |= FlagCheck;
				}
				else
				{
					flags &= ~FlagCheck;
				}
			}
			return true;
		}

		/// <summary>It checks if a state is valid basically, if the other king is not in check
		/// 	</summary>
		private bool IsValid()
		{
			return (!bbAttacks.IsSquareAttacked(this, kings & GetOthers(), !GetTurn()));
		}

		/// <summary>Sets check flag if the own king is in check</summary>
		private void SetCheckFlags()
		{
			if (bbAttacks.IsSquareAttacked(this, kings & GetMines(), GetTurn()))
			{
				flags |= FlagCheck;
			}
			else
			{
				flags &= ~FlagCheck;
			}
		}

		public virtual void UndoMove()
		{
			UndoMove(moveNumber - 1);
		}

		public virtual void UndoMove(int moveNumber)
		{
			if (moveNumber < 0 || moveNumber < initialMoveNumber)
			{
				return;
			}
			this.moveNumber = moveNumber;
			whites = whitesHistory[moveNumber];
			blacks = blacksHistory[moveNumber];
			pawns = pawnsHistory[moveNumber];
			knights = knightsHistory[moveNumber];
			bishops = bishopsHistory[moveNumber];
			rooks = rooksHistory[moveNumber];
			queens = queensHistory[moveNumber];
			kings = kingsHistory[moveNumber];
			flags = flagsHistory[moveNumber];
			key[0] = keyHistory[moveNumber][0];
			key[1] = keyHistory[moveNumber][1];
			fiftyMovesRule = fiftyMovesRuleHistory[moveNumber];
		}

		/// <summary>0 no, 1 whites won, -1 blacks won, 99 draw</summary>
		public virtual int IsEndGame()
		{
			int endGame = 0;
			GenerateLegalMoves();
			if (legalMoveCount == 0)
			{
				if (GetCheck())
				{
					endGame = (GetTurn() ? -1 : 1);
				}
				else
				{
					endGame = 99;
				}
			}
			else
			{
				if (IsDraw())
				{
					endGame = 99;
				}
			}
			return endGame;
		}

		public virtual bool IsMate()
		{
			int endgameState = IsEndGame();
			return endgameState == 1 || endgameState == -1;
		}

		/// <summary>checks draw by fifty move rule and threefold repetition</summary>
		public virtual bool IsDraw()
		{
			if (fiftyMovesRule >= 100)
			{
				return true;
			}
			int repetitions = 0;
			for (int i = 0; i < (moveNumber - 1); i++)
			{
				if (keyHistory[i][0] == key[0] && keyHistory[i][1] == key[1])
				{
					repetitions++;
				}
				if (repetitions >= 2)
				{
					// with the last one they are 3
					return true;
				}
			}
			// Draw by no material to mate
			// Kk, KNk, KNNk, KBK by FIDE rules, be careful: KNnk IS NOT a draw
			return (pawns == 0 && rooks == 0 && queens == 0) && ((bishops == 0 && knights == 
				0) || (knights == 0 && BitboardUtils.PopCount(bishops) == 1) || (bishops == 0 &&
				 (BitboardUtils.PopCount(knights) == 1 || (BitboardUtils.PopCount(knights) == 2 
				&& (BitboardUtils.PopCount(knights & whites) == 2 || BitboardUtils.PopCount(knights
				 & ~whites) == 2)))));
		}

		//
		// KNNk, check same color
		public virtual int See(int move)
		{
			return See(Move.GetFromIndex(move), Move.GetToIndex(move), Move.GetPieceMoved(move
				), Move.IsCapture(move) ? Move.GetPieceCaptured(this, move) : 0);
		}

		public virtual int See(int move, AttacksInfo attacksInfo)
		{
			int them = GetTurn() ? 1 : 0;
			if ((attacksInfo.attackedSquares[them] & Move.GetToSquare(move)) == 0 && (attacksInfo
				.mayPin[them] & Move.GetFromSquare(move)) == 0)
			{
				return Move.IsCapture(move) ? Com.Alonsoruibal.Chess.Board.SeePieceValues[Move.GetPieceCaptured
					(this, move)] : 0;
			}
			else
			{
				return See(move);
			}
		}

		/// <summary>The SWAP algorithm https://chessprogramming.wikispaces.com/SEE+-+The+Swap+Algorithm
		/// 	</summary>
		public virtual int See(int fromIndex, int toIndex, int pieceMoved, int targetPiece
			)
		{
			int d = 0;
			long mayXray = pawns | bishops | rooks | queens;
			// not kings nor knights
			long fromSquare = unchecked((long)(0x1L)) << fromIndex;
			long all = GetAll();
			long attacks = bbAttacks.GetIndexAttacks(this, toIndex);
			long fromCandidates;
			seeGain[d] = SeePieceValues[targetPiece];
			do
			{
				long side = (d & 1) == 0 ? GetOthers() : GetMines();
				d++;
				// next depth and side speculative store, if defended
				seeGain[d] = SeePieceValues[pieceMoved] - seeGain[d - 1];
				attacks ^= fromSquare;
				// reset bit in set to traverse
				all ^= fromSquare;
				// reset bit in temporary occupancy (for X-Rays)
				if ((fromSquare & mayXray) != 0)
				{
					attacks |= bbAttacks.GetXrayAttacks(this, toIndex, all);
				}
				// Gets the next attacker
				if ((fromCandidates = attacks & pawns & side) != 0)
				{
					pieceMoved = Piece.Pawn;
				}
				else
				{
					if ((fromCandidates = attacks & knights & side) != 0)
					{
						pieceMoved = Piece.Knight;
					}
					else
					{
						if ((fromCandidates = attacks & bishops & side) != 0)
						{
							pieceMoved = Piece.Bishop;
						}
						else
						{
							if ((fromCandidates = attacks & rooks & side) != 0)
							{
								pieceMoved = Piece.Rook;
							}
							else
							{
								if ((fromCandidates = attacks & queens & side) != 0)
								{
									pieceMoved = Piece.Queen;
								}
								else
								{
									if ((fromCandidates = attacks & kings & side) != 0)
									{
										pieceMoved = Piece.King;
									}
								}
							}
						}
					}
				}
				fromSquare = BitboardUtils.Lsb(fromCandidates);
			}
			while (fromSquare != 0);
			while (--d != 0)
			{
				seeGain[d - 1] = -Math.Max(-seeGain[d - 1], seeGain[d]);
			}
			return seeGain[0];
		}

		public virtual bool IsUsingBook()
		{
			return outBookMove > moveNumber;
		}

		public virtual void SetOutBookMove(int outBookMove)
		{
			this.outBookMove = outBookMove;
		}

		/// <summary>Check if a passed pawn is in the index, useful to trigger extensions</summary>
		public virtual bool IsPassedPawn(int index)
		{
			int rank = index >> 3;
			int file = 7 - index & 7;
			long square = unchecked((long)(0x1L)) << index;
			if ((whites & square) != 0)
			{
				return ((BitboardUtils.File[file] | BitboardUtils.FilesAdjacent[file]) & BitboardUtils
					.RanksUpwards[rank] & pawns & blacks) == 0;
			}
			else
			{
				if ((blacks & square) != 0)
				{
					return ((BitboardUtils.File[file] | BitboardUtils.FilesAdjacent[file]) & BitboardUtils
						.RanksDownwards[rank] & pawns & whites) == 0;
				}
			}
			return false;
		}

		/// <summary>Returns true if move is legal</summary>
		public virtual bool IsMoveLegal(int move)
		{
			GenerateLegalMoves();
			for (int i = 0; i < legalMoveCount; i++)
			{
				// logger.debug(Move.toStringExt(legalMoves[i]));
				if (move == legalMoves[i])
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Generates legal moves for the position when not already generated</summary>
		internal virtual void GenerateLegalMoves()
		{
			if ((key[0] != legalMovesKey[0]) || (key[1] != legalMovesKey[1]))
			{
				legalMoveCount = legalMoveGenerator.GenerateMoves(this, legalMoves, 0);
				legalMovesKey[0] = key[0];
				legalMovesKey[1] = key[1];
			}
		}

		public virtual int GetLegalMoves(int[] moves)
		{
			GenerateLegalMoves();
			System.Array.Copy(legalMoves, 0, moves, 0, (legalMoveCount != -1 ? legalMoveCount
				 : 0));
			return legalMoveCount;
		}

		public virtual string GetSanMove(int moveNumber)
		{
			return movesSan[moveNumber];
		}

		public virtual bool GetMoveTurn(int moveNumber)
		{
			return (flagsHistory[moveNumber] & FlagTurn) == 0;
		}

		public virtual string GetInitialFen()
		{
			return initialFen;
		}

		public virtual string GetMoves()
		{
			StringBuilder oSB = new StringBuilder();
			for (int i = initialMoveNumber; i < moveNumber; i++)
			{
				if (oSB.Length > 0)
				{
					oSB.Append(" ");
				}
				oSB.Append(Move.ToString(moveHistory[i]));
			}
			return oSB.ToString();
		}

		public virtual string GetMovesSan()
		{
			StringBuilder oSB = new StringBuilder();
			for (int i = initialMoveNumber; i < moveNumber; i++)
			{
				if (oSB.Length > 0)
				{
					oSB.Append(" ");
				}
				oSB.Append(movesSan[i]);
			}
			return oSB.ToString();
		}

		public virtual string ToSanNextMoves(string moves)
		{
			if (moves == null || string.Empty.Equals(moves.Trim()))
			{
				return string.Empty;
			}
			StringBuilder oSB = new StringBuilder();
			string[] movesArray = moves.Split(" ");
			int savedMoveNumber = moveNumber;
			foreach (string moveString in movesArray)
			{
				int move = Move.GetFromString(this, moveString, true);
				if (!DoMove(move))
				{
					UndoMove(savedMoveNumber);
					return string.Empty;
				}
				if (oSB.Length > 0)
				{
					oSB.Append(" ");
				}
				oSB.Append(GetLastMoveSan());
			}
			UndoMove(savedMoveNumber);
			return oSB.ToString();
		}

		public virtual int GetLastMove()
		{
			if (moveNumber == 0)
			{
				return Move.None;
			}
			return moveHistory[moveNumber - 1];
		}

		public virtual string GetLastMoveSan()
		{
			if (moveNumber == 0)
			{
				return null;
			}
			return movesSan[moveNumber - 1];
		}

		/// <summary>Convenience method to apply all the moves in a string separated by spaces
		/// 	</summary>
		public virtual void DoMoves(string moves)
		{
			if (moves == null || string.Empty.Equals(moves.Trim()))
			{
				return;
			}
			string[] movesArray = moves.Split(" ");
			foreach (string moveString in movesArray)
			{
				int move = Move.GetFromString(this, moveString, true);
				DoMove(move);
			}
		}
	}
}
