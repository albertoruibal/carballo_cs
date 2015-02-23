using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Log;
using Sharpen;
using Util;

namespace Com.Alonsoruibal.Chess.Book
{
	/// <summary>Polyglot opening book support</summary>
	/// <author>rui</author>
	public class ResourceBook : Com.Alonsoruibal.Chess.Book.Book
	{
		/// <summary>Logger for this class</summary>
		private static readonly Logger logger = Logger.GetLogger("ResourceBook");

		private string bookName;

		internal IList<int> moves = new AList<int>();

		internal IList<int> weights = new AList<int>();

		internal long totalWeight;

		private readonly Random random = new Random();

		public ResourceBook(string fileName)
		{
			bookName = fileName;
			logger.Debug("Using opening book " + bookName);
		}

		/// <summary>
		/// "move" is a bit field with the following meaning (bit 0 is the least significant bit)
		/// <p/>
		/// bits                meaning
		/// ===================================
		/// 0,1,2               to file
		/// 3,4,5               to row
		/// 6,7,8               from file
		/// 9,10,11             from row
		/// 12,13,14            promotion piece
		/// "promotion piece" is encoded as follows
		/// none       0
		/// knight     1
		/// bishop     2
		/// rook       3
		/// queen      4
		/// </summary>
		/// <param name="move"/>
		/// <returns/>
		private string Int2MoveString(int move)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append((char)('a' + ((move >> 6) & unchecked((int)(0x7)))));
			sb.Append(((move >> 9) & unchecked((int)(0x7))) + 1);
			sb.Append((char)('a' + (move & unchecked((int)(0x7)))));
			sb.Append(((move >> 3) & unchecked((int)(0x7))) + 1);
			if (((move >> 12) & unchecked((int)(0x7))) != 0)
			{
				sb.Append("nbrq"[((move >> 12) & unchecked((int)(0x7))) - 1]);
			}
			return sb.ToString();
		}

		public virtual void GenerateMoves(Board board)
		{
			totalWeight = 0;
			moves.Clear();
			weights.Clear();
			long key2Find = board.GetKey();
			try
			{
				BigEndianReader dataInputStream = new BigEndianReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(bookName));
				long key;
				int moveInt;
				int weight;
				while (true)
				{
					key = dataInputStream.ReadInt64();
					if (key == key2Find)
					{
						moveInt = dataInputStream.ReadInt16();
						weight = dataInputStream.ReadInt16();
						dataInputStream.ReadInt32();
						// Unused learn field
						int move = Move.GetFromString(board, Int2MoveString(moveInt), true);
						// Add only if it is legal
						if (board.IsMoveLegal(move))
						{
							moves.AddItem(move);
							weights.AddItem(weight);
							totalWeight += weight;
						}
					}
					else
					{
						dataInputStream.ReadBytes(8);
					}
				}
			}
			catch (Exception )
			{
			}
		}

		/// <summary>Gets a random move from the book taking care of weights</summary>
		public virtual int GetMove(Board board)
		{
			GenerateMoves(board);
			long randomWeight = (long) (random.NextDouble() * totalWeight);
			for (int i = 0; i < moves.Count; i++)
			{
				randomWeight -= weights[i];
				if (randomWeight <= 0)
				{
					return moves[i];
				}
			}
			return 0;
		}
	}
}
