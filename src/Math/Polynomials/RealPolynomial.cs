using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;
using Math.LinearAlgebra;
using Math.Rationals;

namespace Math.Polynomials
{
	/// <summary>
	/// A polynomial with real coefficients
	/// </summary>
	public class RealPolynomial : RingMember, IFactorable
	{
		/**
		 * Contains the regexes... regeces (?)... anyway, they can check for variable name correctness,
		 * and two types of monomials.
		 */
		#region Static

		/// <summary>
		/// The regex to determine if a variable name is allowed
		/// </summary>
		private const string VariableNamesRegex = @"^[a-zA-Z][a-zA-Z_0-9]*$";
		
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
		
		private Vector<Real> _coefficients;

		public int Degree { get; private set; }

		/// <summary>
		/// The getter and setter of the coefficitents vector, setter automatically sets the degree
		/// </summary>
		public Vector<Real> Coefficients
		{
			get => _coefficients;
			private set
			{
				_coefficients = value;
				// Degree of a polynomial is always the amount of coefficients minus 1,
				// check it for yourself if you don't believe me
				Degree = value.Dimension - 1;
			}
		}

		#region Constructors

		/// <summary>
		/// Create a polynomial with the given vector as coefficients
		/// </summary>
		/// <param name="coefficients"></param>
		public RealPolynomial(Vector<Real> coefficients)
		{
			Coefficients = coefficients;
		}

		/// <summary>
		/// Construct a constant polynomial with single coefficient r
		/// </summary>
		/// <param name="r"></param>
		public RealPolynomial(Real r)
		{
			Coefficients = new Vector<Real>(new []{r});
		}

		public RealPolynomial(Real[] coefficients)
		{
			Coefficients = new Vector<Real>(coefficients);
		}

		public RealPolynomial(double[] coefficients)
		{
			Coefficients = new RealVector(coefficients);
		}

		/// <summary>
		/// Create a polynomial with another polynomial
		/// </summary>
		/// <param name="p"></param>
		public RealPolynomial(RealPolynomial p)
		{
			Coefficients = new Vector<Real>(p.Coefficients);
		}

		public RealPolynomial()
		{
			Coefficients = new Vector<Real>(new Real[] { 0 });
		}

		#endregion

		#region Tests

		public bool IsMonic()
		{
			return Coefficients[Degree].Equals(1);
		}

		#endregion

		#region Overrides

