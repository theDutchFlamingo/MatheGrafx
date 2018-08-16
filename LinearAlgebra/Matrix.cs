using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LinearAlgebra.ComplexLinearAlgebra;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Fields;
using LinearAlgebra.Main;

namespace LinearAlgebra
{
	public class Matrix : MatrixBase<Real>
	{
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
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					Indices[i, j] = indices[i, j];
				}
			}
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
	}
}
