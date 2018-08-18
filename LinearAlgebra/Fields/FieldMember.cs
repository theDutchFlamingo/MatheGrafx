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
		public virtual T Inner<T>(T fieldMember) where T : FieldMember => Multiply(fieldMember);

		public abstract double ToDouble();

		public object Value { get; set; }

		protected FieldMember(object value)
		{
			Value = value;
		}

		/// <summary>
		/// Whether this member of the FieldMember is the null member.
		/// Is abstract because the subtype needs to test Equals(Null&lt;Type&gt;)
		/// with Type the Type corresponding to the inheriting class.
		/// </summary>
		/// <returns></returns>
		public abstract bool IsNull();

		/// <summary>
		/// Whether this member of the FieldMember is the unit member.
		/// Is abstract because the subtype needs to test Equals(Null&lt;Type&gt;)
		/// with Type the Type corresponding to the inheriting class.
		/// </summary>
		/// <returns></returns>
		public abstract bool IsUnit();

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
