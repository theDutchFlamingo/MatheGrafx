using System;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Main;

namespace LinearAlgebra.Fields
{
	/// <summary>
	/// A class that represents real numbers
	/// </summary>
	public class Real : FieldMember, INumerical
	{
		/*
		 * Contains the 
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
			return (T)(FieldMember)(-this);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Complex c)
				return (T)(FieldMember)(this * c);
			throw new IncorrectFieldException(GetType(), "added", other.GetType());
		}

		internal override T Inverse<T>()
		{
			return (T)(FieldMember)(1 / this);
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
				return (T)(FieldMember)new Complex(0, 0);
			throw new IncorrectFieldException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
				return (T)(FieldMember)new Complex(1, 0);
			throw new IncorrectFieldException(this, "unit", typeof(T));
		}

		public override double ToDouble()
		{
			return this;
		}

		public override bool Equals<T>(T other)
		{
			if (other is Real r)
				return LinearMath.Equals(this, r);
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
		/// Divide one FieldMember<V> by another
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

		public string ToString(string format)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
