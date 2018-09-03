using System;
using System.Globalization;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Ordering;
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;
using Math.LinearAlgebra;

namespace Math.Algebra.Structures.Fields.Members
{
	/// <summary>
	/// A class that represents real numbers
	/// </summary>
	public class Real : FieldMember, INumerical, ITotallyOrdered
	{
		/**
		 * Contains the double value of this real number
		 */
		#region Properties

		private double Value { get; }
		
		#endregion

		#region Constructors
		
		public Real()
		{
			
		}

		public Real(double value)
		{
			Value = value;
		}
		
		#endregion

		#region Override Methods

		internal override T Add<T>(T other)
		{
			if (other is Real r)
			{
				return (T)(MonoidMember)new Real(Value + r.Value);
			}
			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T Negative<T>()
		{
			return (T)(INegatable)new Real(-Value);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Real c)
				return (T)(RingMember)new Real(Value * c.Value);
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
				return (T)(GroupMember)new Real(1);
			}
			throw new IncorrectSetException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Value.CloseTo(0);

		public override bool IsUnit() => Value.CloseTo(1);

		public bool GreaterThan<T>(T other)
		{
			if (other is Real r)
			{
				return Value > r.Value;
			}

			throw new IncorrectSetException(GetType(), "compared", typeof(T));
		}

		public bool LessThan<T>(T other)
		{
			if (other is Real r)
			{
				return Value < r.Value;
			}

			throw new IncorrectSetException(GetType(), "compared", typeof(T));
		}

		[Obsolete]
		public override double ToDouble()
		{
			return this;
		}

		public override bool Equals<T>(T other)
		{
			if (other is Real r)
				return this.CloseTo(r);
			return false;
		}

		public INumerical Round()
		{
			return new Real(System.Math.Round(Value));
		}

		public INumerical Log10()
		{
			return new Real(System.Math.Log10(Value));
		}

		public INumerical LongestValue()
		{
			return this;
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
			return left.LessThan(right);
		}

		public static bool operator <(Real left, Real right)
		{
			return left.GreaterThan(right);
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

		public static implicit operator double(Real r)
		{
			return r.Value;
		}

		public static implicit operator Real(double r)
		{
			return new Real(r);
		}

		public static implicit operator Real(int i)
		{
			return new Real(i);
		}

		public static implicit operator Real(long l)
		{
			return new Real(l);
		}

		public static implicit operator Real(float f)
		{
			return new Real(f);
		}

		public override string ToString()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}
		
		#endregion
	}
}
