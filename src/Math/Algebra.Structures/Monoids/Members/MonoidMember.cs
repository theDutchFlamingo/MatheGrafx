namespace Math.Algebra.Structures.Monoids.Members
{
	public abstract class MonoidMember : Element
	{
		internal abstract T Add<T>(T other) where T : MonoidMember;

		public abstract T Null<T>() where T : MonoidMember;

		public abstract bool Equals<T>(T other);
	}
}
