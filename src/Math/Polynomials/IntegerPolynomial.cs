using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Math.Algebra.Expressions;
using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;
using Math.LinearAlgebra;
using Math.Rationals;
using static System.Math;

namespace Math.Polynomials
{
	/// <summary>
	/// A polynomial with real coefficients
	/// </summary>
	public class IntegerPolynomial : RingMember, IFactorable
	{
		/**
		 * Contains the regexes... regeces (?)... anyway, they can check for variable name correctness,
		 * and two types of monomials.
		 */
		#region Static

		/// <summary>
		/// The regex to match a monomial
		/// </summary>
		/// <returns></returns>
		private static string MonomialRegex(string variable) => "^([0-9]+(?:.[0-9]+)?)" + // Coefficient of a monomial
		                                                        "[*]?" + // Optional multiplication sign
		                                                        $"{variable}" + // Variable name
		                                                        "^[(]?" + // Power sign '^' with optional opening bracket
		                                                        "(0-9)+[)]?"; // Whole number exponent with optional closing bracket

		/// <summary>
		/// The regex to match a single x term (without ^2 or higher)
		/// </summary>
		/// <param name="variable"></param>
		/// <returns></returns>
		private static string LinearRegex(string variable) => "([0-9]+(.[0-9]+)?)" + // Coefficient of a linear monomial
		                                                     "[*]?" + // Optional multiplication sign
		                                                     $"{variable}"; // Variable name

		#endregion
		
		private Vector<Integer> _coefficients;

		public int Degree { get;
			private set; }

		/// <summary>
		/// The getter and setter of the coefficitents vector, setter automatically sets the degree
		/// </summary>
		public Vector<Integer> Coefficients
		{
			get => _coefficients;
			set
			{
				_coefficients = value;
				// Degree of a polynomial is always the amount of coefficients minus 1,
				// check it for yourself if you don't believe me
				Degree = _coefficients.Dimension - 1;
			}
		}

		#region Constructors

		/// <summary>
		/// Create a polynomial with the given vector as coefficients
		/// </summary>
		/// <param name="coefficients"></param>
		public IntegerPolynomial(Vector<Integer> coefficients)
		{
			Coefficients = coefficients;
		}

		public IntegerPolynomial(Integer[] coefficients) : this(new Vector<Integer>(coefficients))
		{
			
		}

		/// <summary>
		/// Create a polynomial with another polynomial
		/// </summary>
		/// <param name="p"></param>
		public IntegerPolynomial(IntegerPolynomial p)
		{
			Coefficients = new Vector<Integer>(p.Coefficients);
		}

		public IntegerPolynomial()
		{
			Coefficients = new Vector<Integer>(new Integer[] {0});
		}

		#endregion

		#region Tests

		public bool IsMonic()
		{
			return Coefficients[Degree].Equals(1);
		}

		#endregion

		/**
		 * Operators to add polynomials or multiply them
		 */
		#region Operators

		/// <summary>
		/// Add the two polynomials together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static IntegerPolynomial operator +(IntegerPolynomial left, IntegerPolynomial right)
		{
			Vector<Integer> newCoefficients =
				new Vector<Integer>(Max(left.Degree, right.Degree) + 1);

			for (int i = 0; i < newCoefficients.Dimension; i++)
			{
				newCoefficients[i] = (left.Degree >= i ? (int) left.Coefficients[i] : 0) +
					(right.Degree >= i ? (int) right.Coefficients[i] : 0);
			}
			
			return new IntegerPolynomial(newCoefficients);
		}
		
		/// <summary>
		/// The negative of this polynomial
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static IntegerPolynomial operator -(IntegerPolynomial p)
		{
			Vector<Integer> newCoefficients = new Vector<Integer>(p.Degree + 1);

			for (int i = 0; i < newCoefficients.Dimension; i++)
			{
				newCoefficients[i] = -p.Coefficients[i];
			}
			
			return new IntegerPolynomial(newCoefficients);
		}
		
		/// <summary>
		/// One polynomial minus the other
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static IntegerPolynomial operator -(IntegerPolynomial left, IntegerPolynomial right)
		{
			return left + -right;
		}
		
