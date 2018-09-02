using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Math.Algebra.Expressions
{
	/// <summary>
	/// A class which provides methods to convert to and form different types of notations,
	/// such as infix, prefix and postfix
	/// </summary>
	public static class ExpressionConversions
	{
		/// <summary>
		/// The regex to determine if a variable name is allowed
		/// </summary>
		public const string VariableNamesRegex = @"[a-zA-Z][a-zA-Z_0-9]*";

		public static bool HasNotationType(this string expression, NotationType type)
		{
			switch (type)
			{
				case NotationType.Infix:
					return expression.IsInfix();
				case NotationType.Prefix:
					return expression.IsPrefix();
				case NotationType.Postfix:
					return expression.IsPostfix();
				default:
					return false;
			}
		}

		private static bool IsInfix(this string expression)
		{
			throw new NotImplementedException();
		}

		private static bool IsPrefix(this string expression)
		{
			throw new NotImplementedException();
		}

		private static bool IsPostfix(this string expression)
		{
			throw new NotImplementedException();
		}

		public static string ConvertTo(this string expression, NotationType type)
		{
			if (expression.HasNotationType(type))
			{
				return expression;
			}

			string prefix = expression.ToPrefixNotation();

			switch (type)
			{
				case NotationType.Prefix:
					return prefix;
				case NotationType.Infix:
					return "";
				case NotationType.Postfix:
					return "";
				default:
					throw new ArgumentException("Given type is not allowed.");
			}

			// TODO add conversions
		}

		public static string ToPrefixNotation(this string expression)
		{
			throw new NotImplementedException();
		}

		public static List<string> ParseVariables(this string expression)
		{
			GroupCollection groups = Regex.Match(expression, VariableNamesRegex).Groups;

			List<string> result = new List<string>();

			for (var i = 1; i < groups.Count; i++)
			{
				result[i] = groups[i].Value;
			}

			return result;
		}
	}
}
