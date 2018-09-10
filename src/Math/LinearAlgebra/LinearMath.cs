using System;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Structures.Fields.Members;
using Math.ComplexLinearAlgebra;
using Math.Exceptions.Operations;
using Math.VectorSpaces;

namespace Math.LinearAlgebra
{
	/// <summary>
	/// The class which allows you to perform more complicated procedures on Matrices,
	/// like determining the eigenvalues and eigenvectors.
	/// Meant to be a sort of Math class for linear algebra-related computations.
	/// </summary>
	public static class LinearMath
	{
		public const double Tolerance = 1E-09D;

		/// <summary>
		/// Gets the eigenvalues of the given matrix, calculated numerically from the
		/// characteristic polynomial of this matrix. Continues until all eigenvalues
		/// have been found.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static RealVector RealEigenValues(this RealMatrix m)
		{
			if (!m.IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Eigenvalue);
			}
			
			ComplexVector values = new ComplexVector(m.Width);

			

			throw new NotImplementedException();
		}

		public static ComplexVector ComplexEigenValues(this RealMatrix m)
		{
			throw new NotImplementedException();
		}

		public static ComplexVector ComplexEigenValues(this ComplexMatrix m)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the eigenbasis of the given matrix and eigenvalue l.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Basis EigenBasis(this RealMatrix m, Real l)
		{
			return null;
		}

		public static bool LinearlyDependent(RealVector[] vectors)
		{
			if (vectors.Select(v => v.Dimension).Distinct().Count() > 1)
			{
				throw new IncompatibleOperationException(VectorOperationType.Dimension);
			}

			// If the amount of vectors is greater than the dimension they're
			// automatically linearly dependent.
			if (vectors[0].Dimension < vectors.Length)
			{
				return true;
			}

			var m = new RealMatrix(vectors);

			if (m.IsSquare())
			{
				return m.Determinant().Equals(0);
			}

			m = m.ToReducedEchelonForm().ToRealMatrix();

			return m.AmountOfNullRows() != 0;
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
		public static RealVector GetLinearCombination(this RealVector v, RealVector[] combination)
		{
			List<RealVector> allVectors = new List<RealVector>(combination) {v};

			if (!LinearlyDependent(allVectors.ToArray())) throw new
				ArgumentException("This vector is not linearly dependent on the given array");

			return null;
		}

		public static VectorSpace ColumnSpace(this RealMatrix m)
		{
			return null;
		}
		
		public static bool CloseTo(this int d1, int d2, double tolerance = Tolerance)
		{
			return System.Math.Abs(d1 - d2) <= tolerance;
		}

		public static bool CloseTo(this double d1, double d2, double tolerance = Tolerance)
		{
			return System.Math.Abs(d1 - d2) <= tolerance;
		}

		public static bool CloseTo(this Real d1, Real d2, double tolerance = Tolerance)
		{
			return System.Math.Abs(d1 - d2) <= tolerance;
		}

		public static RealMatrix UnitMatrix(int size)
		{
			return new RealMatrix(size);
		}

		public static Matrix<T> UnitMatrix<T>(int size) where T : FieldMember, new()
		{
			return new Matrix<T>(size);
		}

		public static RealMatrix NullMatrix(int height, int width)
		{
			return new RealMatrix(height, width);
		}
	}
}
