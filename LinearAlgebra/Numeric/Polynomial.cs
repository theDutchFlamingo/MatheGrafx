using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinearAlgebra.Numeric
{
	/// <summary>
	/// A polynomial with real coefficients
	/// </summary>
	public class Polynomial
	{
		public static string AllowedVariableNames = @"^[a-zA-Z][a-zA-Z_0-9]+";
		
		private RealVector _coefficients;

		public int Degree { get; private set; }

		/// <summary>
		/// The getter and setter of the coefficitents vector, setter automatically sets the degree
		/// </summary>
		public RealVector Coefficients
		{
			get => _coefficients;
			set
			{
				_coefficients = value;
				Degree = value.Dimension;
			}
		}

		/// <summary>
		/// Create a polynomial with the given vector as coefficients
		/// </summary>
		/// <param name="coefficients"></param>
		public Polynomial(RealVector coefficients)
		{
			Coefficients = coefficients;
		}

		/// <summary>
		/// Create a polynomial with another polynomial
		/// </summary>
		/// <param name="p"></param>
		public Polynomial(Polynomial p)
		{
			Coefficients = new RealVector(p.Coefficients);
		}

		/**
		 * Operators to add polynomials or multiply them
		 */
		#region Operators

		public static Polynomial operator +(Polynomial left, Polynomial right)
		{
			RealVector newCoefficients = new RealVector(Math.Max(left.Degree, right.Degree));

			for (int i = 0; i < newCoefficients.Dimension; i++)
			{
				newCoefficients[i] = left.Degree >= i ? 0 : left.Coefficients[i] +
					right.Degree >= i ? 0 : right.Coefficients[i];
			}
			
			return new Polynomial(newCoefficients);
		}
		
		public static Polynomial operator *(Polynomial left, Polynomial right)
		{
			RealVector newCoefficients = new RealVector(left.Degree + right.Degree);

			for (int l = 0; l < left.Degree; l++)
			{
				for (int r = 0; r < right.Degree; r++)
				{
					newCoefficients[l + r] += left.Coefficients[l] * right.Coefficients[r];
				}
			}
			
			return new Polynomial(newCoefficients);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Polynomial operator *(Polynomial left, double right)
		{
			return new Polynomial(left.Coefficients * right);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="right"></param>
		/// <param name="left"></param>
		/// <returns></returns>
		public static Polynomial operator *(double left, Polynomial right)
		{
			return right * left;
		}

		public static Polynomial operator ^(Polynomial left, int right)
		{
			if (right < 0) throw new ArgumentException("Exponent of a polynomial must be at least 0");
			if (right == 0) return new Polynomial(new RealVector(new []{1}));
			
			Polynomial result = new Polynomial(left);

			for (int i = 0; i < right; i++)
			{
				result = result * left;
			}
			
			return result;
		}

		#endregion

		/**
		 * Convert this polynomial to a string or parse a polynomial from a string
		 */
		#region Conversion

		public static explicit operator Polynomial(string polynomial)
		{
			// TODO parse a polynomial from a string
		}

		public static explicit operator string(Polynomial polynomial)
		{
			return polynomial.ToString();
		}

		public override string ToString()
		{
			return ToString("x");
		}

		public string ToString(string variable)
		{
			// First check if variable name is allowed
			if (!Regex.IsMatch(variable, AllowedVariableNames))
				throw new ArgumentException("Variable name must start with a letter and contain only letters, numbers and underscores.");
			
			string result = "";

			for (int i = Degree - 1; i > 1; i--)
			{
				result += $"{Coefficients[i]}{variable}" + (i != 0 ? "^{i}" : "") + " + ";
			}

			result += $"{Coefficients[0]}";

			return result;
		}

		#endregion
	}
}
