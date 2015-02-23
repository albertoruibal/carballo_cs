using System;
using System.IO;

namespace Util
{
	public class BigEndianReader : BinaryReader
	{
		private byte[] a16 = new byte[2];
		private byte[] a32 = new byte[4];
		private byte[] a64 = new byte[8];

		public BigEndianReader(Stream stream) : base(stream) { }

		public override Int16 ReadInt16()
		{
			a16 = base.ReadBytes(2);
			Array.Reverse(a16);
			return BitConverter.ToInt16(a16, 0);
		}

		public override int ReadInt32()
		{
			a32 = base.ReadBytes(4);
			Array.Reverse(a32);
			return BitConverter.ToInt32(a32, 0);
		}

		public override Int64 ReadInt64()
		{
			a64 = base.ReadBytes(8);
			Array.Reverse(a64);
			return BitConverter.ToInt64(a64, 0);
		}

		public override UInt16 ReadUInt16()
		{
			a16 = base.ReadBytes(2);
			Array.Reverse(a16);
			return BitConverter.ToUInt16(a16, 0);
		}

		public override UInt32 ReadUInt32()
		{
			a32 = base.ReadBytes(4);
			Array.Reverse(a32);
			return BitConverter.ToUInt32(a32, 0);
		}

		public override Single ReadSingle()
		{
			a32 = base.ReadBytes(4);
			Array.Reverse(a32);
			return BitConverter.ToSingle(a32, 0);
		}

		public override UInt64 ReadUInt64()
		{
			a64 = base.ReadBytes(8);
			Array.Reverse(a64);
			return BitConverter.ToUInt64(a64, 0);
		}

		public override Double ReadDouble()
		{
			a64 = base.ReadBytes(8);
			Array.Reverse(a64);
			return BitConverter.ToUInt64(a64, 0);
		}

		public string ReadStringToNull()
		{
			string result = "";
			char c;
			for (int i = 0; i < base.BaseStream.Length; i++)
			{
				if ((c = (char)base.ReadByte()) == 0)
				{
					break;
				}
				result += c.ToString();
			}
			return result;
		}
	}

	public class BigEndianWriter : BinaryWriter
	{
		private byte[] a16 = new byte[2];
		private byte[] a32 = new byte[4];
		private byte[] a64 = new byte[8];

		public BigEndianWriter(Stream output) : base(output) { }

		public override void Write(Int16 value)
		{
			a16 = BitConverter.GetBytes(value);
			Array.Reverse(a16);
			base.Write(a16);
		}

		public override void Write(Int32 value)
		{
			a32 = BitConverter.GetBytes(value);
			Array.Reverse(a32);
			base.Write(a32);
		}

		public override void Write(Int64 value)
		{
			a64 = BitConverter.GetBytes(value);
			Array.Reverse(a64);
			base.Write(a64);
		}

		public override void Write(UInt16 value)
		{
			a16 = BitConverter.GetBytes(value);
			Array.Reverse(a16);
			base.Write(a16);
		}

		public override void Write(UInt32 value)
		{
			a32 = BitConverter.GetBytes(value);
			Array.Reverse(a32);
			base.Write(a32);
		}

		public override void Write(Single value)
		{
			a32 = BitConverter.GetBytes(value);
			Array.Reverse(a32);
			base.Write(a32);
		}

		public override void Write(UInt64 value)
		{
			a64 = BitConverter.GetBytes(value);
			Array.Reverse(a64);
			base.Write(a64);
		}

		public override void Write(Double value)
		{
			a64 = BitConverter.GetBytes(value);
			Array.Reverse(a64);
			base.Write(a64);
		}
	}
}

