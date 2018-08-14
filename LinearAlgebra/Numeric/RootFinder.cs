using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra.ComplexLinearAlgebra;

namespace LinearAlgebra.Numeric
{
	public static class RootFinder
	{
		public static readonly Complex FirstInitial = 0.4 + 0.9 * Complex.I;

		/// <summary>
		/// Use the Durand-Kerner method to find the roots of a polynomial
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static List<Complex> DurandKerner(Polynomial p)
		{
			ComplexVector initial = InitialCondition(p.Degree, );

			return null;
		}

		/// <summary>
		/// Creates n 
		/// </summary>
		/// <param name="n"></param>
		/// <param name="first"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		private static ComplexVector InitialCondition(int n, double radius, Complex first)
		{
			ComplexVector vector = new ComplexVector(n);

			for (int i = 0; i < vector.Dimension; i++)
			{
				vector[i] = first ^ i;
			}

			return vector;
		}

		/// <summary>
		/// Creates n 
		/// </summary>
		/// <param name="n"></param>
		/// <param name="first"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		private static ComplexVector InitialCondition(int n, double radius)
		{
			return InitialCondition(n, radius, FirstInitial);
		}

		/// <summary>
		/// Scale the polynomial so that the coefficient of highest degree is 1
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static Polynomial ScalePolynomial(Polynomial p)
		{
			double scale = p.Coefficients[p.Degree - 1];

			return new Polynomial(p.Coefficients / scale);
		}
	}
}
