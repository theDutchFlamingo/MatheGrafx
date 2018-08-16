using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra.Fields
{
	public abstract class FieldMember<T> : IEquatable<FieldMember<T>>
	{
		public abstract FieldMember<T> AdditiveInverse();

		public abstract FieldMember<T> MultiplicativeInverse();

		protected abstract FieldMember<T> Add(FieldMember<T> fieldMember);

		protected abstract FieldMember<T> Multiply(FieldMember<T> fieldMember);

		/// <summary>
		/// The type of product that is used when calculating when calculating
		/// an inner product (with vectors); defaults to normal multiplication.
		/// For example, with complex numbers this would be left * right.Conjugate().
		/// </summary>
		/// <param name="fieldMember"></param>
		/// <returns></returns>
		public virtual FieldMember<T> Inner(FieldMember<T> fieldMember) => Add(fieldMember);

		public abstract FieldMember<T> Null();

		public abstract FieldMember<T> Unit();

		public abstract double ToDouble();

		public T Value { get; set; }

		protected FieldMember()
		{
			Value = Null().Value;
		}

		protected FieldMember(T value)
		{
			Value = value;
		}

		/// <summary>
		/// Whether this member of the FieldMember<V> is the null member
		/// </summary>
		/// <returns></returns>
		public bool IsNull => Equals(Null());

		/// <summary>
		/// Whether this member of the FieldMember<V> is the unit member
		/// </summary>
		/// <returns></returns>
		public bool IsUnit => Equals(Unit());

		public abstract bool Equals(FieldMember<T> other);

		/// <summary>
		/// Add the two together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static FieldMember<T> operator +(FieldMember<T> left, FieldMember<T> right)
		{
			return left.Add(right);
		}

		/// <summary>
		/// Negative of this FieldMember
		/// </summary>
		/// <param name="fieldMember"></param>
		/// <returns></returns>
		public static FieldMember<T> operator -(FieldMember<T> fieldMember)
		{
			return fieldMember.AdditiveInverse();
		}

		/// <summary>
		/// Subtract right from left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static FieldMember<T> operator -(FieldMember<T> left, FieldMember<T> right)
		{
			return left + -right;
		}

		/// <summary>
		/// Multiply the two together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static FieldMember<T> operator *(FieldMember<T> left, FieldMember<T> right)
		{
			return left.Multiply(right);
		}

		/// <summary>
		/// Divide one FieldMember<V> by another
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static FieldMember<T> operator /(FieldMember<T> left, FieldMember<T> right)
		{
			return left * right.MultiplicativeInverse();
		}

		/// <summary>
		/// Gets the double version of this object; not necessarily possible for all instances.
		/// For complex numbers, for example, only possible if the imaginary part is 0.
		/// </summary>
		/// <param name="fieldMember"></param>
		public static explicit operator double (FieldMember<T> fieldMember)
		{
			return fieldMember.ToDouble();
		}
	}
}
