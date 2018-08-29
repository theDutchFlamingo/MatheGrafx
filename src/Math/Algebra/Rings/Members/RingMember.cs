using Math.Groups;

namespace Math.Algebra.Rings.Members
{
	public abstract class RingMember : GroupMember, INegatable
	{
		public abstract T Negative<T>() where T : INegatable;
	}
}