		internal override T Add<T>(T other)
		{
			if (other is RealPolynomial r)
			{
				return (T)(MonoidMember)new RealPolynomial(Coefficients + r.Coefficients);
			}
			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T Negative<T>()
		{
			return (T)(INegatable)new RealPolynomial(-Coefficients);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is RealPolynomial c)
				return (T)(RingMember) (this * c);
			throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
				return (T)(MonoidMember) new RealPolynomial(0);
			throw new IncorrectSetException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
				return (T)(GroupMember)new RealPolynomial(1);
			throw new IncorrectSetException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Coefficients.Equals(new Vector<Real>(1));

		public override bool IsUnit() => Coefficients.Equals(new Vector<Real>(1, 1));

		public override bool Equals<T>(T other)
		{
			if (other is RealPolynomial p)
			{
				return p.Coefficients == Coefficients;
			}

			return false;
		}

		[Obsolete]
		public override double ToDouble()
		{
			throw new NotImplementedException();
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
		public static RealPolynomial operator +(RealPolynomial left, RealPolynomial right)
		{
			Vector<Real> newCoefficients = new Vector<Real>(System.Math.Max(left.Degree, right.Degree) + 1);

			for (int i = 0; i < newCoefficients.Dimension; i++)
			{
				newCoefficients[i] = (left.Degree >= i ? (int) left.Coefficients[i] : 0) +
					(right.Degree >= i ? (int) right.Coefficients[i] : 0);
			}
			
			return new RealPolynomial(newCoefficients);
		}
		
		/// <summary>
		/// The negative of this polynomial
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static RealPolynomial operator -(RealPolynomial p)
		{
			Vector<Real> newCoefficients = new Vector<Real>(p.Degree + 1);

			for (int i = 0; i < newCoefficients.Dimension; i++)
			{
				newCoefficients[i] = -p.Coefficients[i];
			}
			
			return new RealPolynomial(newCoefficients);
		}
		
		/// <summary>
		/// One polynomial minus the other
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealPolynomial operator -(RealPolynomial left, RealPolynomial right)
		{
			return left + -right;
		}
		
		/// <summary>
		/// The product of the polynomials
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealPolynomial operator *(RealPolynomial left, RealPolynomial right)
		{
			Vector<Real> newCoefficients = new Vector<Real>(left.Degree + right.Degree + 1);

			for (int l = 0; l < left.Degree + 1; l++)
			{
				for (int r = 0; r < right.Degree + 1; r++)
				{
					newCoefficients[l + r] += left.Coefficients[l] * right.Coefficients[r];
				}
			}
			
			return new RealPolynomial(newCoefficients);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealPolynomial operator *(RealPolynomial left, int right)
		{
			return new RealPolynomial(left.Coefficients * right);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="right"></param>
		/// <param name="left"></param>
		/// <returns></returns>
		public static RealPolynomial operator *(int left, RealPolynomial right)
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
		public static RealPolynomial operator ^(RealPolynomial left, int right)
		{
			if (right < 0) throw new ArgumentException("Exponent of a polynomial must be at least 0");
			if (right == 0) return new RealPolynomial(new Vector<Real>(new Real[]{1}));
			
			RealPolynomial result = new RealPolynomial(left);

			for (int i = 1; i < right; i++)
			{
				result = result * left;
			}
			
			return result;
		}

		#endregion

		/**
		 * Convert this polynomial to a string or parse a polynomial from a string.
		 * Also, this polynomial can be converted to a complex polynomial.
		 */
		#region Conversion

		public static explicit operator RealPolynomial(string polynomial)
		{
			return Parse(polynomial);
		}

		public static explicit operator RealPolynomial(IntegerPolynomial polynomial)
		{
			return new RealPolynomial((RealVector) polynomial.Coefficients);
		}

		public static explicit operator RealPolynomial(Polynomial<Fraction> polynomial)
		{
			return new RealPolynomial((RealVector) polynomial.Coefficients);
		}

		public static RealPolynomial Parse(string polynomial, string variable = "x")
		{
			// First check if variable name is allowed
			if (!Regex.IsMatch(variable, VariableNamesRegex))
			{
				throw new ArgumentException("Variable name must start with a letter and" +
				                            "contain only letters, numbers and underscores.");
			}
			
			// Split the strings on + signs, change - to +-
			// (to be sure that all negatives are the same) and remove all spaces
			List<string> splitStrings = polynomial.Split('+').Select(str => str.Replace("-", "+-")).
				Select(str => str.Replace(" ", "")).ToList();

			// Create a constant zero polynomial
			RealPolynomial result = new RealPolynomial(new Vector<Real>(new Real[]{0}));
			
			foreach (var str in splitStrings)
			{
				if (str == "")
				{
					continue;
				}

				// Remove double negatives and format all minuses as +- for simplicity
				string newStr = str.Replace("--", "+").Replace("-", "+-");

				// Continue with the string without double negatives
				foreach (var clean in newStr.Split('+'))
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
							- ParseMonomial(clean.Split('-')[1], variable);
					}
					else
					{
						result += ParseMonomial(clean, variable);
					}
				}
			}
			
			return result;
		}

		public static RealPolynomial ParseMonomial(string monomial, string variable)
		{
			try
			{
				return new RealPolynomial(new Vector<Real>(new[] {(Real) Int32.Parse(monomial)}));
			}
			catch (FormatException)
			{
				// This means that the monomial isn't constant
			}
			
			// First try matching it with the linear (x^1) term
			Match linear = Regex.Match(monomial, LinearRegex(variable));

			if (linear.Success)
			{
				return new RealPolynomial(new Real[]{Double.Parse(linear.Groups[1].ToString()), 0});
			}
			
			Match mono = Regex.Match(monomial, MonomialRegex(variable));

			if (!mono.Success)
			{
				return new RealPolynomial(new []{(Real) 0});
			}

			uint exp = UInt32.Parse(mono.Groups[2].ToString());

			// TODO parse a monomial from a string
			return null;
		}

		public static explicit operator string(RealPolynomial polynomial)
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
			if (!Regex.IsMatch(variable, VariableNamesRegex))
			{
				throw new ArgumentException("Variable name must start with a letter and contain only" +
				                            "letters, numbers and underscores.");
			}
			
			string result = "";

			for (int i = Degree; i >= 1; i--)
			{
				double coef = Coefficients[i];
				if (!coef.CloseTo(0))
				{
					result += (coef.CloseTo(1) ? "" : $"{Coefficients[i]}") +
					          $"{variable}" + (i != 1 ? $"^{i}" : "") + " + ";
				}
			}

			result += $"{Coefficients[0]}";

			return result;
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
