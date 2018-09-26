using System;
using System.Text.RegularExpressions;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Ordering;
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;
using Math.LinearAlgebra;
using Math.Parsing;
using Math.Rationals;
using static System.Math;

namespace Math.Algebra.Structures.Fields.Members
{
	/// <summary>
	/// A class that represents real numbers
	/// </summary>
	public class Real : FieldMember, INumerical, ITotallyOrdered, IParsable<Real>
	{
		/**
		 * Contains the double value of this real number
		 */
		#region Properties
		
		///<summary>
		/// The value of this Real number is calculated as Value * 10^Offset
		/// </summary>
		private Integer Offset { get; }
		private Integer Value { get; }

		public Real Absolute => new Real(Value.Absolute, Offset);
		
		#endregion

		#region Constructors
		
		public Real()
		{
			
		}

		public Real(Fraction value)
		{

		}

		public Real(Integer value)
		{
			Value = value;
			Offset = 0;
		}

		public Real(Integer value, Integer offset)
		{
			Value = value;
			Offset = offset;
		}

		public Real(double value)
		{
			// TODO convert double to integer with correct offset
			bool positive = value >= 0;
			Integer offset = 0;
			value = Abs(value);

			if (value > 1)
			{
				for (var i = 0;; i++)
				{
					if (value / 10 < 1)
					{
						break;
					}

					value /= 10;
					offset++;
				}
			}
			else if (value.CloseTo(0))
			{
				Value = new Integer(0);
			}
			else
			{
				offset--;
				for (var i = 0;; i++)
				{
					if (System.Math.Round(value).CloseTo(value))
					{
						break;
					}

					value *= 10;
					offset--;
				}
			}

			// TODO

			Value = new Integer(new Natural((int) (value * Pow(10, -offset))), positive);
			Offset = offset;
		}

		public Real(string value, bool hex = false)
		{
			if (!hex && !Regex.IsMatch(value, @"^-?[0-9]*(\.[0-9]+)?$") ||
			    hex && !Regex.IsMatch(value, @"^-?[0-9a-fA-F]*(\.[0-9a-fA-F]+)?$") ||
			    value == "")
			{
				throw new ArgumentException("String is not in correct format.");
			}

			bool positive = !value.StartsWith("-");

			if (value.Contains("."))
			{
				string before, after;
				(before, after) = (value.Split('.')[0], value.Split('.')[1]);

				int offset = value.Length - value.IndexOf('.');
				Offset = value.Length - value.IndexOf('.');

				if (before == "")
				{
					Value = Integer.Parse(after, positive);
				}
				else
				{
					Value = Integer.Parse(before, positive) * new Integer(10) ^ Offset;
				}
			}
			else
			{
				int offset = Regex.Match(value, @"^.*?(0*)$").Groups[1].Length;
				Value = Integer.Parse(value.TrimEnd('0'), positive);
				Offset = Value == 0 ? 0 : offset;
			}
		}

		#endregion

		#region Tests

		public bool IsInteger()
		{
			return Offset > 0;
		}

		#endregion

		#region Override Methods

		internal override T Add<T>(T other)
		{
			if (!(other is Real r))
			{
				throw new IncorrectSetException(GetType(), "added", other.GetType());
			}

			Integer min = Min(Offset, r.Offset);
			Natural diff = (Offset - r.Offset).Absolute;

			if (this > r)
			{
				return (T)(MonoidMember)new Real(Value + r.Value * Integer.Pow(10, diff), min);
			}

			return (T)(MonoidMember)new Real(Value * Integer.Pow(10, diff) + r.Value, min);
		}

		public override T Negative<T>()
		{
			return (T)(INegatable)new Real(-Value);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Real c)
			{
				return (T)(RingMember)new Real(Value * c.Value);
			}
			throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
		}

