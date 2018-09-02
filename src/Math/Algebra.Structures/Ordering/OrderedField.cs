using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;

namespace Math.Algebra.Structures.Ordering
{
	public abstract class OrderedField<T> : Field<T> where T : FieldMember, ITotallyOrdered
	{
		public abstract bool Equal(T left, T right);

		public abstract bool LessThan(T left, T right);
	}
}
