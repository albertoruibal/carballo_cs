using System.Text;
using Com.Alonsoruibal.Chess.Bitboard;
using Sharpen;

namespace Com.Alonsoruibal.Chess
{
	/// <summary>
	/// For efficiency Moves are int, this is a static class to threat with this
	/// <p>
	/// Move format (18 bits):
	/// MTXCPPPFFFFFFTTTTTT
	/// -------------^ To index (6 bits)
	/// -------^ From index (6 bits)
	/// ----^ Piece moved (3 bits)
	/// ---^ Is capture (1 bit)
	/// --^ Is check (1 bit)
	/// ^ Move type (2 bits)
	/// </summary>
	/// <author>Alberto Alonso Ruibal</author>
	public class Move
	{
		public const int None = 0;

		public const int Null = -1;

		public const string NoneString = "none";

		public const string NullString = "null";

		public const string PieceLettersLowercase = " pnbrqk";

		public const string PieceLettersUppercase = " PNBRQK";

		public const int TypeKingsideCastling = 1;

		public const int TypeQueensideCastling = 2;

		public const int TypePassant = 3;

		public const int TypePromotionQueen = 4;

		public const int TypePromotionKnight = 5;

		public const int TypePromotionBishop = 6;

		public const int TypePromotionRook = 7;

		public const int CheckMask = unchecked((int)(0x1)) << 16;

		public const int CaptureMask = unchecked((int)(0x1)) << 15;

		// Predefined moves
		// Move Types
		// Promotions must be always >= TYPE_PROMOTION_QUEEN
		public static int GenMove(int fromIndex, int toIndex, int pieceMoved, bool capture
			, bool check, int moveType)
		{
			return toIndex | fromIndex << 6 | pieceMoved << 12 | (capture ? CaptureMask : 0) 
				| (check ? CheckMask : 0) | moveType << 17;
		}

		public static int GenMove(int fromIndex, int toIndex, int pieceMoved, bool capture
			, int moveType)
		{
			return toIndex | fromIndex << 6 | pieceMoved << 12 | (capture ? CaptureMask : 0) 
				| moveType << 17;
		}

		public static int GetToIndex(int move)
		{
			return move & unchecked((int)(0x3f));
		}

		public static long GetToSquare(int move)
		{
			return unchecked((long)(0x1L)) << (move & unchecked((int)(0x3f)));
		}

		public static int GetFromIndex(int move)
		{
			return (((int)(((uint)move) >> 6)) & unchecked((int)(0x3f)));
		}

		public static long GetFromSquare(int move)
		{
			return unchecked((long)(0x1L)) << (((int)(((uint)move) >> 6)) & unchecked((int)(0x3f
				)));
		}

		public static int GetPieceMoved(int move)
		{
			return (((int)(((uint)move) >> 12)) & unchecked((int)(0x7)));
		}

		public static int GetPieceCaptured(Board board, int move)
		{
			if (GetMoveType(move) == TypePassant)
			{
				return Piece.Pawn;
			}
			long toSquare = GetToSquare(move);
			if ((toSquare & board.pawns) != 0)
			{
				return Piece.Pawn;
			}
			else
			{
				if ((toSquare & board.knights) != 0)
				{
					return Piece.Knight;
				}
				else
				{
					if ((toSquare & board.bishops) != 0)
					{
						return Piece.Bishop;
					}
					else
					{
						if ((toSquare & board.rooks) != 0)
						{
							return Piece.Rook;
						}
						else
						{
							if ((toSquare & board.queens) != 0)
							{
								return Piece.Queen;
							}
						}
					}
				}
			}
			return 0;
		}

		public static bool IsCapture(int move)
		{
			return (move & CaptureMask) != 0;
		}

		public static bool IsCheck(int move)
		{
			return (move & CheckMask) != 0;
		}

		public static bool IsCaptureOrCheck(int move)
		{
			return (move & (CheckMask | CaptureMask)) != 0;
		}

		public static int GetMoveType(int move)
		{
			return (((int)(((uint)move) >> 17)) & unchecked((int)(0x7)));
		}

		// Pawn push to 7 or 8th rank
		public static bool IsPawnPush(int move)
		{
			return Move.GetPieceMoved(move) == Piece.Pawn && (Move.GetToIndex(move) < 16 || Move
				.GetToIndex(move) > 47);
		}

		// Pawn push to 6, 7 or 8th rank
		public static bool IsPawnPush678(int move)
		{
			return Move.GetPieceMoved(move) == Piece.Pawn && (Move.GetFromIndex(move) < Move.
				GetToIndex(move) ? Move.GetToIndex(move) >= 40 : Move.GetToIndex(move) < 24);
		}

