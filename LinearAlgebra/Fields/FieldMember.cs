using System;

namespace LinearAlgebra.Fields
{
	public abstract class FieldMember : IEquatable<FieldMember>
	{
		internal abstract T Add<T>(T other) where T : FieldMember;

		internal abstract T Negative<T>() where T : FieldMember;

		internal abstract T Multiply<T>(T other) where T : FieldMember;

		internal abstract T Inverse<T>() where T : FieldMember;

		public abstract T Null<T>() where T : FieldMember;

		public abstract T Unit<T>() where T : FieldMember;

		/// <summary>
		/// The type of product that is used when calculating when calculating
		/// an inner product (with vectors); defaults to normal multiplication.
		/// For example, with complex numbers this would be left * right.Conjugate().
		/// </summary>
		/// <param name="fieldMember"></param>
		/// <returns></returns>
		public virtual T Inner<T>(T fieldMember) where T : FieldMember => Add(fieldMember);

		public abstract double ToDouble();

		public object Value { get; set; }

		protected FieldMember(object value)
		{
			Value = value;
		}

		/// <summary>
		/// Whether this member of the FieldMember is the null member
		/// </summary>
		/// <returns></returns>
		public bool IsNull => Equals(Null<FieldMember>());

		/// <summary>
		/// Whether this member of the FieldMember is the unit member
		/// </summary>
		/// <returns></returns>
		public bool IsUnit => Equals(Unit<FieldMember>());

		public abstract bool Equals<T>(T other);

		public virtual bool Equals(FieldMember other)
		{
			if (GetType() != other?.GetType()) return false;

			return Value == other.Value;
		}

		/// <summary>
		/// Gets the double version of this object; not necessarily possible for all instances.
		/// For complex numbers, for example, only possible if the imaginary part is 0.
		/// </summary>
		/// <param name="fieldMember"></param>
		public static explicit operator double (FieldMember fieldMember)
		{
			return fieldMember.ToDouble();
		}
	}
}
