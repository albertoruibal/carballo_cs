using Sharpen;

namespace Com.Alonsoruibal.Chess.Util
{
	public class StringUtils
	{
		private static string Spaces = "                     ";

		public static string PadRight(string str, int totalChars)
		{
			return str + Sharpen.Runtime.Substring(Spaces, 0, totalChars - str.Length);
		}

		public static string PadLeft(string str, int totalChars)
		{
			return Sharpen.Runtime.Substring(Spaces, 0, totalChars - str.Length) + str;
		}
	}
}