		// Pawn push to 5, 6, 7 or 8th rank
		public static bool IsPawnPush5678(int move)
		{
			return Move.GetPieceMoved(move) == Piece.Pawn && (Move.GetFromIndex(move) < Move.
				GetToIndex(move) ? Move.GetToIndex(move) >= 32 : Move.GetToIndex(move) < 32);
		}

		/// <summary>Checks if this move is a promotion</summary>
		public static bool IsPromotion(int move)
		{
			return Move.GetMoveType(move) >= TypePromotionQueen;
		}

		public static int GetPiecePromoted(int move)
		{
			switch (GetMoveType(move))
			{
				case TypePromotionQueen:
				{
					return Piece.Queen;
				}

				case TypePromotionRook:
				{
					return Piece.Rook;
				}

				case TypePromotionKnight:
				{
					return Piece.Knight;
				}

				case TypePromotionBishop:
				{
					return Piece.Bishop;
				}
			}
			return 0;
		}

		/// <summary>Is capture or promotion</summary>
		/// <param name="move"/>
		/// <returns/>
		public static bool IsTactical(int move)
		{
			return (Move.IsCapture(move) || Move.IsPromotion(move));
		}

		public static bool IsCastling(int move)
		{
			return Move.GetMoveType(move) == TypeKingsideCastling || Move.GetMoveType(move) ==
				 TypeQueensideCastling;
		}

