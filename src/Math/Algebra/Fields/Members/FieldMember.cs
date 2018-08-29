using System;
using Math.Algebra.Groups.Members;
using Math.Algebra.Rings.Members;

namespace Math.Algebra.Fields.Members
{
	public abstract class FieldMember : RingMember, IInvertible, IEquatable<FieldMember>
	{
		public abstract override T Negative<T>();

		public abstract T Inverse<T>() where T : IInvertible;

		/// <summary>
		/// The type of product that is used when calculating when calculating
		/// an inner product (with vectors); defaults to normal multiplication.
		/// For example, with complex numbers this would be left * right.Conjugate(),
		/// for matrices, this would be the determinant.
		/// </summary>
		/// <param name="fieldMember"></param>
		/// <returns></returns>
		public virtual T Inner<T>(T fieldMember) where T : FieldMember => Multiply(fieldMember);

		public abstract bool Equals(FieldMember other);

		public override bool Equals(GroupMember other)
		{
			if (other is FieldMember f)
			{
				return Equals(f);
			}

			return false;
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
