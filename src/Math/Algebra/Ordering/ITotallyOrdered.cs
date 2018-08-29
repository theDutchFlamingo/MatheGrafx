using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Algebra.Ordering
{
	public interface ITotallyOrdered
	{
		bool LessThan<T>(T other);

		bool GreaterThan<T>(T other);

		bool Equal<T>(T other);
	}
}
