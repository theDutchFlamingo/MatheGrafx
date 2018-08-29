using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Algebra.Fields.Members;
using Math.Algebra.Ordering;
using Math.Fields;

namespace Math.Algebra.Fields
{
	public abstract class OrderedField<T> : Field<T> where T : FieldMember, ITotallyOrdered
	{

	}
}
