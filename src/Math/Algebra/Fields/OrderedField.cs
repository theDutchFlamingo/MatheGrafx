using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Algebra.Fields.Members;
using Math.Algebra.Ordering;

namespace Math.Algebra.Fields
{
	public abstract class OrderedField<T> : Field<T> where T : FieldMember, ITotallyOrdered
	{
		public abstract bool Equal(T left, T right);

		public abstract bool LessThan(T left, T right);
	}
}
