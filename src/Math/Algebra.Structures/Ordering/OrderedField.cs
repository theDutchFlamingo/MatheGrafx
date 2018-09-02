using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;

namespace Math.Algebra.Structures.Ordering
{
	public abstract class OrderedField<T> : Field<T> where T : FieldMember, ITotallyOrdered
	{
		public bool Equal(T left, T right)
		{
			return left.Equals(right);
		}

		public bool LessThan(T left, T right)
		{
			return left.LessThan(right);
		}

		public bool GreaterThan(T left, T right)
		{
			return left.GreaterThan(right);
		}

		public bool IsPositive(T member)
		{
			return member.GreaterThan(Null());
		}

		public bool IsNegative(T member)
		{
			return member.LessThan(Null());
		}
	}
}
