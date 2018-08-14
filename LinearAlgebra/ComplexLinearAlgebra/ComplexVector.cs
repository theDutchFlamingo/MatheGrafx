using System;
using System.Collections.Generic;
using System.Linq;
using LinearAlgebra.Exceptions;

namespace LinearAlgebra.ComplexLinearAlgebra
{
	public class ComplexVector : Vector, IEnumerable<Complex>
	{
		public new Complex[] Indices
		{
			get => ComplexIndices;
			set => ComplexIndices = value;
		}

		public ComplexVector(Complex[] indices) : base(indices)
		{

		}

		/// <summary>
		/// Create a vector with a double array
		/// </summary>
		/// <param name="indices"></param>
		public ComplexVector(double[] indices) : base(indices)
		{
			
		}

		/// <summary>
		/// Create a vector from another vector
		/// </summary>
		/// <param name="v"></param>
		public ComplexVector(ComplexVector v) : base((Vector) v)
		{
			Indices = new Complex[v.Indices.Length];

			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = v.Indices[i];
			}
		}

		/// <summary>Creates the (n+1)ᵗʰ unit vector of the given dimension,
		/// a.k.a. the 1 is at position n</summary>

		public ComplexVector(int dimension, int n) : base(dimension, n)
		{
			
		}

		///<summary>Creates a null vector of given dimension</summary>
		public ComplexVector(int dimension) : base(dimension)
		{
			
		}

		/// <summary>
		/// Gets the norm of this vector
		/// </summary>
		/// <returns></returns>
		public new double Norm()
		{
			return Math.Sqrt((this * this).Real);
		}

		/// <summary>
		/// Whether all elements of this vector are real
		/// </summary>
		/// <returns></returns>
		public bool IsReal()
		{
			return this.OfType<Complex>().All(c => c.IsReal());
		}

		/// <summary>
		/// Whether all elements of this vector are imaginary
		/// </summary>
		/// <returns></returns>
		public bool IsImaginary()
		{
			return this.OfType<Complex>().All(c => c.IsImaginary());
		}

		/// <summary>
		/// Gets the double at the given index i
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public new Complex this[int i]
		{
			get => Indices[i];
			set => Indices[i] = value;
		}

		/// <summary>
		/// Add the two vectors together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexVector operator +(ComplexVector left, ComplexVector right)
		{
			if (!Comparable(left, right))
				throw new
					IncompatibleOperationException(IncompatibleVectorOperationType.Addition);

			Complex[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] += right[i];
			}

			return new ComplexVector(indices);
		}

		/// <summary>
		/// The negative of this vector
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static ComplexVector operator -(ComplexVector v)
		{
			Complex[] indices = new Complex[v.Dimension];

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = -v[i];
			}

			return new ComplexVector(indices);
		}

		/// <summary>
		/// Return the left minus the right vector
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexVector operator -(ComplexVector left, ComplexVector right)
		{
			return left + -right;
		}

		/// <summary>
		/// Inner product of two vectors
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Complex operator *(ComplexVector left, ComplexVector right)
		{
			if (!Comparable(left, right))
				throw new
					IncompatibleOperationException(IncompatibleVectorOperationType.Inner);

			Complex result = 0;

			for (int i = 0; i < left.Dimension; i++)
			{
				result += left[i] * right[i].Conjugate();
			}

			return result;
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexVector operator *(ComplexVector left, double right)
		{
			Complex[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] *= right;
			}

			return new ComplexVector(indices);
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexVector operator *(double left, ComplexVector right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexVector operator /(ComplexVector left, double right)
		{
			return left * (1 / right);
		}

		IEnumerator<Complex> IEnumerable<Complex>.GetEnumerator()
		{
			return ((IEnumerable<Complex>)Indices).GetEnumerator();
		}
	}
}
