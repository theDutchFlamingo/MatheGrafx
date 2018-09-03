using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Math.Latex;
using Math.Settings;
using static System.Char;

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
		public const string VariableNamesRegex = @"[a-zA-Z][a-zA-Z_]*";

		public static readonly List<string> DisallowedSymbols = new List<string>
		{
			"#", "$", "&", "`", "'", "\"", "~", "\\", "<", ">", "?", ";", "..", ".,", ",.", ",,"
		};

		public static bool HasNotationType(this string expression, NotationType type)
		{
			if (!expression.CheckSyntax())
			{
				return false;
			}

			switch (type)
			{
				case NotationType.Infix:
					return expression.IsInfixExpression();
				case NotationType.Prefix:
					return expression.IsPrefixExpression();
				case NotationType.Postfix:
					return expression.IsPostfixExpression();
				default:
					return false;
			}
		}

		/// <summary>
		/// A basic check for syntax correctness that is shared by all notation types
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		private static bool CheckSyntax(this string expression)
		{
			foreach (string s in DisallowedSymbols)
			{
				if (expression.Contains(s))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Whether this char is a number or a part of a number ('.' and ',' are allowed).
		/// </summary>
		/// <param name="test"></param>
		/// <returns></returns>
		private static bool IsntNumber(char test)
		{
			return IsNumber(test) || test == '.' || test == ',';
		}

		private static bool IsInfixExpression(this string expression)
		{
			// If there are more closing delimiters than opening delimiters,
			// the expression cannot be evaluated.
			foreach (char ch in LatexExtensions.Delimiters)
			{
				if (expression.Count(c => c == ch.MatchingDelimiter()) > expression.Count(c => c == ch))
				{
					return false;
				}
			}

			int i = 0;
			bool recentNumber = false; // Whether the last read piece of the string was a number

			while (expression.Length > i)
			{
				if (IsNumber(expression[i]))
				{
					if (recentNumber)
					{
						// If two numbers follow each other without operator,
						// then this expression is not infix
						return false;
					}

					if (expression.Length > i + 1)
					{
						int end = expression.First(IsntNumber);
						expression = expression.Remove(i, end - i);
					}

					recentNumber = true;
					continue;
				}

				recentNumber = false;

				i++;
			}

			throw new NotImplementedException();
		}

		private static bool IsPrefixExpression(this string expression)
		{
			throw new NotImplementedException();
		}

		private static bool IsPostfixExpression(this string expression)
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
