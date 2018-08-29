using System;
using System.Linq;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Fields;

namespace LinearAlgebra.ComplexLinearAlgebra
{
	/// <summary>
	/// Specific implementation of MatrixBase&lt;Complex&gt; because ComplexMatrices are still pretty common.
	/// Also adds some specific complex methods.
	/// </summary>
	public class ComplexMatrix : Matrix<Complex>
	{
		#region Constructors

		/// <summary>
		/// Creates a complex matrix with each index
		/// </summary>
		/// <param name="indices"></param>
		public ComplexMatrix(Complex[,] indices) : base(indices)
		{
			// All work is done in the base class
		}

		/// <summary>
		/// Create a complex matrix with complex vectors
		/// </summary>
		/// <param name="vectors"></param>
		/// <param name="type"></param>
		public ComplexMatrix(ComplexVector[] vectors, VectorType type) : base(vectors, type)
		{
			
		}

		/// <summary>
		/// Creates a unit matrix with given size
		/// </summary>
		/// <param name="size"></param>
		public ComplexMatrix(int size) : base(size)
		{

		}

		/// <summary>
		/// Creates a null matrix with given height and width
		/// </summary>
		/// <param name="height"></param>
		/// <param name="width"></param>
		public ComplexMatrix(int height, int width) : base(height, width)
		{

		}

		#endregion

		#region Complex-specific Methods

		/// <summary>
		/// The transpose conjugated, or the conjugate transposed, whatever you like more
		/// </summary>
		/// <returns></returns>
		public ComplexMatrix ConjugateTranspose()
		{
			return (ComplexMatrix) Conjugate().Transpose();
		}

		/// <summary>
		/// Gets this matrix with all its indices conjugated
		/// </summary>
		/// <returns></returns>
		public ComplexMatrix Conjugate()
		{
			Complex[,] indices = new Complex[Height, Width];

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					indices[i, j] = Indices[i, j].Conjugate();
				}
			}

			return new ComplexMatrix(indices);
		}

		/// <summary>
		/// Whether this matrix is Hermitian, that is, whether it's equal to the conjugate of its transpose
		/// </summary>
		/// <returns></returns>
		public bool IsHermitian()
		{
			return this == ConjugateTranspose();
		}

		/// <summary>
		/// Whether this matrix is anti-Hermitian, or skew-Hermitian, that is, whether it's equal to
		/// the negative of the conjugate of its transpose
		/// </summary>
		/// <returns></returns>
		public bool IsAntiHermitian()
		{
			return this == -ConjugateTranspose();
		}

		#endregion

		#region Operators

		/// <summary>
		/// Add two matrices together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator +(ComplexMatrix left, ComplexMatrix right)
		{
			if (left.Addable(right))
			{
				Complex[,] indices = new Complex[left.Width, left.Height];

				for (int i = 0; i < left.Width; i++)
				{
					for (int j = 0; j < left.Height; j++)
					{
						indices[i, j] = left.Indices[i, j] + right.Indices[i, j];
					}
				}

				return new ComplexMatrix(indices);
			}
			throw new IncompatibleOperationException(MatrixOperationType.Addition);
		}

		/// <summary>
		/// Returns the negative of this matrix
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static ComplexMatrix operator -(ComplexMatrix m)
		{
			Complex[,] indices = new Complex[m.Width, m.Height];

			for (int i = 0; i < m.Width; i++)
			{
				for (int j = 0; j < m.Height; j++)
				{
					indices[i, j] = -m.Indices[i, j];
				}
			}

			return new ComplexMatrix(indices);
		}

		/// <summary>
		/// Subtract the right matrix from the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator -(ComplexMatrix left, ComplexMatrix right)
		{
			if (left.Addable(right))
			{
				return left + -right;
			}
			throw new IncompatibleOperationException(MatrixOperationType.Addition,
				"The two matrices could not be subtracted because their dimensions were unequal.");
		}

		/// <summary>
		/// Multiply the two matrices together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator *(ComplexMatrix left, ComplexMatrix right)
		{
			if (!left.Multipliable(right))
				throw new
					IncompatibleOperationException(MatrixOperationType.Multiplication);

			Complex[,] indices = new Complex[left.Height, right.Width];

			for (int i = 0; i < left.Height; i++)
			{
				for (int j = 0; j < right.Width; j++)
				{
					indices[i, j] = left[i, VectorType.Row] * right[j, VectorType.Column];
				}
			}

			return new ComplexMatrix(indices);
		}

		/// <summary>
		/// Power of a matrix (returns unit for right = 0, and Inverse(left)^(-right) for right &lt; 0
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator ^(ComplexMatrix left, int right)
		{
			if (!left.IsSquare())
				throw new
					IncompatibleOperationException(MatrixOperationType.Multiplication);

			if (right == 0)
				return new ComplexMatrix(left.Width);

			if (right < 0)
				return (ComplexMatrix) left.Inverse() ^ (-right);

			ComplexMatrix m = left;

			for (int i = 1; i < right; i++)
			{
				m = m * left;
			}

			return m;
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator *(ComplexMatrix left, double right)
		{
			return left * (Complex) right;
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator *(double left, ComplexMatrix right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division of a matrix and a real number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator /(ComplexMatrix left, double right)
		{
			return left * (1 / right);
		}

		/// <summary>
		/// Scalar multiplication with a complex number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator *(ComplexMatrix left, Complex right)
		{
			Complex[,] indices = left.Indices;

			for (int i = 0; i < left.Height; i++)
			{
				for (int j = 0; j < left.Width; j++)
				{
					indices[i, j] *= right;
				}
			}

			return new ComplexMatrix(indices);
		}

		/// <summary>
		/// Scalar multiplication with a complex number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator *(Complex left, ComplexMatrix right)
		{
			return right * left;
		}

		public static ComplexMatrix operator /(ComplexMatrix left, Complex right)
		{
			return left * (1 / right);
		}

		#endregion
	}
}