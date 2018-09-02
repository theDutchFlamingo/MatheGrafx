using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids;

namespace Math.Algebra.Structures.Groups
{
	/// <summary>
	/// A group is a monoid in which each element also has an inverse
	/// (usually called negative in the case of additive inverses). <br></br>
	/// In summary, a group has one operation with an identity and inverses
	/// for every member of the group. <br></br>
	/// An example of a group is a rubix cube group, where the elements are the moves,
	/// the operation is the composition of two moves (one after the other),
	/// the identity is not moving anything, and the inverse is simply the opposite move.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Group<T> : Monoid<T> where T : GroupMember
	{
		public abstract override T Create(params object[] value);

		public abstract override T Null();

		public T Negative(T member)
		{
			return member.Negative<T>();
		}
	}
}
