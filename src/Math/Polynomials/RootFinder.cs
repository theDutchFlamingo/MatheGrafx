using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Algebra.Fields.Members;
using Math.ComplexLinearAlgebra;
using Math.Main;
using Math.Polynomials;

namespace Math.Numeric
{
	public static class RootFinder
	{
		/// <summary>
		/// A complex number with absolute value of 1.
		/// </summary>
		public static readonly Complex FirstInitial = 0.43588989435406735523 + 0.9 * Complex.I;

		/// <summary>
		/// Use the Durand-Kerner method to find the roots of a polynomial
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static List<Complex> DurandKerner(IntegerPolynomial p)
		{
			// According to Wikipedia, radius of initial condition should be 1 + max(|a_1|,|a_2|,...,|a_n-1|)
			ComplexVector current = InitialCondition(p.Degree, 1 + p.Coefficients.Select(d => System.Math.Abs(d)).Max());

			ComplexVector Iterate(ComplexVector v)
			{
				ComplexVector newVector = new ComplexVector(v);

				for (int i = 0; i < v.Dimension; i++)
				{

				}

				return v;
			}
			
			ComplexVector next = new ComplexVector(current.Dimension);

			while (!next.CloseTo(current))
			{
				next = Iterate(current);
			}

			return null;
		}

		/// <summary>
		/// Use the Durand-Kerner method to find the roots of a polynomial
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static List<Complex> DurandKerner(RealPolynomial p)
		{
			// According to Wikipedia, radius of initial condition should be 1 + max(|a_1|,|a_2|,...,|a_n-1|)
			ComplexVector current = InitialCondition(p.Degree, 1 + p.Coefficients.Select(d => System.Math.Abs(d)).Max());

			ComplexVector Iterate(ComplexVector v)
			{
				ComplexVector newVector = new ComplexVector(v);

				for (int i = 0; i < v.Dimension; i++)
				{

				}

				return v;
			}

			ComplexVector next = new ComplexVector(current.Dimension);

			while (!next.CloseTo(current))
			{
				next = Iterate(current);
			}

			return null;
		}

		/// <summary>
		/// Whether this vector is (in terms of norm) within the tolerance bound of the other
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public static bool CloseTo(this RealVector a, RealVector b, double tolerance = LinearMath.Tolerance)
		{
			return (a - b).Norm().CloseTo(0, tolerance);
		}

		/// <summary>
		/// Whether this complex vector is (in terms of norm) within the tolerance bound of the other
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public static bool CloseTo(this ComplexVector a, ComplexVector b,
			double tolerance = LinearMath.Tolerance)
		{
			return (a - b).Norm().CloseTo(0, tolerance);
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

			// A unit vector in the same direction
			Complex unit = first / first.Modulus;

			for (int i = 0; i < vector.Dimension; i++)
			{
				vector[i] = radius * (unit ^ i);
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
		public static IntegerPolynomial ScalePolynomial(IntegerPolynomial p)
		{
			int scale = p.Coefficients[p.Degree - 1];

			return new IntegerPolynomial(p.Coefficients / scale);
		}
	}
}
