using System;
using System.Collections.Generic;
using System.Linq;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Fields;
using LinearAlgebra.VectorSpaces;

namespace LinearAlgebra.Main
{
	/// <summary>
	/// The class which allows you to perform more complicated procedures on Matrices,
	/// like determining the eigenvalues and eigenvectors.
	/// Meant to be a sort of Math class for linear algebra-related computations.
	/// </summary>
	public static class LinearMath
	{
		public const double Tolerance = 0.0000001;

		/// <summary>
		/// Gets the eigenvalues of the given matrix, calculated numerically from the
		/// characteristic polynomial of this matrix. Continues until all eigenvalues
		/// have been found.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Vector EigenValues(this Matrix m)
		{
			return null;
		}

		/// <summary>
		/// Gets the eigenvectors of the given matrix.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Vector[] EigenVectors(this Matrix m)
		{
			return null;
		}

		public static bool LinearlyDependent(Vector[] vectors)
		{
			if (vectors.Select(v => v.Dimension).Distinct().Count() > 1)
				throw new IncompatibleOperationException(IncompatibleVectorOperationType.Dimension);

			// If the amount of vectors is greater than the dimension they're
			// automatically linearly dependent.
			if (vectors[0].Dimension < vectors.Length) return true;



			return false;
		}

		/// <summary>
		/// Returns the vector containing the scalars which form the orginial vector when
		/// multiplied with each corresponding vector in the given array.
		/// In other words, if you put the array of combination vector in a matrix A,
		/// the return value x is the vector that satisfies Ax = v
		/// </summary>
		/// <param name="v"></param>
		/// <param name="combination"></param>
		/// <returns></returns>
		public static Vector GetLinearCombination(this Vector v, Vector[] combination)
		{
			List<Vector> allVectors = new List<Vector>(combination) {v};

			if (!LinearlyDependent(allVectors.ToArray())) throw new
				ArgumentException("This vector is not linearly dependent on the given array");

			return null;
		}

		public static VectorSpace ColumnSpace(this Matrix m)
		{
			return null;
		}

		public static bool CloseTo(this double d1, double d2, double tolerance = Tolerance)
		{
			return Math.Abs(d1 - d2) <= tolerance;
		}

		public static bool CloseTo(this Real d1, Real d2, double tolerance = Tolerance)
		{
			return Math.Abs(d1 - d2) <= tolerance;
		}

		public static Matrix UnitMatrix(int size)
		{
			return new Matrix(size);
		}

		public static Matrix NullMatrix(int height, int width)
		{
			return new Matrix(height, width);
		}
	}
}
