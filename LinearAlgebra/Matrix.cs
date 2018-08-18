using System;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Fields;

namespace LinearAlgebra
{
	/// <summary>
	/// Specific implementation of MatrixBase&lt;Real&gt; because real matrices are by far the most common ones
	/// </summary>
	public class Matrix : MatrixBase<Real>
	{
		#region Constructors

		/// <summary>
		/// Constructor of a MatrixBase
		/// </summary>
		/// <param name="indices"></param>
		public Matrix(Real[,] indices) : base(indices)
		{
			
		}

		/// <summary>
		/// Constructor of a MatrixBase
		/// </summary>
		/// <param name="indices"></param>
		public Matrix(double[,] indices) : base(indices.GetLength(0), indices.GetLength(1))
		{
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					Indices[i, j] = indices[i, j];
				}
			}
		}

		/// <summary>
		/// Constructor of a MatrixBase
		/// </summary>
		/// <param name="indices"></param>
		public Matrix(int[,] indices) : base(indices.GetLength(0), indices.GetLength(1))
		{
			Real[,] realIndices = new Real[Height, Width];

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					realIndices[i, j] = new Real(indices[i, j]);
				}
			}

			Indices = realIndices;
		}

		/// <summary>
		/// Clone the given matrix
		/// </summary>
		/// <param name="m"></param>
		public Matrix(Matrix m) : base(m)
		{
			
		}

		/// <summary>
		/// Matrix constructor with an array of vectors,
		/// can be columns or rows based on type
		/// </summary>
		/// <param name="vectors"></param>
		public Matrix(Vector[] vectors) : this(vectors, DefaultVectorType)
		{

		}

		/// <summary>
		/// Matrix constructor with an array of vectors,
		/// can be columns or rows based on type
		/// </summary>
		/// <param name="vectors"></param>
		/// <param name="type">Determines whether the vectors are columns or rows</param>
		public Matrix(Vector[] vectors, VectorType type) : base(vectors, type)
		{
			
		}

		/// <summary>
		/// Creates a diagonal matrix with the given vector on the diagonal
		/// </summary>
		/// <param name="diagonal"></param>
		public Matrix(Vector diagonal) : base(diagonal)
		{
			
		}

		/// <summary>
		/// Creates a diagonal matrix with the given array on the diagonal
		/// </summary>
		/// <param name="diagonal"></param>
		public Matrix(Real[] diagonal) : base(diagonal)
		{
			
		}

		/// <summary>
		/// Create a unit matrix with the given size
		/// </summary>
		/// <param name="size"></param>
		public Matrix(int size) : base(size)
		{
			
		}

		/// <summary>
		/// Create a null matrix with given sizes
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Matrix(int height, int width) : base(height, width)
		{
			
		}

		#endregion

		#region Indexing

		/// <summary>
		/// Get or set a vector at index n, whether it's a column or vector depends on
		/// the given VectorType type
		/// </summary>
		/// <param name="n"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public new Vector this[int n, VectorType type]
		{
			get => this[type][n];
			set
			{
				int i;
				int j;

				switch (type)
				{
					case VectorType.Column:
						if (n >= Width)
							throw new IndexOutOfRangeException($"Index was {n}, max is {Width - 1}");
						if (value.Dimension != Height)
							throw new ArgumentException("Vector does not have the correct size" +
														$", should be: {Height}");

						j = n;

						for (i = 0; i < value.Dimension; i++)
						{
							Indices[i, j] = value[i];
						}

						break;
					case VectorType.Row:
						i = n;

						for (j = 0; j < value.Dimension; j++)
						{
							Indices[i, j] = value[j];
						}

						break;
				}
			}
		}

		/// <summary>
		/// Turn this Matrix into an array of vectors (the type parameter determines
		/// whether the columns or rows will be returned).
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public new Vector[] this[VectorType type]
		{
			get
			{
				Vector[] result;

				switch (type)
				{
					case VectorType.Column:
						result = new Vector[Width];

						for (int j = 0; j < result.Length; j++)
						{
							result[j] = new Vector(Height);
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
						result = new Vector[Height];

						for (int i = 0; i < result.Length; i++)
						{
							result[i] = new Vector(Width);
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
				Real[,] indices = null;

				switch (type)
				{
					case VectorType.Column:
						indices = new Real[value[0].Dimension, value.Length];

						for (int j = 0; j < value.Length; j++)
						{
							for (int i = 0; i < value[0].Dimension; i++)
							{
								indices[i, j] = value[j][i];
							}
						}
						break;
					case VectorType.Row:
						indices = new Real[value.Length, value[0].Dimension];

						for (int i = 0; i < value.Length; i++)
						{
							for (int j = 0; j < value[0].Dimension; j++)
							{
								indices[i, j] = value[i][j];
							}
						}
						break;
				}

				Indices = indices;
			}
		}

		#endregion

		#region Operators

		/// <summary>
		/// Add two matrices together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator +(Matrix left, Matrix right)
		{
			if (left.Addable(right))
			{
				Real[,] indices = new Real[left.Width, left.Height];

				for (int i = 0; i < left.Width; i++)
				{
					for (int j = 0; j < left.Height; j++)
					{
						indices[i, j] = left.Indices[i, j] + right.Indices[i, j];
					}
				}

				return new Matrix(indices);
			}
			throw new IncompatibleOperationException(IncompatibleMatrixOperationType.Addition);
		}

		/// <summary>
		/// Returns the negative of this matrix
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Matrix operator -(Matrix m)
		{
			Real[,] indices = new Real[m.Width, m.Height];

			for (int i = 0; i < m.Width; i++)
			{
				for (int j = 0; j < m.Height; j++)
				{
					indices[i, j] = -m.Indices[i, j];
				}
			}

			return new Matrix(indices);
		}

		/// <summary>
		/// Subtract the right matrix from the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator -(Matrix left, Matrix right)
		{
			if (left.Addable(right))
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
		public static Matrix operator *(Matrix left, Matrix right)
		{
			if (!left.Multipliable(right))
				throw new
					IncompatibleOperationException(IncompatibleMatrixOperationType.Multiplication);

			Real[,] indices = new Real[left.Height, right.Width];

			for (int i = 0; i < left.Height; i++)
			{
				for (int j = 0; j < right.Width; j++)
				{
					indices[i, j] = left[i, VectorType.Row] * right[j, VectorType.Column];
				}
			}

			return new Matrix(indices);
		}

		/// <summary>
		/// Power of a matrix (returns unit for right = 0, and Inverse(left)^(-right) for right &lt; 0
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator ^(Matrix left, int right)
		{
			if (!left.IsSquare())
				throw new
					IncompatibleOperationException(IncompatibleMatrixOperationType.Multiplication);

			if (right == 0)
				return new Matrix(left.Width);

			if (right < 0)
				return (Matrix)left.Inverse() ^ (-right);

			Matrix m = left;

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
		public static Matrix operator *(Matrix left, double right)
		{
			return left * (Real) right;
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator *(double left, Matrix right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division of a matrix and a real number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator /(Matrix left, double right)
		{
			return left * (1 / right);
		}

		/// <summary>
		/// Scalar multiplication with a complex number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator *(Matrix left, Real right)
		{
			Real[,] indices = left.Indices;

			for (int i = 0; i < left.Height; i++)
			{
				for (int j = 0; j < left.Width; j++)
				{
					indices[i, j] *= right;
				}
			}

			return new Matrix(indices);
		}

		/// <summary>
		/// Scalar multiplication with a complex number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator *(Real left, Matrix right)
		{
			return right * left;
		}

		/// <summary>
		/// Divide the matirx by a scalar
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix operator /(Matrix left, Real right)
		{
			return left * (1 / right);
		}

		#endregion
	}
}
