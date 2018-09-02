using System;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Rings.Members;

namespace Math.Algebra.Structures.Fields.Members
{
	public abstract class FieldMember : RingMember, IInvertible
	{
		internal abstract override T Add<T>(T other);
		
		public abstract override T Null<T>();

		public abstract override bool IsNull();
		
		public abstract override T Negative<T>();
		
		internal abstract override T Multiply<T>(T other);

		public abstract override T Unit<T>();

		public abstract override bool IsUnit();

		public abstract T Inverse<T>() where T : IInvertible;

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
