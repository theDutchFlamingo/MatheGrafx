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
using Math.Parsing;
using Math.Rationals;

namespace Math.Polynomials
{
    public class Polynomial<T> : RingMember, IFactorable where T : RingMember, IParsable<T>, new()
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
		                                                        "[^][(]?" + // Power sign '^' with optional opening bracket
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
		
		private Vector<T> _coefficients;

		public int Degree { get; private set; }

		/// <summary>
		/// The getter and setter of the coefficitents vector, setter automatically sets the degree
		/// </summary>
		public Vector<T> Coefficients
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
		public Polynomial(Vector<T> coefficients)
		{
			Coefficients = coefficients;
		}

		/// <summary>
		/// Construct a constant polynomial with single coefficient r
		/// </summary>
		/// <param name="r"></param>
		public Polynomial(T r) : this(new []{r})
		{
			
		}

		/// <summary>
		/// Construct a polynomial with an array of coefficients.
		/// </summary>
		/// <param name="coefficients"></param>
		public Polynomial(params T[] coefficients) : this(new Vector<T>(coefficients))
		{
			
		}

		/// <summary>
		/// Create a polynomial with another polynomial
		/// </summary>
		/// <param name="p"></param>
		public Polynomial(Polynomial<T> p) : this(p.Coefficients)
		{
			
		}

		public Polynomial()
		{
			Coefficients = new Vector<T>(new [] { new T() });
		}

		#endregion

		#region Tests

		public bool IsMonic()
		{
			return Coefficients[Degree].Equals(1);
		}

		#endregion

		#region Overrides