		public override T Inverse<T>()
		{
			return (T)(IInvertible)new Real(1 / Value);
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T)(MonoidMember) new Real(0);
			}
			throw new IncorrectSetException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T)(GroupMember) new Real(1);
			}
			throw new IncorrectSetException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Value.Equals(0);

		public override bool IsUnit() => Value.Equals(1);

		public bool GreaterThan<T>(T other) where T : ITotallyOrdered
		{
			switch (other) {
				case Real r:
					return Value > r.Value;
				case Integer i:
					return Value > i;
				case Fraction r:
					return Value > (double)r;
			}// TODO improve these comparisons

			throw new IncorrectSetException(GetType(), "compared", typeof(T));
		}

		public bool LessThan<T>(T other) where T : ITotallyOrdered
		{
			switch (other) {
				case Real r:
					return Value < r.Value;
				case Integer i:
					return Value < i;
				case Fraction r:
					return Value < (double) r;
			}// TODO improve these comparisons

			throw new IncorrectSetException(GetType(), "compared", typeof(T));
		}

		[Obsolete]
		public override double ToDouble()
		{
			return this;
		}

		public override bool Equals<T>(T other)
		{
			switch (other) {
				case Real r:
					return this.CloseTo(r);
				case Integer i:
					return ((double)this).CloseTo(i);
				case Fraction f:
					return this.CloseTo((double)f);
				case double d:
					return this.CloseTo(d);
			}

			return false;
		}

		public INumerical Round()
		{
			return Value - Value % (Integer) Pow(10, Offset);
		}

		public INumerical Log10()
		{
			return new Real(System.Math.Log10(Value));
		}

		public INumerical LongestValue()
		{
			return this;
		}

		public Real FromString(string value)
		{
			return Set.Reals.Create(value);
		}

		#endregion

		#region Operators

		/// <summary>
		/// Add the two together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Real operator +(Real left, Real right)
		{
			return left.Add(right);
		}

		/// <summary>
		/// Negative of this FieldMember
		/// </summary>
		/// <param name="fieldMember"></param>
		/// <returns></returns>
		public static Real operator -(Real fieldMember)
		{
			return fieldMember.Negative<Real>();
		}

		/// <summary>
		/// Subtract right from left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Real operator -(Real left, Real right)
		{
			return left + -right;
		}

		/// <summary>
		/// Multiply the two together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Real operator *(Real left, Real right)
		{
			return left.Multiply(right);
		}

		/// <summary>
		/// Divide one FieldMember&lt;V&gt; by another
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Real operator /(Real left, Real right)
		{
			return left * right.Inverse<Real>();
		}

		#endregion

		#region Comparisons

		public static bool operator >(Real left, Real right)
		{
			return left.GreaterThan(right);
		}

		public static bool operator <(Real left, Real right)
		{
			return left.LessThan(right);
		}

		public static bool operator ==(Real left, Real right)
		{
			return left?.Equals(right) ?? false;
		}

		public static bool operator !=(Real left, Real right)
		{
			return !(left == right);
		}

		public static bool operator <=(Real left, Real right)
		{
			return left < right || left == right;
		}

		public static bool operator >=(Real left, Real right)
		{
			return left > right || left == right;
		}

		public override bool Equals(object other)
		{
			if (other is double d)
			{
				return d.CloseTo(Value);
			}

			if (other is Real r)
			{
				return r.Equals(this);
			}

			return false;
		}

		public override int GetHashCode() => Value.GetHashCode();

		#endregion

		#region Conversions

		/// <summary>
		/// Explicit because not all Reals can be converted to double
		/// </summary>
		/// <param name="r"></param>
		public static implicit operator double(Real r)
		{
			string intr = r.Value.ToString();
			return Double.Parse(intr.Length > 16 ? intr.Remove(16) : intr);
		}

		public static implicit operator Real(double r)
		{
			return new Real(r);
		}

		public override string ToString()
		{
			if (IsInteger())
			{
				return Value.ToString().Insert(Value.ToString().Length - Offset, ".");
			}

			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}
		
		#endregion
	}
}
