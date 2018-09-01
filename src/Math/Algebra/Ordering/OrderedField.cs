using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;

namespace Math.Algebra.Ordering
{
	public abstract class OrderedField<T> : Field<T> where T : FieldMember, ITotallyOrdered
	{
		public abstract bool Equal(T left, T right);

		public abstract bool LessThan(T left, T right);
	}
}
