using System;
using LinearAlgebra.Main;

namespace LinearAlgebra.ComplexLinearAlgebra
{
	public class Complex
	{
		public double Real { get; set; }
		public double Imaginary { get; set; }

		public double Modulus => Math.Sqrt(Real * Real + Imaginary * Imaginary);
		public double Argument => Math.Atan2(Imaginary, Real);

		public static readonly Complex I = new Complex
		{
			Real = 0,
			Imaginary = 1
		};

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

		/// <summary>
		/// String representation of the complex number
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// String representation with option to represent in exponential form
		/// </summary>
		/// <param name="exponential"></param>
		/// <returns></returns>
		public string ToString(bool exponential)
		{
			if (exponential) return $"{Modulus}*e^({Argument}i)";
			return $"{Real} + {Imaginary}i";
		}

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
			return right.Conjugate() * left / right.Modulus;
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
			return ComplexMath.Exponential(b.Modulus * exp, b.Argument);
		}

		/// <summary>
		/// Exponentiation of a real with a complex number
		/// </summary>
		/// <param name="b"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static Complex operator ^(double b, Complex exp)
		{
			return ComplexMath.Exponential(Math.Pow(b, exp.Real), exp.Imaginary * Math.Log(b));
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
			       (Math.E ^ (b.Argument * (exp.Real * I - exp.Imaginary)));
		}

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
			catch (ArgumentException e)
			{
				result = null;
				return false;
			}
		}
	}
}