		/// <summary>
		/// Given a board creates a move from a String in uci format or short
		/// algebraic form.
		/// </summary>
		/// <remarks>
		/// Given a board creates a move from a String in uci format or short
		/// algebraic form. Checklegality true is mandatory if using sort algebraic
		/// </remarks>
		/// <param name="board"/>
		/// <param name="move"/>
		public static int GetFromString(Board board, string move, bool checkLegality)
		{
			if (NullString.Equals(move))
			{
				return Move.Null;
			}
			else
			{
				if (string.Empty.Equals(move) || NoneString.Equals(move))
				{
					return Move.None;
				}
			}
			int fromIndex;
			int toIndex;
			int moveType = 0;
			int pieceMoved = 0;
			bool check = move.IndexOf("+") > 0 || move.IndexOf("#") > 0;
			long mines = board.GetMines();
			bool turn = board.GetTurn();
			// Ignore checks, captures indicators...
			move = move.Replace("+", string.Empty).Replace("x", string.Empty).Replace("-", string.Empty
				).Replace("=", string.Empty).Replace("#", string.Empty).ReplaceAll(" ", string.Empty
				).ReplaceAll("0", "o").ReplaceAll("O", "o");
			if ("oo".Equals(move))
			{
				move = BitboardUtils.SquareNames[BitboardUtils.Square2Index(board.kings & mines)]
					 + BitboardUtils.SquareNames[BitboardUtils.Square2Index(board.chess960 ? board.castlingRooks
					[turn ? 0 : 2] : Board.CastlingKingDestinySquare[turn ? 0 : 2])];
			}
			else
			{
				//
				if ("ooo".Equals(move))
				{
					move = BitboardUtils.SquareNames[BitboardUtils.Square2Index(board.kings & mines)]
						 + BitboardUtils.SquareNames[BitboardUtils.Square2Index(board.chess960 ? board.castlingRooks
						[turn ? 1 : 3] : Board.CastlingKingDestinySquare[turn ? 1 : 3])];
				}
				else
				{
					//
					char promo = move[move.Length - 1];
					switch (System.Char.ToLower(promo))
					{
						case 'q':
						{
							moveType = TypePromotionQueen;
							break;
						}

						case 'n':
						{
							moveType = TypePromotionKnight;
							break;
						}

						case 'b':
						{
							moveType = TypePromotionBishop;
							break;
						}

						case 'r':
						{
							moveType = TypePromotionRook;
							break;
						}
					}
					// If promotion, remove the last char
					if (moveType != 0)
					{
						move = Sharpen.Runtime.Substring(move, 0, move.Length - 1);
					}
				}
			}
			// To is always the last 2 characters
			toIndex = BitboardUtils.Algebraic2Index(Sharpen.Runtime.Substring(move, move.Length
				 - 2, move.Length));
			long to = unchecked((long)(0x1L)) << toIndex;
			long from = 0;
			BitboardAttacks bbAttacks = BitboardAttacks.GetInstance();
			switch (move[0])
			{
				case 'N':
				{
					// Fills from with a mask of possible from values
					from = board.knights & mines & bbAttacks.knight[toIndex];
					break;
				}

				case 'K':
				{
					from = board.kings & mines & bbAttacks.king[toIndex];
					break;
				}

				case 'R':
				{
					from = board.rooks & mines & bbAttacks.GetRookAttacks(toIndex, board.GetAll());
					break;
				}

				case 'B':
				{
					from = board.bishops & mines & bbAttacks.GetBishopAttacks(toIndex, board.GetAll()
						);
					break;
				}

				case 'Q':
				{
					from = board.queens & mines & (bbAttacks.GetRookAttacks(toIndex, board.GetAll()) 
						| bbAttacks.GetBishopAttacks(toIndex, board.GetAll()));
					break;
				}
			}
			if (from != 0)
			{
				// remove the piece char
				move = Sharpen.Runtime.Substring(move, 1);
			}
			else
			{
				// Pawn moves
				if (move.Length == 2)
				{
					if (turn)
					{
						from = board.pawns & mines & (((long)(((ulong)to) >> 8)) | ((((long)(((ulong)to) 
							>> 8)) & board.GetAll()) == 0 ? ((long)(((ulong)to) >> 16)) : 0));
					}
					else
					{
						from = board.pawns & mines & ((to << 8) | (((to << 8) & board.GetAll()) == 0 ? (to
							 << 16) : 0));
					}
				}
				if (move.Length == 3)
				{
					// Pawn capture
					from = board.pawns & mines & bbAttacks.pawn[turn ? Color.B : Color.W][toIndex];
				}
			}
			if (move.Length == 3)
			{
				// now disambiaguate
				char disambiguate = move[0];
				int i = "abcdefgh".IndexOf(disambiguate);
				if (i >= 0)
				{
					from &= BitboardUtils.File[i];
				}
				int j = "12345678".IndexOf(disambiguate);
				if (j >= 0)
				{
					from &= BitboardUtils.Rank[j];
				}
			}
			if (move.Length == 4)
			{
				// was algebraic complete e2e4 (=UCI!)
				from = BitboardUtils.Algebraic2Square(Sharpen.Runtime.Substring(move, 0, 2));
			}
			if (from == 0 || (from & board.GetMines()) == 0)
			{
				return None;
			}
			// Treats multiple froms, choosing the first Legal Move
			while (from != 0)
			{
				long myFrom = BitboardUtils.Lsb(from);
				from ^= myFrom;
				fromIndex = BitboardUtils.Square2Index(myFrom);
				bool capture = false;
				if ((myFrom & board.pawns) != 0)
				{
					pieceMoved = Piece.Pawn;
					// for passant captures
					if ((toIndex != (fromIndex - 8)) && (toIndex != (fromIndex + 8)) && (toIndex != (
						fromIndex - 16)) && (toIndex != (fromIndex + 16)))
					{
						if ((to & board.GetAll()) == 0)
						{
							moveType = TypePassant;
							capture = true;
						}
					}
					// later is changed if it was not a pawn
					// Default promotion to queen if not specified
					if ((to & (BitboardUtils.b_u | BitboardUtils.b_d)) != 0 && (moveType < TypePromotionQueen
						))
					{
						moveType = TypePromotionQueen;
					}
				}
				if ((myFrom & board.bishops) != 0)
				{
					pieceMoved = Piece.Bishop;
				}
				else
				{
					if ((myFrom & board.knights) != 0)
					{
						pieceMoved = Piece.Knight;
					}
					else
					{
						if ((myFrom & board.rooks) != 0)
						{
							pieceMoved = Piece.Rook;
						}
						else
						{
							if ((myFrom & board.queens) != 0)
							{
								pieceMoved = Piece.Queen;
							}
							else
							{
								if ((myFrom & board.kings) != 0)
								{
									pieceMoved = Piece.King;
									if ((turn ? board.GetWhiteKingsideCastling() : board.GetBlackKingsideCastling()) 
										&& (toIndex == (fromIndex - 2) || to == board.castlingRooks[turn ? 0 : 2]))
									{
										//
										moveType = TypeKingsideCastling;
									}
									if ((turn ? board.GetWhiteQueensideCastling() : board.GetBlackQueensideCastling()
										) && (toIndex == (fromIndex + 2) || to == board.castlingRooks[turn ? 1 : 3]))
									{
										//
										moveType = TypeQueensideCastling;
									}
								}
							}
						}
					}
				}
				// Now set captured piece flag
				if ((to & (turn ? board.blacks : board.whites)) != 0)
				{
					capture = true;
				}
				int moveInt = Move.GenMove(fromIndex, toIndex, pieceMoved, capture, check, moveType
					);
				if (checkLegality)
				{
					if (board.DoMove(moveInt, true, false))
					{
						if (board.GetCheck())
						{
							moveInt = moveInt | CheckMask;
						}
						// If the move didn't has the check flag set
						board.UndoMove();
						return moveInt;
					}
				}
				else
				{
					return moveInt;
				}
			}
			return None;
		}

