using System;
using Com.Alonsoruibal.Chess;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Search
{
	public class SearchEngineThreaded : SearchEngine
	{
		internal Sharpen.Thread thread;

		public SearchEngineThreaded(Config config)
			: base(config)
		{
		}

		/// <summary>Threaded version</summary>
		public override void Go(SearchParameters searchParameteres)
		{
			if (!IsInitialized())
			{
				return;
			}
			if (!IsSearching())
			{
				SetSearchParameters(searchParameteres);
				try
				{
					PrepareRun();
					thread = new Sharpen.Thread(this);
					thread.Start();
				}
				catch (SearchFinishedException)
				{
				}
			}
		}

		/// <summary>Stops thinking</summary>
		public override void Stop()
		{
			base.Stop();
			while (IsSearching())
			{
				try
				{
					Sharpen.Thread.Sleep(10);
				}
				catch (Exception e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}
		}
	}
}
