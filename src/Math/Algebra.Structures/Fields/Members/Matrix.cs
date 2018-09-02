using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.ComplexLinearAlgebra;
using Math.Exceptions;
using Math.Settings;

namespace Math.Algebra.Structures.Fields.Members
{
	/// <summary>
	/// A class for matrices of any type. The generic parameter must be a subclass of
	/// RingMember to ensure that basic operations like addition, subtraction and multiplication
	/// are possible. If the type is also IInvertible, then this matrix can have an inverse, and
	/// scalar division is possible.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Matrix<T> : FieldMember, IEnumerable<Vector<T>>, IEnumerable<T> where T : RingMember, new()
	{
		/**
		 * All fields and properties: the indices of the matrix, the height and width,
		 * and the vector type that this matrix is described by.
		 */
		#region Fields & Properties

		private T[,] _indices;

		public T[,] Indices
		{
			get
			{
				var indices = new T[Height, Width];

				for (var i = 0; i < Height; i++)
				{
					for (var j = 0; j < Width; j++)
					{
						indices[i, j] = _indices[i, j];
					}
				}

				return indices;
			}
			set
			{
				Height = value.GetLength(0);
				Width = value.GetLength(1);

				var indices = new T[Height, Width];

				for (var i = 0; i < Height; i++)
				{
					for (var j = 0; j < Width; j++)
					{
						indices[i, j] = value[i, j];
					}
				}

				_indices = indices;

			}
		}

		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public VectorType Type { get; set; } = ConversionSettings.DefaultVectorType;

		#endregion

		/**
		 * The constructors; two for general matrices with all indices filled,
		 * two for diagonal matrices, one for unit matrices and one for null matrices.
		 */
		#region Constructors
		
		/// <summary>
		/// Constructor of a MatrixBase with no arguments: simply generates a 1x1 null matrix.
		/// </summary>
		public Matrix()
		{
			Indices = new [,]{{ new T() }};
		}

		/// <summary>
		/// Constructor of a MatrixBase
		/// </summary>
		/// <param name="indices"></param>
		public Matrix(T[,] indices)
		{
			Indices = indices;
		}

		/// <summary>
		/// Clone the given matrix
		/// </summary>
		/// <param name="m"></param>
		public Matrix(Matrix<T> m)
		{
			Indices = new T[m.Height, m.Width];

			// Copies the indices per individual double to make a clone
			// (not another pointer to the same object)
			for (var i = 0; i < m.Height; i++)
			{
				for (var j = 0; j < m.Width; j++)
				{
					_indices[i, j] = m.Indices[i, j];
				}
			}
		}

		/// <summary>
		/// Matrix constructor with an array of vectors,
		/// can be columns or rows based on type
		/// </summary>
		/// <param name="vectors"></param>
		public Matrix(Vector<T>[] vectors) : this(vectors, ConversionSettings.DefaultVectorType)
		{

		}

		/// <summary>
		/// Matrix constructor with an array of vectors,
		/// can be columns or rows based on type
		/// </summary>
		/// <param name="vectors"></param>
		/// <param name="type">Determines whether the vectors are columns or rows</param>
		public Matrix(Vector<T>[] vectors, VectorType type)
		{
			Indices = new T[vectors.Length, vectors[0].Dimension];

			this[type] = vectors;

			Type = type;
		}

		/// <summary>
		/// Creates a diagonal matrix with the given vector on the diagonal
		/// </summary>
		/// <param name="diagonal"></param>
		public Matrix(Vector<T> diagonal)
		{
			Indices = new T[diagonal.Dimension, diagonal.Dimension];

			for (var k = 0; k < diagonal.Dimension; k++)
			{
				Indices[k, k] = diagonal[k];
			}
		}

		/// <summary>
		/// Creates a diagonal matrix with the given array on the diagonal
		/// </summary>
		/// <param name="diagonal"></param>
		public Matrix(T[] diagonal)
		{
			var size = diagonal.Length;

			Indices = new T[size, size];

			for (var i = 0; i < size; i++)
			{
				Indices[i, i] = diagonal[i];
			}
		}

		/// <summary>
		/// Create a unit matrix with the given size
		/// </summary>
		/// <param name="size"></param>
		public Matrix(int size)
		{
			Indices = new T[size, size];

			for (var k = 0; k < size; k++)
			{
				Indices[k, k] = new T().Unit<T>();
			}
		}

		/// <summary>
		/// Create a null matrix with given sizes
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Matrix(int height, int width)
		{
			Indices = new T[height, width];

			for (var i = 0; i < height; i++)
			{
				for (var j = 0; j < width; j++)
				{
					_indices[i, j] = new T();
				}
			}
		}

		#endregion

		/**
		 * This is where the biggest part of the MatrixBase class is, with the Determinant(),
		 * the Inverse(), and all the intermediate steps
		 */
		#region Main Functionality

		/// <summary>
		/// Find the determinant by expanding along the first row
		/// </summary>
		/// <returns></returns>
		public T Determinant()
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);
			}

			if (Width == 1)
			{
				return Indices[0, 0];
			}

			var result = new T();

			var i = 0;

			for (var j = 0; j < Width; j++)
			{
				result = result.Add(Indices[i, j].Multiply(Cofactor(i, j)));
			}

			return result;
		}

		/// <summary>
		/// Get the inverse of the matrix, which is done by dividing the adjugate matrix by the determinant
		/// </summary>
		/// <returns></returns>
		public Matrix<T> Inverse()
		{
			if (!IsSquare() || Determinant().IsNull())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Inverse);
			}

			return Adjugate() / Determinant();
		}

		/// <summary>
		/// Get the transpose of this matrix, that is, each value i,j becomes j,i
		/// </summary>
		/// <returns></returns>
		public Matrix<T> Transpose()
		{
			var indices = new T[Width, Height];

			for (var i = 0; i < Height; i++)
			{
				for (var j = 0; j < Width; j++)
				{
					indices[j, i] = Indices[i, j];
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Returns the submatrix obtained by excluding row m and column n
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public Matrix<T> SubMatrix(int m, int n)
		{
			var indices = new T[Height - 1, Width - 1];

			if (m >= Height || n >= Width)
			{
				throw new IndexOutOfRangeException();
			}

			for (var i = 0; i < Height; i++)
			{
				if (i == m)
				{
					continue;
				}

				for (var j = 0; j < Width; j++)
				{
					if (j == n)
					{
						continue;
					}

					indices[i > m ? i - 1 : i, j > n ? j - 1 : j] = Indices[i, j];
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Gets the m,n minor, that is, the determinant of the m,n submatrix
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public T Minor(int m, int n)
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);
			}

			return SubMatrix(m, n).Determinant();
		}

		/// <summary>
		/// Gets the matrix of minors, which replaces every index its corresponding minor
		/// </summary>
		/// <returns></returns>
		public Matrix<T> MatrixOfMinors()
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);
			}

			var indices = Indices;

			for (var i = 0; i < Height; i++)
			{
				for (var j = 0; j < Width; j++)
				{
					indices[i, j] = Minor(i, j);
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Gets the i,j-th cofactor of this matrix
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		public T Cofactor(int i, int j)
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);
			}

			return (i + j) % 2 == 0 ? Minor(i, j) : Minor(i, j).Negative<T>();
		}

		/// <summary>
		/// Gets the cofactor matrix, which is the matrix of minors where every index
		/// 
		/// </summary>
		/// <returns></returns>
		public Matrix<T> CofactorMatrix()
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);
			}

			var indices = Indices;

			for (var i = 0; i < Height; i++)
			{
				for (var j = 0; j < Width; j++)
				{
					indices[i, j] = Cofactor(i, j);
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Gets the adjugate matrix, which is the transpose of the cofactor matrix
		/// </summary>
		/// <returns></returns>
		public Matrix<T> Adjugate()
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);
			}

			return CofactorMatrix().Transpose();
		}

		public Vector<T> Diagonal()
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Diagonal);
			}

			var diag = new Vector<T>(Width);

			for (var i = 0; i < Width; i++)
			{
				diag[i] = this[i, i];
			}

			return diag;
		}

		public T Trace()
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Trace);
			}

			var result = new T();

			for (var i = 0; i < Width; i++)
			{
				result = result.Add(this[i, i]);
			}

			return result;
		}

		#endregion

		/**
		 * Find out if a matrix is square, symmetric or anti-symmetric,
		 * and if it can be added or multiplied to another matrix.
		 */
		#region Tests

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
		/// Find out whether the two matrices can be added
		/// </summary>
		/// <param name="right"></param>
		/// <returns></returns>
		protected bool Addable(Matrix<T> right)
		{
			return Width == right.Width && Height == right.Height;
		}

		/// <summary>
		/// Find out whether the two matrices can be multiplied
		/// </summary>
		/// <param name="right"></param>
		/// <returns></returns>
		protected bool Multipliable(Matrix<T> right)
		{
			return Width == right.Height;
		}

		#endregion

		/**
		 * Indexing of the matrix. You can index by vectors, by 2D-indices, or a 1D-indexer.
		 */
		#region Indexing

		/// <summary>
		/// Get the index at position index, based on whether the VectorType type is set to column or row.
		/// In case of Column, it goes up to down, left to right, and vice versa in case of Row.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
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
		public T this[int i, int j]
		{
			get => _indices[i, j];
			set => _indices[i, j] = value;
		}

		/// <summary>
		/// Get or set a vector at index n, whether it's a column or vector depends on
		/// the given VectorType type
		/// </summary>
		/// <param name="n"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public Vector<T> this[int n, VectorType type]
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
						{
							throw new IndexOutOfRangeException($"Index was {n}, max is {Width - 1}");
						}

						if (value.Dimension != Height)
						{
							throw new ArgumentException("Vector does not have the correct size" +
							                            $", should be: {Height}");
						}

						j = n;

						for (i = 0; i < value.Dimension; i++)
						{
							_indices[i, j] = value[i];
						}

						break;
					case VectorType.Row:
						i = n;

						for (j = 0; j < value.Dimension; j++)
						{
							_indices[i, j] = value[j];
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
		public Vector<T>[] this[VectorType type]
		{
			get
			{
				Vector<T>[] result;

				switch (type)
				{
					case VectorType.Column:
						result = new Vector<T>[Width];

						for (var j = 0; j < result.Length; j++)
						{
							result[j] = new Vector<T>(Height);
						}

						for (var i = 0; i < Height; i++)
						{
							for (var j = 0; j < Width; j++)
							{
								result[j][i] = Indices[i, j];
							}
						}

						return result;
					case VectorType.Row:
						result = new Vector<T>[Height];

						for (var i = 0; i < result.Length; i++)
						{
							result[i] = new Vector<T>(Width);
						}

						for (var i = 0; i < Height; i++)
						{
							for (var j = 0; j < Width; j++)
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
						Indices = new T[value[0].Dimension, value.Length];

						for (var j = 0; j < value.Length; j++)
						{
							for (var i = 0; i < value[0].Dimension; i++)
							{
								Indices[i, j] = value[j][i];
							}
						}

						break;
					case VectorType.Row:
						Indices = new T[value.Length, value[0].Dimension];

						for (var i = 0; i < value.Length; i++)
						{
							for (var j = 0; j < value[0].Dimension; j++)
							{
								Indices[i, j] = value[i][j];
							}
						}

						break;
				}
			}
		}

		#endregion

		/**
		 * When you need to convert a Matrix to a string. Can be formatted in the style of a 2D array
		 * or as a table with or without determinant signs around it.
		 */
		#region String Conversions

		/// <summary>
		/// Format the matrix as a string, written the same way you'd write a 2d array
		/// (Like { { 0, 1}, {2, 3} })
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(ConversionSettings.DefaultStringDelimiter);
		}

		public string ToString(char delimiter)
		{
			var result = delimiter + " ";

			foreach (var vector in this[VectorType.Row])
			{
				result += vector.ToString(delimiter) + ", ";
			}

			result = result.Remove(result.Length - 2, 1);

			result += "" + delimiter.MatchingDelimiter();

			return result;
		}

		/// <summary>
		/// Format the matrix as a table
		/// </summary>
		/// <param name="precision"></param>
		/// <param name="spacing"></param>
		/// <param name="wholeNumbers"></param>
		/// <returns></returns>
		public string ToTable(int precision, int spacing = 1, bool wholeNumbers = false)
		{
			var result = "";

			var padding = this[VectorType.Row].Select(v => v.Padding(precision)).Max();

			foreach (var vector in this[VectorType.Row])
			{
				result += vector.ToTable(precision, VectorType.Row, padding, spacing, wholeNumbers) + "\n";
			}

			result = result.Remove(result.Length - 1);

			return result;
		}

		/// <summary>
		/// Convert the matrix to a string, and in doing so add bars to the left and right of the table,
		/// like a determinant is being taken.
		/// </summary>
		/// <param name="precision"></param>
		/// <param name="spacing"></param>
		/// <param name="addResult">Whether to add the value of the determinant</param>
		/// <returns></returns>
		public string ToDeterminant(int precision, bool addResult = false, int spacing = 1)
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Determinant);
			}

			var result = "";

			var middle = (int)System.Math.Floor((double)Height / 2);
			var i = 0;
			var padding = this[VectorType.Row].Select(v => v.Padding(precision)).Max();

			foreach (var vector in this[VectorType.Row])
			{
				result += "| " + vector.ToTable(precision, VectorType.Row, padding, spacing) +
				          (i == middle && addResult ? $" | = {Determinant()}\n" : " |\n");
				i++;
			}

			result = result.Remove(result.Length - 1);

			return result;
		}

		#endregion

		/**
		 * Equality tests
		 */
		#region Equality Methods

		protected bool Equals(RealMatrix other)
		{
			return Equals(Indices, other.Indices);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((RealMatrix)obj);
		}

		public override int GetHashCode()
		{
			return Indices?.GetHashCode() ?? 0;
		}

		#endregion

		/**
		 * All operators. Matrices support addition, multiplication, powers, and negation.
		 */
		#region Operators

		/// <summary>
		/// Equality comparison for matrices
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(Matrix<T> left, Matrix<T> right)
		{
			// If the property width is null, the matrix must also be null
			if (left?.Width == null && right?.Width == null)
			{
				return true;
			}

			if (left?.Width == null || right?.Width == null)
			{
				return false;
			}

			if (!left.Addable(right))
			{
				return false;
			}

			for (var k = 0; k < left.Height * left.Width; k++)
			{
				if (left.GetIndices().ToList()[k] != right.GetIndices().ToList()[k])
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Inequality for matrices
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(Matrix<T> left, Matrix<T> right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Add two matrices together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
		{
			if (left.Addable(right))
			{
				var indices = new T[left.Width, left.Height];

				for (var i = 0; i < left.Width; i++)
				{
					for (var j = 0; j < left.Height; j++)
					{
						indices[i, j] = left.Indices[i, j].Add(right.Indices[i, j]);
					}
				}

				return new Matrix<T>(indices);
			}
			throw new IncompatibleOperationException(MatrixOperationType.Addition);
		}

		/// <summary>
		/// Returns the negative of this matrix
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Matrix<T> operator -(Matrix<T> m)
		{
			var indices = new T[m.Width, m.Height];

			for (var i = 0; i < m.Width; i++)
			{
				for (var j = 0; j < m.Height; j++)
				{
					indices[i, j] = m.Indices[i, j].Negative<T>();
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Subtract the right matrix from the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
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
		public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
		{
			if (!left.Multipliable(right))
			{
				throw new IncompatibleOperationException(MatrixOperationType.Multiplication);
			}

			var indices = new T[left.Height, right.Width];

			for (var i = 0; i < left.Height; i++)
			{
				for (var j = 0; j < right.Width; j++)
				{
					indices[i, j] = left[i, VectorType.Row] * right[j, VectorType.Column];
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Power of a matrix (returns unit for right = 0, and Inverse(left)^(-right) for right &lt; 0
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix<T> operator ^(Matrix<T> left, int right)
		{
			if (!left.IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Multiplication);
			}

			if (right == 0)
			{
				return new Matrix<T>(left.Width);
			}

			if (right < 0)
			{
				return left.Inverse() ^ -right;
			}

			var m = left;

			for (var i = 1; i < right; i++)
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
		public static Matrix<T> operator *(Matrix<T> left, T right)
		{
			var indices = left.Indices;

			for (var i = 0; i < left.Height; i++)
			{
				for (var j = 0; j < left.Width; j++)
				{
					indices[i, j] = indices[i, j].Multiply(right);
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix<T> operator *(T left, Matrix<T> right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division of a matrix and a real number
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix<T> operator /(Matrix<T> left, T right)
		{
			if (right is IInvertible i)
			{
				return left * (T) i.Inverse<IInvertible>();
			}

			throw new IncompatibleOperationException(MatrixOperationType.Division);
		}

		#endregion

		/**
		 * All the implementations of the IEnumerable interface. Also contains a specific
		 * method to get the list of rows or columns
		 */
		#region Enumerables and Enumerators

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<double>)this).GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			var position = 0;

			while (position < Width * Height)
			{
				yield return this[position];
				position++;
			}
		}

		IEnumerator<Vector<T>> IEnumerable<Vector<T>>.GetEnumerator()
		{
			var position = 0;

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
		public IEnumerable<T> GetIndices()
		{
			return this;
		}

		/// <summary>
		/// Get the enumerable that loops over the vectors (rows or columns based on the given VectorType)
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IEnumerable<Vector<T>> GetVectors(VectorType type)
		{
			Type = type;

			return this;
		}

		/// <summary>
		/// Get the enumerable that loops over the rows
		/// </summary>
		/// <returns></returns>
		public List<Vector<T>> GetRows()
		{
			return GetVectors(VectorType.Row).ToList();
		}

		/// <summary>
		/// Get the enumerable that loops over the columns
		/// </summary>
		/// <returns></returns>
		public List<Vector<T>> GetColumns()
		{
			return GetVectors(VectorType.Column).ToList();
		}

		#endregion

		/**
		 * This section allows conversion to commonly used subtypes, like real and complex matrices
		 */
		#region SubTypes

		public RealMatrix ToRealMatrix()
		{
			if (_indices is Real[,] indices)
			{
				return new RealMatrix(indices);
			}

			throw new InvalidOperationException("Can't convert this matrix to a real matrix");
		}

		public ComplexMatrix ToComplexMatrix()
		{
			if (_indices is Complex[,] complexIndices)
			{
				return new ComplexMatrix(complexIndices);
			}

			throw new InvalidOperationException("Can't convert this matrix to a complex matrix");
		}

		#endregion

		#region Overrides

		internal override T1 Add<T1>(T1 other)
		{
			if (other is Matrix<T> m)
			{
				return (T1) (MonoidMember) (this + m);
			}

			throw new IncorrectSetException(GetType(), "added", typeof(T1));
		}

		public override T1 Negative<T1>()
		{
			return (T1)(INegatable)(-this);
		}

		internal override T1 Multiply<T1>(T1 other)
		{
			if (other is Matrix<T> m)
			{
				return (T1)(GroupMember)(this * m);
			}

			throw new IncorrectSetException(GetType(), "multiplied", typeof(T1));
		}

		public override T1 Inverse<T1>()
		{
			return (T1)(IInvertible)Inverse();
		}

		public override T1 Null<T1>()
		{
			if (typeof(T1) == typeof(T))
			{
				return (T1)(MonoidMember)new Matrix<T>(Height, Width);
			}

			throw new IncorrectSetException(this, "null", typeof(T1));
		}

		public override T1 Unit<T1>()
		{
			if (!IsSquare())
			{
				throw new IncompatibleOperationException(MatrixOperationType.Unit);
			}

			if (typeof(T1) == typeof(T))
			{
				return (T1)
					(GroupMember)
					new Real(1);
			}

			throw new IncorrectSetException(this, "unit", typeof(T1));
		}

		public override bool IsNull()
		{
			return GetRows().All(i => i.IsNull());
		}

		public override bool IsUnit()
		{
			return (this - new Matrix<T>(Diagonal())).IsNull() && Diagonal().All(d => d.IsUnit());
		}

		[Obsolete]
		public override double ToDouble()
		{
			// Because there's no sensible other thing to do, should I maybe delete this method from GroupMember?
			return Determinant().ToDouble();
		}

		public override bool Equals<T1>(T1 other)
		{
			if (other is Matrix<T> r)
			{
				return this == r;
			}

			return false;
		}

		#endregion
	}
}