		internal override T1 Add<T1>(T1 other)
		{
			if (other is Polynomial<T> r)
			{
				return (T1)(MonoidMember)new Polynomial<T>(Coefficients + r.Coefficients);
			}
			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T1 Negative<T1>()
		{
			return (T1)(INegatable)new Polynomial<T>(-Coefficients);
		}

		internal override T1 Multiply<T1>(T1 other)
		{
			if (other is Polynomial<T> c)
				return (T1)(RingMember) (this * c);
			throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
		}

		public override T1 Null<T1>()
		{
			if (typeof(T1) == GetType())
				return (T1)(MonoidMember) new Polynomial<T>(new T());
			throw new IncorrectSetException(this, "null", typeof(T1));
		}

		public override T1 Unit<T1>()
		{
			if (typeof(T1) == GetType())
				return (T1)(GroupMember)new Polynomial<T>(new T().Unit<T>());
			throw new IncorrectSetException(this, "unit", typeof(T1));
		}

		public override bool IsNull() => Coefficients.Equals(new Vector<T>(1));

		public override bool IsUnit() => Coefficients.Equals(new Vector<T>(1, 1));

		public override bool Equals<T1>(T1 other)
		{
			if (other is Polynomial<T> p)
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

	    public bool IsPrime()
	    {
		    throw new NotImplementedException();
	    }

	    public T1[] Factors<T1>() where T1 : IFactorable
	    {
		    if (!typeof(T1).IsSubclassOfGeneric(typeof(Polynomial<>)))
		    {
			    throw new FactorTypeException();
		    }

		    return Factors<Polynomial<T>>(false).Select(factor => (T1) (IFactorable) factor).ToArray();
	    }

	    public T1[] Factors<T1>(bool allowComplex) where T1 : Polynomial<T>
	    {
		    if (!allowComplex)
		    {
			    
		    }
		    
		    throw new NotImplementedException();
	    }

	    public bool TryFactor<T1>(out T1[] factors) where T1 : IFactorable
	    {
		    throw new NotImplementedException();
	    }

	    public T1 Without<T1>(T1 factor) where T1 : IFactorable
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
		public static Polynomial<T> operator +(Polynomial<T> left, Polynomial<T> right)
		{
			Vector<T> newCoefficients = new Vector<T>(System.Math.Max(left.Degree, right.Degree) + 1);

			for (int i = 0; i < newCoefficients.Dimension; i++)
			{
				newCoefficients[i] = (left.Degree >= i ? left.Coefficients[i] : new T()).Add(
					right.Degree >= i ? right.Coefficients[i] : new T());
			}
			
			return new Polynomial<T>(newCoefficients);
		}
		
		/// <summary>
		/// The negative of this polynomial
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static Polynomial<T> operator -(Polynomial<T> p)
		{
			Vector<T> newCoefficients = new Vector<T>(p.Degree + 1);

			for (int i = 0; i < newCoefficients.Dimension; i++)
			{
				newCoefficients[i] = p.Coefficients[i].Negative<T>();
			}
			
			return new Polynomial<T>(newCoefficients);
		}
		
		/// <summary>
		/// One polynomial minus the other
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Polynomial<T> operator -(Polynomial<T> left, Polynomial<T> right)
		{
			return left + -right;
		}
		
		/// <summary>
		/// The product of the polynomials
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Polynomial<T> operator *(Polynomial<T> left, Polynomial<T> right)
		{
			Vector<T> newCoefficients = new Vector<T>(left.Degree + right.Degree + 1);

			for (int l = 0; l < left.Degree + 1; l++)
			{
				for (int r = 0; r < right.Degree + 1; r++)
				{
					newCoefficients[l + r] = newCoefficients[l + r].
						Add(left.Coefficients[l].Multiply(right.Coefficients[r]));
				}
			}
			
			return new Polynomial<T>(newCoefficients);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Polynomial<T> operator *(Polynomial<T> left, T right)
		{
			return new Polynomial<T>(left.Coefficients * right);
		}
		
		/// <summary>
		/// Multiply this polynomial by a constant on the right
		/// </summary>
		/// <param name="right"></param>
		/// <param name="left"></param>
		/// <returns></returns>
		public static Polynomial<T> operator *(T left, Polynomial<T> right)
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
		public static Polynomial<T> operator ^(Polynomial<T> left, int right)
		{
			if (right < 0) throw new ArgumentException("Exponent of a polynomial must be at least 0");
			if (right == 0) return new Polynomial<T>(new []{ new T().Unit<T>() });
			
			Polynomial<T> result = new Polynomial<T>(left);

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

		public static explicit operator Polynomial<T>(string polynomial)
		{
			return Parse(polynomial);
		}

		public static Polynomial<T> Parse(string polynomial, string variable = "x")
		{
			// First check if variable name is allowed
			if (!Regex.IsMatch(variable, ExpressionConversions.VariableNamesRegex))
				throw new ArgumentException("Variable name must start with a letter and contain only letters, numbers and underscores.");
			
			// Split the strings on + signs, change - to +- (to be sure that all negatives are the same) and remove all spaces
			List<string> splitStrings = polynomial.Split('+').Select(str => str.Replace("-", "+-")).
				Select(str => str.Replace(" ", "")).ToList();

			// Create a constant zero polynomial
			Polynomial<T> result = new Polynomial<T>(new []{ new T() });
			
			foreach (var str in splitStrings)
			{
				if (str == "") continue;

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

		public static Polynomial<T> ParseMonomial(string monomial, string variable)
		{
			try
			{
				return new Polynomial<T>(new T().Parse(monomial));
			}
			catch (FormatException)
			{
				// This means that the monomial isn't constant
			}
			
			// First try matching it with the linear (x^1) term
			Match linear = Regex.Match(monomial, LinearRegex(variable));
			
			if (linear.Success)
			{
				return new Polynomial<T>(new []
				{
					new T().Parse(linear.Groups[1].ToString()), new T()
				});
			}
			
			Match mono = Regex.Match(monomial, MonomialRegex(variable));

			if (!mono.Success)
			{
				return new Polynomial<T>(new []{ new T() });
			}

			uint exp = UInt32.Parse(mono.Groups[2].ToString());

			// TODO parse a monomial from a string
			return null;
		}

		public static explicit operator string(Polynomial<T> polynomial)
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
				T coef = Coefficients[i];
				if (!coef.IsNull())
				{
					result += (coef.IsUnit() ? "" : $"{Coefficients[i]}") + $"{variable}" + (i != 1 ? $"^{i}" : "") + " + ";
				}
			}

			if (!Coefficients[0].Equals(new T()))
			{
				result += $"{Coefficients[0]}";
			}
			else
			{
				result = result.Remove(result.Length - 3);
			}

			return result.Replace("+ -", "- ");
			// No need to check for negative values if you can easily replace all '+ -' with '- '
		}

		#endregion
    }
}