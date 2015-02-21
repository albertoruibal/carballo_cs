using System;
using System.IO;
using System.Text;
using Sharpen;

namespace Com.Alonsoruibal.Chess
{
	/// <summary>
	/// TODO Parse comments
	/// <p/>
	/// 1.
	/// </summary>
	/// <remarks>
	/// TODO Parse comments
	/// <p/>
	/// 1. Event: the name of the tournament or match event. 2. Site: the location of
	/// the event. This is in "City, Region COUNTRY" format, where COUNTRY is the
	/// 3-letter International Olympic Committee code for the country. An example is
	/// "New York City, NY USA". 3. Date: the starting date of the game, in
	/// YYYY.MM.DD form. "??" are used for unknown values. 4. Round: the playing
	/// round ordinal of the game within the event. 5. White: the player of the white
	/// pieces, in "last name, first name" format. 6. Black: the player of the black
	/// pieces, same format as White. 7. Result: the result of the game. This can
	/// only have four possible values: "1-0" (White won), "0-1" (Black won),
	/// "1/2-1/2" (Draw), or "*" (other, e.g., the game is ongoing).
	/// </remarks>
	/// <author>rui</author>
	public class PgnFile : Pgn
	{
		public virtual string GetGameNumber(InputStream @is, int gameNumber)
		{
			// logger.debug("Loading GameNumber " + gameNumber);
			BufferedReader br = new BufferedReader(new InputStreamReader(@is));
			string line;
			int counter = 0;
			try
			{
				while (true)
				{
					line = br.ReadLine();
					if (line == null)
					{
						break;
					}
					if (line.IndexOf("[Event ") == 0)
					{
						if (counter == gameNumber)
						{
							StringBuilder pgnSb = new StringBuilder();
							try
							{
								while (true)
								{
									pgnSb.Append(line);
									pgnSb.Append("\n");
									line = br.ReadLine();
									if (line == null || line.IndexOf("[Event ") == 0)
									{
										break;
									}
								}
							}
							catch (IOException)
							{
							}
							return pgnSb.ToString();
						}
						counter++;
					}
				}
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			return null;
		}
	}
}
