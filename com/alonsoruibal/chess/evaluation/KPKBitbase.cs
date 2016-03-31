using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Bitboard;
using Com.Alonsoruibal.Chess.Log;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Evaluation
{
	/// <summary>Derived from Stockfish bitbase.cpp</summary>
	public class KPKBitbase
	{
		private static readonly Logger logger = Logger.GetLogger("KPKBitbase");

		public int[] bitbase = new int[] { unchecked((int)(0xfffffcfc)), unchecked((int)(
			0xff7fffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xff7fffff)), unchecked(
			(int)(0xfffff1f1)), unchecked((int)(0xff7fffff)), unchecked((int)(0xffffe3e3)), 
			unchecked((int)(0xff7fffff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xff7fffff
			)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xff7fffff)), unchecked((int)(
			0xffff1f1f)), unchecked((int)(0xff7fffff)), unchecked((int)(0xffff3f3f)), unchecked(
			(int)(0xff7fffff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xff7fffff)), 
			unchecked((int)(0xfff8f8f8)), unchecked((int)(0xff7fffff)), unchecked((int)(0xfff1f1f1
			)), unchecked((int)(0xff7fffff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(
			0xff7fffff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xff7fffff)), unchecked(
			(int)(0xff8f8f8f)), unchecked((int)(0xff7fffff)), unchecked((int)(0xff1f1f1f)), 
			unchecked((int)(0xff7fffff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xff7fffff
			)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xff7fffff)), unchecked((int)(
			0xf8f8f8ff)), unchecked((int)(0xff7fffff)), unchecked((int)(0xf1f1f1ff)), unchecked(
			(int)(0xff7fffff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xff7fffff)), 
			unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xff7fffff)), unchecked((int)(0x8f8f8fff
			)), unchecked((int)(0xff7fffff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(
			0xff7fffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xff7fffff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xff7ffffc)), unchecked((int)(0xf8f8ffff)), 
			unchecked((int)(0xff7ffff8)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xff7ffff1
			)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xff7fffe3)), unchecked((int)(
			0xc7c7ffff)), unchecked((int)(0xff7fffc7)), unchecked((int)(0x8f8fffff)), unchecked(
			(int)(0xff7fff8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(0xff7fff1f)), 
			unchecked((int)(0x3f3fffff)), unchecked((int)(0xff7fff3f)), unchecked((int)(0xfcffffff
			)), unchecked((int)(0xff7ffcfc)), unchecked((int)(0xf8ffffff)), unchecked((int)(
			0xff7ff8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xff7ff1f1)), unchecked(
			(int)(0xe3ffffff)), unchecked((int)(0xff7fe3e3)), unchecked((int)(0xc7ffffff)), 
			unchecked((int)(0xff7fc7c7)), unchecked((int)(0x8fffffff)), unchecked((int)(0xff7f8f8f
			)), unchecked((int)(0x1fffffff)), unchecked((int)(0xff7f1f1f)), unchecked((int)(
			0x3fffffff)), unchecked((int)(0xff7f3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff7cfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xff78f8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff71f1f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xff63e3e3)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x7f47c7c7)), unchecked((int)(0x0)), unchecked((int)(0x60008000)), unchecked((int
			)(0x0)), unchecked((int)(0x40000000)), unchecked((int)(0x0)), unchecked((int)(0xc0000000
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xfc7cfcff)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xf878f8ff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xf171f1ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xe363e3ff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0x4747c7ff)), unchecked((int)(0x0)
			), unchecked((int)(0x8000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffffff)), unchecked((int
			)(0xfc7cffff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf878ffff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf171ffff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xe363ffff)), unchecked((int)(0xffffffff)), unchecked((int)(0x4747ffff
			)), unchecked((int)(0x0)), unchecked((int)(0xe000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfffffcfc
			)), unchecked((int)(0xff7fffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(
			0xff7fffff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xff7fffff)), unchecked(
			(int)(0xffffe3e3)), unchecked((int)(0xff7fffff)), unchecked((int)(0xffffc7c7)), 
			unchecked((int)(0xff7fffff)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xff7fffff
			)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xff7fffff)), unchecked((int)(
			0xffff3f3f)), unchecked((int)(0xff7fffff)), unchecked((int)(0xfffcfcfc)), unchecked(
			(int)(0xff7fffff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xff7fffff)), 
			unchecked((int)(0xfff1f1f1)), unchecked((int)(0xff7fffff)), unchecked((int)(0xffe3e3e3
			)), unchecked((int)(0xff7fffff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(
			0xff7fffff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xff7fffff)), unchecked(
			(int)(0xff1f1f1f)), unchecked((int)(0xff7fffff)), unchecked((int)(0xff3f3f3f)), 
			unchecked((int)(0xff7fffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xff7fffff
			)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xff7fffff)), unchecked((int)(
			0xf1f1f1ff)), unchecked((int)(0xff7fffff)), unchecked((int)(0xe3e3e3ff)), unchecked(
			(int)(0xff7fffff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xff7fffff)), 
			unchecked((int)(0x8f8f8fff)), unchecked((int)(0xff7fffff)), unchecked((int)(0x1f1f1fff
			)), unchecked((int)(0xff7fffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(
			0xff7fffff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xff7ffffc)), unchecked(
			(int)(0xf8f8ffff)), unchecked((int)(0xff7ffff8)), unchecked((int)(0xf1f1ffff)), 
			unchecked((int)(0xff7ffff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xff7fffe3
			)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xff7fffc7)), unchecked((int)(
			0x8f8fffff)), unchecked((int)(0xff7fff8f)), unchecked((int)(0x1f1fffff)), unchecked(
			(int)(0xff7fff1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xff7fff3f)), 
			unchecked((int)(0xfcffffff)), unchecked((int)(0xff7ffcfc)), unchecked((int)(0xf8ffffff
			)), unchecked((int)(0xff7ff8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(
			0xff7ff1f1)), unchecked((int)(0xe3ffffff)), unchecked((int)(0xff7fe3e3)), unchecked(
			(int)(0xc7ffffff)), unchecked((int)(0xff7fc7c7)), unchecked((int)(0x8fffffff)), 
			unchecked((int)(0xff7f8f8f)), unchecked((int)(0x1fffffff)), unchecked((int)(0xff7f1f1f
			)), unchecked((int)(0x3fffffff)), unchecked((int)(0xff7f3f3f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff7cfcfc)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff78f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xff71f1f1)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff63e3e3)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xff47c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xff0f8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xff1f1f1f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xfc7cfcff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf878f8ff
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xf171f1ff)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xe363e3ff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xc747c7ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf0f8fff)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0xffffffff)), unchecked((int)(0xfc7cffff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf878ffff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf171ffff)), unchecked((int)(0xffffffff)), unchecked((int)(0xe363ffff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xc747ffff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf0fffff)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfffffcfc)), unchecked((int
			)(0xffbfffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffbfffff)), unchecked(
			(int)(0xfffff1f1)), unchecked((int)(0xffbfffff)), unchecked((int)(0xffffe3e3)), 
			unchecked((int)(0xffbfffff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffbfffff
			)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffbfffff)), unchecked((int)(
			0xffff1f1f)), unchecked((int)(0xffbfffff)), unchecked((int)(0xffff3f3f)), unchecked(
			(int)(0xffbfffff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffbfffff)), 
			unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffbfffff)), unchecked((int)(0xfff1f1f1
			)), unchecked((int)(0xffbfffff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(
			0xffbfffff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffbfffff)), unchecked(
			(int)(0xff8f8f8f)), unchecked((int)(0xffbfffff)), unchecked((int)(0xff1f1f1f)), 
			unchecked((int)(0xffbfffff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffbfffff
			)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffbfffff)), unchecked((int)(
			0xf8f8f8ff)), unchecked((int)(0xffbfffff)), unchecked((int)(0xf1f1f1ff)), unchecked(
			(int)(0xffbfffff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffbfffff)), 
			unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffbfffff)), unchecked((int)(0x8f8f8fff
			)), unchecked((int)(0xffbfffff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(
			0xffbfffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffbfffff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xffbffffc)), unchecked((int)(0xf8f8ffff)), 
			unchecked((int)(0xffbffff8)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xffbffff1
			)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffbfffe3)), unchecked((int)(
			0xc7c7ffff)), unchecked((int)(0xffbfffc7)), unchecked((int)(0x8f8fffff)), unchecked(
			(int)(0xffbfff8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(0xffbfff1f)), 
			unchecked((int)(0x3f3fffff)), unchecked((int)(0xffbfff3f)), unchecked((int)(0xfcffffff
			)), unchecked((int)(0xffbffcfc)), unchecked((int)(0xf8ffffff)), unchecked((int)(
			0xffbff8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xffbff1f1)), unchecked(
			(int)(0xe3ffffff)), unchecked((int)(0xffbfe3e3)), unchecked((int)(0xc7ffffff)), 
			unchecked((int)(0xffbfc7c7)), unchecked((int)(0x8fffffff)), unchecked((int)(0xffbf8f8f
			)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffbf1f1f)), unchecked((int)(
			0x3fffffff)), unchecked((int)(0xffbf3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffbcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffb8f8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffb1f1f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffa3e3e3)), unchecked((int)(0x0)), unchecked((int)(0xf080c000
			)), unchecked((int)(0x0)), unchecked((int)(0xe0808000)), unchecked((int)(0x0)), 
			unchecked((int)(0xe0000000)), unchecked((int)(0x0)), unchecked((int)(0xe0202000)
			), unchecked((int)(0xffffffff)), unchecked((int)(0xfcbcfcff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8b8f8ff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf1b1f1ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xe3a3e3ff)), unchecked(
			(int)(0x0)), unchecked((int)(0xc080c000)), unchecked((int)(0x0)), unchecked((int
			)(0x80800000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x20200000)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xfcbcffff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8b8ffff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf1b1ffff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xe3a3ffff)), unchecked((int)(0x0)), unchecked((int)(0xc080f000)
			), unchecked((int)(0x0)), unchecked((int)(0x80804000)), unchecked((int)(0x0)), unchecked(
			(int)(0xa000)), unchecked((int)(0x0)), unchecked((int)(0x20204000)), unchecked((
			int)(0xfffffcfc)), unchecked((int)(0xffbfffff)), unchecked((int)(0xfffff8f8)), unchecked(
			(int)(0xffbfffff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffbfffff)), 
			unchecked((int)(0xffffe3e3)), unchecked((int)(0xffbfffff)), unchecked((int)(0xffffc7c7
			)), unchecked((int)(0xffbfffff)), unchecked((int)(0xffff8f8f)), unchecked((int)(
			0xffbfffff)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffbfffff)), unchecked(
			(int)(0xffff3f3f)), unchecked((int)(0xffbfffff)), unchecked((int)(0xfffcfcfc)), 
			unchecked((int)(0xffbfffff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffbfffff
			)), unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffbfffff)), unchecked((int)(
			0xffe3e3e3)), unchecked((int)(0xffbfffff)), unchecked((int)(0xffc7c7c7)), unchecked(
			(int)(0xffbfffff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffbfffff)), 
			unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffbfffff)), unchecked((int)(0xff3f3f3f
			)), unchecked((int)(0xffbfffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(
			0xffbfffff)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffbfffff)), unchecked(
			(int)(0xf1f1f1ff)), unchecked((int)(0xffbfffff)), unchecked((int)(0xe3e3e3ff)), 
			unchecked((int)(0xffbfffff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffbfffff
			)), unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffbfffff)), unchecked((int)(
			0x1f1f1fff)), unchecked((int)(0xffbfffff)), unchecked((int)(0x3f3f3fff)), unchecked(
			(int)(0xffbfffff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffbffffc)), 
			unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffbffff8)), unchecked((int)(0xf1f1ffff
			)), unchecked((int)(0xffbffff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(
			0xffbfffe3)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffbfffc7)), unchecked(
			(int)(0x8f8fffff)), unchecked((int)(0xffbfff8f)), unchecked((int)(0x1f1fffff)), 
			unchecked((int)(0xffbfff1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffbfff3f
			)), unchecked((int)(0xfcffffff)), unchecked((int)(0xffbffcfc)), unchecked((int)(
			0xf8ffffff)), unchecked((int)(0xffbff8f8)), unchecked((int)(0xf1ffffff)), unchecked(
			(int)(0xffbff1f1)), unchecked((int)(0xe3ffffff)), unchecked((int)(0xffbfe3e3)), 
			unchecked((int)(0xc7ffffff)), unchecked((int)(0xffbfc7c7)), unchecked((int)(0x8fffffff
			)), unchecked((int)(0xffbf8f8f)), unchecked((int)(0x1fffffff)), unchecked((int)(
			0xffbf1f1f)), unchecked((int)(0x3fffffff)), unchecked((int)(0xffbf3f3f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffbcfcfc)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffb8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xffb1f1f1
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffa3e3e3)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff87c7c7)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff8f8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xff1f1f1f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xfcbcfcff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf8b8f8ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf1b1f1ff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xe3a3e3ff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xc787c7ff)), unchecked((int)(0x0)), unchecked((int)(0x80808000)
			), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x30303000)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcbcffff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xf8b8ffff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf1b1ffff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xe3a3ffff)), unchecked((int)(0xffffffff)), unchecked((int)(0xc787ffff)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1050f0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfffffcfc)), 
			unchecked((int)(0xffdfffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffdfffff
			)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffdfffff)), unchecked((int)(
			0xffffe3e3)), unchecked((int)(0xffdfffff)), unchecked((int)(0xffffc7c7)), unchecked(
			(int)(0xffdfffff)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffdfffff)), 
			unchecked((int)(0xffff1f1f)), unchecked((int)(0xffdfffff)), unchecked((int)(0xffff3f3f
			)), unchecked((int)(0xffdfffff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(
			0xffdfffff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffdfffff)), unchecked(
			(int)(0xfff1f1f1)), unchecked((int)(0xffdfffff)), unchecked((int)(0xffe3e3e3)), 
			unchecked((int)(0xffdfffff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffdfffff
			)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffdfffff)), unchecked((int)(
			0xff1f1f1f)), unchecked((int)(0xffdfffff)), unchecked((int)(0xff3f3f3f)), unchecked(
			(int)(0xffdfffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffdfffff)), 
			unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffdfffff)), unchecked((int)(0xf1f1f1ff
			)), unchecked((int)(0xffdfffff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(
			0xffdfffff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffdfffff)), unchecked(
			(int)(0x8f8f8fff)), unchecked((int)(0xffdfffff)), unchecked((int)(0x1f1f1fff)), 
			unchecked((int)(0xffdfffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffdfffff
			)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffdffffc)), unchecked((int)(
			0xf8f8ffff)), unchecked((int)(0xffdffff8)), unchecked((int)(0xf1f1ffff)), unchecked(
			(int)(0xffdffff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffdfffe3)), 
			unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffdfffc7)), unchecked((int)(0x8f8fffff
			)), unchecked((int)(0xffdfff8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(
			0xffdfff1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffdfff3f)), unchecked(
			(int)(0xfcffffff)), unchecked((int)(0xffdffcfc)), unchecked((int)(0xf8ffffff)), 
			unchecked((int)(0xffdff8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xffdff1f1
			)), unchecked((int)(0xe3ffffff)), unchecked((int)(0xffdfe3e3)), unchecked((int)(
			0xc7ffffff)), unchecked((int)(0xffdfc7c7)), unchecked((int)(0x8fffffff)), unchecked(
			(int)(0xffdf8f8f)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffdf1f1f)), 
			unchecked((int)(0x3fffffff)), unchecked((int)(0xffdf3f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffdcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffd8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xffd1f1f1)), unchecked(
			(int)(0x0)), unchecked((int)(0xf8c0e000)), unchecked((int)(0x0)), unchecked((int
			)(0x70404000)), unchecked((int)(0x0)), unchecked((int)(0x70000000)), unchecked((
			int)(0x0)), unchecked((int)(0x70101000)), unchecked((int)(0x0)), unchecked((int)
			(0xf8183800)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcdcfcff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8d8f8ff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf1d1f1ff)), unchecked((int)(0x0)), unchecked((int)(0xe0c0e000)
			), unchecked((int)(0x0)), unchecked((int)(0x40400000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x10100000)), unchecked((int
			)(0x0)), unchecked((int)(0x38183800)), unchecked((int)(0xffffffff)), unchecked((
			int)(0xfcdcffff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8d8ffff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf1d1ffff)), unchecked((int)(0x0)), unchecked(
			(int)(0xe0c0f800)), unchecked((int)(0x0)), unchecked((int)(0x40402000)), unchecked(
			(int)(0x0)), unchecked((int)(0x5000)), unchecked((int)(0x0)), unchecked((int)(0x10102000
			)), unchecked((int)(0x0)), unchecked((int)(0x38183800)), unchecked((int)(0xfffffcfc
			)), unchecked((int)(0xffdfffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(
			0xffdfffff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffdfffff)), unchecked(
			(int)(0xffffe3e3)), unchecked((int)(0xffdfffff)), unchecked((int)(0xffffc7c7)), 
			unchecked((int)(0xffdfffff)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffdfffff
			)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffdfffff)), unchecked((int)(
			0xffff3f3f)), unchecked((int)(0xffdfffff)), unchecked((int)(0xfffcfcfc)), unchecked(
			(int)(0xffdfffff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffdfffff)), 
			unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffdfffff)), unchecked((int)(0xffe3e3e3
			)), unchecked((int)(0xffdfffff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(
			0xffdfffff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffdfffff)), unchecked(
			(int)(0xff1f1f1f)), unchecked((int)(0xffdfffff)), unchecked((int)(0xff3f3f3f)), 
			unchecked((int)(0xffdfffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffdfffff
			)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffdfffff)), unchecked((int)(
			0xf1f1f1ff)), unchecked((int)(0xffdfffff)), unchecked((int)(0xe3e3e3ff)), unchecked(
			(int)(0xffdfffff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffdfffff)), 
			unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffdfffff)), unchecked((int)(0x1f1f1fff
			)), unchecked((int)(0xffdfffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(
			0xffdfffff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffdffffc)), unchecked(
			(int)(0xf8f8ffff)), unchecked((int)(0xffdffff8)), unchecked((int)(0xf1f1ffff)), 
			unchecked((int)(0xffdffff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffdfffe3
			)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffdfffc7)), unchecked((int)(
			0x8f8fffff)), unchecked((int)(0xffdfff8f)), unchecked((int)(0x1f1fffff)), unchecked(
			(int)(0xffdfff1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffdfff3f)), 
			unchecked((int)(0xfcffffff)), unchecked((int)(0xffdffcfc)), unchecked((int)(0xf8ffffff
			)), unchecked((int)(0xffdff8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(
			0xffdff1f1)), unchecked((int)(0xe3ffffff)), unchecked((int)(0xffdfe3e3)), unchecked(
			(int)(0xc7ffffff)), unchecked((int)(0xffdfc7c7)), unchecked((int)(0x8fffffff)), 
			unchecked((int)(0xffdf8f8f)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffdf1f1f
			)), unchecked((int)(0x3fffffff)), unchecked((int)(0xffdf3f3f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffdcfcfc)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffd8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xffd1f1f1)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffc3e3e3)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xff8f8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xff1f1f1f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xff1f3f3f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xfcdcfcff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8d8f8ff
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xf1d1f1ff)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xe3c3e3ff)), unchecked((int)(0x0)), unchecked((int
			)(0xc0c0c000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x18181800)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x3f1f3fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcdcffff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8d8ffff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf1d1ffff)), unchecked((int)(0xffffffff)), unchecked((int)(0xe3c3ffff
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x88a8f8)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(
			0xffffffff)), unchecked((int)(0x3f1fffff)), unchecked((int)(0xfffffcfc)), unchecked(
			(int)(0xffefffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffefffff)), 
			unchecked((int)(0xfffff1f1)), unchecked((int)(0xffefffff)), unchecked((int)(0xffffe3e3
			)), unchecked((int)(0xffefffff)), unchecked((int)(0xffffc7c7)), unchecked((int)(
			0xffefffff)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffefffff)), unchecked(
			(int)(0xffff1f1f)), unchecked((int)(0xffefffff)), unchecked((int)(0xffff3f3f)), 
			unchecked((int)(0xffefffff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffefffff
			)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffefffff)), unchecked((int)(
			0xfff1f1f1)), unchecked((int)(0xffefffff)), unchecked((int)(0xffe3e3e3)), unchecked(
			(int)(0xffefffff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffefffff)), 
			unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffefffff)), unchecked((int)(0xff1f1f1f
			)), unchecked((int)(0xffefffff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(
			0xffefffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffefffff)), unchecked(
			(int)(0xf8f8f8ff)), unchecked((int)(0xffefffff)), unchecked((int)(0xf1f1f1ff)), 
			unchecked((int)(0xffefffff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffefffff
			)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffefffff)), unchecked((int)(
			0x8f8f8fff)), unchecked((int)(0xffefffff)), unchecked((int)(0x1f1f1fff)), unchecked(
			(int)(0xffefffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffefffff)), 
			unchecked((int)(0xfcfcffff)), unchecked((int)(0xffeffffc)), unchecked((int)(0xf8f8ffff
			)), unchecked((int)(0xffeffff8)), unchecked((int)(0xf1f1ffff)), unchecked((int)(
			0xffeffff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffefffe3)), unchecked(
			(int)(0xc7c7ffff)), unchecked((int)(0xffefffc7)), unchecked((int)(0x8f8fffff)), 
			unchecked((int)(0xffefff8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(0xffefff1f
			)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffefff3f)), unchecked((int)(
			0xfcffffff)), unchecked((int)(0xffeffcfc)), unchecked((int)(0xf8ffffff)), unchecked(
			(int)(0xffeff8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xffeff1f1)), 
			unchecked((int)(0xe3ffffff)), unchecked((int)(0xffefe3e3)), unchecked((int)(0xc7ffffff
			)), unchecked((int)(0xffefc7c7)), unchecked((int)(0x8fffffff)), unchecked((int)(
			0xffef8f8f)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffef1f1f)), unchecked(
			(int)(0x3fffffff)), unchecked((int)(0xffef3f3f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffecfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffe8f8f8
			)), unchecked((int)(0x0)), unchecked((int)(0x7c607000)), unchecked((int)(0x0)), 
			unchecked((int)(0x38202000)), unchecked((int)(0x0)), unchecked((int)(0x38000000)
			), unchecked((int)(0x0)), unchecked((int)(0x38080800)), unchecked((int)(0x0)), unchecked(
			(int)(0x7c0c1c00)), unchecked((int)(0xffffffff)), unchecked((int)(0xff2f3f3f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfcecfcff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8e8f8ff)), unchecked((int)(0x0)), unchecked((int)(0x70607000
			)), unchecked((int)(0x0)), unchecked((int)(0x20200000)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x8080000)), unchecked(
			(int)(0x0)), unchecked((int)(0x1c0c1c00)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0x3f2f3fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcecffff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xf8e8ffff)), unchecked((int)(0x0)
			), unchecked((int)(0x70607c00)), unchecked((int)(0x0)), unchecked((int)(0x20201000
			)), unchecked((int)(0x0)), unchecked((int)(0x2800)), unchecked((int)(0x0)), unchecked(
			(int)(0x8081000)), unchecked((int)(0x0)), unchecked((int)(0x1c0c7c00)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x3f2fffff)), unchecked((int)(0xfffffcfc)), 
			unchecked((int)(0xffefffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffefffff
			)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffefffff)), unchecked((int)(
			0xffffe3e3)), unchecked((int)(0xffefffff)), unchecked((int)(0xffffc7c7)), unchecked(
			(int)(0xffefffff)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffefffff)), 
			unchecked((int)(0xffff1f1f)), unchecked((int)(0xffefffff)), unchecked((int)(0xffff3f3f
			)), unchecked((int)(0xffefffff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(
			0xffefffff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffefffff)), unchecked(
			(int)(0xfff1f1f1)), unchecked((int)(0xffefffff)), unchecked((int)(0xffe3e3e3)), 
			unchecked((int)(0xffefffff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffefffff
			)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffefffff)), unchecked((int)(
			0xff1f1f1f)), unchecked((int)(0xffefffff)), unchecked((int)(0xff3f3f3f)), unchecked(
			(int)(0xffefffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffefffff)), 
			unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffefffff)), unchecked((int)(0xf1f1f1ff
			)), unchecked((int)(0xffefffff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(
			0xffefffff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffefffff)), unchecked(
			(int)(0x8f8f8fff)), unchecked((int)(0xffefffff)), unchecked((int)(0x1f1f1fff)), 
			unchecked((int)(0xffefffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffefffff
			)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffeffffc)), unchecked((int)(
			0xf8f8ffff)), unchecked((int)(0xffeffff8)), unchecked((int)(0xf1f1ffff)), unchecked(
			(int)(0xffeffff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffefffe3)), 
			unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffefffc7)), unchecked((int)(0x8f8fffff
			)), unchecked((int)(0xffefff8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(
			0xffefff1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffefff3f)), unchecked(
			(int)(0xfcffffff)), unchecked((int)(0xffeffcfc)), unchecked((int)(0xf8ffffff)), 
			unchecked((int)(0xffeff8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xffeff1f1
			)), unchecked((int)(0xe3ffffff)), unchecked((int)(0xffefe3e3)), unchecked((int)(
			0xc7ffffff)), unchecked((int)(0xffefc7c7)), unchecked((int)(0x8fffffff)), unchecked(
			(int)(0xffef8f8f)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffef1f1f)), 
			unchecked((int)(0x3fffffff)), unchecked((int)(0xffef3f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffecfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffe8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xffe1f1f1)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xff8f8f8f
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xff0f1f1f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff2f3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfcecfcff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8e8f8ff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xf1e1f1ff)), unchecked((int)(0x0)
			), unchecked((int)(0x60606000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0xc0c0c00)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0x1f0f1fff)), unchecked((int)(0xffffffff)), unchecked((int)(0x3f2f3fff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfcecffff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8e8ffff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf1e1ffff)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)
			), unchecked((int)(0x44547c)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x1f0fffff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x3f2fffff)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xffff7fff
			)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffff7fff)), unchecked((int)(
			0xfffff1f1)), unchecked((int)(0xffff7fff)), unchecked((int)(0xffffe3e3)), unchecked(
			(int)(0xffff7fff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffff7fff)), 
			unchecked((int)(0xffff8f8f)), unchecked((int)(0xffff7fff)), unchecked((int)(0xffff1f1f
			)), unchecked((int)(0xffff7fff)), unchecked((int)(0xffff3f3f)), unchecked((int)(
			0xffff7fff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffff7fff)), unchecked(
			(int)(0xfff8f8f8)), unchecked((int)(0xffff7fff)), unchecked((int)(0xfff1f1f1)), 
			unchecked((int)(0xffff7fff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffff7fff
			)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffff7fff)), unchecked((int)(
			0xff8f8f8f)), unchecked((int)(0xffff7fff)), unchecked((int)(0xff1f1f1f)), unchecked(
			(int)(0xffff7fff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffff7fff)), 
			unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffff7fff)), unchecked((int)(0xf8f8f8ff
			)), unchecked((int)(0xffff7fff)), unchecked((int)(0xf1f1f1ff)), unchecked((int)(
			0xffff7fff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffff7fff)), unchecked(
			(int)(0xc7c7c7ff)), unchecked((int)(0xffff7fff)), unchecked((int)(0x8f8f8fff)), 
			unchecked((int)(0xffff7fff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(0xffff7fff
			)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffff7fff)), unchecked((int)(
			0xfcfcffff)), unchecked((int)(0xffff7ffc)), unchecked((int)(0xf8f8ffff)), unchecked(
			(int)(0xffff7ff8)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xffff7ff1)), 
			unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffff7fe3)), unchecked((int)(0xc7c7ffff
			)), unchecked((int)(0xffff7fc7)), unchecked((int)(0x8f8fffff)), unchecked((int)(
			0xffff7f8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(0xffff7f1f)), unchecked(
			(int)(0x3f3fffff)), unchecked((int)(0xffff7f3f)), unchecked((int)(0xfcffffff)), 
			unchecked((int)(0xffff7cfc)), unchecked((int)(0xf8ffffff)), unchecked((int)(0xffff78f8
			)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xffff71f1)), unchecked((int)(
			0xe3ffffff)), unchecked((int)(0xffff63e3)), unchecked((int)(0x0)), unchecked((int
			)(0xe0f04040)), unchecked((int)(0x0)), unchecked((int)(0x40e00000)), unchecked((
			int)(0x0)), unchecked((int)(0xc00000)), unchecked((int)(0x0)), unchecked((int)(0xc00000
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xfffc7cfc)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xfff878f8)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfff171f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xffe363e3)), 
			unchecked((int)(0x0)), unchecked((int)(0x60404000)), unchecked((int)(0x0)), unchecked(
			(int)(0x40000000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfc7cff
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f878ff)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xf1f171ff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xe3e363ff)), unchecked((int)(0x0)), unchecked((int)(0x40404000)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfcfc7fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f87fff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xf1f17fff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xe3e37fff)), unchecked((int)(0x0)), unchecked((int)(0x40406000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfffffcfc
			)), unchecked((int)(0xffff7fff)), unchecked((int)(0xfffff8f8)), unchecked((int)(
			0xffff7fff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffff7fff)), unchecked(
			(int)(0xffffe3e3)), unchecked((int)(0xffff7fff)), unchecked((int)(0xffffc7c7)), 
			unchecked((int)(0xffff7fff)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffff7fff
			)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffff7fff)), unchecked((int)(
			0xffff3f3f)), unchecked((int)(0xffff7fff)), unchecked((int)(0xfffcfcfc)), unchecked(
			(int)(0xffff7fff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffff7fff)), 
			unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffff7fff)), unchecked((int)(0xffe3e3e3
			)), unchecked((int)(0xffff7fff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(
			0xffff7fff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffff7fff)), unchecked(
			(int)(0xff1f1f1f)), unchecked((int)(0xffff7fff)), unchecked((int)(0xff3f3f3f)), 
			unchecked((int)(0xffff7fff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffff7fff
			)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffff7fff)), unchecked((int)(
			0xf1f1f1ff)), unchecked((int)(0xffff7fff)), unchecked((int)(0xe3e3e3ff)), unchecked(
			(int)(0xffff7fff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffff7fff)), 
			unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffff7fff)), unchecked((int)(0x1f1f1fff
			)), unchecked((int)(0xffff7fff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(
			0xffff7fff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffff7ffc)), unchecked(
			(int)(0xf8f8ffff)), unchecked((int)(0xffff7ff8)), unchecked((int)(0xf1f1ffff)), 
			unchecked((int)(0xffff7ff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffff7fe3
			)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffff7fc7)), unchecked((int)(
			0x8f8fffff)), unchecked((int)(0xffff7f8f)), unchecked((int)(0x1f1fffff)), unchecked(
			(int)(0xffff7f1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffff7f3f)), 
			unchecked((int)(0xfcffffff)), unchecked((int)(0xffff7cfc)), unchecked((int)(0xf8ffffff
			)), unchecked((int)(0xffff78f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(
			0xffff71f1)), unchecked((int)(0xe3ffffff)), unchecked((int)(0xffff63e3)), unchecked(
			(int)(0xc7ffffff)), unchecked((int)(0xffff47c7)), unchecked((int)(0x8fffffff)), 
			unchecked((int)(0xffff0f8f)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffff1f1f
			)), unchecked((int)(0x3fffffff)), unchecked((int)(0xffff3f3f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xfffc7cfc)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfff878f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff171f1)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffe363e3)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffc747c7)), unchecked((int)(0x0)), unchecked((int)(0xe0800000
			)), unchecked((int)(0x0)), unchecked((int)(0x40000000)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfc7cff)
			), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f878ff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf1f171ff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xe3e363ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xc7c747ff)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfcfc7fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f87fff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xf1f17fff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xe3e37fff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xc7c77fff)), unchecked((int)(0x0)), unchecked((int)(0x6000)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xfffffcfc)), unchecked((int)(0xffffbfff)), unchecked((int)(0xfffff8f8)), 
			unchecked((int)(0xffffbfff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffbfff
			)), unchecked((int)(0xffffe3e3)), unchecked((int)(0xffffbfff)), unchecked((int)(
			0xffffc7c7)), unchecked((int)(0xffffbfff)), unchecked((int)(0xffff8f8f)), unchecked(
			(int)(0xffffbfff)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffbfff)), 
			unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffbfff)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0xffffbfff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(
			0xffffbfff)), unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffffbfff)), unchecked(
			(int)(0xffe3e3e3)), unchecked((int)(0xffffbfff)), unchecked((int)(0xffc7c7c7)), 
			unchecked((int)(0xffffbfff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffbfff
			)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffbfff)), unchecked((int)(
			0xff3f3f3f)), unchecked((int)(0xffffbfff)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xffffbfff)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffffbfff)), 
			unchecked((int)(0xf1f1f1ff)), unchecked((int)(0xffffbfff)), unchecked((int)(0xe3e3e3ff
			)), unchecked((int)(0xffffbfff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(
			0xffffbfff)), unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffffbfff)), unchecked(
			(int)(0x1f1f1fff)), unchecked((int)(0xffffbfff)), unchecked((int)(0x3f3f3fff)), 
			unchecked((int)(0xffffbfff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffbffc
			)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffffbff8)), unchecked((int)(
			0xf1f1ffff)), unchecked((int)(0xffffbff1)), unchecked((int)(0xe3e3ffff)), unchecked(
			(int)(0xffffbfe3)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffffbfc7)), 
			unchecked((int)(0x8f8fffff)), unchecked((int)(0xffffbf8f)), unchecked((int)(0x1f1fffff
			)), unchecked((int)(0xffffbf1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(
			0xffffbf3f)), unchecked((int)(0xfcffffff)), unchecked((int)(0xffffbcfc)), unchecked(
			(int)(0xf8ffffff)), unchecked((int)(0xffffb8f8)), unchecked((int)(0xf1ffffff)), 
			unchecked((int)(0xffffb1f1)), unchecked((int)(0xe0000000)), unchecked((int)(0xf0f8a0e0
			)), unchecked((int)(0x0)), unchecked((int)(0xe0f080c0)), unchecked((int)(0x0)), 
			unchecked((int)(0xe08080)), unchecked((int)(0x0)), unchecked((int)(0xe00000)), unchecked(
			(int)(0x0)), unchecked((int)(0xe02020)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfffcbcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff8b8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfff1b1f1)), unchecked((int)(0xe0000000
			)), unchecked((int)(0xf0e0a0e0)), unchecked((int)(0x0)), unchecked((int)(0xe0c080c0
			)), unchecked((int)(0x0)), unchecked((int)(0x808000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x202000)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xfcfcbcff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xf8f8b8ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf1f1b1ff)), 
			unchecked((int)(0xe0000000)), unchecked((int)(0xe0e0a0f8)), unchecked((int)(0x0)
			), unchecked((int)(0xc0c080f0)), unchecked((int)(0x0)), unchecked((int)(0x808000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x200000)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcbfff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8f8bfff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf1f1bfff)), unchecked((int)(0xf8000000)), unchecked((int)(0xe0e0b8f8
			)), unchecked((int)(0x0)), unchecked((int)(0xc0c090f0)), unchecked((int)(0x0)), 
			unchecked((int)(0x8080a000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0xa000)), unchecked((int)(0xfffffcfc)), unchecked((
			int)(0xffffbfff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffffbfff)), unchecked(
			(int)(0xfffff1f1)), unchecked((int)(0xffffbfff)), unchecked((int)(0xffffe3e3)), 
			unchecked((int)(0xffffbfff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffbfff
			)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffffbfff)), unchecked((int)(
			0xffff1f1f)), unchecked((int)(0xffffbfff)), unchecked((int)(0xffff3f3f)), unchecked(
			(int)(0xffffbfff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffffbfff)), 
			unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffffbfff)), unchecked((int)(0xfff1f1f1
			)), unchecked((int)(0xffffbfff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(
			0xffffbfff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffbfff)), unchecked(
			(int)(0xff8f8f8f)), unchecked((int)(0xffffbfff)), unchecked((int)(0xff1f1f1f)), 
			unchecked((int)(0xffffbfff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffbfff
			)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffffbfff)), unchecked((int)(
			0xf8f8f8ff)), unchecked((int)(0xffffbfff)), unchecked((int)(0xf1f1f1ff)), unchecked(
			(int)(0xffffbfff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffbfff)), 
			unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffffbfff)), unchecked((int)(0x8f8f8fff
			)), unchecked((int)(0xffffbfff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(
			0xffffbfff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffbfff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xffffbffc)), unchecked((int)(0xf8f8ffff)), 
			unchecked((int)(0xffffbff8)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xffffbff1
			)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffffbfe3)), unchecked((int)(
			0xc7c7ffff)), unchecked((int)(0xffffbfc7)), unchecked((int)(0x8f8fffff)), unchecked(
			(int)(0xffffbf8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(0xffffbf1f)), 
			unchecked((int)(0x3f3fffff)), unchecked((int)(0xffffbf3f)), unchecked((int)(0xfcffffff
			)), unchecked((int)(0xffffbcfc)), unchecked((int)(0xf8ffffff)), unchecked((int)(
			0xffffb8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xffffb1f1)), unchecked(
			(int)(0xe3ffffff)), unchecked((int)(0xffffa3e3)), unchecked((int)(0xc7ffffff)), 
			unchecked((int)(0xffff87c7)), unchecked((int)(0x8fffffff)), unchecked((int)(0xffff8f8f
			)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffff1f1f)), unchecked((int)(
			0x3fffffff)), unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfffcbcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff8b8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfff1b1f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffe3a3e3)), unchecked((int)(0xe0000000)), unchecked((int)(
			0xf0c080c0)), unchecked((int)(0x0)), unchecked((int)(0xe0808080)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xf0303030
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcbcff)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xf8f8b8ff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xf1f1b1ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xe3e3a3ff)), 
			unchecked((int)(0xf8000000)), unchecked((int)(0xc0c080f8)), unchecked((int)(0x0)
			), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffffff)), unchecked((int
			)(0xfcfcbfff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f8bfff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf1f1bfff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xe3e3bfff)), unchecked((int)(0xf8000000)), unchecked((int)(0xc0c0b8f8
			)), unchecked((int)(0x0)), unchecked((int)(0x808090f0)), unchecked((int)(0x0)), 
			unchecked((int)(0xa000)), unchecked((int)(0x0)), unchecked((int)(0x203010f0)), unchecked(
			(int)(0xfffffcfc)), unchecked((int)(0xffffdfff)), unchecked((int)(0xfffff8f8)), 
			unchecked((int)(0xffffdfff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffdfff
			)), unchecked((int)(0xffffe3e3)), unchecked((int)(0xffffdfff)), unchecked((int)(
			0xffffc7c7)), unchecked((int)(0xffffdfff)), unchecked((int)(0xffff8f8f)), unchecked(
			(int)(0xffffdfff)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffdfff)), 
			unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffdfff)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0xffffdfff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(
			0xffffdfff)), unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffffdfff)), unchecked(
			(int)(0xffe3e3e3)), unchecked((int)(0xffffdfff)), unchecked((int)(0xffc7c7c7)), 
			unchecked((int)(0xffffdfff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffdfff
			)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffdfff)), unchecked((int)(
			0xff3f3f3f)), unchecked((int)(0xffffdfff)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xffffdfff)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffffdfff)), 
			unchecked((int)(0xf1f1f1ff)), unchecked((int)(0xffffdfff)), unchecked((int)(0xe3e3e3ff
			)), unchecked((int)(0xffffdfff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(
			0xffffdfff)), unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffffdfff)), unchecked(
			(int)(0x1f1f1fff)), unchecked((int)(0xffffdfff)), unchecked((int)(0x3f3f3fff)), 
			unchecked((int)(0xffffdfff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffdffc
			)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffffdff8)), unchecked((int)(
			0xf1f1ffff)), unchecked((int)(0xffffdff1)), unchecked((int)(0xe3e3ffff)), unchecked(
			(int)(0xffffdfe3)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffffdfc7)), 
			unchecked((int)(0x8f8fffff)), unchecked((int)(0xffffdf8f)), unchecked((int)(0x1f1fffff
			)), unchecked((int)(0xffffdf1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(
			0xffffdf3f)), unchecked((int)(0xfcffffff)), unchecked((int)(0xffffdcfc)), unchecked(
			(int)(0xf8ffffff)), unchecked((int)(0xffffd8f8)), unchecked((int)(0xf0000000)), 
			unchecked((int)(0xf8fcd0f0)), unchecked((int)(0x0)), unchecked((int)(0xf0f8c0e0)
			), unchecked((int)(0x0)), unchecked((int)(0x704040)), unchecked((int)(0x0)), unchecked(
			(int)(0x700000)), unchecked((int)(0x0)), unchecked((int)(0x701010)), unchecked((
			int)(0x0)), unchecked((int)(0x78f81838)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfffcdcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff8d8f8)), 
			unchecked((int)(0xf0000000)), unchecked((int)(0xf8f0d0f0)), unchecked((int)(0x0)
			), unchecked((int)(0xf0e0c0e0)), unchecked((int)(0x0)), unchecked((int)(0x404000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x101000)), unchecked((int)(0x0)), unchecked((int)(0x78381838)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfcfcdcff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf8f8d8ff)), unchecked((int)(0xf0000000)), unchecked((int)(0xf0f0d0fc
			)), unchecked((int)(0x0)), unchecked((int)(0xe0e0c0f8)), unchecked((int)(0x0)), 
			unchecked((int)(0x404000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x101000)), unchecked((int)(0x0)), unchecked((int)(
			0x383818f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcdfff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8f8dfff)), unchecked((int)(0xfc000000)), 
			unchecked((int)(0xf0f0dcfc)), unchecked((int)(0x0)), unchecked((int)(0xe0e0c8f8)
			), unchecked((int)(0x0)), unchecked((int)(0x40405000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x10105000)), unchecked((int
			)(0x0)), unchecked((int)(0x383898f8)), unchecked((int)(0xfffffcfc)), unchecked((
			int)(0xffffdfff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffffdfff)), unchecked(
			(int)(0xfffff1f1)), unchecked((int)(0xffffdfff)), unchecked((int)(0xffffe3e3)), 
			unchecked((int)(0xffffdfff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffdfff
			)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffffdfff)), unchecked((int)(
			0xffff1f1f)), unchecked((int)(0xffffdfff)), unchecked((int)(0xffff3f3f)), unchecked(
			(int)(0xffffdfff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffffdfff)), 
			unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffffdfff)), unchecked((int)(0xfff1f1f1
			)), unchecked((int)(0xffffdfff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(
			0xffffdfff)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffdfff)), unchecked(
			(int)(0xff8f8f8f)), unchecked((int)(0xffffdfff)), unchecked((int)(0xff1f1f1f)), 
			unchecked((int)(0xffffdfff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffdfff
			)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffffdfff)), unchecked((int)(
			0xf8f8f8ff)), unchecked((int)(0xffffdfff)), unchecked((int)(0xf1f1f1ff)), unchecked(
			(int)(0xffffdfff)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffdfff)), 
			unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffffdfff)), unchecked((int)(0x8f8f8fff
			)), unchecked((int)(0xffffdfff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(
			0xffffdfff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffdfff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xffffdffc)), unchecked((int)(0xf8f8ffff)), 
			unchecked((int)(0xffffdff8)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xffffdff1
			)), unchecked((int)(0xe3e3ffff)), unchecked((int)(0xffffdfe3)), unchecked((int)(
			0xc7c7ffff)), unchecked((int)(0xffffdfc7)), unchecked((int)(0x8f8fffff)), unchecked(
			(int)(0xffffdf8f)), unchecked((int)(0x1f1fffff)), unchecked((int)(0xffffdf1f)), 
			unchecked((int)(0x3f3fffff)), unchecked((int)(0xffffdf3f)), unchecked((int)(0xfcffffff
			)), unchecked((int)(0xffffdcfc)), unchecked((int)(0xf8ffffff)), unchecked((int)(
			0xffffd8f8)), unchecked((int)(0xf1ffffff)), unchecked((int)(0xffffd1f1)), unchecked(
			(int)(0xe3ffffff)), unchecked((int)(0xffffc3e3)), unchecked((int)(0xc7ffffff)), 
			unchecked((int)(0xffffc7c7)), unchecked((int)(0x8fffffff)), unchecked((int)(0xffff8f8f
			)), unchecked((int)(0x1fffffff)), unchecked((int)(0xffff1f1f)), unchecked((int)(
			0x3fffffff)), unchecked((int)(0xffff1f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfffcdcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff8d8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfff1d1f1)), unchecked((int)(0xf0000000
			)), unchecked((int)(0xf8e0c0e0)), unchecked((int)(0x0)), unchecked((int)(0xf0c0c0c0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x78181818)), unchecked((int)(0x7c000000)), unchecked((int)(0xfc3c1c3c)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcdcff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8f8d8ff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf1f1d1ff)), unchecked((int)(0xfc000000)), unchecked((int)(0xe0e0c0fc)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfc000000)), unchecked(
			(int)(0x3c3c1cfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcdfff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xf8f8dfff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf1f1dfff)), unchecked((int)(0xfc000000)), unchecked((int)(
			0xe0e0dcfc)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c8f8)), unchecked((int
			)(0x0)), unchecked((int)(0x5000)), unchecked((int)(0x0)), unchecked((int)(0x181898f8
			)), unchecked((int)(0xfc000000)), unchecked((int)(0x3c3cdcfc)), unchecked((int)(
			0xfffffcfc)), unchecked((int)(0xffffefff)), unchecked((int)(0xfffff8f8)), unchecked(
			(int)(0xffffefff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffefff)), 
			unchecked((int)(0xffffe3e3)), unchecked((int)(0xffffefff)), unchecked((int)(0xffffc7c7
			)), unchecked((int)(0xffffefff)), unchecked((int)(0xffff8f8f)), unchecked((int)(
			0xffffefff)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffefff)), unchecked(
			(int)(0xffff3f3f)), unchecked((int)(0xffffefff)), unchecked((int)(0xfffcfcfc)), 
			unchecked((int)(0xffffefff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffffefff
			)), unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffffefff)), unchecked((int)(
			0xffe3e3e3)), unchecked((int)(0xffffefff)), unchecked((int)(0xffc7c7c7)), unchecked(
			(int)(0xffffefff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffefff)), 
			unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffefff)), unchecked((int)(0xff3f3f3f
			)), unchecked((int)(0xffffefff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(
			0xffffefff)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffffefff)), unchecked(
			(int)(0xf1f1f1ff)), unchecked((int)(0xffffefff)), unchecked((int)(0xe3e3e3ff)), 
			unchecked((int)(0xffffefff)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffffefff
			)), unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffffefff)), unchecked((int)(
			0x1f1f1fff)), unchecked((int)(0xffffefff)), unchecked((int)(0x3f3f3fff)), unchecked(
			(int)(0xffffefff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffeffc)), 
			unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffffeff8)), unchecked((int)(0xf1f1ffff
			)), unchecked((int)(0xffffeff1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(
			0xffffefe3)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffffefc7)), unchecked(
			(int)(0x8f8fffff)), unchecked((int)(0xffffef8f)), unchecked((int)(0x1f1fffff)), 
			unchecked((int)(0xffffef1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffffef3f
			)), unchecked((int)(0xfcffffff)), unchecked((int)(0xffffecfc)), unchecked((int)(
			0xf8000000)), unchecked((int)(0xfcfee8f8)), unchecked((int)(0x0)), unchecked((int
			)(0x787c6070)), unchecked((int)(0x0)), unchecked((int)(0x382020)), unchecked((int
			)(0x0)), unchecked((int)(0x380000)), unchecked((int)(0x0)), unchecked((int)(0x380808
			)), unchecked((int)(0x0)), unchecked((int)(0x3c7c0c1c)), unchecked((int)(0x3e000000
			)), unchecked((int)(0x7efe2e3e)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xfffcecfc)), unchecked((int)(0xf8000000)), unchecked((int)(0xfcf8e8f8)), unchecked(
			(int)(0x0)), unchecked((int)(0x78706070)), unchecked((int)(0x0)), unchecked((int
			)(0x202000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)
			), unchecked((int)(0x80800)), unchecked((int)(0x0)), unchecked((int)(0x3c1c0c1c)
			), unchecked((int)(0x3e000000)), unchecked((int)(0x7e3e2e3e)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xfcfcecff)), unchecked((int)(0xf8000000)), unchecked((int)(
			0xf8f8e8fe)), unchecked((int)(0x0)), unchecked((int)(0x7070607c)), unchecked((int
			)(0x0)), unchecked((int)(0x202000)), unchecked((int)(0x0)), unchecked((int)(0x0)
			), unchecked((int)(0x0)), unchecked((int)(0x80800)), unchecked((int)(0x0)), unchecked(
			(int)(0x1c1c0c7c)), unchecked((int)(0x3e000000)), unchecked((int)(0x3e3e2efe)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcefff)), unchecked((int)(0xfe000000
			)), unchecked((int)(0xf8f8eefe)), unchecked((int)(0x0)), unchecked((int)(0x7070647c
			)), unchecked((int)(0x0)), unchecked((int)(0x20202800)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x8082800)), unchecked(
			(int)(0x0)), unchecked((int)(0x1c1c4c7c)), unchecked((int)(0xfe000000)), unchecked(
			(int)(0x3e3eeefe)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xffffefff)), 
			unchecked((int)(0xfffff8f8)), unchecked((int)(0xffffefff)), unchecked((int)(0xfffff1f1
			)), unchecked((int)(0xffffefff)), unchecked((int)(0xffffe3e3)), unchecked((int)(
			0xffffefff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffefff)), unchecked(
			(int)(0xffff8f8f)), unchecked((int)(0xffffefff)), unchecked((int)(0xffff1f1f)), 
			unchecked((int)(0xffffefff)), unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffefff
			)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffffefff)), unchecked((int)(
			0xfff8f8f8)), unchecked((int)(0xffffefff)), unchecked((int)(0xfff1f1f1)), unchecked(
			(int)(0xffffefff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffffefff)), 
			unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffefff)), unchecked((int)(0xff8f8f8f
			)), unchecked((int)(0xffffefff)), unchecked((int)(0xff1f1f1f)), unchecked((int)(
			0xffffefff)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffefff)), unchecked(
			(int)(0xfcfcfcff)), unchecked((int)(0xffffefff)), unchecked((int)(0xf8f8f8ff)), 
			unchecked((int)(0xffffefff)), unchecked((int)(0xf1f1f1ff)), unchecked((int)(0xffffefff
			)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffefff)), unchecked((int)(
			0xc7c7c7ff)), unchecked((int)(0xffffefff)), unchecked((int)(0x8f8f8fff)), unchecked(
			(int)(0xffffefff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(0xffffefff)), 
			unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffefff)), unchecked((int)(0xfcfcffff
			)), unchecked((int)(0xffffeffc)), unchecked((int)(0xf8f8ffff)), unchecked((int)(
			0xffffeff8)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xffffeff1)), unchecked(
			(int)(0xe3e3ffff)), unchecked((int)(0xffffefe3)), unchecked((int)(0xc7c7ffff)), 
			unchecked((int)(0xffffefc7)), unchecked((int)(0x8f8fffff)), unchecked((int)(0xffffef8f
			)), unchecked((int)(0x1f1fffff)), unchecked((int)(0xffffef1f)), unchecked((int)(
			0x3f3fffff)), unchecked((int)(0xffffef3f)), unchecked((int)(0xfcffffff)), unchecked(
			(int)(0xffffecfc)), unchecked((int)(0xf8ffffff)), unchecked((int)(0xffffe8f8)), 
			unchecked((int)(0xf1ffffff)), unchecked((int)(0xffffe1f1)), unchecked((int)(0xe3ffffff
			)), unchecked((int)(0xffffe3e3)), unchecked((int)(0xc7ffffff)), unchecked((int)(
			0xffffc7c7)), unchecked((int)(0x8fffffff)), unchecked((int)(0xffff8f8f)), unchecked(
			(int)(0x1fffffff)), unchecked((int)(0xffff0f1f)), unchecked((int)(0x3fffffff)), 
			unchecked((int)(0xffff2f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xfffcecfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff8e8f8)), unchecked((int)(
			0xf8000000)), unchecked((int)(0xfcf0e0f0)), unchecked((int)(0x0)), unchecked((int
			)(0x78606060)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x3c0c0c0c)), unchecked((int)(0x3e000000)), unchecked((int)(
			0x7e1e0e1e)), unchecked((int)(0xffffffff)), unchecked((int)(0xff3f2f3f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfcfcecff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf8f8e8ff)), unchecked((int)(0xfe000000)), unchecked((int)(0xf0f0e0fe
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfe000000
			)), unchecked((int)(0x1e1e0efe)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x3f3f2fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcefff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8f8efff)), unchecked((int)(0xfe000000)), 
			unchecked((int)(0xf0f0eefe)), unchecked((int)(0x0)), unchecked((int)(0x6060647c)
			), unchecked((int)(0x0)), unchecked((int)(0x2800)), unchecked((int)(0x0)), unchecked(
			(int)(0xc0c4c7c)), unchecked((int)(0xfe000000)), unchecked((int)(0x1e1eeefe)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x3f3fefff)), unchecked((int)(0xfffffcfc)), 
			unchecked((int)(0xffffff7f)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffffff7f
			)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffff7f)), unchecked((int)(
			0xffffe3e3)), unchecked((int)(0xffffff7f)), unchecked((int)(0xffffc7c7)), unchecked(
			(int)(0xffffff7f)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffffff7f)), 
			unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffff7f)), unchecked((int)(0xffff3f3f
			)), unchecked((int)(0xffffff7f)), unchecked((int)(0xfffcfcfc)), unchecked((int)(
			0xffffff7f)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffffff7f)), unchecked(
			(int)(0xfff1f1f1)), unchecked((int)(0xffffff7f)), unchecked((int)(0xffe3e3e3)), 
			unchecked((int)(0xffffff7f)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffff7f
			)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffff7f)), unchecked((int)(
			0xff1f1f1f)), unchecked((int)(0xffffff7f)), unchecked((int)(0xff3f3f3f)), unchecked(
			(int)(0xffffff7f)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffffff7f)), 
			unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffffff7f)), unchecked((int)(0xf1f1f1ff
			)), unchecked((int)(0xffffff7f)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(
			0xffffff7f)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffffff7f)), unchecked(
			(int)(0x8f8f8fff)), unchecked((int)(0xffffff7f)), unchecked((int)(0x1f1f1fff)), 
			unchecked((int)(0xffffff7f)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffff7f
			)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffff7c)), unchecked((int)(
			0xf8f8ffff)), unchecked((int)(0xffffff78)), unchecked((int)(0xf1f1ffff)), unchecked(
			(int)(0xffffff71)), unchecked((int)(0xe0000000)), unchecked((int)(0xf0f0f860)), 
			unchecked((int)(0x40000000)), unchecked((int)(0xe0e0f040)), unchecked((int)(0x0)
			), unchecked((int)(0xc0e000)), unchecked((int)(0x0)), unchecked((int)(0xc000)), 
			unchecked((int)(0x0)), unchecked((int)(0xc000)), unchecked((int)(0xfcffffff)), unchecked(
			(int)(0xfffffc7c)), unchecked((int)(0xf8ffffff)), unchecked((int)(0xfffff878)), 
			unchecked((int)(0xf1ffffff)), unchecked((int)(0xfffff171)), unchecked((int)(0x0)
			), unchecked((int)(0xe0f0e060)), unchecked((int)(0x0)), unchecked((int)(0x40604040
			)), unchecked((int)(0x0)), unchecked((int)(0x400000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xfffcfc7c)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xfff8f878)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff1f171)), unchecked(
			(int)(0x0)), unchecked((int)(0xe0e0e000)), unchecked((int)(0x0)), unchecked((int
			)(0x40404000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfcfcfc7f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf8f8f87f)), unchecked((int)(0xffffffff)), unchecked((int)(0xf1f1f17f
			)), unchecked((int)(0x0)), unchecked((int)(0xe0e0e000)), unchecked((int)(0x0)), 
			unchecked((int)(0x40400000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcff7f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8f8ff7f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf1f1ff7f)), unchecked((int)(0x0)), unchecked((int)(0xe0e0e000)), unchecked((int
			)(0x0)), unchecked((int)(0x40400000)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xffffff7f)), unchecked(
			(int)(0xfffff8f8)), unchecked((int)(0xffffff7f)), unchecked((int)(0xfffff1f1)), 
			unchecked((int)(0xffffff7f)), unchecked((int)(0xffffe3e3)), unchecked((int)(0xffffff7f
			)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffff7f)), unchecked((int)(
			0xffff8f8f)), unchecked((int)(0xffffff7f)), unchecked((int)(0xffff1f1f)), unchecked(
			(int)(0xffffff7f)), unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffff7f)), 
			unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffffff7f)), unchecked((int)(0xfff8f8f8
			)), unchecked((int)(0xffffff7f)), unchecked((int)(0xfff1f1f1)), unchecked((int)(
			0xffffff7f)), unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffffff7f)), unchecked(
			(int)(0xffc7c7c7)), unchecked((int)(0xffffff7f)), unchecked((int)(0xff8f8f8f)), 
			unchecked((int)(0xffffff7f)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffff7f
			)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffff7f)), unchecked((int)(
			0xfcfcfcff)), unchecked((int)(0xffffff7f)), unchecked((int)(0xf8f8f8ff)), unchecked(
			(int)(0xffffff7f)), unchecked((int)(0xf1f1f1ff)), unchecked((int)(0xffffff7f)), 
			unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffff7f)), unchecked((int)(0xc7c7c7ff
			)), unchecked((int)(0xffffff7f)), unchecked((int)(0x8f8f8fff)), unchecked((int)(
			0xffffff7f)), unchecked((int)(0x1f1f1fff)), unchecked((int)(0xffffff7f)), unchecked(
			(int)(0x3f3f3fff)), unchecked((int)(0xffffff7f)), unchecked((int)(0xfcfcffff)), 
			unchecked((int)(0xffffff7c)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffffff78
			)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xffffff71)), unchecked((int)(
			0xe3e3ffff)), unchecked((int)(0xffffff63)), unchecked((int)(0xc7c7ffff)), unchecked(
			(int)(0xffffff47)), unchecked((int)(0x8f8fffff)), unchecked((int)(0xffffff0f)), 
			unchecked((int)(0x1f1fffff)), unchecked((int)(0xffffff1f)), unchecked((int)(0x3f3fffff
			)), unchecked((int)(0xffffff3f)), unchecked((int)(0xfcffffff)), unchecked((int)(
			0xfffffc7c)), unchecked((int)(0xf8ffffff)), unchecked((int)(0xfffff878)), unchecked(
			(int)(0xf1ffffff)), unchecked((int)(0xfffff171)), unchecked((int)(0xe3ffffff)), 
			unchecked((int)(0xffffe363)), unchecked((int)(0xc0000000)), unchecked((int)(0xf0f0c040
			)), unchecked((int)(0x0)), unchecked((int)(0xe0e08000)), unchecked((int)(0x0)), 
			unchecked((int)(0xc00000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfffcfc7c)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xfff8f878)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff1f171
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffe3e363)), unchecked((int)(
			0x0)), unchecked((int)(0xe0c0c040)), unchecked((int)(0x0)), unchecked((int)(0x40000000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcfc7f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8f8f87f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf1f1f17f)), unchecked((int)(0xffffffff)), unchecked((int)(0xe3e3e37f
			)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c000)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((
			int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffffff)), unchecked((int)
			(0xfcfcff7f)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f8ff7f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf1f1ff7f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xe3e3ff7f)), unchecked((int)(0x0)), unchecked((int)(0xc0c0e000)
			), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfffffcfc
			)), unchecked((int)(0xffffffbf)), unchecked((int)(0xfffff8f8)), unchecked((int)(
			0xffffffbf)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffffbf)), unchecked(
			(int)(0xffffe3e3)), unchecked((int)(0xffffffbf)), unchecked((int)(0xffffc7c7)), 
			unchecked((int)(0xffffffbf)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffffffbf
			)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffffbf)), unchecked((int)(
			0xffff3f3f)), unchecked((int)(0xffffffbf)), unchecked((int)(0xfffcfcfc)), unchecked(
			(int)(0xffffffbf)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffffffbf)), 
			unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffffffbf)), unchecked((int)(0xffe3e3e3
			)), unchecked((int)(0xffffffbf)), unchecked((int)(0xffc7c7c7)), unchecked((int)(
			0xffffffbf)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffffbf)), unchecked(
			(int)(0xff1f1f1f)), unchecked((int)(0xffffffbf)), unchecked((int)(0xff3f3f3f)), 
			unchecked((int)(0xffffffbf)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffffffbf
			)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffffffbf)), unchecked((int)(
			0xf1f1f1ff)), unchecked((int)(0xffffffbf)), unchecked((int)(0xe3e3e3ff)), unchecked(
			(int)(0xffffffbf)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffffffbf)), 
			unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffffffbf)), unchecked((int)(0x1f1f1fff
			)), unchecked((int)(0xffffffbf)), unchecked((int)(0x3f3f3fff)), unchecked((int)(
			0xffffffbf)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffffbc)), unchecked(
			(int)(0xf8f8ffff)), unchecked((int)(0xffffffb8)), unchecked((int)(0xf0e00000)), 
			unchecked((int)(0xf8f8fcb0)), unchecked((int)(0xe0e00000)), unchecked((int)(0xf0f0f8a0
			)), unchecked((int)(0xc0000000)), unchecked((int)(0xe0f080)), unchecked((int)(0x80000000
			)), unchecked((int)(0xe080)), unchecked((int)(0x0)), unchecked((int)(0xe000)), unchecked(
			(int)(0x20000000)), unchecked((int)(0xe020)), unchecked((int)(0xfcffffff)), unchecked(
			(int)(0xfffffcbc)), unchecked((int)(0xf8ffffff)), unchecked((int)(0xfffff8b8)), 
			unchecked((int)(0xf0e00000)), unchecked((int)(0xf0f8f0b0)), unchecked((int)(0xc0000000
			)), unchecked((int)(0xe0f0e0a0)), unchecked((int)(0xc0000000)), unchecked((int)(
			0xe0c080)), unchecked((int)(0x0)), unchecked((int)(0x8080)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x2020)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfffcfcbc)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xfff8f8b8)), unchecked((int)(0xf0e00000)), unchecked((int)(0xf0f0f0b0
			)), unchecked((int)(0xc0000000)), unchecked((int)(0xe0e0e0a0)), unchecked((int)(
			0x0)), unchecked((int)(0xc0c080)), unchecked((int)(0x0)), unchecked((int)(0x8000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x2000)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcfcbf)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8f8f8bf)), unchecked((int)(0xf0e00000)), 
			unchecked((int)(0xf0f0f0b8)), unchecked((int)(0xc0000000)), unchecked((int)(0xe0e0e0b0
			)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c080)), unchecked((int)(0x0)), 
			unchecked((int)(0x808000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x202000)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfcfcffbf)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f8ffbf)), 
			unchecked((int)(0xf8e00000)), unchecked((int)(0xf0f0fcb8)), unchecked((int)(0xc0000000
			)), unchecked((int)(0xe0e0f8b0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0f080
			)), unchecked((int)(0x0)), unchecked((int)(0x8080e000)), unchecked((int)(0x0)), 
			unchecked((int)(0xe000)), unchecked((int)(0x0)), unchecked((int)(0x2020e000)), unchecked(
			(int)(0xfffffcfc)), unchecked((int)(0xffffffbf)), unchecked((int)(0xfffff8f8)), 
			unchecked((int)(0xffffffbf)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffffbf
			)), unchecked((int)(0xffffe3e3)), unchecked((int)(0xffffffbf)), unchecked((int)(
			0xffffc7c7)), unchecked((int)(0xffffffbf)), unchecked((int)(0xffff8f8f)), unchecked(
			(int)(0xffffffbf)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffffbf)), 
			unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffffbf)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0xffffffbf)), unchecked((int)(0xfff8f8f8)), unchecked((int)(
			0xffffffbf)), unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffffffbf)), unchecked(
			(int)(0xffe3e3e3)), unchecked((int)(0xffffffbf)), unchecked((int)(0xffc7c7c7)), 
			unchecked((int)(0xffffffbf)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffffbf
			)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffffbf)), unchecked((int)(
			0xff3f3f3f)), unchecked((int)(0xffffffbf)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xffffffbf)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffffffbf)), 
			unchecked((int)(0xf1f1f1ff)), unchecked((int)(0xffffffbf)), unchecked((int)(0xe3e3e3ff
			)), unchecked((int)(0xffffffbf)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(
			0xffffffbf)), unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffffffbf)), unchecked(
			(int)(0x1f1f1fff)), unchecked((int)(0xffffffbf)), unchecked((int)(0x3f3f3fff)), 
			unchecked((int)(0xffffffbf)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffffbc
			)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffffffb8)), unchecked((int)(
			0xf1f1ffff)), unchecked((int)(0xffffffb1)), unchecked((int)(0xe3e3ffff)), unchecked(
			(int)(0xffffffa3)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffffff87)), 
			unchecked((int)(0x8f8fffff)), unchecked((int)(0xffffff8f)), unchecked((int)(0x1f1fffff
			)), unchecked((int)(0xffffff1f)), unchecked((int)(0x3f3fffff)), unchecked((int)(
			0xffffff3f)), unchecked((int)(0xfcffffff)), unchecked((int)(0xfffffcbc)), unchecked(
			(int)(0xf8ffffff)), unchecked((int)(0xfffff8b8)), unchecked((int)(0xf1ffffff)), 
			unchecked((int)(0xfffff1b1)), unchecked((int)(0xe0e00000)), unchecked((int)(0xf8f8e0a0
			)), unchecked((int)(0xc0e00000)), unchecked((int)(0xf0f0c080)), unchecked((int)(
			0x80000000)), unchecked((int)(0xe08080)), unchecked((int)(0x0)), unchecked((int)
			(0x0)), unchecked((int)(0x30000000)), unchecked((int)(0xf03030)), unchecked((int
			)(0xffffffff)), unchecked((int)(0xfffcfcbc)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfff8f8b8)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff1f1b1)), 
			unchecked((int)(0xf0e00000)), unchecked((int)(0xf0e0e0a0)), unchecked((int)(0xc0000000
			)), unchecked((int)(0xe0c0c080)), unchecked((int)(0x0)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((
			int)(0x0)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcfcbf)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8f8f8bf)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xf1f1f1bf)), unchecked((int)(0xf8e00000)), unchecked((int)(0xe0e0e0b8
			)), unchecked((int)(0xc0000000)), unchecked((int)(0xc0c0c0b0)), unchecked((int)(
			0x0)), unchecked((int)(0x80808080)), unchecked((int)(0x0)), unchecked((int)(0x0)
			), unchecked((int)(0x0)), unchecked((int)(0x30303030)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xfcfcffbf)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf8f8ffbf)), unchecked((int)(0xffffffff)), unchecked((int)(0xf1f1ffbf)), unchecked(
			(int)(0xf8e00000)), unchecked((int)(0xe0e0fcbc)), unchecked((int)(0xc0000000)), 
			unchecked((int)(0xc0c0f8b8)), unchecked((int)(0x0)), unchecked((int)(0x8080f0b0)
			), unchecked((int)(0x0)), unchecked((int)(0x10f0b0)), unchecked((int)(0x0)), unchecked(
			(int)(0x3030f0b0)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xffffffdf)), 
			unchecked((int)(0xfffff8f8)), unchecked((int)(0xffffffdf)), unchecked((int)(0xfffff1f1
			)), unchecked((int)(0xffffffdf)), unchecked((int)(0xffffe3e3)), unchecked((int)(
			0xffffffdf)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffffdf)), unchecked(
			(int)(0xffff8f8f)), unchecked((int)(0xffffffdf)), unchecked((int)(0xffff1f1f)), 
			unchecked((int)(0xffffffdf)), unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffffdf
			)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffffffdf)), unchecked((int)(
			0xfff8f8f8)), unchecked((int)(0xffffffdf)), unchecked((int)(0xfff1f1f1)), unchecked(
			(int)(0xffffffdf)), unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffffffdf)), 
			unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffffdf)), unchecked((int)(0xff8f8f8f
			)), unchecked((int)(0xffffffdf)), unchecked((int)(0xff1f1f1f)), unchecked((int)(
			0xffffffdf)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffffdf)), unchecked(
			(int)(0xfcfcfcff)), unchecked((int)(0xffffffdf)), unchecked((int)(0xf8f8f8ff)), 
			unchecked((int)(0xffffffdf)), unchecked((int)(0xf1f1f1ff)), unchecked((int)(0xffffffdf
			)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffffdf)), unchecked((int)(
			0xc7c7c7ff)), unchecked((int)(0xffffffdf)), unchecked((int)(0x8f8f8fff)), unchecked(
			(int)(0xffffffdf)), unchecked((int)(0x1f1f1fff)), unchecked((int)(0xffffffdf)), 
			unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffffdf)), unchecked((int)(0xfcfcffff
			)), unchecked((int)(0xffffffdc)), unchecked((int)(0xf8f00000)), unchecked((int)(
			0xfcfcfed8)), unchecked((int)(0xf0f00000)), unchecked((int)(0xf8f8fcd0)), unchecked(
			(int)(0xe0000000)), unchecked((int)(0xf0f8c0)), unchecked((int)(0x40000000)), unchecked(
			(int)(0x7040)), unchecked((int)(0x0)), unchecked((int)(0x7000)), unchecked((int)
			(0x10000000)), unchecked((int)(0x7010)), unchecked((int)(0x38000000)), unchecked(
			(int)(0x78f818)), unchecked((int)(0xfcffffff)), unchecked((int)(0xfffffcdc)), unchecked(
			(int)(0xf8f00000)), unchecked((int)(0xf8fcf8d8)), unchecked((int)(0xe0000000)), 
			unchecked((int)(0xf0f8f0d0)), unchecked((int)(0xe0000000)), unchecked((int)(0xf0e0c0
			)), unchecked((int)(0x0)), unchecked((int)(0x4040)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1010)), unchecked((int)(0x38000000
			)), unchecked((int)(0x783818)), unchecked((int)(0xffffffff)), unchecked((int)(0xfffcfcdc
			)), unchecked((int)(0xf8f00000)), unchecked((int)(0xf8f8f8d8)), unchecked((int)(
			0xe0000000)), unchecked((int)(0xf0f0f0d0)), unchecked((int)(0x0)), unchecked((int
			)(0xe0e0c0)), unchecked((int)(0x0)), unchecked((int)(0x4000)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1000)), unchecked(
			(int)(0x0)), unchecked((int)(0x383818)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfcfcfcdf)), unchecked((int)(0xf8f00000)), unchecked((int)(0xf8f8f8dc)), 
			unchecked((int)(0xe0000000)), unchecked((int)(0xf0f0f0d8)), unchecked((int)(0x0)
			), unchecked((int)(0xe0e0e0c0)), unchecked((int)(0x0)), unchecked((int)(0x404000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x101000)), unchecked((int)(0x0)), unchecked((int)(0x38383818)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfcfcffdf)), unchecked((int)(0xfcf00000)), 
			unchecked((int)(0xf8f8fedc)), unchecked((int)(0xe0000000)), unchecked((int)(0xf0f0fcd8
			)), unchecked((int)(0x0)), unchecked((int)(0xe0e0f8c0)), unchecked((int)(0x0)), 
			unchecked((int)(0x40407000)), unchecked((int)(0x0)), unchecked((int)(0x7000)), unchecked(
			(int)(0x0)), unchecked((int)(0x10107000)), unchecked((int)(0x0)), unchecked((int
			)(0x3838f818)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xffffffdf)), unchecked(
			(int)(0xfffff8f8)), unchecked((int)(0xffffffdf)), unchecked((int)(0xfffff1f1)), 
			unchecked((int)(0xffffffdf)), unchecked((int)(0xffffe3e3)), unchecked((int)(0xffffffdf
			)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffffdf)), unchecked((int)(
			0xffff8f8f)), unchecked((int)(0xffffffdf)), unchecked((int)(0xffff1f1f)), unchecked(
			(int)(0xffffffdf)), unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffffdf)), 
			unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffffffdf)), unchecked((int)(0xfff8f8f8
			)), unchecked((int)(0xffffffdf)), unchecked((int)(0xfff1f1f1)), unchecked((int)(
			0xffffffdf)), unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffffffdf)), unchecked(
			(int)(0xffc7c7c7)), unchecked((int)(0xffffffdf)), unchecked((int)(0xff8f8f8f)), 
			unchecked((int)(0xffffffdf)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffffdf
			)), unchecked((int)(0xff3f3f3f)), unchecked((int)(0xffffffdf)), unchecked((int)(
			0xfcfcfcff)), unchecked((int)(0xffffffdf)), unchecked((int)(0xf8f8f8ff)), unchecked(
			(int)(0xffffffdf)), unchecked((int)(0xf1f1f1ff)), unchecked((int)(0xffffffdf)), 
			unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffffdf)), unchecked((int)(0xc7c7c7ff
			)), unchecked((int)(0xffffffdf)), unchecked((int)(0x8f8f8fff)), unchecked((int)(
			0xffffffdf)), unchecked((int)(0x1f1f1fff)), unchecked((int)(0xffffffdf)), unchecked(
			(int)(0x3f3f3fff)), unchecked((int)(0xffffffdf)), unchecked((int)(0xfcfcffff)), 
			unchecked((int)(0xffffffdc)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffffffd8
			)), unchecked((int)(0xf1f1ffff)), unchecked((int)(0xffffffd1)), unchecked((int)(
			0xe3e3ffff)), unchecked((int)(0xffffffc3)), unchecked((int)(0xc7c7ffff)), unchecked(
			(int)(0xffffffc7)), unchecked((int)(0x8f8fffff)), unchecked((int)(0xffffff8f)), 
			unchecked((int)(0x1f1fffff)), unchecked((int)(0xffffff1f)), unchecked((int)(0x3f3fffff
			)), unchecked((int)(0xffffff1f)), unchecked((int)(0xfcffffff)), unchecked((int)(
			0xfffffcdc)), unchecked((int)(0xf8ffffff)), unchecked((int)(0xfffff8d8)), unchecked(
			(int)(0xf0f00000)), unchecked((int)(0xfcfcf0d0)), unchecked((int)(0xe0f00000)), 
			unchecked((int)(0xf8f8e0c0)), unchecked((int)(0xc0000000)), unchecked((int)(0xf0c0c0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x18000000)), 
			unchecked((int)(0x781818)), unchecked((int)(0x3c7c0000)), unchecked((int)(0xfcfc3c1c
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xfffcfcdc)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xfff8f8d8)), unchecked((int)(0xf8f00000)), unchecked(
			(int)(0xf8f0f0d0)), unchecked((int)(0xe0000000)), unchecked((int)(0xf0e0e0c0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((
			int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x3c000000
			)), unchecked((int)(0x7c3c3c1c)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xfcfcfcdf)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f8f8df)), unchecked(
			(int)(0xfcf00000)), unchecked((int)(0xf0f0f0dc)), unchecked((int)(0xe0000000)), 
			unchecked((int)(0xe0e0e0d8)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c0c0)
			), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x18181818)), unchecked((int)(0x3c000000)), unchecked((int)(0x3c3c3cdc)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcffdf)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8f8ffdf)), unchecked((int)(0xfcf00000)), unchecked((int)(
			0xf0f0fede)), unchecked((int)(0xe0000000)), unchecked((int)(0xe0e0fcdc)), unchecked(
			(int)(0x0)), unchecked((int)(0xc0c0f8d8)), unchecked((int)(0x0)), unchecked((int
			)(0x88f8d8)), unchecked((int)(0x0)), unchecked((int)(0x1818f8d8)), unchecked((int
			)(0x3c000000)), unchecked((int)(0x3c3cfcdc)), unchecked((int)(0xfffffcfc)), unchecked(
			(int)(0xffffffef)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xffffffef)), 
			unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffffef)), unchecked((int)(0xffffe3e3
			)), unchecked((int)(0xffffffef)), unchecked((int)(0xffffc7c7)), unchecked((int)(
			0xffffffef)), unchecked((int)(0xffff8f8f)), unchecked((int)(0xffffffef)), unchecked(
			(int)(0xffff1f1f)), unchecked((int)(0xffffffef)), unchecked((int)(0xffff3f3f)), 
			unchecked((int)(0xffffffef)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xffffffef
			)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffffffef)), unchecked((int)(
			0xfff1f1f1)), unchecked((int)(0xffffffef)), unchecked((int)(0xffe3e3e3)), unchecked(
			(int)(0xffffffef)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffffef)), 
			unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffffef)), unchecked((int)(0xff1f1f1f
			)), unchecked((int)(0xffffffef)), unchecked((int)(0xff3f3f3f)), unchecked((int)(
			0xffffffef)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffffffef)), unchecked(
			(int)(0xf8f8f8ff)), unchecked((int)(0xffffffef)), unchecked((int)(0xf1f1f1ff)), 
			unchecked((int)(0xffffffef)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffffef
			)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffffffef)), unchecked((int)(
			0x8f8f8fff)), unchecked((int)(0xffffffef)), unchecked((int)(0x1f1f1fff)), unchecked(
			(int)(0xffffffef)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffffef)), 
			unchecked((int)(0xfcf80000)), unchecked((int)(0xfefeffec)), unchecked((int)(0xf8f80000
			)), unchecked((int)(0xfcfcfee8)), unchecked((int)(0x70000000)), unchecked((int)(
			0x787c60)), unchecked((int)(0x20000000)), unchecked((int)(0x3820)), unchecked((int
			)(0x0)), unchecked((int)(0x3800)), unchecked((int)(0x8000000)), unchecked((int)(
			0x3808)), unchecked((int)(0x1c000000)), unchecked((int)(0x3c7c0c)), unchecked((int
			)(0x3e3e0000)), unchecked((int)(0x7e7efe2e)), unchecked((int)(0xfcf80000)), unchecked(
			(int)(0xfcfefcec)), unchecked((int)(0xf0000000)), unchecked((int)(0xf8fcf8e8)), 
			unchecked((int)(0x70000000)), unchecked((int)(0x787060)), unchecked((int)(0x0)), 
			unchecked((int)(0x2020)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x808)), unchecked((int)(0x1c000000)), unchecked((int
			)(0x3c1c0c)), unchecked((int)(0x1e000000)), unchecked((int)(0x3e7e3e2e)), unchecked(
			(int)(0xfcf80000)), unchecked((int)(0xfcfcfcec)), unchecked((int)(0xf0000000)), 
			unchecked((int)(0xf8f8f8e8)), unchecked((int)(0x0)), unchecked((int)(0x707060)), 
			unchecked((int)(0x0)), unchecked((int)(0x2000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x800)), unchecked((int)(0x0
			)), unchecked((int)(0x1c1c0c)), unchecked((int)(0x1e000000)), unchecked((int)(0x3e3e3e2e
			)), unchecked((int)(0xfcf80000)), unchecked((int)(0xfcfcfcee)), unchecked((int)(
			0xf0000000)), unchecked((int)(0xf8f8f8ec)), unchecked((int)(0x0)), unchecked((int
			)(0x70707060)), unchecked((int)(0x0)), unchecked((int)(0x202000)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x80800))
			, unchecked((int)(0x0)), unchecked((int)(0x1c1c1c0c)), unchecked((int)(0x1e000000
			)), unchecked((int)(0x3e3e3e6e)), unchecked((int)(0xfef80000)), unchecked((int)(
			0xfcfcffee)), unchecked((int)(0xf0000000)), unchecked((int)(0xf8f8feec)), unchecked(
			(int)(0x0)), unchecked((int)(0x70707c60)), unchecked((int)(0x0)), unchecked((int
			)(0x20203800)), unchecked((int)(0x0)), unchecked((int)(0x3800)), unchecked((int)
			(0x0)), unchecked((int)(0x8083800)), unchecked((int)(0x0)), unchecked((int)(0x1c1c7c0c
			)), unchecked((int)(0x1e000000)), unchecked((int)(0x3e3efe6e)), unchecked((int)(
			0xfffffcfc)), unchecked((int)(0xffffffef)), unchecked((int)(0xfffff8f8)), unchecked(
			(int)(0xffffffef)), unchecked((int)(0xfffff1f1)), unchecked((int)(0xffffffef)), 
			unchecked((int)(0xffffe3e3)), unchecked((int)(0xffffffef)), unchecked((int)(0xffffc7c7
			)), unchecked((int)(0xffffffef)), unchecked((int)(0xffff8f8f)), unchecked((int)(
			0xffffffef)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffffef)), unchecked(
			(int)(0xffff3f3f)), unchecked((int)(0xffffffef)), unchecked((int)(0xfffcfcfc)), 
			unchecked((int)(0xffffffef)), unchecked((int)(0xfff8f8f8)), unchecked((int)(0xffffffef
			)), unchecked((int)(0xfff1f1f1)), unchecked((int)(0xffffffef)), unchecked((int)(
			0xffe3e3e3)), unchecked((int)(0xffffffef)), unchecked((int)(0xffc7c7c7)), unchecked(
			(int)(0xffffffef)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffffef)), 
			unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffffef)), unchecked((int)(0xff3f3f3f
			)), unchecked((int)(0xffffffef)), unchecked((int)(0xfcfcfcff)), unchecked((int)(
			0xffffffef)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xffffffef)), unchecked(
			(int)(0xf1f1f1ff)), unchecked((int)(0xffffffef)), unchecked((int)(0xe3e3e3ff)), 
			unchecked((int)(0xffffffef)), unchecked((int)(0xc7c7c7ff)), unchecked((int)(0xffffffef
			)), unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffffffef)), unchecked((int)(
			0x1f1f1fff)), unchecked((int)(0xffffffef)), unchecked((int)(0x3f3f3fff)), unchecked(
			(int)(0xffffffef)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffffec)), 
			unchecked((int)(0xf8f8ffff)), unchecked((int)(0xffffffe8)), unchecked((int)(0xf1f1ffff
			)), unchecked((int)(0xffffffe1)), unchecked((int)(0xe3e3ffff)), unchecked((int)(
			0xffffffe3)), unchecked((int)(0xc7c7ffff)), unchecked((int)(0xffffffc7)), unchecked(
			(int)(0x8f8fffff)), unchecked((int)(0xffffff8f)), unchecked((int)(0x1f1fffff)), 
			unchecked((int)(0xffffff0f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffffff2f
			)), unchecked((int)(0xfcffffff)), unchecked((int)(0xfffffcec)), unchecked((int)(
			0xf8f80000)), unchecked((int)(0xfefef8e8)), unchecked((int)(0xf0f80000)), unchecked(
			(int)(0xfcfcf0e0)), unchecked((int)(0x60000000)), unchecked((int)(0x786060)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xc000000)), unchecked((int)
			(0x3c0c0c)), unchecked((int)(0x1e3e0000)), unchecked((int)(0x7e7e1e0e)), unchecked(
			(int)(0x3f3f0000)), unchecked((int)(0xffff3f2f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xfffcfcec)), unchecked((int)(0xfcf80000)), unchecked((int)(0xfcf8f8e8
			)), unchecked((int)(0xf0000000)), unchecked((int)(0xf8f0f0e0)), unchecked((int)(
			0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1e000000)), unchecked((int
			)(0x3e1e1e0e)), unchecked((int)(0x7f3f0000)), unchecked((int)(0x7f3f3f2f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfcfcfcef)), unchecked((int)(0xfef80000)), 
			unchecked((int)(0xf8f8f8ee)), unchecked((int)(0xf0000000)), unchecked((int)(0xf0f0f0ec
			)), unchecked((int)(0x0)), unchecked((int)(0x60606060)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c0c)), unchecked(
			(int)(0x1e000000)), unchecked((int)(0x1e1e1e6e)), unchecked((int)(0xff3f0000)), 
			unchecked((int)(0x3f3f3fef)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcffef
			)), unchecked((int)(0xfef80000)), unchecked((int)(0xf8f8ffef)), unchecked((int)(
			0xf0000000)), unchecked((int)(0xf0f0feee)), unchecked((int)(0x0)), unchecked((int
			)(0x60607c6c)), unchecked((int)(0x0)), unchecked((int)(0x447c6c)), unchecked((int
			)(0x0)), unchecked((int)(0xc0c7c6c)), unchecked((int)(0x1e000000)), unchecked((int
			)(0x1e1efeee)), unchecked((int)(0xff3f0000)), unchecked((int)(0x3f3fffef)), unchecked(
			(int)(0x7ffffcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0x7ffff8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0x7ffff1f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0x7fffe3e3)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x7fffc7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0x7fff8f8f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x7fff1f1f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x7fff3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0x7ffcfcfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0x7ff8f8f8)), unchecked((int)(
			0xffffffff)), unchecked((int)(0x7ff1f1f1)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0x7fe3e3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0x7fc7c7c7)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0x7f8f8f8f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0x7f1f1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x7f3f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0x7cfcfcff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x78f8f8ff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x70f00000)), unchecked((int)(0xf0f8f8fc)), unchecked((int)(0x60e00000
			)), unchecked((int)(0xe0f0f0f8)), unchecked((int)(0x40400000)), unchecked((int)(
			0xe0e0f0)), unchecked((int)(0x0)), unchecked((int)(0xc0e0)), unchecked((int)(0x0
			)), unchecked((int)(0xc0)), unchecked((int)(0x0)), unchecked((int)(0xc0)), unchecked(
			(int)(0x7cfcffff)), unchecked((int)(0xfffffffc)), unchecked((int)(0x78f8ffff)), 
			unchecked((int)(0xfffffff8)), unchecked((int)(0x70000000)), unchecked((int)(0xf0f0f8f0
			)), unchecked((int)(0x60000000)), unchecked((int)(0xe0e0f0e0)), unchecked((int)(
			0x40000000)), unchecked((int)(0x406040)), unchecked((int)(0x0)), unchecked((int)
			(0x4000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x7cffffff)), unchecked((int)(0xfffffcfc)
			), unchecked((int)(0x78ffffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0x0
			)), unchecked((int)(0xf0f0f0f0)), unchecked((int)(0x0)), unchecked((int)(0xe0e0e0e0
			)), unchecked((int)(0x0)), unchecked((int)(0x404040)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x7fffffff)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0x7fffffff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(
			0x0)), unchecked((int)(0xf0f0f0f0)), unchecked((int)(0x0)), unchecked((int)(0xe0e0e000
			)), unchecked((int)(0x0)), unchecked((int)(0x404000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x7fffffff)), unchecked((int)(0xfcfcfcff
			)), unchecked((int)(0x7fffffff)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(
			0x0)), unchecked((int)(0xf0f0f0f0)), unchecked((int)(0x0)), unchecked((int)(0xe0e0e000
			)), unchecked((int)(0x0)), unchecked((int)(0x400000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x7fffffff)), unchecked((int)(0xfcfcffff
			)), unchecked((int)(0x7fffffff)), unchecked((int)(0xf8f8ffff)), unchecked((int)(
			0x0)), unchecked((int)(0xf0f0f0f0)), unchecked((int)(0x0)), unchecked((int)(0xe0e0e000
			)), unchecked((int)(0x0)), unchecked((int)(0x40400000)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((
			int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x7ffffcfc)), unchecked((int)
			(0xffffffff)), unchecked((int)(0x7ffff8f8)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0x7ffff1f1)), unchecked((int)(0xffffffff)), unchecked((int)(0x7fffe3e3)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0x7fffc7c7)), unchecked((int)(0xffffffff
			)), unchecked((int)(0x7fff8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x7fff1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(0x7fff3f3f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x7ffcfcfc)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x7ff8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0x7ff1f1f1
			)), unchecked((int)(0xffffffff)), unchecked((int)(0x7fe3e3e3)), unchecked((int)(
			0xffffffff)), unchecked((int)(0x7fc7c7c7)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0x7f8f8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0x7f1f1f1f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0x7f3f3f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0x7cfcfcff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x78f8f8ff)), unchecked((int)(0xffffffff)), unchecked((int)(0x71f1f1ff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x63e3e3ff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x47c7c7ff)), unchecked((int)(0xffffffff)), unchecked((int)(0xf8f8fff
			)), unchecked((int)(0xffffffff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(
			0xffffffff)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0x7cfcffff)), unchecked((int)(0xfffffffc)), unchecked((int)(0x78f8ffff)), 
			unchecked((int)(0xfffffff8)), unchecked((int)(0x71f1ffff)), unchecked((int)(0xfffffff1
			)), unchecked((int)(0x60e00000)), unchecked((int)(0xf0f8f8e0)), unchecked((int)(
			0x40c00000)), unchecked((int)(0xe0f0f0c0)), unchecked((int)(0x0)), unchecked((int
			)(0xe0e080)), unchecked((int)(0x0)), unchecked((int)(0xc000)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x7cffffff)), unchecked((int)(0xfffffcfc
			)), unchecked((int)(0x78ffffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(
			0x71ffffff)), unchecked((int)(0xfffff1f1)), unchecked((int)(0x60000000)), unchecked(
			(int)(0xf0f0e0e0)), unchecked((int)(0x40000000)), unchecked((int)(0xe0e0c0c0)), 
			unchecked((int)(0x0)), unchecked((int)(0x400000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x7fffffff
			)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0x7fffffff)), unchecked((int)(
			0xfff8f8f8)), unchecked((int)(0x7fffffff)), unchecked((int)(0xfff1f1f1)), unchecked(
			(int)(0x0)), unchecked((int)(0xf0e0e0e0)), unchecked((int)(0x0)), unchecked((int
			)(0xe0c0c0c0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x7fffffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0x7fffffff)), 
			unchecked((int)(0xf8f8f8ff)), unchecked((int)(0x7fffffff)), unchecked((int)(0xf1f1f1ff
			)), unchecked((int)(0x0)), unchecked((int)(0xe0e0e0f0)), unchecked((int)(0x0)), 
			unchecked((int)(0xc0c0c000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x7fffffff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0x7fffffff
			)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0x7fffffff)), unchecked((int)(
			0xf1f1ffff)), unchecked((int)(0x0)), unchecked((int)(0xe0e0f0f0)), unchecked((int
			)(0x0)), unchecked((int)(0xc0c0e000)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0xbffffcfc)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xbffff8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xbffff1f1)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xbfffe3e3)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xbfffc7c7)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xbfff8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xbfff1f1f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xbfff3f3f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xbffcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xbff8f8f8
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xbff1f1f1)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xbfe3e3e3)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xbfc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xbf8f8f8f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xbf1f1f1f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xbf3f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xbcfcfcff)), unchecked((int)(0xffffffff)), unchecked((int)(0xb8f8f8f0)), unchecked(
			(int)(0xf8fcfcfe)), unchecked((int)(0xb0f0e000)), unchecked((int)(0xf0f8f8fc)), 
			unchecked((int)(0xa0e0e000)), unchecked((int)(0xf0f0f8)), unchecked((int)(0x80c00000
			)), unchecked((int)(0xe0f0)), unchecked((int)(0x80800000)), unchecked((int)(0xe0
			)), unchecked((int)(0x0)), unchecked((int)(0xe0)), unchecked((int)(0x20200000)), 
			unchecked((int)(0xe0)), unchecked((int)(0xbcfcffff)), unchecked((int)(0xfffffffc
			)), unchecked((int)(0xb8f8f8f0)), unchecked((int)(0xf8f8fcf8)), unchecked((int)(
			0xb0f0e000)), unchecked((int)(0xf0f0f8f0)), unchecked((int)(0xa0c00000)), unchecked(
			(int)(0xe0f0e0)), unchecked((int)(0x80c00000)), unchecked((int)(0xe0c0)), unchecked(
			(int)(0x80000000)), unchecked((int)(0x80)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x20000000)), unchecked((int)(0x20)), unchecked((int)(0xbcffffff
			)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xb8f8f8f0)), unchecked((int)(
			0xf8f8f8f8)), unchecked((int)(0xb0f0e000)), unchecked((int)(0xf0f0f0f0)), unchecked(
			(int)(0xa0c00000)), unchecked((int)(0xe0e0e0)), unchecked((int)(0x80000000)), unchecked(
			(int)(0xc0c0)), unchecked((int)(0x0)), unchecked((int)(0x80)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x20)), unchecked(
			(int)(0xbfffffff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xb8f8f8f0)), 
			unchecked((int)(0xf8f8f8f8)), unchecked((int)(0xb0f0e000)), unchecked((int)(0xf0f0f0f0
			)), unchecked((int)(0xa0c00000)), unchecked((int)(0xe0e0e0)), unchecked((int)(0x80000000
			)), unchecked((int)(0xc0c0c0)), unchecked((int)(0x0)), unchecked((int)(0x8080)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((
			int)(0x2020)), unchecked((int)(0xbfffffff)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xb8f8f8f0)), unchecked((int)(0xf8f8f8fa)), unchecked((int)(0xb0f0e000)), 
			unchecked((int)(0xf0f0f0f4)), unchecked((int)(0xa0c00000)), unchecked((int)(0xe0e0e0e8
			)), unchecked((int)(0x80000000)), unchecked((int)(0xc0c0d0)), unchecked((int)(0x0
			)), unchecked((int)(0x8080a0)), unchecked((int)(0x0)), unchecked((int)(0x40)), unchecked(
			(int)(0x0)), unchecked((int)(0x2020a0)), unchecked((int)(0xbfffffff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xb8f8f8f0)), unchecked((int)(0xf8f8fefa)), 
			unchecked((int)(0xb0f0e000)), unchecked((int)(0xf0f0fcf0)), unchecked((int)(0xa0c00000
			)), unchecked((int)(0xe0e0f8e0)), unchecked((int)(0x80000000)), unchecked((int)(
			0xc0c0f0c0)), unchecked((int)(0x0)), unchecked((int)(0x8080e080)), unchecked((int
			)(0x0)), unchecked((int)(0xe000)), unchecked((int)(0x0)), unchecked((int)(0x2020e020
			)), unchecked((int)(0xbffffcfc)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xbffff8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xbffff1f1)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xbfffe3e3)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xbfffc7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xbfff8f8f
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xbfff1f1f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xbfff3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xbffcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xbff8f8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xbff1f1f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xbfe3e3e3)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xbfc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xbf8f8f8f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xbf1f1f1f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xbf3f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xbcfcfcff
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xb8f8f8ff)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xb1f1f1ff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xa3e3e3ff)), unchecked((int)(0xffffffff)), unchecked((int)(0x87c7c7ff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0x8f8f8fff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0x1f1f1fff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x3f3f3fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xbcfcffff)), unchecked(
			(int)(0xfffffffc)), unchecked((int)(0xb8f8ffff)), unchecked((int)(0xfffffff8)), 
			unchecked((int)(0xb0f0f8f0)), unchecked((int)(0xf8fcfcf0)), unchecked((int)(0xa0e0e000
			)), unchecked((int)(0xf0f8f8e0)), unchecked((int)(0x80c0e000)), unchecked((int)(
			0xf0f0c0)), unchecked((int)(0x80800000)), unchecked((int)(0xe080)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x30300000)), unchecked((int)(0xf030
			)), unchecked((int)(0xbcffffff)), unchecked((int)(0xfffffcfc)), unchecked((int)(
			0xb8ffffff)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xb0f8f8f0)), unchecked(
			(int)(0xf8f8f0f0)), unchecked((int)(0xa0f0e000)), unchecked((int)(0xf0f0e0e0)), 
			unchecked((int)(0x80c00000)), unchecked((int)(0xe0c0c0)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((
			int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xbfffffff)), unchecked((int)
			(0xfffcfcfc)), unchecked((int)(0xbfffffff)), unchecked((int)(0xfff8f8f8)), unchecked(
			(int)(0xb8f8f8f0)), unchecked((int)(0xf8f0f0f0)), unchecked((int)(0xb0f0e000)), 
			unchecked((int)(0xf0e0e0e0)), unchecked((int)(0xa0c00000)), unchecked((int)(0xe0c0c0c0
			)), unchecked((int)(0x80000000)), unchecked((int)(0x808080)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x30000000)), unchecked((int)(0x303030
			)), unchecked((int)(0xbfffffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(
			0xbfffffff)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xbef8f8f0)), unchecked(
			(int)(0xf0f0f0fa)), unchecked((int)(0xbcf0e000)), unchecked((int)(0xe0e0e0f4)), 
			unchecked((int)(0xb8c00000)), unchecked((int)(0xc0c0c0e8)), unchecked((int)(0xb0000000
			)), unchecked((int)(0x808080d0)), unchecked((int)(0xa0000000)), unchecked((int)(
			0xa0)), unchecked((int)(0xb0000000)), unchecked((int)(0x30303070)), unchecked((int
			)(0xbfffffff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xbfffffff)), unchecked(
			(int)(0xf8f8ffff)), unchecked((int)(0xb8f8f8f0)), unchecked((int)(0xf0f0fefe)), 
			unchecked((int)(0xb0f0e000)), unchecked((int)(0xe0e0fcfc)), unchecked((int)(0xa0c00000
			)), unchecked((int)(0xc0c0f8f8)), unchecked((int)(0x80000000)), unchecked((int)(
			0x8080f0f0)), unchecked((int)(0x0)), unchecked((int)(0x10f0f0)), unchecked((int)
			(0x30000000)), unchecked((int)(0x3030f0f0)), unchecked((int)(0xdffffcfc)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xdffff8f8)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xdffff1f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xdfffe3e3
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xdfffc7c7)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xdfff8f8f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xdfff1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(0xdfff3f3f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xdffcfcfc)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xdff8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xdff1f1f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xdfe3e3e3)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xdfc7c7c7)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xdf8f8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xdf1f1f1f
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xdf3f3f3f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xdcfcfcf8)), unchecked((int)(0xfcfefeff)), unchecked(
			(int)(0xd8f8f000)), unchecked((int)(0xf8fcfcfe)), unchecked((int)(0xd0f0f000)), 
			unchecked((int)(0xf8f8fc)), unchecked((int)(0xc0e00000)), unchecked((int)(0xf0f8
			)), unchecked((int)(0x40400000)), unchecked((int)(0x70)), unchecked((int)(0x0)), 
			unchecked((int)(0x70)), unchecked((int)(0x10100000)), unchecked((int)(0x70)), unchecked(
			(int)(0x18380000)), unchecked((int)(0x78f8)), unchecked((int)(0xdcfcfcf8)), unchecked(
			(int)(0xfcfcfefc)), unchecked((int)(0xd8f8f000)), unchecked((int)(0xf8f8fcf8)), 
			unchecked((int)(0xd0e00000)), unchecked((int)(0xf0f8f0)), unchecked((int)(0xc0e00000
			)), unchecked((int)(0xf0e0)), unchecked((int)(0x40000000)), unchecked((int)(0x40
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x10000000)), 
			unchecked((int)(0x10)), unchecked((int)(0x18380000)), unchecked((int)(0x7838)), 
			unchecked((int)(0xdcfcfcf8)), unchecked((int)(0xfcfcfcfc)), unchecked((int)(0xd8f8f000
			)), unchecked((int)(0xf8f8f8f8)), unchecked((int)(0xd0e00000)), unchecked((int)(
			0xf0f0f0)), unchecked((int)(0xc0000000)), unchecked((int)(0xe0e0)), unchecked((int
			)(0x0)), unchecked((int)(0x40)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x10)), unchecked((int)(0x18000000)), unchecked((int
			)(0x3838)), unchecked((int)(0xdcfcfcf8)), unchecked((int)(0xfcfcfcfc)), unchecked(
			(int)(0xd8f8f000)), unchecked((int)(0xf8f8f8f8)), unchecked((int)(0xd0e00000)), 
			unchecked((int)(0xf0f0f0)), unchecked((int)(0xc0000000)), unchecked((int)(0xe0e0e0
			)), unchecked((int)(0x0)), unchecked((int)(0x4040)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1010)), unchecked((int)(0x18000000
			)), unchecked((int)(0x383838)), unchecked((int)(0xdcfcfcf8)), unchecked((int)(0xfcfcfcfd
			)), unchecked((int)(0xd8f8f000)), unchecked((int)(0xf8f8f8fa)), unchecked((int)(
			0xd0e00000)), unchecked((int)(0xf0f0f0f4)), unchecked((int)(0xc0000000)), unchecked(
			(int)(0xe0e0e8)), unchecked((int)(0x0)), unchecked((int)(0x404050)), unchecked((
			int)(0x0)), unchecked((int)(0x20)), unchecked((int)(0x0)), unchecked((int)(0x101050
			)), unchecked((int)(0x18000000)), unchecked((int)(0x3838b8)), unchecked((int)(0xdcfcfcf8
			)), unchecked((int)(0xfcfcfffd)), unchecked((int)(0xd8f8f000)), unchecked((int)(
			0xf8f8fef8)), unchecked((int)(0xd0e00000)), unchecked((int)(0xf0f0fcf0)), unchecked(
			(int)(0xc0000000)), unchecked((int)(0xe0e0f8e0)), unchecked((int)(0x0)), unchecked(
			(int)(0x40407040)), unchecked((int)(0x0)), unchecked((int)(0x7000)), unchecked((
			int)(0x0)), unchecked((int)(0x10107010)), unchecked((int)(0x18000000)), unchecked(
			(int)(0x3838f838)), unchecked((int)(0xdffffcfc)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xdffff8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xdffff1f1
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xdfffe3e3)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xdfffc7c7)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xdfff8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xdfff1f1f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xdfff3f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xdffcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xdff8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xdff1f1f1)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xdfe3e3e3)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xdfc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xdf8f8f8f
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xdf1f1f1f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xdf3f3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xdcfcfcff)), unchecked((int)(0xffffffff)), unchecked((int)(0xd8f8f8ff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xd1f1f1ff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xc3e3e3ff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xc7c7c7ff)), unchecked((int)(0xffffffff)), unchecked((int)(0x8f8f8fff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0x1f1f1fff)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x1f3f3fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xdcfcffff
			)), unchecked((int)(0xfffffffc)), unchecked((int)(0xd8f8fcf8)), unchecked((int)(
			0xfcfefef8)), unchecked((int)(0xd0f0f000)), unchecked((int)(0xf8fcfcf0)), unchecked(
			(int)(0xc0e0f000)), unchecked((int)(0xf8f8e0)), unchecked((int)(0xc0c00000)), unchecked(
			(int)(0xf0c0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x18180000
			)), unchecked((int)(0x7818)), unchecked((int)(0x1c3c7c00)), unchecked((int)(0xfcfc3c
			)), unchecked((int)(0xdcffffff)), unchecked((int)(0xfffffcfc)), unchecked((int)(
			0xd8fcfcf8)), unchecked((int)(0xfcfcf8f8)), unchecked((int)(0xd0f8f000)), unchecked(
			(int)(0xf8f8f0f0)), unchecked((int)(0xc0e00000)), unchecked((int)(0xf0e0e0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1c3c0000)), unchecked(
			(int)(0x7c3c3c)), unchecked((int)(0xdfffffff)), unchecked((int)(0xfffcfcfc)), unchecked(
			(int)(0xdcfcfcf8)), unchecked((int)(0xfcf8f8f8)), unchecked((int)(0xd8f8f000)), 
			unchecked((int)(0xf8f0f0f0)), unchecked((int)(0xd0e00000)), unchecked((int)(0xf0e0e0e0
			)), unchecked((int)(0xc0000000)), unchecked((int)(0xc0c0c0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x18000000)), unchecked((int)(0x181818
			)), unchecked((int)(0x5c3c0000)), unchecked((int)(0x7c3c3c3c)), unchecked((int)(
			0xdfffffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xdffcfcf8)), unchecked(
			(int)(0xf8f8f8fd)), unchecked((int)(0xdef8f000)), unchecked((int)(0xf0f0f0fa)), 
			unchecked((int)(0xdce00000)), unchecked((int)(0xe0e0e0f4)), unchecked((int)(0xd8000000
			)), unchecked((int)(0xc0c0c0e8)), unchecked((int)(0x50000000)), unchecked((int)(
			0x50)), unchecked((int)(0xd8000000)), unchecked((int)(0x181818b8)), unchecked((int
			)(0xdc3c0000)), unchecked((int)(0x3c3c3c7c)), unchecked((int)(0xdfffffff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xdcfcfcf8)), unchecked((int)(0xf8f8ffff)), 
			unchecked((int)(0xd8f8f000)), unchecked((int)(0xf0f0fefe)), unchecked((int)(0xd0e00000
			)), unchecked((int)(0xe0e0fcfc)), unchecked((int)(0xc0000000)), unchecked((int)(
			0xc0c0f8f8)), unchecked((int)(0x0)), unchecked((int)(0x88f8f8)), unchecked((int)
			(0x18000000)), unchecked((int)(0x1818f8f8)), unchecked((int)(0x5c3c0000)), unchecked(
			(int)(0x3c3cfcfc)), unchecked((int)(0xeffffcfc)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xeffff8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xeffff1f1
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xefffe3e3)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xefffc7c7)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xefff8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xefff1f1f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xefff3f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xeffcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xeff8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xeff1f1f1)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xefe3e3e3)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xefc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xef8f8f8f
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xef1f1f1f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xef3f3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xecfcf800)), unchecked((int)(0xfcfefeff)), unchecked((int)(0xe8f8f800)), 
			unchecked((int)(0xfcfcfe)), unchecked((int)(0x60700000)), unchecked((int)(0x787c
			)), unchecked((int)(0x20200000)), unchecked((int)(0x38)), unchecked((int)(0x0)), 
			unchecked((int)(0x38)), unchecked((int)(0x8080000)), unchecked((int)(0x38)), unchecked(
			(int)(0xc1c0000)), unchecked((int)(0x3c7c)), unchecked((int)(0x2e3e3e00)), unchecked(
			(int)(0x7e7efe)), unchecked((int)(0xecfcf800)), unchecked((int)(0xfcfcfefc)), unchecked(
			(int)(0xe8f00000)), unchecked((int)(0xf8fcf8)), unchecked((int)(0x60700000)), unchecked(
			(int)(0x7870)), unchecked((int)(0x20000000)), unchecked((int)(0x20)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x8000000)), unchecked((int)
			(0x8)), unchecked((int)(0xc1c0000)), unchecked((int)(0x3c1c)), unchecked((int)(0x2e1e0000
			)), unchecked((int)(0x3e7e3e)), unchecked((int)(0xecfcf800)), unchecked((int)(0xfcfcfcfc
			)), unchecked((int)(0xe8f00000)), unchecked((int)(0xf8f8f8)), unchecked((int)(0x60000000
			)), unchecked((int)(0x7070)), unchecked((int)(0x0)), unchecked((int)(0x20)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x8))
			, unchecked((int)(0xc000000)), unchecked((int)(0x1c1c)), unchecked((int)(0x2e1e0000
			)), unchecked((int)(0x3e3e3e)), unchecked((int)(0xecfcf800)), unchecked((int)(0xfcfcfcfc
			)), unchecked((int)(0xe8f00000)), unchecked((int)(0xf8f8f8)), unchecked((int)(0x60000000
			)), unchecked((int)(0x707070)), unchecked((int)(0x0)), unchecked((int)(0x2020)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((
			int)(0x808)), unchecked((int)(0xc000000)), unchecked((int)(0x1c1c1c)), unchecked(
			(int)(0x2e1e0000)), unchecked((int)(0x3e3e3e)), unchecked((int)(0xecfcf800)), unchecked(
			(int)(0xfcfcfcfd)), unchecked((int)(0xe8f00000)), unchecked((int)(0xf8f8f8fa)), 
			unchecked((int)(0x60000000)), unchecked((int)(0x707074)), unchecked((int)(0x0)), 
			unchecked((int)(0x202028)), unchecked((int)(0x0)), unchecked((int)(0x10)), unchecked(
			(int)(0x0)), unchecked((int)(0x80828)), unchecked((int)(0xc000000)), unchecked((
			int)(0x1c1c5c)), unchecked((int)(0x2e1e0000)), unchecked((int)(0x3e3e3ebe)), unchecked(
			(int)(0xecfcf800)), unchecked((int)(0xfcfcfffc)), unchecked((int)(0xe8f00000)), 
			unchecked((int)(0xf8f8fef8)), unchecked((int)(0x60000000)), unchecked((int)(0x70707c70
			)), unchecked((int)(0x0)), unchecked((int)(0x20203820)), unchecked((int)(0x0)), 
			unchecked((int)(0x3800)), unchecked((int)(0x0)), unchecked((int)(0x8083808)), unchecked(
			(int)(0xc000000)), unchecked((int)(0x1c1c7c1c)), unchecked((int)(0x2e1e0000)), unchecked(
			(int)(0x3e3efe3e)), unchecked((int)(0xeffffcfc)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xeffff8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xeffff1f1
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xefffe3e3)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xefffc7c7)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xefff8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xefff1f1f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xefff3f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xeffcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xeff8f8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xeff1f1f1)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xefe3e3e3)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xefc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xef8f8f8f
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xef1f1f1f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xef3f3f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xecfcfcff)), unchecked((int)(0xffffffff)), unchecked((int)(0xe8f8f8ff)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xe1f1f1ff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xe3e3e3ff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xc7c7c7ff)), unchecked((int)(0xffffffff)), unchecked((int)(0x8f8f8fff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf1f1fff)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0x2f3f3fff)), unchecked((int)(0xffffffff)), unchecked((int)(0xecfcfefc)), 
			unchecked((int)(0xfefffffc)), unchecked((int)(0xe8f8f800)), unchecked((int)(0xfcfefef8
			)), unchecked((int)(0xe0f0f800)), unchecked((int)(0xfcfcf0)), unchecked((int)(0x60600000
			)), unchecked((int)(0x7860)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xc0c0000)), unchecked((int)(0x3c0c)), unchecked((int)(0xe1e3e00)), unchecked(
			(int)(0x7e7e1e)), unchecked((int)(0x2f3f3f00)), unchecked((int)(0x7fffff3f)), unchecked(
			(int)(0xecfefefc)), unchecked((int)(0xfefefcfc)), unchecked((int)(0xe8fcf800)), 
			unchecked((int)(0xfcfcf8f8)), unchecked((int)(0xe0f00000)), unchecked((int)(0xf8f0f0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xe1e0000
			)), unchecked((int)(0x3e1e1e)), unchecked((int)(0x2f7f3f00)), unchecked((int)(0x7f7f3f3f
			)), unchecked((int)(0xeefefefc)), unchecked((int)(0xfefcfcfc)), unchecked((int)(
			0xecfcf800)), unchecked((int)(0xfcf8f8f8)), unchecked((int)(0xe8f00000)), unchecked(
			(int)(0xf8f0f0f0)), unchecked((int)(0x60000000)), unchecked((int)(0x606060)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xc000000)), unchecked((int)
			(0xc0c0c)), unchecked((int)(0x2e1e0000)), unchecked((int)(0x3e1e1e1e)), unchecked(
			(int)(0x6f7f3f00)), unchecked((int)(0x7f3f3f3f)), unchecked((int)(0xeffefefc)), 
			unchecked((int)(0xfcfcfcfe)), unchecked((int)(0xeffcf800)), unchecked((int)(0xf8f8f8fd
			)), unchecked((int)(0xeef00000)), unchecked((int)(0xf0f0f0fa)), unchecked((int)(
			0x6c000000)), unchecked((int)(0x60606074)), unchecked((int)(0x28000000)), unchecked(
			(int)(0x28)), unchecked((int)(0x6c000000)), unchecked((int)(0xc0c0c5c)), unchecked(
			(int)(0xee1e0000)), unchecked((int)(0x1e1e1ebe)), unchecked((int)(0xef7f3f00)), 
			unchecked((int)(0x3f3f3f7f)), unchecked((int)(0xeefefefc)), unchecked((int)(0xfcfcffff
			)), unchecked((int)(0xecfcf800)), unchecked((int)(0xf8f8ffff)), unchecked((int)(
			0xe8f00000)), unchecked((int)(0xf0f0fefe)), unchecked((int)(0x60000000)), unchecked(
			(int)(0x60607c7c)), unchecked((int)(0x0)), unchecked((int)(0x447c7c)), unchecked(
			(int)(0xc000000)), unchecked((int)(0xc0c7c7c)), unchecked((int)(0x2e1e0000)), unchecked(
			(int)(0x1e1efefe)), unchecked((int)(0x6f7f3f00)), unchecked((int)(0x3f3fffff)), 
			unchecked((int)(0xff7ffcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xff7ff8f8
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xff7ff1f1)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff7fe3e3)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff7fc7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xff7f8f8f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff7f1f1f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xff7f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xff7cfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xfe78f800)), unchecked(
			(int)(0xf8f8fcfc)), unchecked((int)(0xfc70f000)), unchecked((int)(0xf0f0f8f8)), 
			unchecked((int)(0xf860e000)), unchecked((int)(0xe0f0f0)), unchecked((int)(0xf0404000
			)), unchecked((int)(0xe0e0)), unchecked((int)(0xe0000000)), unchecked((int)(0xc0
			)), unchecked((int)(0xc0000000)), unchecked((int)(0x0)), unchecked((int)(0xc0000000
			)), unchecked((int)(0x0)), unchecked((int)(0xfc7cfcff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8780000)), unchecked((int)(0xf8f8f8fc)), unchecked((int)(
			0xf0700000)), unchecked((int)(0xf0f0f0f8)), unchecked((int)(0xe0600000)), unchecked(
			(int)(0xe0e0f0)), unchecked((int)(0x40400000)), unchecked((int)(0x4060)), unchecked(
			(int)(0x0)), unchecked((int)(0x40)), unchecked((int)(0x0)), unchecked((int)(0x0)
			), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfc7cffff)), unchecked(
			(int)(0xfffffffc)), unchecked((int)(0xf8000000)), unchecked((int)(0xf8f8f8f8)), 
			unchecked((int)(0xf0000000)), unchecked((int)(0xf0f0f0f0)), unchecked((int)(0xe0000000
			)), unchecked((int)(0xe0e0e0)), unchecked((int)(0x40000000)), unchecked((int)(0x4040
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfc7fffff
			)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xf8000000)), unchecked((int)(
			0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)), unchecked((int
			)(0x0)), unchecked((int)(0xe0e0e0)), unchecked((int)(0x0)), unchecked((int)(0x4040
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff7fffff
			)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xf8000000)), unchecked((int)(
			0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)), unchecked((int
			)(0x0)), unchecked((int)(0xe0e000)), unchecked((int)(0x0)), unchecked((int)(0x4000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff7fffff
			)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xf8000000)), unchecked((int)(
			0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)), unchecked((int
			)(0x0)), unchecked((int)(0xe0e000)), unchecked((int)(0x0)), unchecked((int)(0x400000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff7fffff
			)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xf8000000)), unchecked((int)(
			0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)), unchecked((int
			)(0x0)), unchecked((int)(0xe0e0e000)), unchecked((int)(0x0)), unchecked((int)(0x40400000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff7ffcfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xff7ff8f8)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff7ff1f1)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff7fe3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xff7fc7c7)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff7f8f8f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xff7f1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xff7f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xff7cfcfc)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xff78f8f8)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xff71f1f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xff63e3e3
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xff47c7c7)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff0f8f8f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff1f1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(0xff3f3f3f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfc7cfcff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf878f8ff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf070f000)), unchecked((int)(0xf8f8fcfc)), unchecked((int)(0xe060e000)), unchecked(
			(int)(0xf0f0f8f8)), unchecked((int)(0xc040c000)), unchecked((int)(0xe0f0f0)), unchecked(
			(int)(0x80000000)), unchecked((int)(0xe0e0)), unchecked((int)(0x0)), unchecked((
			int)(0xc0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfc7cffff
			)), unchecked((int)(0xfffffffc)), unchecked((int)(0xf878ffff)), unchecked((int)(
			0xfffffff8)), unchecked((int)(0xf0700000)), unchecked((int)(0xf8f8f8f0)), unchecked(
			(int)(0xe0600000)), unchecked((int)(0xf0f0f0e0)), unchecked((int)(0xc0400000)), 
			unchecked((int)(0xe0e0c0)), unchecked((int)(0x0)), unchecked((int)(0x4000)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0xfc7fffff)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xf87fffff
			)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xf0000000)), unchecked((int)(
			0xf8f8f0f0)), unchecked((int)(0xe0000000)), unchecked((int)(0xf0f0e0e0)), unchecked(
			(int)(0xc0000000)), unchecked((int)(0xe0c0c0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0xff7fffff)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0xff7fffff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(
			0xf8000000)), unchecked((int)(0xf8f0f0f0)), unchecked((int)(0x0)), unchecked((int
			)(0xf0e0e0e0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff7fffff)), unchecked((int
			)(0xfcfcfcff)), unchecked((int)(0xff7fffff)), unchecked((int)(0xf8f8f8ff)), unchecked(
			(int)(0xf8000000)), unchecked((int)(0xf0f0f0f8)), unchecked((int)(0x0)), unchecked(
			(int)(0xe0e0e0f0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c000)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff7fffff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xff7fffff)), unchecked((int)(0xf8f8ffff)), 
			unchecked((int)(0xf8000000)), unchecked((int)(0xf0f0f8f8)), unchecked((int)(0x0)
			), unchecked((int)(0xe0e0f0f0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0e000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffbffcfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffbff8f8)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffbff1f1)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffbfe3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xffbfc7c7)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffbf8f8f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffbf1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffbf3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xffbcfcfc)), unchecked(
			(int)(0xfcfcfefe)), unchecked((int)(0xfeb8f8f8)), unchecked((int)(0xf8f8fcfc)), 
			unchecked((int)(0xfcb0f0e0)), unchecked((int)(0xf0f8f8)), unchecked((int)(0xf8a0e0e0
			)), unchecked((int)(0xf0f0)), unchecked((int)(0xf080c000)), unchecked((int)(0xe0
			)), unchecked((int)(0xe0808000)), unchecked((int)(0x0)), unchecked((int)(0xe0000000
			)), unchecked((int)(0x0)), unchecked((int)(0xe0202000)), unchecked((int)(0x0)), 
			unchecked((int)(0xfcbcfcfc)), unchecked((int)(0xfcfcfcfe)), unchecked((int)(0xf8b8f8f8
			)), unchecked((int)(0xf8f8f8fc)), unchecked((int)(0xf0b0f0e0)), unchecked((int)(
			0xf0f0f8)), unchecked((int)(0xe0a0c000)), unchecked((int)(0xe0f0)), unchecked((int
			)(0xc080c000)), unchecked((int)(0xe0)), unchecked((int)(0x80800000)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x20200000
			)), unchecked((int)(0x0)), unchecked((int)(0xfcbcfcfc)), unchecked((int)(0xfcfcfcfc
			)), unchecked((int)(0xf8b8f8f8)), unchecked((int)(0xf8f8f8f8)), unchecked((int)(
			0xf0b0f0e0)), unchecked((int)(0xf0f0f0)), unchecked((int)(0xe0a0c000)), unchecked(
			(int)(0xe0e0)), unchecked((int)(0xc0800000)), unchecked((int)(0xc0)), unchecked(
			(int)(0x80000000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x20000000)), unchecked((int)(0x0)), unchecked((int)(0xfcbcfcfc
			)), unchecked((int)(0xfcfcfcfc)), unchecked((int)(0xf8b8f8f8)), unchecked((int)(
			0xf8f8f8f8)), unchecked((int)(0xf0b0f0e0)), unchecked((int)(0xf0f0f0)), unchecked(
			(int)(0xe0a0c000)), unchecked((int)(0xe0e0)), unchecked((int)(0xc0800000)), unchecked(
			(int)(0xc0c0)), unchecked((int)(0x80000000)), unchecked((int)(0x80)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x20000000)), unchecked((int
			)(0x20)), unchecked((int)(0xfdbcfcfc)), unchecked((int)(0xfcfcfcfc)), unchecked(
			(int)(0xfab8f8f8)), unchecked((int)(0xf8f8f8f8)), unchecked((int)(0xf4b0f0e0)), 
			unchecked((int)(0xf0f0f0)), unchecked((int)(0xe8a0c000)), unchecked((int)(0xe0e0e0
			)), unchecked((int)(0xd0800000)), unchecked((int)(0xc0c0)), unchecked((int)(0xa0000000
			)), unchecked((int)(0x8080)), unchecked((int)(0x40000000)), unchecked((int)(0x0)
			), unchecked((int)(0xa0000000)), unchecked((int)(0x2020)), unchecked((int)(0xfdbcfcfc
			)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xf8b8f8f8)), unchecked((int)(
			0xf8f8f8fe)), unchecked((int)(0xf0b0f0e0)), unchecked((int)(0xf0f0f0fc)), unchecked(
			(int)(0xe0a0c000)), unchecked((int)(0xe0e0f8)), unchecked((int)(0xc0800000)), unchecked(
			(int)(0xc0c0f0)), unchecked((int)(0x80000000)), unchecked((int)(0x8080e0)), unchecked(
			(int)(0x0)), unchecked((int)(0xe0)), unchecked((int)(0x20000000)), unchecked((int
			)(0x2020e0)), unchecked((int)(0xffbcfcfc)), unchecked((int)(0xfcfcffff)), unchecked(
			(int)(0xfeb8f8f8)), unchecked((int)(0xf8f8fefe)), unchecked((int)(0xfcb0f0e0)), 
			unchecked((int)(0xf0f0fcfc)), unchecked((int)(0xf8a0c000)), unchecked((int)(0xe0e0f8f8
			)), unchecked((int)(0xf0800000)), unchecked((int)(0xc0c0f0f0)), unchecked((int)(
			0xf0000000)), unchecked((int)(0x8080f0f0)), unchecked((int)(0xf0000000)), unchecked(
			(int)(0xf0f0)), unchecked((int)(0xf0000000)), unchecked((int)(0x2020f0f0)), unchecked(
			(int)(0xffbffcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffbff8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffbff1f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffbfe3e3)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffbfc7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xffbf8f8f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffbf1f1f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffbf3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xffbcfcfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffb8f8f8)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffb1f1f1)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffa3e3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xff87c7c7)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xff3f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcbcfcff)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xf8b8f8fc)), unchecked((int)(0xfcfcfefe)), 
			unchecked((int)(0xf0b0f0f8)), unchecked((int)(0xf8f8fcfc)), unchecked((int)(0xe0a0e0e0
			)), unchecked((int)(0xf0f8f8)), unchecked((int)(0xc080c0e0)), unchecked((int)(0xf0f0
			)), unchecked((int)(0x80808000)), unchecked((int)(0xe0)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x30303000)), unchecked((int)(0xf0)), unchecked(
			(int)(0xfcbcffff)), unchecked((int)(0xfffffffc)), unchecked((int)(0xf8b8fcfc)), 
			unchecked((int)(0xfcfcfcf8)), unchecked((int)(0xf0b0f8f8)), unchecked((int)(0xf8f8f8f0
			)), unchecked((int)(0xe0a0f0e0)), unchecked((int)(0xf0f0e0)), unchecked((int)(0xc080c000
			)), unchecked((int)(0xe0c0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0xfcbfffff)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xf8bcfcfc
			)), unchecked((int)(0xfcfcf8f8)), unchecked((int)(0xf0b8f8f8)), unchecked((int)(
			0xf8f8f0f0)), unchecked((int)(0xe0b0f0e0)), unchecked((int)(0xf0e0e0)), unchecked(
			(int)(0xc0a0c000)), unchecked((int)(0xe0c0c0)), unchecked((int)(0x80800000)), unchecked(
			(int)(0x8080)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x30300000
			)), unchecked((int)(0x3030)), unchecked((int)(0xffbfffff)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0xfdbffcfc)), unchecked((int)(0xfcf8f8f8)), unchecked((int)(
			0xfabef8f8)), unchecked((int)(0xf8f0f0f0)), unchecked((int)(0xf4bcf0e0)), unchecked(
			(int)(0xf0e0e0e0)), unchecked((int)(0xe8b8c000)), unchecked((int)(0xc0c0c0)), unchecked(
			(int)(0xd0b00000)), unchecked((int)(0x808080)), unchecked((int)(0xa0a00000)), unchecked(
			(int)(0x0)), unchecked((int)(0x70b00000)), unchecked((int)(0x303030)), unchecked(
			(int)(0xffbfffff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffbcfcfc)), 
			unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xfeb8f8f8)), unchecked((int)(0xf0f0f0fe
			)), unchecked((int)(0xfcb0f0e0)), unchecked((int)(0xe0e0e0fc)), unchecked((int)(
			0xf8a0c000)), unchecked((int)(0xc0c0c0f8)), unchecked((int)(0xf0800000)), unchecked(
			(int)(0x808080f0)), unchecked((int)(0xf0000000)), unchecked((int)(0x10f0)), unchecked(
			(int)(0xf0300000)), unchecked((int)(0x303030f0)), unchecked((int)(0xffbfffff)), 
			unchecked((int)(0xfcfcffff)), unchecked((int)(0xffbffcfc)), unchecked((int)(0xf8f8ffff
			)), unchecked((int)(0xfebef8f8)), unchecked((int)(0xf0f0fefe)), unchecked((int)(
			0xfcbcf0e0)), unchecked((int)(0xe0e0fcfc)), unchecked((int)(0xf8b8c000)), unchecked(
			(int)(0xc0c0f8f8)), unchecked((int)(0xf8b80000)), unchecked((int)(0x8088f8f8)), 
			unchecked((int)(0xf8b80000)), unchecked((int)(0x18f8f8)), unchecked((int)(0xf8b80000
			)), unchecked((int)(0x3038f8f8)), unchecked((int)(0xffdffcfc)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffdff8f8)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffdff1f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xffdfe3e3)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffdfc7c7)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffdf8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffdf1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(0xffdf3f3f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffdcfcfc)), unchecked((int)(0xfcfcfefe)), 
			unchecked((int)(0xfed8f8f0)), unchecked((int)(0xf8fcfc)), unchecked((int)(0xfcd0f0f0
			)), unchecked((int)(0xf8f8)), unchecked((int)(0xf8c0e000)), unchecked((int)(0xf0
			)), unchecked((int)(0x70404000)), unchecked((int)(0x0)), unchecked((int)(0x70000000
			)), unchecked((int)(0x0)), unchecked((int)(0x70101000)), unchecked((int)(0x0)), 
			unchecked((int)(0xf8183800)), unchecked((int)(0x78)), unchecked((int)(0xfcdcfcfc
			)), unchecked((int)(0xfcfcfcfe)), unchecked((int)(0xf8d8f8f0)), unchecked((int)(
			0xf8f8fc)), unchecked((int)(0xf0d0e000)), unchecked((int)(0xf0f8)), unchecked((int
			)(0xe0c0e000)), unchecked((int)(0xf0)), unchecked((int)(0x40400000)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x10100000
			)), unchecked((int)(0x0)), unchecked((int)(0x38183800)), unchecked((int)(0x78)), 
			unchecked((int)(0xfcdcfcfc)), unchecked((int)(0xfcfcfcfc)), unchecked((int)(0xf8d8f8f0
			)), unchecked((int)(0xf8f8f8)), unchecked((int)(0xf0d0e000)), unchecked((int)(0xf0f0
			)), unchecked((int)(0xe0c00000)), unchecked((int)(0xe0)), unchecked((int)(0x40000000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x10000000)), unchecked((int)(0x0)), unchecked((int)(0x38180000)), unchecked(
			(int)(0x38)), unchecked((int)(0xfcdcfcfc)), unchecked((int)(0xfcfcfcfc)), unchecked(
			(int)(0xf8d8f8f0)), unchecked((int)(0xf8f8f8)), unchecked((int)(0xf0d0e000)), unchecked(
			(int)(0xf0f0)), unchecked((int)(0xe0c00000)), unchecked((int)(0xe0e0)), unchecked(
			(int)(0x40000000)), unchecked((int)(0x40)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x10000000)), unchecked((int)(0x10)), unchecked((int)(0x38180000
			)), unchecked((int)(0x3838)), unchecked((int)(0xfddcfcfc)), unchecked((int)(0xfcfcfcfc
			)), unchecked((int)(0xfad8f8f0)), unchecked((int)(0xf8f8f8)), unchecked((int)(0xf4d0e000
			)), unchecked((int)(0xf0f0f0)), unchecked((int)(0xe8c00000)), unchecked((int)(0xe0e0
			)), unchecked((int)(0x50000000)), unchecked((int)(0x4040)), unchecked((int)(0x20000000
			)), unchecked((int)(0x0)), unchecked((int)(0x50000000)), unchecked((int)(0x1010)
			), unchecked((int)(0xb8180000)), unchecked((int)(0x3838)), unchecked((int)(0xfcdcfcfc
			)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xf8d8f8f0)), unchecked((int)(
			0xf8f8f8fe)), unchecked((int)(0xf0d0e000)), unchecked((int)(0xf0f0fc)), unchecked(
			(int)(0xe0c00000)), unchecked((int)(0xe0e0f8)), unchecked((int)(0x40000000)), unchecked(
			(int)(0x404070)), unchecked((int)(0x0)), unchecked((int)(0x70)), unchecked((int)
			(0x10000000)), unchecked((int)(0x101070)), unchecked((int)(0x38180000)), unchecked(
			(int)(0x3838f8)), unchecked((int)(0xffdcfcfc)), unchecked((int)(0xfcfcffff)), unchecked(
			(int)(0xfed8f8f0)), unchecked((int)(0xf8f8fefe)), unchecked((int)(0xfcd0e000)), 
			unchecked((int)(0xf0f0fcfc)), unchecked((int)(0xf8c00000)), unchecked((int)(0xe0e0f8f8
			)), unchecked((int)(0xf8000000)), unchecked((int)(0x4040f8f8)), unchecked((int)(
			0xf8000000)), unchecked((int)(0xf8f8)), unchecked((int)(0xf8000000)), unchecked(
			(int)(0x1010f8f8)), unchecked((int)(0xf8180000)), unchecked((int)(0x3838f8f8)), 
			unchecked((int)(0xffdffcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffdff8f8
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffdff1f1)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffdfe3e3)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffdfc7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xffdf8f8f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffdf1f1f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffdf3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffdcfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffd8f8f8)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffd1f1f1)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffc3e3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xffc7c7c7
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff1f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcdcfcfe)), 
			unchecked((int)(0xfefeffff)), unchecked((int)(0xf8d8f8fc)), unchecked((int)(0xfcfcfefe
			)), unchecked((int)(0xf0d0f0f0)), unchecked((int)(0xf8fcfc)), unchecked((int)(0xe0c0e0f0
			)), unchecked((int)(0xf8f8)), unchecked((int)(0xc0c0c000)), unchecked((int)(0xf0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x18181800)), 
			unchecked((int)(0x78)), unchecked((int)(0x3c1c3c7c)), unchecked((int)(0xfcfc)), 
			unchecked((int)(0xfcdcfefe)), unchecked((int)(0xfefefefc)), unchecked((int)(0xf8d8fcfc
			)), unchecked((int)(0xfcfcfcf8)), unchecked((int)(0xf0d0f8f0)), unchecked((int)(
			0xf8f8f0)), unchecked((int)(0xe0c0e000)), unchecked((int)(0xf0e0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x3c1c3c00)), unchecked((int
			)(0x7c3c)), unchecked((int)(0xfcdefefe)), unchecked((int)(0xfefefcfc)), unchecked(
			(int)(0xf8dcfcfc)), unchecked((int)(0xfcfcf8f8)), unchecked((int)(0xf0d8f8f0)), 
			unchecked((int)(0xf8f0f0)), unchecked((int)(0xe0d0e000)), unchecked((int)(0xf0e0e0
			)), unchecked((int)(0xc0c00000)), unchecked((int)(0xc0c0)), unchecked((int)(0x0)
			), unchecked((int)(0x0)), unchecked((int)(0x18180000)), unchecked((int)(0x1818))
			, unchecked((int)(0x3c5c3c00)), unchecked((int)(0x7c3c3c)), unchecked((int)(0xfedffefe
			)), unchecked((int)(0xfefcfcfc)), unchecked((int)(0xfddffcfc)), unchecked((int)(
			0xfcf8f8f8)), unchecked((int)(0xfadef8f0)), unchecked((int)(0xf8f0f0f0)), unchecked(
			(int)(0xf4dce000)), unchecked((int)(0xe0e0e0)), unchecked((int)(0xe8d80000)), unchecked(
			(int)(0xc0c0c0)), unchecked((int)(0x50500000)), unchecked((int)(0x0)), unchecked(
			(int)(0xb8d80000)), unchecked((int)(0x181818)), unchecked((int)(0x7cdc3c00)), unchecked(
			(int)(0x3c3c3c)), unchecked((int)(0xffdefefe)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xffdcfcfc)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xfed8f8f0)), 
			unchecked((int)(0xf0f0f0fe)), unchecked((int)(0xfcd0e000)), unchecked((int)(0xe0e0e0fc
			)), unchecked((int)(0xf8c00000)), unchecked((int)(0xc0c0c0f8)), unchecked((int)(
			0xf8000000)), unchecked((int)(0x88f8)), unchecked((int)(0xf8180000)), unchecked(
			(int)(0x181818f8)), unchecked((int)(0xfc5c3c00)), unchecked((int)(0x3c3c3cfc)), 
			unchecked((int)(0xffdffefe)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffdffcfc
			)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0xfedef8f0)), unchecked((int)(
			0xf0f0fefe)), unchecked((int)(0xfcdce000)), unchecked((int)(0xe0e0fcfc)), unchecked(
			(int)(0xfcdc0000)), unchecked((int)(0xc0c4fcfc)), unchecked((int)(0xfcdc0000)), 
			unchecked((int)(0x8cfcfc)), unchecked((int)(0xfcdc0000)), unchecked((int)(0x181cfcfc
			)), unchecked((int)(0xfcdc3c00)), unchecked((int)(0x3c3cfcfc)), unchecked((int)(
			0xffeffcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffeff8f8)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffeff1f1)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffefe3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xffefc7c7
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffef8f8f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffef1f1f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffef3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xffecfcf8)), 
			unchecked((int)(0xfcfefe)), unchecked((int)(0xfee8f8f8)), unchecked((int)(0xfcfc
			)), unchecked((int)(0x7c607000)), unchecked((int)(0x78)), unchecked((int)(0x38202000
			)), unchecked((int)(0x0)), unchecked((int)(0x38000000)), unchecked((int)(0x0)), 
			unchecked((int)(0x38080800)), unchecked((int)(0x0)), unchecked((int)(0x7c0c1c00)
			), unchecked((int)(0x3c)), unchecked((int)(0xfe2e3e3e)), unchecked((int)(0x7e7e)
			), unchecked((int)(0xfcecfcf8)), unchecked((int)(0xfcfcfe)), unchecked((int)(0xf8e8f000
			)), unchecked((int)(0xf8fc)), unchecked((int)(0x70607000)), unchecked((int)(0x78
			)), unchecked((int)(0x20200000)), unchecked((int)(0x0)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x8080000)), unchecked((int)(0x0)), unchecked(
			(int)(0x1c0c1c00)), unchecked((int)(0x3c)), unchecked((int)(0x3e2e1e00)), unchecked(
			(int)(0x3e7e)), unchecked((int)(0xfcecfcf8)), unchecked((int)(0xfcfcfc)), unchecked(
			(int)(0xf8e8f000)), unchecked((int)(0xf8f8)), unchecked((int)(0x70600000)), unchecked(
			(int)(0x70)), unchecked((int)(0x20000000)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x8000000)), unchecked((int)(0x0
			)), unchecked((int)(0x1c0c0000)), unchecked((int)(0x1c)), unchecked((int)(0x3e2e1e00
			)), unchecked((int)(0x3e3e)), unchecked((int)(0xfcecfcf8)), unchecked((int)(0xfcfcfc
			)), unchecked((int)(0xf8e8f000)), unchecked((int)(0xf8f8)), unchecked((int)(0x70600000
			)), unchecked((int)(0x7070)), unchecked((int)(0x20000000)), unchecked((int)(0x20
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x8000000)), unchecked(
			(int)(0x8)), unchecked((int)(0x1c0c0000)), unchecked((int)(0x1c1c)), unchecked((
			int)(0x3e2e1e00)), unchecked((int)(0x3e3e)), unchecked((int)(0xfdecfcf8)), unchecked(
			(int)(0xfcfcfc)), unchecked((int)(0xfae8f000)), unchecked((int)(0xf8f8f8)), unchecked(
			(int)(0x74600000)), unchecked((int)(0x7070)), unchecked((int)(0x28000000)), unchecked(
			(int)(0x2020)), unchecked((int)(0x10000000)), unchecked((int)(0x0)), unchecked((
			int)(0x28000000)), unchecked((int)(0x808)), unchecked((int)(0x5c0c0000)), unchecked(
			(int)(0x1c1c)), unchecked((int)(0xbe2e1e00)), unchecked((int)(0x3e3e3e)), unchecked(
			(int)(0xfcecfcf8)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xf8e8f000)), 
			unchecked((int)(0xf8f8fe)), unchecked((int)(0x70600000)), unchecked((int)(0x70707c
			)), unchecked((int)(0x20000000)), unchecked((int)(0x202038)), unchecked((int)(0x0
			)), unchecked((int)(0x38)), unchecked((int)(0x8000000)), unchecked((int)(0x80838
			)), unchecked((int)(0x1c0c0000)), unchecked((int)(0x1c1c7c)), unchecked((int)(0x3e2e1e00
			)), unchecked((int)(0x3e3efe)), unchecked((int)(0xffecfcf8)), unchecked((int)(0xfcfcffff
			)), unchecked((int)(0xfee8f000)), unchecked((int)(0xf8f8fefe)), unchecked((int)(
			0x7c600000)), unchecked((int)(0x70707c7c)), unchecked((int)(0x7c000000)), unchecked(
			(int)(0x20207c7c)), unchecked((int)(0x7c000000)), unchecked((int)(0x7c7c)), unchecked(
			(int)(0x7c000000)), unchecked((int)(0x8087c7c)), unchecked((int)(0x7c0c0000)), unchecked(
			(int)(0x1c1c7c7c)), unchecked((int)(0xfe2e1e00)), unchecked((int)(0x3e3efefe)), 
			unchecked((int)(0xffeffcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffeff8f8
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffeff1f1)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffefe3e3)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffefc7c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xffef8f8f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffef1f1f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffef3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffecfcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffe8f8f8)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffe1f1f1)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xffc7c7c7
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xff0f1f1f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xff2f3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcecfcfe)), 
			unchecked((int)(0xfefeffff)), unchecked((int)(0xf8e8f8f8)), unchecked((int)(0xfcfefe
			)), unchecked((int)(0xf0e0f0f8)), unchecked((int)(0xfcfc)), unchecked((int)(0x60606000
			)), unchecked((int)(0x78)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xc0c0c00)), unchecked((int)(0x3c)), unchecked((int)(0x1e0e1e3e)), unchecked(
			(int)(0x7e7e)), unchecked((int)(0x3f2f3f3f)), unchecked((int)(0x7fffff)), unchecked(
			(int)(0xfcecfefe)), unchecked((int)(0xfefefefc)), unchecked((int)(0xf8e8fcf8)), 
			unchecked((int)(0xfcfcf8)), unchecked((int)(0xf0e0f000)), unchecked((int)(0xf8f0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1e0e1e00
			)), unchecked((int)(0x3e1e)), unchecked((int)(0x3f2f7f3f)), unchecked((int)(0x7f7f3f
			)), unchecked((int)(0xfceefefe)), unchecked((int)(0xfefefcfc)), unchecked((int)(
			0xf8ecfcf8)), unchecked((int)(0xfcf8f8)), unchecked((int)(0xf0e8f000)), unchecked(
			(int)(0xf8f0f0)), unchecked((int)(0x60600000)), unchecked((int)(0x6060)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0000)), unchecked((int)
			(0xc0c)), unchecked((int)(0x1e2e1e00)), unchecked((int)(0x3e1e1e)), unchecked((int
			)(0x3f6f7f3f)), unchecked((int)(0x7f3f3f)), unchecked((int)(0xfeeffefe)), unchecked(
			(int)(0xfefcfcfc)), unchecked((int)(0xfdeffcf8)), unchecked((int)(0xfcf8f8f8)), 
			unchecked((int)(0xfaeef000)), unchecked((int)(0xf0f0f0)), unchecked((int)(0x746c0000
			)), unchecked((int)(0x606060)), unchecked((int)(0x28280000)), unchecked((int)(0x0
			)), unchecked((int)(0x5c6c0000)), unchecked((int)(0xc0c0c)), unchecked((int)(0xbeee1e00
			)), unchecked((int)(0x1e1e1e)), unchecked((int)(0x7fef7f3f)), unchecked((int)(0x7f3f3f3f
			)), unchecked((int)(0xffeefefe)), unchecked((int)(0xfcfcfcff)), unchecked((int)(
			0xffecfcf8)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xfee8f000)), unchecked(
			(int)(0xf0f0f0fe)), unchecked((int)(0x7c600000)), unchecked((int)(0x6060607c)), 
			unchecked((int)(0x7c000000)), unchecked((int)(0x447c)), unchecked((int)(0x7c0c0000
			)), unchecked((int)(0xc0c0c7c)), unchecked((int)(0xfe2e1e00)), unchecked((int)(0x1e1e1efe
			)), unchecked((int)(0xff6f7f3f)), unchecked((int)(0x3f3f3fff)), unchecked((int)(
			0xffeffefe)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffeffcf8)), unchecked(
			(int)(0xf8f8ffff)), unchecked((int)(0xfeeef000)), unchecked((int)(0xf0f0fefe)), 
			unchecked((int)(0xfeee0000)), unchecked((int)(0x60e2fefe)), unchecked((int)(0xfeee0000
			)), unchecked((int)(0xc6fefe)), unchecked((int)(0xfeee0000)), unchecked((int)(0xc8efefe
			)), unchecked((int)(0xfeee1e00)), unchecked((int)(0x1e1efefe)), unchecked((int)(
			0xffef7f3f)), unchecked((int)(0x3f3fffff)), unchecked((int)(0xffff7cfc)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffff78f8)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffff71f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff63e3
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff47c7)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffff0f8f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xc00000)), unchecked((int)(0x0)), unchecked((int)(0xc00000)), unchecked((
			int)(0x0)), unchecked((int)(0xfffc7cfc)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfef87800)), unchecked((int)(0xf8f8fefe)), unchecked((int)(0xfcf07000)), 
			unchecked((int)(0xf0f0fcfc)), unchecked((int)(0xf8e06000)), unchecked((int)(0xe0f8f8
			)), unchecked((int)(0xf0404000)), unchecked((int)(0xf0f0)), unchecked((int)(0xf0000000
			)), unchecked((int)(0xf0f0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfcfc7cff)), unchecked((int
			)(0xffffffff)), unchecked((int)(0xf8f80000)), unchecked((int)(0xf8f8f8fe)), unchecked(
			(int)(0xf0f00000)), unchecked((int)(0xf0f0f0fc)), unchecked((int)(0xe0e00000)), 
			unchecked((int)(0xe0e0f8)), unchecked((int)(0x40400000)), unchecked((int)(0x4070
			)), unchecked((int)(0x0)), unchecked((int)(0x60)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfcfc7fff
			)), unchecked((int)(0xfffffffc)), unchecked((int)(0xf8000000)), unchecked((int)(
			0xf8f8f8f8)), unchecked((int)(0xf0000000)), unchecked((int)(0xf0f0f0f0)), unchecked(
			(int)(0xe0000000)), unchecked((int)(0xe0e0e0)), unchecked((int)(0x40000000)), unchecked(
			(int)(0x4040)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xfcff7fff)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xf8000000)), 
			unchecked((int)(0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)
			), unchecked((int)(0x0)), unchecked((int)(0xe0e0e0)), unchecked((int)(0x0)), unchecked(
			(int)(0x4040)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xffff7fff)), unchecked((int)(0xfffcfcfc)), unchecked((int)(0xf8000000)), 
			unchecked((int)(0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)
			), unchecked((int)(0x0)), unchecked((int)(0xe0e000)), unchecked((int)(0x0)), unchecked(
			(int)(0x4000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xffff7fff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xf8000000)), 
			unchecked((int)(0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)
			), unchecked((int)(0x0)), unchecked((int)(0xe0e000)), unchecked((int)(0x0)), unchecked(
			(int)(0x400000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(
			0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xffff7fff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xf8000000)), 
			unchecked((int)(0xf8f8f8f8)), unchecked((int)(0x0)), unchecked((int)(0xf0f0f0f0)
			), unchecked((int)(0x0)), unchecked((int)(0xe0e0e000)), unchecked((int)(0x0)), unchecked(
			(int)(0x40400000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0xffff7cfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff78f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffff71f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffff63e3)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffff47c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff0f8f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xfffc7cfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff878f8)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xfff171f1)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffe363e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xffc747c7)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff8f0f8f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x0)), unchecked((int)(0x0)), unchecked((int)(0xfcfc7cff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8f878ff)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xf0f07000)), unchecked((int)(0xf8f8fefe)), unchecked((int)(0xe0e06000)), unchecked(
			(int)(0xf0f0fcfc)), unchecked((int)(0xc0c04000)), unchecked((int)(0xe0f8f8)), unchecked(
			(int)(0x80000000)), unchecked((int)(0xf0f0)), unchecked((int)(0x0)), unchecked((
			int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfcfc7fff
			)), unchecked((int)(0xfffffffc)), unchecked((int)(0xf8f87fff)), unchecked((int)(
			0xfffffff8)), unchecked((int)(0xf0f00000)), unchecked((int)(0xf8f8f8f0)), unchecked(
			(int)(0xe0e00000)), unchecked((int)(0xf0f0f0e0)), unchecked((int)(0xc0c00000)), 
			unchecked((int)(0xe0e0c0)), unchecked((int)(0x0)), unchecked((int)(0x4000)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0xfcff7fff)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xf8ff7fff
			)), unchecked((int)(0xfffff8f8)), unchecked((int)(0xf0000000)), unchecked((int)(
			0xf8f8f0f0)), unchecked((int)(0xe0000000)), unchecked((int)(0xf0f0e0e0)), unchecked(
			(int)(0xc0000000)), unchecked((int)(0xe0c0c0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0xffff7fff)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0xffff7fff)), unchecked((int)(0xfff8f8f8)), unchecked((int)(
			0xf8000000)), unchecked((int)(0xf8f0f0f0)), unchecked((int)(0x0)), unchecked((int
			)(0xf0e0e0e0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffff7fff)), unchecked((int
			)(0xfcfcfcff)), unchecked((int)(0xffff7fff)), unchecked((int)(0xf8f8f8ff)), unchecked(
			(int)(0xf8000000)), unchecked((int)(0xf0f0f0f8)), unchecked((int)(0x0)), unchecked(
			(int)(0xe0e0e0f0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0c000)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffff7fff)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xffff7fff)), unchecked((int)(0xf8f8ffff)), 
			unchecked((int)(0xf8000000)), unchecked((int)(0xf0f0f8f8)), unchecked((int)(0x0)
			), unchecked((int)(0xe0e0f0f0)), unchecked((int)(0x0)), unchecked((int)(0xc0c0e000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xffffbcfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffffb8f8)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffffb1f1)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffffa3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff87c7)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xe08080)), unchecked((int)(0x0)), 
			unchecked((int)(0xe00000)), unchecked((int)(0x0)), unchecked((int)(0xe02020)), unchecked(
			(int)(0x0)), unchecked((int)(0xfffcbcfc)), unchecked((int)(0xfcfcffff)), unchecked(
			(int)(0xfef8b8f8)), unchecked((int)(0xf8f8fefe)), unchecked((int)(0xfcf0b0f0)), 
			unchecked((int)(0xf0fcfc)), unchecked((int)(0xf8e0a0c0)), unchecked((int)(0xf8f8
			)), unchecked((int)(0xf8c080c0)), unchecked((int)(0xf8f8)), unchecked((int)(0x808000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x202000)), unchecked((int)(0x0)), unchecked((int)(0xfcfcbcfc)), unchecked(
			(int)(0xfcfcfcff)), unchecked((int)(0xf8f8b8f8)), unchecked((int)(0xf8f8f8fe)), 
			unchecked((int)(0xf0f0b0f0)), unchecked((int)(0xf0f0fc)), unchecked((int)(0xe0e0a0c0
			)), unchecked((int)(0xe0f8)), unchecked((int)(0xc0c08000)), unchecked((int)(0xf0
			)), unchecked((int)(0x800000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x200000)), unchecked((int)(0x0)), unchecked((int)(
			0xfcfcbcfc)), unchecked((int)(0xfcfcfcfc)), unchecked((int)(0xf8f8b8f8)), unchecked(
			(int)(0xf8f8f8f8)), unchecked((int)(0xf0f0b0f0)), unchecked((int)(0xf0f0f0)), unchecked(
			(int)(0xe0e0a0c0)), unchecked((int)(0xe0e0)), unchecked((int)(0xc0c08000)), unchecked(
			(int)(0xc0)), unchecked((int)(0x80800000)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x20200000)), unchecked((int)(0x0
			)), unchecked((int)(0xfcfdbcfc)), unchecked((int)(0xfcfcfcfc)), unchecked((int)(
			0xf8fab8f8)), unchecked((int)(0xf8f8f8f8)), unchecked((int)(0xf0f4b0f0)), unchecked(
			(int)(0xf0f0f0)), unchecked((int)(0xe0e8a0c0)), unchecked((int)(0xe0e0)), unchecked(
			(int)(0xc0d08000)), unchecked((int)(0xc0)), unchecked((int)(0x80a00000)), unchecked(
			(int)(0x80)), unchecked((int)(0x400000)), unchecked((int)(0x0)), unchecked((int)
			(0x20a00000)), unchecked((int)(0x20)), unchecked((int)(0xfffdbcfc)), unchecked((
			int)(0xfcfcfcfc)), unchecked((int)(0xfef8b8f8)), unchecked((int)(0xf8f8f8f8)), unchecked(
			(int)(0xfcf0b0f0)), unchecked((int)(0xf0f0f0)), unchecked((int)(0xf8e0a0c0)), unchecked(
			(int)(0xe0e0)), unchecked((int)(0xf0c08000)), unchecked((int)(0xc0c0)), unchecked(
			(int)(0xe0800000)), unchecked((int)(0x8080)), unchecked((int)(0xe0000000)), unchecked(
			(int)(0x0)), unchecked((int)(0xe0200000)), unchecked((int)(0x2020)), unchecked((
			int)(0xffffbcfc)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xfefeb8f8)), unchecked(
			(int)(0xf8f8f8fe)), unchecked((int)(0xfcfcb0f0)), unchecked((int)(0xf0f0fc)), unchecked(
			(int)(0xf8f8a0c0)), unchecked((int)(0xe0e0f8)), unchecked((int)(0xf0f08000)), unchecked(
			(int)(0xc0c0f0)), unchecked((int)(0xf0f00000)), unchecked((int)(0x8080f0)), unchecked(
			(int)(0xf0f00000)), unchecked((int)(0xf0)), unchecked((int)(0xf0f00000)), unchecked(
			(int)(0x2020f0)), unchecked((int)(0xffffbffc)), unchecked((int)(0xfcfcffff)), unchecked(
			(int)(0xfefebef8)), unchecked((int)(0xf8f8fefe)), unchecked((int)(0xfcfcbcf0)), 
			unchecked((int)(0xf0f0fcfc)), unchecked((int)(0xf8f8b8c0)), unchecked((int)(0xe0e0f8f8
			)), unchecked((int)(0xf8f8b800)), unchecked((int)(0xc0c0f8f8)), unchecked((int)(
			0xf8f8b800)), unchecked((int)(0x8080f8f8)), unchecked((int)(0xf8f8b800)), unchecked(
			(int)(0xf8f8)), unchecked((int)(0xf8f8b800)), unchecked((int)(0x2020f8f8)), unchecked(
			(int)(0xffffbcfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffffb8f8)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffffb1f1)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffffa3e3)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffff87c7)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff8f8f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffff1f1f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffff3f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xfffcbcfc
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff8b8f8)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xfff1b1f1)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffe3a3e3)), unchecked((int)(0xffffffff)), unchecked((int)(0xffc787c7)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xff8f8f8f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff3f3f3f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcbcff)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xf8f8b8fc)), unchecked((int)(0xfcfcffff)), unchecked((int)(
			0xf0f0b0f8)), unchecked((int)(0xf8f8fefe)), unchecked((int)(0xe0e0a0f0)), unchecked(
			(int)(0xf0fcfc)), unchecked((int)(0xc0c080c0)), unchecked((int)(0xf8f8)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0))
			, unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xfcfcbfff)), unchecked(
			(int)(0xfffffffc)), unchecked((int)(0xf8f8bcfc)), unchecked((int)(0xfcfcfcf8)), 
			unchecked((int)(0xf0f0b8f8)), unchecked((int)(0xf8f8f8f0)), unchecked((int)(0xe0e0b0f0
			)), unchecked((int)(0xf0f0e0)), unchecked((int)(0xc0c0a0c0)), unchecked((int)(0xe0c0
			)), unchecked((int)(0x80808000)), unchecked((int)(0x80)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x30303000)), unchecked((int)(0x30)), unchecked(
			(int)(0xfcffbfff)), unchecked((int)(0xfffffcfc)), unchecked((int)(0xf8fdbffc)), 
			unchecked((int)(0xfcfcf8f8)), unchecked((int)(0xf0fabef8)), unchecked((int)(0xf8f8f0f0
			)), unchecked((int)(0xe0f4bcf0)), unchecked((int)(0xf0e0e0)), unchecked((int)(0xc0e8b8c0
			)), unchecked((int)(0xc0c0)), unchecked((int)(0x80d0b000)), unchecked((int)(0x8080
			)), unchecked((int)(0xa0a000)), unchecked((int)(0x0)), unchecked((int)(0x3070b000
			)), unchecked((int)(0x3030)), unchecked((int)(0xffffbfff)), unchecked((int)(0xfffcfcfc
			)), unchecked((int)(0xffffbcfc)), unchecked((int)(0xfcf8f8f8)), unchecked((int)(
			0xfefeb8f8)), unchecked((int)(0xf8f0f0f0)), unchecked((int)(0xfcfcb0f0)), unchecked(
			(int)(0xe0e0e0)), unchecked((int)(0xf8f8a0c0)), unchecked((int)(0xc0c0c0)), unchecked(
			(int)(0xf0f08000)), unchecked((int)(0x808080)), unchecked((int)(0xf0f00000)), unchecked(
			(int)(0x10)), unchecked((int)(0xf0f03000)), unchecked((int)(0x303030)), unchecked(
			(int)(0xffffbfff)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffffbffc)), 
			unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xfefebef8)), unchecked((int)(0xf0f0f0fe
			)), unchecked((int)(0xfcfcbcf0)), unchecked((int)(0xe0e0e0fc)), unchecked((int)(
			0xf8f8b8c0)), unchecked((int)(0xc0c0c0f8)), unchecked((int)(0xf8f8b800)), unchecked(
			(int)(0x808088f8)), unchecked((int)(0xf8f8b800)), unchecked((int)(0x18f8)), unchecked(
			(int)(0xf8f8b800)), unchecked((int)(0x303038f8)), unchecked((int)(0xffffbfff)), 
			unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffbfff)), unchecked((int)(0xf8f8ffff
			)), unchecked((int)(0xfefebefe)), unchecked((int)(0xf0f0fefe)), unchecked((int)(
			0xfcfcbcfc)), unchecked((int)(0xe0e0fcfc)), unchecked((int)(0xfcfcbcfc)), unchecked(
			(int)(0xc0c4fcfc)), unchecked((int)(0xfcfcbcfc)), unchecked((int)(0x808cfcfc)), 
			unchecked((int)(0xfcfcbcfc)), unchecked((int)(0x1cfcfc)), unchecked((int)(0xfcfcbcfc
			)), unchecked((int)(0x303cfcfc)), unchecked((int)(0xffffdcfc)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffffd8f8)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffffd1f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xffffc3e3)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0x704040)), unchecked((int)(0x0)), 
			unchecked((int)(0x700000)), unchecked((int)(0x0)), unchecked((int)(0x701010)), unchecked(
			(int)(0x0)), unchecked((int)(0xffff1f3f)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfffcdcfc)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xfef8d8f8)), 
			unchecked((int)(0xf8fefe)), unchecked((int)(0xfcf0d0e0)), unchecked((int)(0xfcfc
			)), unchecked((int)(0xfce0c0e0)), unchecked((int)(0xfcfc)), unchecked((int)(0x404000
			)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x101000)), unchecked((int)(0x0)), unchecked((int)(0xfc3c1c3c)), unchecked(
			(int)(0xfcfc)), unchecked((int)(0xfcfcdcfc)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xf8f8d8f8)), unchecked((int)(0xf8f8fe)), unchecked((int)(0xf0f0d0e0)), unchecked(
			(int)(0xf0fc)), unchecked((int)(0xe0e0c000)), unchecked((int)(0xf8)), unchecked(
			(int)(0x400000)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(
			0x0)), unchecked((int)(0x100000)), unchecked((int)(0x0)), unchecked((int)(0x38381800
			)), unchecked((int)(0xf8)), unchecked((int)(0xfcfcdcfc)), unchecked((int)(0xfcfcfcfc
			)), unchecked((int)(0xf8f8d8f8)), unchecked((int)(0xf8f8f8)), unchecked((int)(0xf0f0d0e0
			)), unchecked((int)(0xf0f0)), unchecked((int)(0xe0e0c000)), unchecked((int)(0xe0
			)), unchecked((int)(0x40400000)), unchecked((int)(0x0)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x10100000)), unchecked((int)(0x0)), unchecked(
			(int)(0x38381800)), unchecked((int)(0x38)), unchecked((int)(0xfcfddcfc)), unchecked(
			(int)(0xfcfcfcfc)), unchecked((int)(0xf8fad8f8)), unchecked((int)(0xf8f8f8)), unchecked(
			(int)(0xf0f4d0e0)), unchecked((int)(0xf0f0)), unchecked((int)(0xe0e8c000)), unchecked(
			(int)(0xe0)), unchecked((int)(0x40500000)), unchecked((int)(0x40)), unchecked((int
			)(0x200000)), unchecked((int)(0x0)), unchecked((int)(0x10500000)), unchecked((int
			)(0x10)), unchecked((int)(0x38b81800)), unchecked((int)(0x38)), unchecked((int)(
			0xfffcdcfc)), unchecked((int)(0xfcfcfcfc)), unchecked((int)(0xfef8d8f8)), unchecked(
			(int)(0xf8f8f8)), unchecked((int)(0xfcf0d0e0)), unchecked((int)(0xf0f0)), unchecked(
			(int)(0xf8e0c000)), unchecked((int)(0xe0e0)), unchecked((int)(0x70400000)), unchecked(
			(int)(0x4040)), unchecked((int)(0x70000000)), unchecked((int)(0x0)), unchecked((
			int)(0x70100000)), unchecked((int)(0x1010)), unchecked((int)(0xf8381800)), unchecked(
			(int)(0x3838)), unchecked((int)(0xffffdcfc)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xfefed8f8)), unchecked((int)(0xf8f8fe)), unchecked((int)(0xfcfcd0e0)), unchecked(
			(int)(0xf0f0fc)), unchecked((int)(0xf8f8c000)), unchecked((int)(0xe0e0f8)), unchecked(
			(int)(0xf8f80000)), unchecked((int)(0x4040f8)), unchecked((int)(0xf8f80000)), unchecked(
			(int)(0xf8)), unchecked((int)(0xf8f80000)), unchecked((int)(0x1010f8)), unchecked(
			(int)(0xf8f81800)), unchecked((int)(0x3838f8)), unchecked((int)(0xffffdffc)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xfefedef8)), unchecked((int)(0xf8f8fefe)), 
			unchecked((int)(0xfcfcdce0)), unchecked((int)(0xf0f0fcfc)), unchecked((int)(0xfcfcdc00
			)), unchecked((int)(0xe0e0fcfc)), unchecked((int)(0xfcfcdc00)), unchecked((int)(
			0x4040fcfc)), unchecked((int)(0xfcfcdc00)), unchecked((int)(0xfcfc)), unchecked(
			(int)(0xfcfcdc00)), unchecked((int)(0x1010fcfc)), unchecked((int)(0xfcfcdc00)), 
			unchecked((int)(0x3838fcfc)), unchecked((int)(0xffffdcfc)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffffd8f8)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xffffd1f1)), unchecked((int)(0xffffffff)), unchecked((int)(0xffffc3e3)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffff8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff1f1f
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff1f3f)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xfffcdcfc)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xfff8d8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff1d1f1)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffe3c3e3)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xffc7c7c7)), unchecked((int)(0xffffffff)), unchecked((int)(
			0x0)), unchecked((int)(0x0)), unchecked((int)(0xff1f1f1f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xff3f1f3f)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xfcfcdcfe)), unchecked((int)(0xfefeffff)), unchecked((int)(0xf8f8d8fc)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xf0f0d0f8)), unchecked((int)(0xf8fefe)), unchecked(
			(int)(0xe0e0c0e0)), unchecked((int)(0xfcfc)), unchecked((int)(0x0)), unchecked((
			int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x3c3c1c3c)), unchecked((int)(0xfcfc)), unchecked(
			(int)(0xfcfcdefe)), unchecked((int)(0xfefefefc)), unchecked((int)(0xf8f8dcfc)), 
			unchecked((int)(0xfcfcfcf8)), unchecked((int)(0xf0f0d8f8)), unchecked((int)(0xf8f8f0
			)), unchecked((int)(0xe0e0d0e0)), unchecked((int)(0xf0e0)), unchecked((int)(0xc0c0c000
			)), unchecked((int)(0xc0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x18181800)), unchecked((int)(0x18)), unchecked((int)(0x3c3c5c3c)), unchecked(
			(int)(0x7c3c)), unchecked((int)(0xfcfedffe)), unchecked((int)(0xfefefcfc)), unchecked(
			(int)(0xf8fddffc)), unchecked((int)(0xfcfcf8f8)), unchecked((int)(0xf0fadef8)), 
			unchecked((int)(0xf8f0f0)), unchecked((int)(0xe0f4dce0)), unchecked((int)(0xe0e0
			)), unchecked((int)(0xc0e8d800)), unchecked((int)(0xc0c0)), unchecked((int)(0x505000
			)), unchecked((int)(0x0)), unchecked((int)(0x18b8d800)), unchecked((int)(0x1818)
			), unchecked((int)(0x3c7cdc3c)), unchecked((int)(0x3c3c)), unchecked((int)(0xffffdefe
			)), unchecked((int)(0xfefcfcfc)), unchecked((int)(0xffffdcfc)), unchecked((int)(
			0xfcf8f8f8)), unchecked((int)(0xfefed8f8)), unchecked((int)(0xf0f0f0)), unchecked(
			(int)(0xfcfcd0e0)), unchecked((int)(0xe0e0e0)), unchecked((int)(0xf8f8c000)), unchecked(
			(int)(0xc0c0c0)), unchecked((int)(0xf8f80000)), unchecked((int)(0x88)), unchecked(
			(int)(0xf8f81800)), unchecked((int)(0x181818)), unchecked((int)(0xfcfc5c3c)), unchecked(
			(int)(0x3c3c3c)), unchecked((int)(0xffffdffe)), unchecked((int)(0xfcfcfcff)), unchecked(
			(int)(0xffffdffc)), unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xfefedef8)), 
			unchecked((int)(0xf0f0f0fe)), unchecked((int)(0xfcfcdce0)), unchecked((int)(0xe0e0e0fc
			)), unchecked((int)(0xfcfcdc00)), unchecked((int)(0xc0c0c4fc)), unchecked((int)(
			0xfcfcdc00)), unchecked((int)(0x8cfc)), unchecked((int)(0xfcfcdc00)), unchecked(
			(int)(0x18181cfc)), unchecked((int)(0xfcfcdc3c)), unchecked((int)(0x3c3c3cfc)), 
			unchecked((int)(0xffffdfff)), unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffdfff
			)), unchecked((int)(0xf8f8ffff)), unchecked((int)(0xfefedefe)), unchecked((int)(
			0xf0f0fefe)), unchecked((int)(0xfefedefe)), unchecked((int)(0xe0e2fefe)), unchecked(
			(int)(0xfefedefe)), unchecked((int)(0xc0c6fefe)), unchecked((int)(0xfefedefe)), 
			unchecked((int)(0x8efefe)), unchecked((int)(0xfefedefe)), unchecked((int)(0x181efefe
			)), unchecked((int)(0xfefedefe)), unchecked((int)(0x3c3efefe)), unchecked((int)(
			0xffffecfc)), unchecked((int)(0xffffffff)), unchecked((int)(0xffffe8f8)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffffe1f1)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x382020)), unchecked((int)(0x0)), unchecked((int)(0x380000)), unchecked(
			(int)(0x0)), unchecked((int)(0x380808)), unchecked((int)(0x0)), unchecked((int)(
			0xffff0f1f)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff2f3f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xfffcecfc)), unchecked((int)(0xfcffff)), unchecked(
			(int)(0xfef8e8f0)), unchecked((int)(0xfefe)), unchecked((int)(0xfef0e0f0)), unchecked(
			(int)(0xfefe)), unchecked((int)(0x202000)), unchecked((int)(0x0)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x80800)), unchecked((int)(0x0))
			, unchecked((int)(0xfe1e0e1e)), unchecked((int)(0xfefe)), unchecked((int)(0xfe3e2e1e
			)), unchecked((int)(0xfefe)), unchecked((int)(0xfcfcecfc)), unchecked((int)(0xfcfcff
			)), unchecked((int)(0xf8f8e8f0)), unchecked((int)(0xf8fe)), unchecked((int)(0x70706000
			)), unchecked((int)(0x7c)), unchecked((int)(0x200000)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x80000)), unchecked((int)(0x0
			)), unchecked((int)(0x1c1c0c00)), unchecked((int)(0x7c)), unchecked((int)(0x3e3e2e1e
			)), unchecked((int)(0x3efe)), unchecked((int)(0xfcfcecfc)), unchecked((int)(0xfcfcfc
			)), unchecked((int)(0xf8f8e8f0)), unchecked((int)(0xf8f8)), unchecked((int)(0x70706000
			)), unchecked((int)(0x70)), unchecked((int)(0x20200000)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x8080000)), unchecked(
			(int)(0x0)), unchecked((int)(0x1c1c0c00)), unchecked((int)(0x1c)), unchecked((int
			)(0x3e3e2e1e)), unchecked((int)(0x3e3e)), unchecked((int)(0xfcfdecfc)), unchecked(
			(int)(0xfcfcfc)), unchecked((int)(0xf8fae8f0)), unchecked((int)(0xf8f8)), unchecked(
			(int)(0x70746000)), unchecked((int)(0x70)), unchecked((int)(0x20280000)), unchecked(
			(int)(0x20)), unchecked((int)(0x100000)), unchecked((int)(0x0)), unchecked((int)
			(0x8280000)), unchecked((int)(0x8)), unchecked((int)(0x1c5c0c00)), unchecked((int
			)(0x1c)), unchecked((int)(0x3ebe2e1e)), unchecked((int)(0x3e3e)), unchecked((int
			)(0xfffcecfc)), unchecked((int)(0xfcfcfc)), unchecked((int)(0xfef8e8f0)), unchecked(
			(int)(0xf8f8)), unchecked((int)(0x7c706000)), unchecked((int)(0x7070)), unchecked(
			(int)(0x38200000)), unchecked((int)(0x2020)), unchecked((int)(0x38000000)), unchecked(
			(int)(0x0)), unchecked((int)(0x38080000)), unchecked((int)(0x808)), unchecked((int
			)(0x7c1c0c00)), unchecked((int)(0x1c1c)), unchecked((int)(0xfe3e2e1e)), unchecked(
			(int)(0x3e3e)), unchecked((int)(0xffffecfc)), unchecked((int)(0xfcfcff)), unchecked(
			(int)(0xfefee8f0)), unchecked((int)(0xf8f8fe)), unchecked((int)(0x7c7c6000)), unchecked(
			(int)(0x70707c)), unchecked((int)(0x7c7c0000)), unchecked((int)(0x20207c)), unchecked(
			(int)(0x7c7c0000)), unchecked((int)(0x7c)), unchecked((int)(0x7c7c0000)), unchecked(
			(int)(0x8087c)), unchecked((int)(0x7c7c0c00)), unchecked((int)(0x1c1c7c)), unchecked(
			(int)(0xfefe2e1e)), unchecked((int)(0x3e3efe)), unchecked((int)(0xffffeffc)), unchecked(
			(int)(0xfcfcffff)), unchecked((int)(0xfefeeef0)), unchecked((int)(0xf8f8fefe)), 
			unchecked((int)(0xfefeee00)), unchecked((int)(0x7070fefe)), unchecked((int)(0xfefeee00
			)), unchecked((int)(0x2020fefe)), unchecked((int)(0xfefeee00)), unchecked((int)(
			0xfefe)), unchecked((int)(0xfefeee00)), unchecked((int)(0x808fefe)), unchecked((
			int)(0xfefeee00)), unchecked((int)(0x1c1cfefe)), unchecked((int)(0xfefeee1e)), unchecked(
			(int)(0x3e3efefe)), unchecked((int)(0xffffecfc)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xffffe8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xffffe1f1
			)), unchecked((int)(0xffffffff)), unchecked((int)(0xffffe3e3)), unchecked((int)(
			0xffffffff)), unchecked((int)(0xffffc7c7)), unchecked((int)(0xffffffff)), unchecked(
			(int)(0xffff8f8f)), unchecked((int)(0xffffffff)), unchecked((int)(0xffff0f1f)), 
			unchecked((int)(0xffffffff)), unchecked((int)(0xffff2f3f)), unchecked((int)(0xffffffff
			)), unchecked((int)(0xfffcecfc)), unchecked((int)(0xffffffff)), unchecked((int)(
			0xfff8e8f8)), unchecked((int)(0xffffffff)), unchecked((int)(0xfff1e1f1)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xffe3e3e3)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0xff8f8f8f)), unchecked(
			(int)(0xffffffff)), unchecked((int)(0xff1f0f1f)), unchecked((int)(0xffffffff)), 
			unchecked((int)(0xff3f2f3f)), unchecked((int)(0xffffffff)), unchecked((int)(0xfcfcecfe
			)), unchecked((int)(0xfefeffff)), unchecked((int)(0xf8f8e8fc)), unchecked((int)(
			0xfcffff)), unchecked((int)(0xf0f0e0f0)), unchecked((int)(0xfefe)), unchecked((int
			)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x0)), unchecked(
			(int)(0x0)), unchecked((int)(0x0)), unchecked((int)(0x1e1e0e1e)), unchecked((int
			)(0xfefe)), unchecked((int)(0x3f3f2f7f)), unchecked((int)(0x7fffff)), unchecked(
			(int)(0xfcfceefe)), unchecked((int)(0xfefefefc)), unchecked((int)(0xf8f8ecfc)), 
			unchecked((int)(0xfcfcf8)), unchecked((int)(0xf0f0e8f0)), unchecked((int)(0xf8f0
			)), unchecked((int)(0x60606000)), unchecked((int)(0x60)), unchecked((int)(0x0)), 
			unchecked((int)(0x0)), unchecked((int)(0xc0c0c00)), unchecked((int)(0xc)), unchecked(
			(int)(0x1e1e2e1e)), unchecked((int)(0x3e1e)), unchecked((int)(0x3f3f6f7f)), unchecked(
			(int)(0x7f7f3f)), unchecked((int)(0xfcfeeffe)), unchecked((int)(0xfefefcfc)), unchecked(
			(int)(0xf8fdeffc)), unchecked((int)(0xfcf8f8)), unchecked((int)(0xf0faeef0)), unchecked(
			(int)(0xf0f0)), unchecked((int)(0x60746c00)), unchecked((int)(0x6060)), unchecked(
			(int)(0x282800)), unchecked((int)(0x0)), unchecked((int)(0xc5c6c00)), unchecked(
			(int)(0xc0c)), unchecked((int)(0x1ebeee1e)), unchecked((int)(0x1e1e)), unchecked(
			(int)(0x3f7fef7f)), unchecked((int)(0x7f3f3f)), unchecked((int)(0xffffeefe)), unchecked(
			(int)(0xfefcfcfc)), unchecked((int)(0xffffecfc)), unchecked((int)(0xf8f8f8)), unchecked(
			(int)(0xfefee8f0)), unchecked((int)(0xf0f0f0)), unchecked((int)(0x7c7c6000)), unchecked(
			(int)(0x606060)), unchecked((int)(0x7c7c0000)), unchecked((int)(0x44)), unchecked(
			(int)(0x7c7c0c00)), unchecked((int)(0xc0c0c)), unchecked((int)(0xfefe2e1e)), unchecked(
			(int)(0x1e1e1e)), unchecked((int)(0xffff6f7f)), unchecked((int)(0x3f3f3f)), unchecked(
			(int)(0xffffeffe)), unchecked((int)(0xfcfcfcff)), unchecked((int)(0xffffeffc)), 
			unchecked((int)(0xf8f8f8ff)), unchecked((int)(0xfefeeef0)), unchecked((int)(0xf0f0f0fe
			)), unchecked((int)(0xfefeee00)), unchecked((int)(0x6060e2fe)), unchecked((int)(
			0xfefeee00)), unchecked((int)(0xc6fe)), unchecked((int)(0xfefeee00)), unchecked(
			(int)(0xc0c8efe)), unchecked((int)(0xfefeee1e)), unchecked((int)(0x1e1e1efe)), unchecked(
			(int)(0xffffef7f)), unchecked((int)(0x3f3f3fff)), unchecked((int)(0xffffefff)), 
			unchecked((int)(0xfcfcffff)), unchecked((int)(0xffffefff)), unchecked((int)(0xf8f8ffff
			)), unchecked((int)(0xffffefff)), unchecked((int)(0xf0f1ffff)), unchecked((int)(
			0xffffefff)), unchecked((int)(0x60e3ffff)), unchecked((int)(0xffffefff)), unchecked(
			(int)(0xc7ffff)), unchecked((int)(0xffffefff)), unchecked((int)(0xc8fffff)), unchecked(
			(int)(0xffffefff)), unchecked((int)(0x1e1fffff)), unchecked((int)(0xffffefff)), 
			unchecked((int)(0x3f3fffff)) };

		internal const int Rank7 = 6;

		// Each int stores results of 32 positions, one per bit, 24Kbytes
		// A KPK bitbase index is an integer in [0, IndexMax] range
		//
		// Information is mapped in a way that minimizes the number of iterations:
		//
		// bit  0- 5: white king index
		// bit  6-11: black king index
		// bit    12: side to move 1 WHITE
		// bit 13-14: white pawn file (from FILE_H to FILE_E)
		// bit 15-17: white pawn RANK_7 - rank (from RANK_7 - RANK_7 to RANK_7 - RANK_2)
		internal virtual int Index(bool whitetoMove, int blackKingIndex, int whiteKingIndex
			, int pawnIndex)
		{
			return whiteKingIndex + (blackKingIndex << 6) + ((whitetoMove ? 1 : 0) << 12) + (
				BitboardUtils.GetFileOfIndex(pawnIndex) << 13) + ((Rank7 - BitboardUtils.GetRankOfIndex
				(pawnIndex)) << 15);
		}

		//
		//
		public virtual bool Probe(int whiteKingIndex, int whitePawnIndex, int blackKingIndex
			, bool whiteToMove)
		{
			int idx = Index(whiteToMove, blackKingIndex, whiteKingIndex, whitePawnIndex);
			return (bitbase[idx / 32] & (1 << (idx & unchecked((int)(0x1F))))) != 0;
		}

		public virtual bool Probe(Board board)
		{
			int whiteKingIndex = BitboardUtils.Square2Index(board.kings & board.whites);
			int blackKingIndex = BitboardUtils.Square2Index(board.kings & board.blacks);
			int pawnIndex = BitboardUtils.Square2Index(board.pawns);
			bool whiteToMove = board.GetTurn();
			// Pawn is black
			if ((board.pawns & board.blacks) != 0)
			{
				// flip vertical and change colors
				int aux = whiteKingIndex;
				whiteKingIndex = 63 - blackKingIndex;
				blackKingIndex = 63 - aux;
				pawnIndex = 63 - pawnIndex;
				whiteToMove = !whiteToMove;
			}
			if (BitboardUtils.GetFileOfIndex(pawnIndex) > 3)
			{
				// flip horizontal
				whiteKingIndex = BitboardUtils.FlipHorizontalIndex(whiteKingIndex);
				blackKingIndex = BitboardUtils.FlipHorizontalIndex(blackKingIndex);
				pawnIndex = BitboardUtils.FlipHorizontalIndex(pawnIndex);
			}
			return Probe(whiteKingIndex, pawnIndex, blackKingIndex, whiteToMove);
		}
	}
}
