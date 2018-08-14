using System;
using System.Linq;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Main;

namespace LinearAlgebra.ComplexLinearAlgebra
{
	public class ComplexMatrix : Matrix
	{
		public new Complex[,] Indices
		{
			get => ComplexIndices;
			set
			{
				ComplexIndices = value;
				Height = value.GetLength(0);
				Width = value.GetLength(1);
			}
		}

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
			Indices = new Complex[vectors.Length, vectors[0].Dimension];

			this[type] = vectors;

			Type = type;
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

		/// <summary>
		/// Find the determinant by expanding along the first row
		/// </summary>
		/// <returns></returns>
		public new Complex Determinant()
		{
			if (!IsSquare())
				throw new
					IncompatibleOperationException(IncompatibleMatrixOperationType.Determinant);

			if (Width == 1)
				return Indices[0, 0];

			Complex result = 0;

			int i = 0;

			for (int j = 0; j < Width; j++)
			{
				result += Indices[i, j] * Cofactor(i, j);
			}

			return result;
		}

		/// <summary>
		/// Get the inverse of the matrix, which is done by dividing the adjugate matrix by the determinant
		/// </summary>
		/// <returns></returns>
		public new ComplexMatrix Inverse()
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(IncompatibleMatrixOperationType.Inverse);

			return Adjugate() / Determinant();
		}

		/// <summary>
		/// The transpose conjugated, or the conjugate transposed, whatever you like more
		/// </summary>
		/// <returns></returns>
		public ComplexMatrix ConjugateTranspose()
		{
			return Conjugate().Transpose();
		}

