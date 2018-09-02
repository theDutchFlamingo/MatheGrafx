using Math.Algebra.Structures.Monoids.Members;

namespace Math.Algebra.Structures.Monoids
{
	/// <summary>
	/// A monoid is a set equipped with an operator (usually called addition)
	/// and an identity element (usually called null) where the operation is
	/// associative. <br></br>
	/// Examples of monoid are the natural numbers (counting numbers).
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Monoid<T> : Set where T : MonoidMember
	{
		public abstract T Create(params object[] value);

		public T Add(T left, T right)
		{
			return left.Add(right);
		}

		public abstract T Null();

		public bool IsNull(T member)
		{
			return member.Equals(Null());
		}
	}
}