		/// <summary>Gets an UCI-String representation of the move</summary>
		/// <param name="move"/>
		/// <returns/>
		public static string ToString(int move)
		{
			if (move == Move.None)
			{
				return NoneString;
			}
			else
			{
				if (move == Move.Null)
				{
					return NullString;
				}
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(BitboardUtils.Index2Algebraic(Move.GetFromIndex(move)));
			sb.Append(BitboardUtils.Index2Algebraic(Move.GetToIndex(move)));
			if (IsPromotion(move))
			{
				sb.Append(PieceLettersLowercase[GetPiecePromoted(move)]);
			}
			return sb.ToString();
		}

		public static string ToStringExt(int move)
		{
			if (move == Move.None)
			{
				return NoneString;
			}
			else
			{
				if (move == Move.Null)
				{
					return NullString;
				}
				else
				{
					if (Move.GetMoveType(move) == TypeKingsideCastling)
					{
						return Move.IsCheck(move) ? "O-O+" : "O-O";
					}
					else
					{
						if (Move.GetMoveType(move) == TypeQueensideCastling)
						{
							return Move.IsCheck(move) ? "O-O-O+" : "O-O-O";
						}
					}
				}
			}
			StringBuilder sb = new StringBuilder();
			if (GetPieceMoved(move) != Piece.Pawn)
			{
				sb.Append(PieceLettersUppercase[GetPieceMoved(move)]);
			}
			sb.Append(BitboardUtils.Index2Algebraic(Move.GetFromIndex(move)));
			sb.Append(IsCapture(move) ? 'x' : '-');
			sb.Append(BitboardUtils.Index2Algebraic(Move.GetToIndex(move)));
			if (IsPromotion(move))
			{
				sb.Append(PieceLettersLowercase[GetPiecePromoted(move)]);
			}
			if (IsCheck(move))
			{
				sb.Append("+");
			}
			return sb.ToString();
		}

		/// <summary>It does not append + or #</summary>
		/// <param name="board"/>
		/// <param name="move"/>
		/// <returns/>
		public static string ToSan(Board board, int move)
		{
			if (move == Move.None)
			{
				return NoneString;
			}
			else
			{
				if (move == Move.Null)
				{
					return NullString;
				}
			}
			board.GenerateLegalMoves();
			bool isLegal = false;
			bool disambiguate = false;
			bool fileEqual = false;
			bool rankEqual = false;
			for (int i = 0; i < board.legalMoveCount; i++)
			{
				int move2 = board.legalMoves[i];
				if (move == move2)
				{
					isLegal = true;
				}
				else
				{
					if (GetToIndex(move) == GetToIndex(move2) && (GetPieceMoved(move) == GetPieceMoved
						(move2)))
					{
						disambiguate = true;
						if ((GetFromIndex(move) % 8) == (GetFromIndex(move2) % 8))
						{
							fileEqual = true;
						}
						if ((GetFromIndex(move) / 8) == (GetFromIndex(move2) / 8))
						{
							rankEqual = true;
						}
					}
				}
			}
			if (!isLegal)
			{
				return Move.NoneString;
			}
			else
			{
				if (Move.GetMoveType(move) == TypeKingsideCastling)
				{
					return Move.IsCheck(move) ? "O-O+" : "O-O";
				}
				else
				{
					if (Move.GetMoveType(move) == TypeQueensideCastling)
					{
						return Move.IsCheck(move) ? "O-O-O+" : "O-O-O";
					}
				}
			}
			StringBuilder sb = new StringBuilder();
			if (GetPieceMoved(move) != Piece.Pawn)
			{
				sb.Append(PieceLettersUppercase[GetPieceMoved(move)]);
			}
			string fromSq = BitboardUtils.Index2Algebraic(Move.GetFromIndex(move));
			if (IsCapture(move) && GetPieceMoved(move) == Piece.Pawn)
			{
				disambiguate = true;
			}
			if (disambiguate)
			{
				if (fileEqual && rankEqual)
				{
					sb.Append(fromSq);
				}
				else
				{
					if (fileEqual)
					{
						sb.Append(fromSq[1]);
					}
					else
					{
						sb.Append(fromSq[0]);
					}
				}
			}
			if (IsCapture(move))
			{
				sb.Append("x");
			}
			sb.Append(BitboardUtils.Index2Algebraic(Move.GetToIndex(move)));
			if (IsPromotion(move))
			{
				sb.Append(PieceLettersUppercase[GetPiecePromoted(move)]);
			}
			if (IsCheck(move))
			{
				sb.Append("+");
			}
			return sb.ToString();
		}

		public static void PrintMoves(int[] moves, int from, int to)
		{
			for (int i = from; i < to; i++)
			{
				System.Console.Out.Write(Move.ToStringExt(moves[i]));
				System.Console.Out.Write(" ");
			}
			System.Console.Out.WriteLine();
		}

		public static string SanToFigurines(string @in)
		{
			return @in == null ? null : @in.Replace("N", "♘").Replace("B", "♗").Replace("R", 
				"♖").Replace("Q", "♕").Replace("K", "♔");
		}
	}
}
