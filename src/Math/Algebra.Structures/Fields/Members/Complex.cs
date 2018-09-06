using System;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.ComplexLinearAlgebra;
using Math.Exceptions;
using Math.LinearAlgebra;
using Math.Parsing;

namespace Math.Algebra.Structures.Fields.Members
{
	public class Complex : FieldMember, INumerical, IParsable<Complex>
	{
		#region Properties

		public double Real { get; set; }
		public double Imaginary { get; set; }

		/// <summary>
		/// The modulus, or absolute value of this complex number
		/// </summary>
		public double Modulus => System.Math.Sqrt(Real * Real + Imaginary * Imaginary);
		/// <summary>
		/// The argument, or angle of this complex number with respect to the x-axis
		/// </summary>
		public double Argument => System.Math.Atan2(Imaginary, Real);

		#endregion

		#region Constants

		public static readonly Complex I = new Complex
		{
			Real = 0,
			Imaginary = 1
		};

		#endregion

		#region Constructors

		/// <summary>
		/// Complex constructor without arguments
		/// </summary>
		public Complex()
		{
			
		}

		public Complex(double real, double imaginary)
		{
			Real = real;
			Imaginary = imaginary;
		}

		#endregion

		#region Complex-specific Methods

		/// <summary>
		/// Whether this complex number is purely real, that is, Im(this) == 0
		/// </summary>
		/// <returns></returns>
		public bool IsReal()
		{
			return Imaginary.CloseTo(0);
		}

		/// <summary>
		/// Whether this complex number is purely imaginary, that is, Real(this) == 0
		/// </summary>
		/// <returns></returns>
		public bool IsImaginary()
		{
			return Real.CloseTo(0);
		}

		/// <summary>
		/// Returns the complex conjugate of this
		/// </summary>
		/// <returns></returns>
		public Complex Conjugate()
		{
			return new Complex(Real, -Imaginary);
		}

		#endregion

		#region Strings and INumerical Overrides

		/// <summary>
		/// String representation of the complex number
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(string format)
		{
			return ToString(false, format);
		}

		public INumerical LongestValue()
		{
			return LongestValue(false);
		}

		/// <summary>
		/// Returns the value which has the longest length when converted to a string
		/// (from Real and Imaginary if exponential is false,
		/// and from modulus and aargument if exponential is true).
		/// </summary>
		/// <returns></returns>
		public INumerical LongestValue(bool exponential)
		{
			if (exponential) return new Real(System.Math.Max(Real, Imaginary));
			return new Real(System.Math.Max(Modulus, Argument));
		}

		public INumerical Round()
		{
			return new Complex(System.Math.Round(Real), System.Math.Round(Imaginary));
		}

		public INumerical Log10()
		{
			return ComplexMath.Log10(this);
		}

		/// <summary>
		/// String representation with option to represent in exponential form
		/// </summary>
		/// <param name="exponential"></param>
		/// <param name="format">Optional format that </param>
		/// <returns></returns>
		public string ToString(bool exponential, string format = "")
		{
			if (exponential)
			{
				return $"{Modulus.ToString(format)}*e^({Argument.ToString(format)}i)";
			}

			if (Real.CloseTo(0) && Imaginary.CloseTo(0))
			{
				return "0";
			}

			if (Real.CloseTo(0))
			{
				if (Imaginary.CloseTo(1))
				{
					return "i";
				}

				if (Imaginary.CloseTo(-1))
				{
					return "-i";
				}

				return Imaginary >= 0
					? $"{Imaginary.ToString(format)}i"
					: $"-{System.Math.Abs(Imaginary).ToString(format)}i";
			}

			if (Imaginary.CloseTo(0))
			{
				return Real.ToString(format);
			}

			string result = "";

			result += Real.ToString(format);

			if (Imaginary.CloseTo(1))
			{
				return result + " + i";
			}

			if (Imaginary.CloseTo(-1))
			{
				return result + " - i";
			}

			result += Imaginary >= 0 ? $" + {Imaginary.ToString(format)}i"
				: $" - {System.Math.Abs(Imaginary).ToString(format)}i";

			return result;
		}

		#endregion

		#region Operators

		/// <summary>
		/// Add the complex to the real
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator +(Complex left, double right)
		{
			return new Complex(left.Real + right, left.Imaginary);
		}

		/// <summary>
		/// Add the complex to the real
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator +(double left, Complex right)
		{
			return right + left;
		}