		/// <summary>
		/// Get the transpose of this matrix, that is, each value i,j becomes j,i
		/// </summary>
		/// <returns></returns>
		public new ComplexMatrix Transpose()
		{
			Complex[,] indices = new Complex[Width, Height];

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					indices[j, i] = Indices[i, j];
				}
			}

			return new ComplexMatrix(indices);
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
		/// Returns the submatrix obtained by excluding row m and column n
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public new ComplexMatrix SubMatrix(int m, int n)
		{
			Complex[,] indices = new Complex[Height - 1, Width - 1];

			if (m >= Height || n >= Width)
				throw new IndexOutOfRangeException();

			for (int i = 0; i < Height; i++)
			{
				if (i == m)
					continue;

				for (int j = 0; j < Width; j++)
				{
					if (j == n)
						continue;

					indices[i > m ? i - 1 : i, j > n ? j - 1 : j] = Indices[i, j];
				}
			}

			return new ComplexMatrix(indices);
		}

		/// <summary>
		/// Gets the m,n minor, that is, the determinant of the m,n submatrix
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public new Complex Minor(int m, int n)
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(IncompatibleMatrixOperationType.Determinant);

			return SubMatrix(m, n).Determinant();
		}

		/// <summary>
		/// Gets the matrix of minors, which replaces every index its corresponding minor
		/// </summary>
		/// <returns></returns>
		public new ComplexMatrix MatrixOfMinors()
		{
			if (!IsSquare())
				throw new
					IncompatibleOperationException(IncompatibleMatrixOperationType.Determinant);

			Complex[,] indices = Indices;

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					indices[i, j] = Minor(i, j);
				}
			}

			return new ComplexMatrix(indices);
		}

		/// <summary>
		/// Gets the i,j-th cofactor of this matrix
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		public new Complex Cofactor(int i, int j)
		{
			if (!IsSquare())
				throw new
					IncompatibleOperationException(IncompatibleMatrixOperationType.Determinant);

			return Math.Pow(-1, i + j) * Minor(i, j);
		}

		/// <summary>
		/// Gets the cofactor matrix, which is the matrix of minors where every index
		/// 
		/// </summary>
		/// <returns></returns>
		public new ComplexMatrix CofactorMatrix()
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(IncompatibleMatrixOperationType.Determinant);

			Complex[,] indices = Indices;

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					indices[i, j] = Cofactor(i, j);
				}
			}

			return new ComplexMatrix(indices);
		}

		/// <summary>
		/// Gets the adjugate matrix, which is the transpose of the cofactor matrix
		/// </summary>
		/// <returns></returns>
		public new ComplexMatrix Adjugate()
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(IncompatibleMatrixOperationType.Determinant);

			return CofactorMatrix().Transpose();
		}

		/// <summary>
		/// Whether this is an n-by-n matrix
		/// </summary>
		/// <returns></returns>
		public new bool IsSquare()
		{
			return Width == Height;
		}

		/// <summary>
		/// Whether this matrix A is symmetric, that is A = Aᵀ
		/// </summary>
		/// <returns></returns>
		public new bool IsSymmetric()
		{
			return this == Transpose();
		}

		/// <summary>
		/// Whether this matrix A is antisymmetric, that is A = -Aᵀ
		/// </summary>
		/// <returns></returns>
		public new bool IsAntiSymmetric()
		{
			return this == -Transpose();
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
		/// Turn this Matrix into an array of vectors (the type parameter determines
		/// whether the columns or rows will be returned).
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public new ComplexVector[] this[VectorType type]
		{
			get
			{
				ComplexVector[] result;

				switch (type)
				{
					case VectorType.Column:
						result = new ComplexVector[Width];

						for (int j = 0; j < result.Length; j++)
						{
							result[j] = new ComplexVector(Height);
						}

						for (int i = 0; i < Height; i++)
						{
							for (int j = 0; j < Width; j++)
							{
								result[j][i] = Indices[i, j];
							}
						}
						return result;
					case VectorType.Row:
						result = new ComplexVector[Height];

						for (int i = 0; i < result.Length; i++)
						{
							result[i] = new ComplexVector(Width);
						}

						for (int i = 0; i < Height; i++)
						{
							for (int j = 0; j < Width; j++)
							{
								result[i][j] = Indices[i, j];
							}
						}
						return result;
					default:
						throw new ArgumentException("Given argument was not a vectortype");
				}
			}
			set
			{
				switch (type)
				{
					case VectorType.Column:
						Indices = new Complex[value[0].Dimension, value.Length];

						for (int j = 0; j < value.Length; j++)
						{
							for (int i = 0; i < value[0].Dimension; i++)
							{
								Indices[i, j] = value[j][i];
							}
						}
						break;
					case VectorType.Row:
						Indices = new Complex[value.Length, value[0].Dimension];

						for (int i = 0; i < value.Length; i++)
						{
							for (int j = 0; j < value[0].Dimension; j++)
							{
								Indices[i, j] = value[i][j];
							}
						}
						break;
				}
			}
		}

		protected bool Equals(ComplexMatrix other)
		{
			return Equals(ComplexIndices, other.ComplexIndices);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != GetType())
				return false;
			return Equals((ComplexMatrix)obj);
		}

		public override int GetHashCode()
		{
			var hashCode = Indices != null ? Indices.GetHashCode() : 0;
			return hashCode;
		}

		/// <summary>
		/// Equality comparison for matrices
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ComplexMatrix left, ComplexMatrix right)
		{
			// If the property width is null, the matrix must also be null
			if (left?.Width == null && right?.Width == null)
				return true;
			if (left?.Width == null || right?.Width == null)
				return false;
			if (!Addable(left, right))
				return false;

			for (int k = 0; k < left.Height * left.Width; k++)
			{
				if (!left.GetIndices().ToList()[k].CloseTo(right.GetIndices().ToList()[k]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Inequality for matrices
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(ComplexMatrix left, ComplexMatrix right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Add two matrices together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexMatrix operator +(ComplexMatrix left, ComplexMatrix right)
		{
			if (Addable(left, right))
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
			throw new IncompatibleOperationException(IncompatibleMatrixOperationType.Addition);
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
			if (Addable(left, right))
			{
				return left + -right;
			}
			throw new IncompatibleOperationException(IncompatibleMatrixOperationType.Addition,
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
			if (!Multipliable(left, right))
				throw new
					IncompatibleOperationException(IncompatibleMatrixOperationType.Multiplication);

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
					IncompatibleOperationException(IncompatibleMatrixOperationType.Multiplication);

			if (right == 0)
				return new ComplexMatrix(left.Width);

			if (right < 0)
				return left.Inverse() ^ (-right);

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
	}
}