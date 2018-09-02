using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Groups.Members;

namespace Math.Algebra.Structures.Rings.Members
{
	public abstract class RingMember : GroupMember
	{
		internal abstract override T Add<T>(T other);

		public abstract override T Null<T>();

		public abstract override bool IsNull();

		public abstract override T Negative<T>();
		
		internal abstract T Multiply<T>(T right) where T : RingMember;

		public abstract T Unit<T>() where T : RingMember;

		public abstract bool IsUnit();
		
		/// <summary>
		/// The type of product that is used when calculating when calculating
		/// an inner product (with vectors); defaults to normal multiplication.
		/// For example, with complex numbers this would be left * right.Conjugate(),
		/// for matrices, this would be the determinant.
		/// </summary>
		/// <param name="ringMember"></param>
		/// <returns></returns>
		public virtual T Inner<T>(T ringMember) where T : RingMember => Multiply(ringMember);

		public abstract override bool Equals<T>(T other);
	}
}
