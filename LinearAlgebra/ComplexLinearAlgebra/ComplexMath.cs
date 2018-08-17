using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra.Fields;
using LinearAlgebra.Main;

namespace LinearAlgebra.ComplexLinearAlgebra
{
	/// <summary>
	/// A class for manipulating Complex numbers (like the exponential version of a complex number)
	/// </summary>
	public static class ComplexMath
	{
		public static Complex Exponential(double modulus, double argument)
		{
			return modulus * (Math.Sin(argument) * Complex.I + Math.Cos(argument));
		}

		/// <summary>
		/// Construct a complex number with a string, should be formatted as {real} + {imaginary)i
		/// (order doesn't matter, and either argument can be left out if 0). 'I' can also be
		/// replaced with 'J'
		/// </summary>
		/// <param name="complex"></param>
		/// <returns></returns>
		public static Complex FromString(string complex)
		{
			string[] split = complex.Split('+');

			double real = 0;
			double imaginary = 0;

			bool valid = false;

			foreach (var str in split)
			{
				var cleanStr = str.Replace(" ", "");

				if (str.ToLower().Contains("i") || str.ToLower().Contains("j"))
				{
					if (Double.TryParse(cleanStr.Replace("i", "").Replace("I", "").Replace("J", "").Replace("j", ""),
						out var increment))
					{
						imaginary += increment;
						valid = true;
					}
				}
				else
				{
					if (Double.TryParse(cleanStr, out var increment))
					{
						real += increment;
						valid = true;
					}
				}
			}

			if (valid) return new Complex(real, imaginary);
			throw new ArgumentException("Given string did not seem to contain any valid (complex) numbers.");
		}

		/// <summary>
		/// Complex power. Especially useful when there are a lot of operators combined,
		/// since ^ doesn't have precedence over * and +
		/// </summary>
		/// <param name="b"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static Complex Pow(Complex b, Complex exp)
		{
			return b ^ exp;
		}

		public static bool Equals(this Complex left, Complex right)
		{
			return left.Real.CloseTo(right.Real) && left.Imaginary.CloseTo(right.Imaginary);
		}
	}
}