		/// <summary>
		/// Complex minus real
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator -(Complex left, double right)
		{
			return left + -right;
		}

		/// <summary>
		/// Real minus complex
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator -(double left, Complex right)
		{
			return left + -right;
		}

		/// <summary>
		/// Multiply a complex number with a real number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator *(Complex left, double right)
		{
			return new Complex(left.Real * right, left.Imaginary * right);
		}

		/// <summary>
		/// Multiply a complex number with a real number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator *(double left, Complex right)
		{
			return right * left;
		}

		/// <summary>
		/// Divide a complex number by a real number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator /(Complex left, double right)
		{
			return left * (1 / right);
		}

		/// <summary>
		/// Divide a real number by a complex number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator /(double left, Complex right)
		{
			return right.Conjugate() * left / System.Math.Pow(right.Modulus, 2);
		}

		/// <summary>
		/// Add the two together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator +(Complex left, Complex right)
		{
			return new Complex(left.Real + right.Real, left.Imaginary + right.Imaginary);
		}

		/// <summary>
		/// Negative of this complex
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Complex operator -(Complex c)
		{
			return new Complex(-c.Real, -c.Imaginary);
		}

		/// <summary>
		/// Subtract right from left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator -(Complex left, Complex right)
		{
			return left + -right;
		}

		/// <summary>
		/// Multiply the two together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator *(Complex left, Complex right)
		{
			return new Complex(left.Real * right.Real - left.Imaginary * right.Imaginary,
				left.Imaginary * right.Real + left.Real * right.Imaginary);
		}

		/// <summary>
		/// Divide one complex by another
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator /(Complex left, Complex right)
		{
			return left * (1.0 / right);
		}

		/// <summary>
		/// Exponentiation of a complex with a real number
		/// </summary>
		/// <param name="b"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static Complex operator ^(Complex b, double exp)
		{
			return ComplexMath.Exponential(System.Math.Pow(b.Modulus, exp), b.Argument * exp);
		}

		/// <summary>
		/// Exponentiation of a real with a complex number
		/// </summary>
		/// <param name="b"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static Complex operator ^(double b, Complex exp)
		{
			return ComplexMath.Exponential(System.Math.Pow(b, exp.Real), exp.Imaginary * System.Math.Log(b));
		}

		/// <summary>
		/// Exponentiation of a complex with a complex number
		/// </summary>
		/// <param name="b"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static Complex operator ^(Complex b, Complex exp)
		{
			return (b.Modulus ^ (exp.Real + exp.Imaginary * I)) *
			       (System.Math.E ^ (b.Argument * (exp.Real * I - exp.Imaginary)));
		}

		#endregion

		#region Conversions

		/// <summary>
		/// Convert implicitly from double to Complex
		/// </summary>
		/// <param name="i"></param>
		public static implicit operator Complex(double i)
		{
			return new Complex(i, 0);
		}

		public static bool TryParse(string s, out Complex result)
		{
			try
			{
				result = ComplexMath.FromString(s);
				return true;
			}
			catch (ArgumentException)
			{
				result = new Complex();
				return false;
			}
		}

		#endregion

		#region Override Methods

		internal override T Add<T>(T other)
		{
			if (other is Complex c)
				return (T) (MonoidMember) (this + c);
			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T Negative<T>()
		{
			return (T) (INegatable) (-this);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Complex c)
				return (T)(GroupMember)(this * c);
			throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
		}

		public override T Inverse<T>()
		{
			return (T) (INegatable) (1 / this);
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
				return (T) (MonoidMember) new Complex(0, 0);
			throw new IncorrectSetException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
				return (T)(GroupMember)new Complex(1, 0);
			throw new IncorrectSetException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Equals(Null<Complex>());

		public override bool IsUnit() => Equals(Unit<Complex>());

		[Obsolete]
		public override double ToDouble()
		{
			return Real;
		}

		public override bool Equals<T>(T other)
		{
			if (other is Complex c)
			{
				return ComplexMath.Equals(this, c);
			}
			return false;
		}

		public override T Inner<T>(T fieldMember)
		{
			if (fieldMember is Complex c)
			{
				return (T) (RingMember) Multiply(c.Conjugate());
			}
			throw new IncorrectSetException(GetType(), "inner", typeof(T));
		}

		public Complex Parse(string value)
		{
			TryParse(value, out Complex c);

			return c;
		}

		#endregion
	}
}
