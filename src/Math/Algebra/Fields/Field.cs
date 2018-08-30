using Math.Algebra.Fields.Members;
using Math.Algebra.Rings;
using Math.Rationals;

namespace Math.Algebra.Fields
{
	/// <summary>
	/// Represents a set of mathematical objects that obey certain properties, more
	/// specifically, it is a ring with an inverse corresponding to the second operator. <br></br>
	/// In summary, it has two operators, usually called addition and multiplication,
	/// with corresponding identity elemtents and inverses for all members that are not
	/// the null element. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Field<T> : Ring<T> where T : FieldMember
	{
		public abstract override T Create(params object[] value);

		public abstract override T Null();

		public abstract override T Unit();

		public T Inverse(T member)
		{
			return member.Negative<T>();
		}

		public bool Contains(T m)
		{
			return m?.GetType() == typeof(T);
		}
	}
}
