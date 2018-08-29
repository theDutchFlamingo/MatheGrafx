using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Math.ComplexLinearAlgebra;
using Math.Fields;
using Math.Main;
using Math.Exceptions;
using Math.Fields.Members;

namespace Math
{
	/// <summary>
	/// The original matrix class without being a subclass of MatrixBase
	/// </summary>
	public class MatrixCopy : IEnumerable<double>, IEnumerable<RealVector>
	{
		public static VectorType DefaultVectorType { get; set; } = VectorType.Column;

		protected Complex[,] ComplexIndices;

		public double[,] Indices
		{
			get
			{
				double[,] result = new double[Height, Width];

				for (int i = 0; i < Height; i++)
				{
					for (int j = 0; j < Width; j++)
					{
						result[i, j] = ComplexIndices[i, j].Real;
					}
				}

				return result;
			}
			set
			{
				Height = value.GetLength(0);
				Width = value.GetLength(1);

				Complex[,] indices = new Complex[Height, Width];

				for (int i = 0; i < Height; i++)
				{
					for (int j = 0; j < Width; j++)
					{
						indices[i, j] = new Complex(value[i, j], 0);
					}
				}

				ComplexIndices = indices;
			}
		}

		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public VectorType Type { get; set; } = DefaultVectorType;


		/// <summary>
		/// Constructor intended specifically for the subclass ComplexMatrix
		/// </summary>
		/// <param name="indices"></param>
		protected MatrixCopy(Complex[,] indices)
		{
			Height = indices.GetLength(0);
			Width = indices.GetLength(1);

			ComplexIndices = indices;
		}

		/// <summary>
		/// Clone the given matrix
		/// </summary>
		/// <param name="m"></param>
		public MatrixCopy(MatrixCopy m)
		{
			Indices = new double[m.Height, m.Width];

			// Copies the indices per individual double to make a clone
			// (not another pointer to the same object)
			for (int i = 0; i < m.Height; i++)
			{
				for (int j = 0; j < m.Width; j++)
				{
					Indices[i, j] = m.Indices[i, j];
				}
			}
		}

		/// <summary>
		/// Creates a Matrix with the given indices
		/// </summary>
		/// <param name="indices"></param>
		public MatrixCopy(double[,] indices)
		{
			Indices = indices;
		}

