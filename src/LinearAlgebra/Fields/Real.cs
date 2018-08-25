using System;
using System.Globalization;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Main;

namespace LinearAlgebra.Fields
{
	/// <summary>
	/// A class that represents real numbers
	/// </summary>
	public class Real : FieldMember, INumerical
	{
		/**
		 * Contains the double value of this real number
		 */
		#region Properties
		public new double Value { get; set; }
		#endregion

		#region Constructors
		public Real() : base(0)
		{

		}

		public Real(double value) : base(value)
		{
			Value = value;
		}
		#endregion

		#region Override Methods
		
		internal override T Add<T>(T other)
		{
			if (other is Real r)
			{
				return (T)(FieldMember)new Real(Value + r.Value);
			}
			throw new IncorrectFieldException(GetType(), "added", other.GetType());
		}

		internal override T Negative<T>()
		{
			return (T)(FieldMember)new Real(-Value);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Real c)
				return (T)(FieldMember)new Real(Value * c.Value);
			throw new IncorrectFieldException(GetType(), "added", other.GetType());
		}

		internal override T Inverse<T>()
		{
			return (T)(FieldMember)new Real(1 / Value);
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
				return (T)(FieldMember) new Real(0);
			throw new IncorrectFieldException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
				return (T)(FieldMember)new Real(1);
			throw new IncorrectFieldException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Value.CloseTo(0);

		public override bool IsUnit() => Value.CloseTo(1);

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
			return new Real(Math.Round(Value));
		}

		public INumerical Log10()
		{
			return new Real(Math.Log10(Value));
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

		#region Conversions
		
		public static implicit operator double(Real r)
		{
			return r.Value;
		}

		public static implicit operator Real(double r)
		{
			return new Real(r);
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
