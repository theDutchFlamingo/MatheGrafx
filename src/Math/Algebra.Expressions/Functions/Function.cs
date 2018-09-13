using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Parsing;

namespace Math.Algebra.Expressions.Functions
{
	public class Function<TOut>
	{
		public Expression Expression;

		public Function(string expression)
		{
			Expression = new Expression(expression);
		}

		public Function(Expression expression)
		{
			Expression = expression;
		}

		/// <summary>
		/// The 
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="declared"></param>
		/// <returns></returns>
		public TOut Evaluate(Parser<TOut> parser, params Declaration[] declared)
		{
			throw new NotImplementedException();
		}
	}
}