		/// <summary>
		/// The product of the polynomials
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static IntegerPolynomial operator *(IntegerPolynomial left, IntegerPolynomial right)
		{
			Vector<Integer> newCoefficients = new Vector<Integer>(left.Degree + right.Degree + 1);

			for (int l = 0; l < left.Degree + 1; l++)
			{
				for (int r = 0; r < right.Degree + 1; r++)
				{
					newCoefficients[l + r] += left.Coefficients[l] * right.Coefficients[r];
				}
			}
			
			return new IntegerPolynomial(newCoefficients);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static IntegerPolynomial operator *(IntegerPolynomial left, int right)
		{
			return new IntegerPolynomial(left.Coefficients * right);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="right"></param>
		/// <param name="left"></param>
		/// <returns></returns>
		public static IntegerPolynomial operator *(int left, IntegerPolynomial right)
		{
			return right * left;
		}
		
		/// <summary>
		/// Power of a polynomial
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static IntegerPolynomial operator ^(IntegerPolynomial left, int right)
		{
			if (right < 0)
			{
				throw new ArgumentException("Exponent of a polynomial must be at least 0");
			}

			if (right == 0)
			{
				return new IntegerPolynomial(new Integer[]{1});
			}
			
			IntegerPolynomial result = new IntegerPolynomial(left);

			for (int i = 1; i < right; i++)
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

		public static explicit operator IntegerPolynomial(string polynomial)
		{
			return Parse(polynomial);
		}

		public static IntegerPolynomial Parse(string polynomial, string variable = "x")
		{
			// First check if variable name is allowed
			if (!Regex.IsMatch(variable, ExpressionConversions.VariableNamesRegex))
			{
				throw new ArgumentException("Variable name must start with a letter and contain only" +
				                            "letters, numbers and underscores.");
			}
			
			// Split the strings on + signs, change - to +- (to be sure that all
			// negatives are the same) and remove all spaces
			List<string> splitStrings = polynomial.Split('+').Select(str => str.Replace("-", "+-")).
				Select(str => str.Replace(" ", "")).ToList();

			// Create a constant zero polynomial
			IntegerPolynomial result = new IntegerPolynomial(new Vector<Integer>(new Integer[]{0}));
			
			foreach (var str in splitStrings)
			{
				if (str == "")
				{
					continue;
				}

				// Remove double negatives and format all minuses as +- for simplicity
				string newStr = str.Replace("--", "+").Replace("-", "+-");

				// Continue with the string without double negatives
				foreach (string clean in newStr.Split('+'))
				{
					if (str == "")
					{
						continue;
					}

					if (clean.Contains('-') && clean.Split('-')[0] == "")
					{
						result -= ParseMonomial(clean.Split('-')[1], variable);
					}
					else if (clean.Contains('-'))
					{
						result += ParseMonomial(clean.Split('-')[0], variable)
							- ParseMonomial(clean.Split('-')[1], variable);}
					else
					{
						result += ParseMonomial(clean, variable);
					}
				}
			}
			
			return result;
		}

		public static IntegerPolynomial ParseMonomial(string monomial, string variable)
		{
			try
			{
				return new IntegerPolynomial(new Vector<Integer>(new[] {(Integer) Int32.Parse(monomial)}));
			}
			catch (FormatException)
			{
				
			}
			
			// First try matching it with the linear (x^1) term
			Match linear = Regex.Match(monomial, LinearRegex(variable));

			if (linear.Success)
			{
				return new IntegerPolynomial(new Integer[]{Int32.Parse(linear.Groups[1].ToString()), 0});
			}
			
			Match mono = Regex.Match(monomial, MonomialRegex(variable));
			
			if (!mono.Success) return new IntegerPolynomial(new Vector<Integer>(new []{(Integer) 0}));

			uint exp = UInt32.Parse(mono.Groups[2].ToString());

			// TODO parse a monomial from a string
			return null;
		}

		public static explicit operator string(IntegerPolynomial polynomial)
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
			if (!Regex.IsMatch(variable, "^" + ExpressionConversions.VariableNamesRegex + "$"))
			{
				throw new ArgumentException("Variable name must start with a letter and contain only " +
				                            "letters and underscores.");
			}

			var result = "";

			for (int i = Degree; i >= 1; i--)
			{
				int coef = Coefficients[i];
				if (coef.CloseTo(0))
				{
					continue;
				}

				var sign = " + ";
				if (Coefficients[i - 1].LessThan(new Integer(0)))
				{
					sign = " - ";
				}

				if (coef == 1)
				{
					result += $"{variable}" + (i != 1 ? $"^{{{i}}}" : "") + sign;
					continue;
				}

				if (i != Degree)
				{
					result += (coef == 1 ? "" : $"{Abs(Coefficients[i])}") + $"{variable}" +
					          (i != 1 ? $"^{i}" : "") + sign;
					continue;
				}

				result += (coef == 1 ? "" : $"{Coefficients[i]}") + $"{variable}" +
				          (i != 1 ? $"^{i}" : "") + sign;
			}

			result += $"{Coefficients[0]}";

			return result;
		}

		#endregion

		#region Overrides

		public override T Negative<T>()
		{
			return (T) (INegatable) (-this);
		}

		public bool IsNegative()
		{
			return Coefficients[Degree] < 0;
		}

		internal override T Add<T>(T other)
		{
			if (other is IntegerPolynomial polynomial)
			{
				return (T) (MonoidMember) (this + polynomial);
			}

			throw new IncorrectSetException(GetType(), "added", typeof(T));
		}

		internal override T Multiply<T>(T other)
		{
			if (other is IntegerPolynomial polynomial)
			{
				return (T) (GroupMember) (this * polynomial);
			}

			throw new IncorrectSetException(GetType(), "added", typeof(T));
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T) (MonoidMember) new IntegerPolynomial();
			}
			
			throw new IncorrectSetException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T) (MonoidMember) new IntegerPolynomial();
			}
			
			throw new IncorrectSetException(this, "unit", typeof(T));
		}

		[Obsolete]
		public override double ToDouble()
		{
			throw new NotImplementedException();
		}

		public override bool IsNull()
		{
			return Coefficients == new Vector<Integer>(new Integer[] { 0 });
		}

		public override bool IsUnit()
		{
			return Coefficients == new Vector<Integer>(new Integer[] { 1 });
		}

		public override bool Equals<T>(T other)
		{
			if (other is IntegerPolynomial p)
			{
				return p.Coefficients == Coefficients;
			}
			
			return false;
		}

		public bool IsPrime()
		{
			throw new NotImplementedException();
		}

		public T[] Factors<T>() where T : IFactorable
		{
			throw new NotImplementedException();
		}

		public bool TryFactor<T>(out T[] factors) where T : IFactorable
		{
			throw new NotImplementedException();
		}

		public T Without<T>(T factor) where T : IFactorable
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
