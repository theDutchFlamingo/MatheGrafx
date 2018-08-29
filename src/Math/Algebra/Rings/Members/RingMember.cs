using LinearAlgebra.Groups;

namespace LinearAlgebra.Algebra.Rings.Members
{
	public abstract class RingMember : GroupMember, INegatable
	{
		public abstract T Negative<T>() where T : INegatable;
	}
}