		/// <summary>
		/// Matrix constructor with integers as indices
		/// </summary>
		/// <param name="indices"></param>
		public MatrixCopy(int[,] indices)
		{
			Indices = new double[indices.GetLength(0), indices.GetLength(1)];

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					Indices[i, j] = indices[i,j];
				}
			}
		}

		/// <summary>
		/// Matrix constructor with an array of vectors,
		/// can be columns or rows based on type
		/// </summary>
		/// <param name="vectors"></param>
		public MatrixCopy(RealVector[] vectors) : this(vectors, DefaultVectorType)
		{
			
		}

		/// <summary>
		/// Matrix constructor with an array of vectors,
		/// can be columns or rows based on type
		/// </summary>
		/// <param name="vectors"></param>
		/// <param name="type">Determines whether the vectors are columns or rows</param>
		public MatrixCopy(RealVector[] vectors, VectorType type)
		{
			Indices = new double[vectors.Length, vectors[0].Dimension];

			this[type] = vectors;

			Type = type;
		}

		/// <summary>
		/// Creates a diagonal matrix with the given vector on the diagonal
		/// </summary>
		/// <param name="diagonal"></param>
		public MatrixCopy(RealVector diagonal)
		{
			Indices = new double[diagonal.Dimension, diagonal.Dimension];

			for (int k = 0; k < diagonal.Dimension; k++)
			{
				Indices[k, k] = diagonal[k];
			}
		}

		/// <summary>
		/// Creates a diagonal matrix with the given array on the diagonal
		/// </summary>
		/// <param name="diagonal"></param>
		public MatrixCopy(double[] diagonal)
		{
			int size = diagonal.Length;

			Indices = new double[size, size];

			for (int i = 0; i < size; i++)
			{
				Indices[i, i] = diagonal[i];
			}
		}

		/// <summary>
		/// Create a unit matrix with the given size
		/// </summary>
		/// <param name="size"></param>
		public MatrixCopy(int size)
		{
			Indices = new double[size,size];

			for (int k = 0; k < size; k++)
			{
				Indices[k, k] = 1;
			}
		}

		/// <summary>
		/// Create a null matrix with given sizes
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public MatrixCopy(int height, int width)
		{
			Indices = new double[height,width];

			// Might be unnecessary?
			//for (int i = 0; i < height; i++)
			//{
			//	for (int j = 0; j < width; j++)
			//	{
			//		Indices[i, j] = 0;
			//	}
			//}
		}

		/// <summary>
		/// Find the determinant by expanding along the first row
		/// </summary>
		/// <returns></returns>
		public double Determinant()
		{
			if (!IsSquare()) throw new
				IncompatibleOperationException(MatrixOperationType.Determinant);

			if (Width == 1) return Indices[0, 0];

			double result = 0;

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
		public MatrixCopy Inverse()
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(MatrixOperationType.Inverse);

			return Adjugate() / Determinant();
		}

		/// <summary>
		/// Get the transpose of this matrix, that is, each value i,j becomes j,i
		/// </summary>
		/// <returns></returns>
		public MatrixCopy Transpose()
		{
			double[,] indices = new double[Width, Height];

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					indices[j, i] = Indices[i, j];
				}
			}

			return new MatrixCopy(indices);
		}

		/// <summary>
		/// Returns the submatrix obtained by excluding row m and column n
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public MatrixCopy SubMatrix(int m, int n)
		{
			double[,] indices = new double[Height - 1, Width - 1];

			if (m >= Height || n >= Width) throw new IndexOutOfRangeException();

			for (int i = 0; i < Height; i++)
			{
				if (i == m) continue;

				for (int j = 0; j < Width; j++)
				{
					if (j == n) continue;

					indices[i > m ? i - 1 : i, j > n ? j - 1 : j] = Indices[i, j];
				}
			}

			return new MatrixCopy(indices);
		}

		/// <summary>
		/// Gets the m,n minor, that is, the determinant of the m,n submatrix
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public double Minor(int m, int n)
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);

			return SubMatrix(m, n).Determinant();
		}

		/// <summary>
		/// Gets the matrix of minors, which replaces every index its corresponding minor
		/// </summary>
		/// <returns></returns>
		public MatrixCopy MatrixOfMinors()
		{
			if (!IsSquare()) throw new
				IncompatibleOperationException(MatrixOperationType.Determinant);

			double[,] indices = Indices;

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					indices[i, j] = Minor(i, j);
				}
			}

			return new MatrixCopy(indices);
		}

		/// <summary>
		/// Gets the i,j-th cofactor of this matrix
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		public double Cofactor(int i, int j)
		{
			if (!IsSquare()) throw new
				IncompatibleOperationException(MatrixOperationType.Determinant);

			return System.Math.Pow(-1, i + j) * Minor(i, j);
		}

		/// <summary>
		/// Gets the cofactor matrix, which is the matrix of minors where every index
		/// 
		/// </summary>
		/// <returns></returns>
		public MatrixCopy CofactorMatrix()
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);

			double[,] indices = Indices;

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					indices[i, j] = Cofactor(i,j);
				}
			}

			return new MatrixCopy(indices);
		}

		/// <summary>
		/// Gets the adjugate matrix, which is the transpose of the cofactor matrix
		/// </summary>
		/// <returns></returns>
		public MatrixCopy Adjugate()
		{
			if (!IsSquare())
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);

			return CofactorMatrix().Transpose();
		}

		/// <summary>
		/// Whether this is an n-by-n matrix
		/// </summary>
		/// <returns></returns>
		public bool IsSquare()
		{
			return Width == Height;
		}

		/// <summary>
		/// Whether this matrix A is symmetric, that is A = Aᵀ
		/// </summary>
		/// <returns></returns>
		public bool IsSymmetric()
		{
			return this == Transpose();
		}

		/// <summary>
		/// Whether this matrix A is antisymmetric, that is A = -Aᵀ
		/// </summary>
		/// <returns></returns>
		public bool IsAntiSymmetric()
		{
			return this == -Transpose();
		}

		/// <summary>
		/// Get the index at position index, based on whether the 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public double this[int index]
		{
			get
			{
				switch (Type)
				{
					case VectorType.Column:
						return Indices[index % Height, index / Height];
					case VectorType.Row:
						return Indices[index / Width, index % Width];
					default:
						throw new ArgumentException("The VectorType was not a valid object", nameof(Type));
				}
			}
			set
			{
				switch (Type)
				{
					case VectorType.Column:
						Indices[index % Height, index / Height] = value;
						break;
					case VectorType.Row:
						Indices[index / Width, index % Width] = value;
						break;
					default:
						throw new ArgumentException("The VectorType was not a valid object", nameof(Type));
				}
			}
		}

		/// <summary>
		/// Get or set a single value at vertical position i, horizontal position j
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		public double this[int i, int j]
		{
			get => Indices[i, j];
			set => Indices[i, j] = value;
		}

		/// <summary>
		/// Get or set a vector at index n, whether it's a column or vector depends on
		/// the given VectorType type
		/// </summary>
		/// <param name="n"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public RealVector this[int n, VectorType type]
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
		public RealVector[] this[VectorType type]
		{
			get
			{
				RealVector[] result;

				switch (type)
				{
					case VectorType.Column:
						result = new RealVector[Width];

						for (int j = 0; j < result.Length; j++)
						{
							result[j] = new RealVector(Height);
						}

						for (int i = 0; i < Height; i++)
						{
							for (int j = 0; j < Width; j++)
							{
								result[j][i] = Indices[i,j];
							}
						}
						return result;
					case VectorType.Row:
						result = new RealVector[Height];

						for (int i = 0; i < result.Length; i++)
						{
							result[i] = new RealVector(Width);
						}

						for (int i = 0; i < Height; i++)
						{
							for (int j = 0; j < Width; j++)
							{
								result[i][j] = Indices[i, j];
							}
						}
						return result;
					default: throw new ArgumentException("Given argument was not a vectortype");
				}
			}
			set
			{
				switch (type)
				{
					case VectorType.Column:
						Indices = new double[value[0].Dimension,value.Length];

						for (int j = 0; j < value.Length; j++)
						{
							for (int i = 0; i < value[0].Dimension; i++)
							{
								Indices[i, j] = value[j][i];
							}
						}
						break;
					case VectorType.Row:
						Indices = new double[value.Length, value[0].Dimension];

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

		/// <summary>
		/// Format the matrix as a string, written the same way you'd write a 2d array
		/// (Like { { 0, 1}, {2, 3} })
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string result = "{ ";

			foreach (var vector in this[VectorType.Row])
			{
				result += vector + ", ";
			}

			result = result.Remove(result.Length - 2, 1);

			result += "}";

			return result;
		}

		/// <summary>
		/// Format the matrix as a table
		/// </summary>
		/// <param name="precision"></param>
		/// <returns></returns>
		public string ToTable(int precision)
		{
			string result = "";

			int padding = this[VectorType.Row].Select(v => v.Padding(precision)).Max();

			foreach (var vector in this[VectorType.Row])
			{
				result += vector.ToTable(precision, VectorType.Row, padding) + "\n";
			}

			result = result.Remove(result.Length - 1);

			return result;
		}

		/// <summary>
		/// Convert the matrix to a string, and in doing so add bars to the left and right of the table,
		/// like a determinant is being taken.
		/// </summary>
		/// <param name="precision"></param>
		/// <param name="addResult">Whether to add the value of the determinant</param>
		/// <returns></returns>
		public string ToDeterminant(int precision, bool addResult = false)
		{
			string result = "";

			int middle = (int) System.Math.Floor((double) Height / 2);
			int i = 0;
			int padding = this[VectorType.Row].Select(v => v.Padding(precision)).Max();

			foreach (var vector in this[VectorType.Row])
			{
				result += "| " + vector.ToTable(precision, VectorType.Row, padding) +
						  (i == middle && addResult ? $" | = {Determinant()}\n" : " |\n");
				i++;
			}

			result = result.Remove(result.Length - 1);

			return result;
		}

		protected bool Equals(MatrixCopy other)
		{
			return Equals(Indices, other.Indices);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != GetType())
				return false;
			return Equals((MatrixCopy)obj);
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
		public static bool operator ==(MatrixCopy left, MatrixCopy right)
		{
			// If the property width is null, the matrix must also be null
			if (left?.Width == null && right?.Width == null) return true;
			if (left?.Width == null || right?.Width == null) return false;
			if (!Addable(left, right)) return false;

			for (int k = 0; k < left.Height * left.Width; k++)
			{
				if (!left.GetIndices().ToList()[k].CloseTo(right.GetIndices().ToList()[k])) return false;
			}

			return true;
		}

		/// <summary>
		/// Inequality for matrices
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(MatrixCopy left, MatrixCopy right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Add two matrices together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static MatrixCopy operator +(MatrixCopy left, MatrixCopy right)
		{
			if (Addable(left, right))
			{
				double[,] indices = new double[left.Width,left.Height];

				for (int i = 0; i < left.Width; i++)
				{
					for (int j = 0; j < left.Height; j++)
					{
						indices[i, j] = left.Indices[i,j] + right.Indices[i,j];
					}
				}

				return new MatrixCopy(indices);
			}
			throw new IncompatibleOperationException(MatrixOperationType.Addition);
		}

		/// <summary>
		/// Returns the negative of this matrix
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static MatrixCopy operator -(MatrixCopy m)
		{
			double[,] indices = new double[m.Width, m.Height];

			for (int i = 0; i < m.Width; i++)
			{
				for (int j = 0; j < m.Height; j++)
				{
					indices[i, j] = -m.Indices[i, j];
				}
			}

			return new MatrixCopy(indices);
		}

		/// <summary>
		/// Subtract the right matrix from the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static MatrixCopy operator -(MatrixCopy left, MatrixCopy right)
		{
			if (Addable(left, right))
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
		public static MatrixCopy operator *(MatrixCopy left, MatrixCopy right)
		{
			if (!Multipliable(left, right)) throw new
				IncompatibleOperationException(MatrixOperationType.Multiplication);

			double [,] indices = new double[left.Height,right.Width];

			for (int i = 0; i < left.Height; i++)
			{
				for (int j = 0; j < right.Width; j++)
				{
					indices[i, j] = left[i, VectorType.Row] * right[j, VectorType.Column];
				}
			}

			return new MatrixCopy(indices);
		}

		/// <summary>
		/// Power of a matrix (returns unit for right = 0, and Inverse(left)^(-right) for right &lt; 0
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static MatrixCopy operator ^(MatrixCopy left, int right)
		{
			if (!left.IsSquare()) throw new
				IncompatibleOperationException(MatrixOperationType.Multiplication);

			if (right == 0) return new MatrixCopy(left.Width);

			if (right < 0) return left.Inverse() ^ (-right);

			MatrixCopy m = left;

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
		public static MatrixCopy operator *(MatrixCopy left, double right)
		{
			double[,] indices = left.Indices;

			for (int i = 0; i < left.Height; i++)
			{
				for (int j = 0; j < left.Width; j++)
				{
					indices[i, j] *= right;
				}
			}

			return new MatrixCopy(indices);
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static MatrixCopy operator *(double left, MatrixCopy right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division of a matrix and a real number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static MatrixCopy operator /(MatrixCopy left, double right)
		{
			return left * (1 / right);
		}

		/// <summary>
		/// Find out whether the two matrices can be added
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
        protected static bool Addable(MatrixCopy left, MatrixCopy right)
        {
            return left.Width == right.Width && left.Height == right.Height;
        }

		/// <summary>
		/// Find out whether the two matrices can be multiplied
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		protected static bool Multipliable(MatrixCopy left, MatrixCopy right)
        {
            return left.Width == right.Height;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
	        return ((IEnumerable<double>) this).GetEnumerator();
        }

		IEnumerator<double> IEnumerable<double>.GetEnumerator()
		{
			int position = 0;

			while (position < Width * Height)
			{
				yield return this[position];
				position++;
			}
		}

		IEnumerator<RealVector> IEnumerable<RealVector>.GetEnumerator()
		{
			int position = 0;

			switch (Type)
			{
				case VectorType.Column:
					while (position < Width)
					{
						yield return this[position, Type];
						position++;
					}
					yield break;
				case VectorType.Row:
					while (position < Height)
					{
						yield return this[position, Type];
						position++;
					}
					yield break;
			}
		}

		/// <summary>
		/// Get the enumerable that loops over the individual values
		/// </summary>
		/// <returns></returns>
		public IEnumerable<double> GetIndices()
		{
			return this;
		}

		/// <summary>
		/// Get the enumerable that loops over the vectors (rows or columns based on the given VectorType)
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IEnumerable<RealVector> GetVectors(VectorType type)
		{
			Type = type;

			return this;
		}

		/// <summary>
		/// Get the enumerable that loops over the rows
		/// </summary>
		/// <returns></returns>
		public List<RealVector> GetRows()
		{
			return GetVectors(VectorType.Row).ToList();
		}

		/// <summary>
		/// Get the enumerable that loops over the columns
		/// </summary>
		/// <returns></returns>
		public List<RealVector> GetColumns()
		{
			return GetVectors(VectorType.Column).ToList();
		}
	}
}
