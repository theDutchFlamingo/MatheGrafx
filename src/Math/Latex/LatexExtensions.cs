using System;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Rings.Members;
using Math.Rationals;
using Math.Settings;
using static System.String;

namespace Math.Latex
{
	public static class LatexExtensions
	{
		public static readonly List<char> Delimiters = new List<char>
		{
			'(', '[', '{', '|'
		};

		public static string ToLatex<T>(this Rational<T> r) where T : RingMember, IFactorable, new()
		{
			return @"\frac{" + r.Num + "}{" + r.Den + "}";
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
			if (!Delimiters.Contains(delimiter))
			{
				throw new ArgumentException("Delimiter for a matrix must be one of: '" +
				                            Join("', '", Delimiters) + "'.");
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
	}
}
