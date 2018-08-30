using Math.Algebra.Groups;
using Math.Algebra.Rings.Members;

namespace Math.Algebra.Rings
{
	/// <summary>
	/// A ring is a group that also has a second operation (usually called
	/// multiplication), and a corresponding identity element (usually called
	/// the unit). <br></br>
	/// In summary, a group has two operations, usually called addition and
	/// multiplication, which both have an identity element, and one of the
	/// operations has an inverse for every member of the ring. <br></br>
	/// An example of a ring is the integers, with adition and multiplication.
	/// The set of polynomials also forms a ring, as it supports addition and
	/// multiplication as well.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Ring<T> : Group<T> where T : RingMember
	{
		public abstract override T Create(params object[] value);

		public abstract override T Null();

		public T Multiply(T left, T right)
		{
			return left.Add(right);
		}

		public abstract T Unit();
	}
}
