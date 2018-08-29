using Math.Algebra.Groups;
using Math.Algebra.Groups.Members;

namespace Math.Algebra.Rings.Members
{
	public abstract class RingMember : GroupMember, INegatable
	{
		public abstract T Negative<T>() where T : INegatable;
	}
}
