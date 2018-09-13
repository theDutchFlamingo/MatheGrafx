using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Algebra.Expressions
{
	public class Declaration : Expression
	{
		public string Variable { get; }
		public Expression Value { get; }

		public Declaration(string variable, Expression expression) : base(variable + "=" + expression)
		{

		}
	}
}
