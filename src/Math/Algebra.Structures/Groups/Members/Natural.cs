using System;
using System.Text.RegularExpressions;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Ordering;
using Math.Algebra.Structures.Rings.Members;
using Math.Bytes;
using Math.Exceptions;
using Math.Parsing;
using Math.Rationals;
using static System.BitConverter;

namespace Math.Algebra.Structures.Groups.Members
{
	public class Natural : MonoidMember, ITotallyOrdered, IParsable<Natural>
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
			set => _value = BitConverter.GetBytes(value);
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

		public Natural(byte[] value)
		{
			_value = value;
		}

		public Natural(string value, bool hex = true)
		{
			_value = hex ? Hexadecimal.FromHexadecimal(value) : Hexadecimal.FromDecimal(value);
		}

		#region Operators

		public static Natural operator +(Natural left, Natural right)
		{
			return left.Add(right);
		}

		public static Integer operator -(Natural n)
		{
			return new Integer(n, false);
		}

		public static Integer operator -(Natural left, Natural right)
		{
			return new Integer(left.Difference(right), left > right);
		}

		public static Natural operator *(Natural left, Natural right)
		{
			return new Natural(left._value.Multiply(right._value));
		}

		public static bool operator >(Natural left, Natural right)
		{
			return left.GreaterThan(right);
		}

		public static bool operator <(Natural left, Natural right)
		{
			return left.LessThan(right);
		}

		#endregion

		#region Overrides

		internal override T Add<T>(T other)
		{
			switch (other) {
				case Natural n:
					return (T) (MonoidMember) new Natural(n._value.Add(_value));
				case Integer i when i >= 0:
					return (T) (MonoidMember) new Natural(i.Value + Value);
			}

			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T)(MonoidMember) new Natural(0);
			}
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

		public bool GreaterThan<T>(T other) where T : ITotallyOrdered
		{
			switch (other)
			{
				case Natural n:
					return _value.GreaterThan(n._value);
				case Integer i:
					return i >= 0 && _value.GreaterThan(i.Absolute._value);
				case Fraction f:
					return this * f.Den > f.Num;
				case Real r:
					return (double)(Fraction)this > (double)r;
					//TODO re-implement this when Real has a better Value
			}


			throw new IncorrectSetException(GetType(), "compared", typeof(T));
		}

		public bool LessThan<T>(T other) where T : ITotallyOrdered
		{
			switch (other)
			{
				case Natural n:
					return n._value.GreaterThan(_value);
				case Integer i:
					return i >= 0 && i.Absolute._value.GreaterThan(_value);
				case Fraction f:
					return this * f.Den < f.Num;
				case Real r:
					return (double)(Fraction)this > (double)r;
					//TODO re-implement this when Real has a better *Value* type
			}


			throw new IncorrectSetException(GetType(), "compared", typeof(T));
		}

		public Natural FromString(string value)
		{
			return new Natural(value);
		}

		public Natural FromString(string value, bool hex)
		{
			return new Natural(value, hex);
		}

		#endregion

		#region Conversion

		public static explicit operator Natural(int i)
		{
			return new Natural(i);
		}

		public static explicit operator Natural(long l)
		{
			return new Natural(l);
		}

		public static implicit operator Natural(uint i)
		{
			return new Natural(i);
		}

		public static implicit operator Natural(ulong l)
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

		public static implicit operator Integer(Natural n)
		{
			return new Integer(n);
		}

		public static implicit operator Fraction(Natural n)
		{
			return new Fraction(n);
		}

		#endregion

		#region Bytes and Conversion

		/// <summary>
		/// Gets the absolute difference between these two natural numbers.
		/// So left - right is always the same as right - left.
		/// </summary>
		/// <param name="right"></param>
		/// <returns></returns>
		public Natural Difference(Natural right)
		{
			return new Natural(this > right
				? _value.Subtract(right._value)
				: right._value.Subtract(_value));
		}

		public byte[] GetBytes()
		{
			return _value;
		}

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

		public override string ToString()
		{
			return _value.ToHexString().ToDecimal();
		}

		public string ToString(string format)
		{
			throw new NotImplementedException();
		}

		public string ToHexString()
		{
			return _value.ToHexString();
		}

		public static Natural Parse(string value, bool hex = false)
		{
			if (!hex && !Regex.IsMatch(value, @"^[0-9]+$") ||
			    hex && !Regex.IsMatch(value, @"^[0-9a-fA-F]+$"))
			{
				throw new ArgumentException("String was not in a valid number format.");
			}

			return new Natural(Hexadecimal.FromDecimal(value));
		}

		public static bool TryParse(string value, out Natural result, bool hex = false)
		{
			try
			{
				result = Parse(value, hex);
				return true;
			}
			catch (FormatException)
			{
				result = 0;
				return false;
			}
		}

		#endregion
	}
}
