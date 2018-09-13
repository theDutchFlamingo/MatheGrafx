using System;
using System.Linq;
using System.Text.RegularExpressions;
using Math.Algebra.Expressions;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.LinearAlgebra;
using Math.Parsing;
using Math.Polynomials;
using Math.Rationals;
using Math.Settings;
using static System.String;

namespace Math.Latex
{
	public static class LatexExtensions
	{
		#region Regex

		public static string Fractions = @"^\\frac{[0-9]+}{[0-9]+}$";

		public static string RealMatrices = @"\";

		#endregion

		#region From Latex

		public static Fraction ParseFraction(string value)
		{
			if (!Regex.IsMatch(value, Fractions))
			{
				throw new FormatException();
			}

			throw new NotImplementedException();
		}

		public static Matrix<T> ParseMatrix<T>(string value) where T : RingMember, IParsable<T>, new()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region To Latex

		public static string ToLatex<T>(this Rational<T> r) where T : RingMember, IFactorable, new()
		{
			return r.ToLatex(t => t.ToString());
		}

		public static string ToLatex<T>(this Rational<T> r, Func<T, string> inner)
			where T : RingMember, IFactorable, new()
		{
			return @"\frac{" + inner(r.Num) + "}{" + inner(r.Den) + "}";
		}

		public static string ToLatex<T>(this Polynomial<T> p, string variable = "x")
			where T : RingMember, IParsable<T>, new()
		{
			string result = p.ToString(variable);

			MatchCollection m = Regex.Matches(result, @"\^([\d]+)");

			foreach (Match match in m)
			{
				result = result.Replace("^" + match.Groups[1].Value, "^{" + match.Groups[1].Value + "}");
			}

			return result;
;		}

		public static string ToLatex<T>(this Polynomial<T> p, Func<T, string> inner, string variable = "x")
			where T : RingMember, IParsable<T>, new()
		{
			// First check if variable name is allowed
			if (!Regex.IsMatch(variable, "^" + ExpressionConversions.VariableNamesRegex + "$"))
			{
				throw new ArgumentException("Variable name must start with a letter and contain only " +
				                            "letters and underscores.");
			}

			var result = "";

			for (int i = p.Degree; i >= 1; i--)
			{
				T coef = p.Coefficients[i];
				if (!coef.IsNull())
				{
					result += (coef.IsUnit() ? "" : $"{inner(coef)}") +
					          $"{variable}" + (i != 1 ? $"^{{{i}}}" : "") + " + ";
				}
			}

			if (!p.Coefficients[0].Equals(new T()))
			{
				result += $"{inner(p.Coefficients[0])}";
			}
			else
			{
				result = result.Remove(result.Length - 3);
			}

			result = result.Replace("+ -", "- ");
			// No need to check for negative values if you can easily replace all '+ -' with '- '

			return result;
		}

		public static string ToLatex<T>(this Matrix<T> m, Func<T, string> inner)
			where T : FieldMember, new()
		{
			return m.ToLatex(ConversionSettings.DefaultLatexDelimiter, inner);
		}

		/// <summary>
		/// A method to convert a matrix to latex, includes the inner parameter,
		/// which serves as a way to convert the indices of the matrix to the
		/// correct latex syntax (if the indices are, say, matrices themselves).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="m"></param>
		/// <param name="delimiter"></param>
		/// <param name="inner"></param>
		/// <returns></returns>
		public static string ToLatex<T>(this Matrix<T> m, char delimiter, Func<T, string> inner)
			where T : FieldMember, new()
		{
			if (!ConversionSettings.OpenDelimiters.Contains(delimiter))
			{
				throw new ArgumentException("Delimiter for a matrix must be one of: '" +
				                            Join("', '", ConversionSettings.OpenDelimiters) + "'.");
			}

			string result = @"\left" + delimiter + @" \begin{array}{" +
			                Join("", Enumerable.Repeat("c", m.Width)) + "}\n";

			for (int i = 0; i < m.Height; i++)
			{
				for (int j = 0; j < m.Width; j++)
				{
					if (j == m.Width - 1)
					{
						if (i == m.Height - 1)
						{
							result += inner(m.Indices[i, j]) + "";
							break;
						}

						result += inner(m.Indices[i, j]) + @" \\" + "\n";
						// Final \n is unnecessary, but it looks prettier

						continue;
					}

					if (j != m.Width - 1)
					{
						result += inner(m.Indices[i, j]) + " & ";
					}
				}
			}

			return result + @" \end{array} \right" + delimiter.MatchingDelimiter();
		}

		public static string ToLatex<T>(this Matrix<T> m) where T : FieldMember, new()
		{
			return m.ToLatex(ConversionSettings.DefaultLatexDelimiter);
		}

		public static string ToLatex<T>(this Matrix<T> m, char delimiter)
			where T : FieldMember, new()
		{
			return m.ToLatex(delimiter, o => o.ToString());
		}

		public static string ToLatex<T>(this Vector<T> v, VectorType vectorType)
			where T : FieldMember, new()
		{
			return v.ToMatrix(vectorType).ToLatex(ConversionSettings.DefaultLatexDelimiter);
		}

		public static string ToLatex<T>(this Vector<T> v, char delimiter,
			VectorType vectorType) where T : FieldMember, new()
		{
			return v.ToMatrix(vectorType).ToLatex(delimiter);
		}

		public static string ToLatex<T>(this Vector<T> v)
			where T : FieldMember, new()
		{
			return v.ToMatrix(ConversionSettings.DefaultVectorType).
				ToLatex(ConversionSettings.DefaultLatexDelimiter);
		}

		public static string ToLatex<T>(this Vector<T> v, char delimiter) where T : FieldMember, new()
		{
			return v.ToMatrix(ConversionSettings.DefaultVectorType).ToLatex(delimiter);
		}
		
		#endregion
	}
}
