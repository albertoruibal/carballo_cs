using System;
using Com.Alonsoruibal.Chess.Log;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Search
{
	public class SearchParameters
	{
		/// <summary>Logger for this class</summary>
		private static readonly Logger logger = Logger.GetLogger("SearchParameters");

		internal int wtime;

		internal int btime;

		internal int winc;

		internal int binc;

		internal int movesToGo;

		internal int depth = int.MaxValue;

		internal int nodes = int.MaxValue;

		internal int mate;

		internal int moveTime = int.MaxValue;

		internal bool infinite;

		internal bool ponder;

		internal bool manageTime;

		// UCI parameters
		// Remaining time
		// Time increment per move
		// Moves to the next time control
		// Analize x plyes only
		// Search only this number of nodes
		// Search for mate in mate moves
		// Search movetime seconds
		// Think infinite
		public virtual bool IsPonder()
		{
			return ponder;
		}

		public virtual void SetPonder(bool ponder)
		{
			this.ponder = ponder;
		}

		public virtual int GetWtime()
		{
			return wtime;
		}

		public virtual void SetWtime(int wtime)
		{
			this.wtime = wtime;
		}

		public virtual int GetBtime()
		{
			return btime;
		}

		public virtual void SetBtime(int btime)
		{
			this.btime = btime;
		}

		public virtual int GetWinc()
		{
			return winc;
		}

		public virtual void SetWinc(int winc)
		{
			this.winc = winc;
		}

		public virtual int GetBinc()
		{
			return binc;
		}

		public virtual void SetBinc(int binc)
		{
			this.binc = binc;
		}

		public virtual int GetMovesToGo()
		{
			return movesToGo;
		}

		public virtual void SetMovesToGo(int movesToGo)
		{
			this.movesToGo = movesToGo;
		}

		public virtual int GetDepth()
		{
			return depth;
		}

		public virtual void SetDepth(int depth)
		{
			this.depth = depth;
		}

		public virtual int GetNodes()
		{
			return nodes;
		}

		public virtual void SetNodes(int nodes)
		{
			this.nodes = nodes;
		}

		public virtual int GetMate()
		{
			return mate;
		}

		public virtual void SetMate(int mate)
		{
			this.mate = mate;
		}

		public virtual int GetMoveTime()
		{
			return moveTime;
		}

		public virtual void SetMoveTime(int moveTime)
		{
			this.moveTime = moveTime;
		}

		public virtual bool IsInfinite()
		{
			return infinite;
		}

		public virtual void SetInfinite(bool infinite)
		{
			this.infinite = infinite;
		}

		/// <summary>Used to detect if it can add more time in case of panic or apply other heuristics to reduce time
		/// 	</summary>
		/// <returns>true if the engine is responsible of managing the remaining time</returns>
		public virtual bool ManageTime()
		{
			return manageTime;
		}

		/// <summary>Time management routine</summary>
		/// <param name="panicTime">is set to true when the score fails low in the root node by 100
		/// 	</param>
		/// <returns>the time to think, or Long.MAX_VALUE if it can think an infinite time</returns>
		public virtual long CalculateMoveTime(bool engineIsWhite, long startTime, bool panicTime
			)
		{
			manageTime = false;
			if (ponder || infinite || depth < int.MaxValue || nodes < int.MaxValue)
			{
				return long.MaxValue;
			}
			if (moveTime != int.MaxValue)
			{
				return startTime + moveTime;
			}
			manageTime = true;
			int calcTime = 0;
			int timeAvailable = engineIsWhite ? wtime : btime;
			int timeInc = engineIsWhite ? winc : binc;
			if (timeAvailable > 0)
			{
				calcTime = timeAvailable / 40 + ((int)(((uint)timeInc) >> 1));
			}
			if (panicTime)
			{
				calcTime = calcTime << 2;
			}
			calcTime = Math.Min(calcTime, (int)(((uint)timeAvailable) >> 4));
			logger.Debug("Thinking for " + calcTime + "Ms");
			return startTime + calcTime;
		}

		public static SearchParameters Get(int moveTime)
		{
			SearchParameters searchParameters = new SearchParameters();
			searchParameters.SetMoveTime(moveTime);
			return searchParameters;
		}
	}
}
