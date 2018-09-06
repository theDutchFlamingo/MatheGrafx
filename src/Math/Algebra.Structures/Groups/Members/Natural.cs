using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;
using static System.BitConverter;

namespace Math.Algebra.Structures.Groups.Members
{
	public class Natural : MonoidMember
	{
		private byte[] _value = {0};

		/// <summary>
		/// A long accessor for the _value
		/// </summary>
		public long Value
		{
			get
			{
				TryConvert(_value, out long result);
				return result;
			}
			set => _value = GetBytes(value);
		}

		public Natural()
		{
			Value = 0;
		}

		public Natural(int value)
		{
			Value = System.Math.Abs(value);
		}

		public Natural(long value)
		{
			Value = System.Math.Abs(value);
		}

		public Natural(uint value)
		{
			Value = value;
		}

		public Natural(ulong value)
		{
			if (value > Int32.MaxValue)
			{
				throw new ArgumentException("Value exceeds the maximum value for integers");
			}
		}

		public Natural(string value, bool hex = true)
		{

		}

		#region Overrides

		internal override T Add<T>(T other)
		{
			if (other is Natural n)
			{
				return (T) (MonoidMember) new Natural(n.Value + Value);
			}

			if (other is Integer i && i >= 0)
			{
				return (T) (MonoidMember) new Natural(i.Value + Value);
			}

			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
				return (T)(MonoidMember) new Natural(0);
			throw new IncorrectSetException(this, "null", typeof(T));
		}

		public override bool Equals<T>(T other)
		{
			switch (other) {
				case Natural n:
					return n.Value == Value;
				case int i:
					return i == Value;
				case long l:
					return l == Value;
				case uint i:
					return i == Value;
				case ulong l:
					return l == (ulong)Value;
			}

			return false;
		}

		#endregion

		#region Conversion

		public static implicit operator Natural(int i)
		{
			return new Natural(i);
		}

		public static implicit operator Natural(long l)
		{
			return new Natural(l);
		}

		public static implicit operator int(Natural n)
		{
			if ((long)n > Int32.MaxValue)
			{
				throw new InvalidCastException("Value exceeds max Int32 value");
			}

			return (int) n.Value;
		}

		public static implicit operator long(Natural n)
		{
			return n.Value;
		}

		#endregion

		#region Bytes and Conversion

		private static bool TryConvert(byte[] value, out int result)
		{
			try
			{
				result = ToInt32(value, 0);
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = 0;
				return false;
			}
		}

		private static bool TryConvert(byte[] value, out long result)
		{
			try
			{
				result = ToInt64(value, 0);
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = 0;
				return false;
			}
		}

		private static bool TryConvert(byte[] value, out double result)
		{
			try
			{
				result = ToDouble(value, 0);
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = 0;
				return false;
			}
		}

		public string ToHexString()
		{
			try
			{
				return BitConverter.ToString(_value);
			}
			catch (ArgumentOutOfRangeException)
			{
				return "";
			}
		}

		public static byte[] FromHexString(string value)
		{
			if (!Regex.IsMatch(value, @"^[0-9a-fA-F]+$"))
			{
				throw new ArgumentException("Hexadecimal string must contain only numbers, " +
				                            "and letters A to F");
			}

			List<char> digits = value.Reverse().ToList();
			List<byte> result = new List<byte>();

			for (int i = 0; i < digits.Count; i += 2)
			{
				if (digits.Count >= i + 2)
				{
					byte b = 0;
				}
			}

			return result.ToArray();
		}

		#endregion
	}
}
