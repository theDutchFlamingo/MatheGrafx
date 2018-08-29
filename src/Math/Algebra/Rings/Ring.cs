using Math.Algebra.Groups;
using Math.Algebra.Rings.Members;

namespace Math.Algebra.Rings
{
	public abstract class Ring<T> : Group<T> where T : RingMember
	{
		public static Ring<Integer> Integers = new WholeNumbers();

		public abstract T Unit();
	}
}
