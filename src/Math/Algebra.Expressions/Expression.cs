using System;
using System.Collections.Generic;

namespace Math.Algebra.Expressions
{
	/// <summary>
	/// An class which serves as an expression, that is, a collection of numbers and
	/// operations which combine to form a single value. An expression may depend
	/// on several unknowns, which are given in the list of strings with the name variables.
	/// </summary>
	public class Expression
	{
		public List<string> Variables { get; set; }

		public string Prefix { get; }

		public Expression(string expression)
		{
			Prefix = expression.ConvertTo(NotationType.Prefix);
			Variables = expression.ParseVariables();
		}

		public Expression(Expression expression)
		{
			Prefix = expression.Prefix;
			Variables = expression.Variables;
		}

		public Expression Evaluate(params Expression[] declarations)
		{
			throw new NotImplementedException();
		}
	}
}
