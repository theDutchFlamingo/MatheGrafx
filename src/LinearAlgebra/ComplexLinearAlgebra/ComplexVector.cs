using System.Linq;
using LinearAlgebra.Algebra.Fields;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Fields;
using LinearAlgebra.Fields.Members;

namespace LinearAlgebra.ComplexLinearAlgebra
{
	/// <summary>
	/// Specific implementation of VectorBase&lt;Complex&gt; because ComplexVectors are still pretty common.
	/// Also adds some specific complex methods.
	/// </summary>
	public class ComplexVector : Vector<Complex>
	{
		#region Constructors

		public ComplexVector(Complex[] indices) : base(indices)
		{

		}

		/// <summary>
		/// Create a vector with a double array
		/// </summary>
		/// <param name="indices"></param>
		public ComplexVector(double[] indices) : base(indices.Length)
		{
			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = indices[i];
			}
		}

		/// <summary>
		/// Create a vector from another vector
		/// </summary>
		/// <param name="v"></param>
		public ComplexVector(ComplexVector v) : base(v)
		{

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

		#endregion

		#region Complex-specific Methods

		/// <summary>
		/// Whether all elements of this vector are real
		/// </summary>
		/// <returns></returns>
		public bool IsReal()
		{
			return this.All(c => c.IsReal());
		}

		/// <summary>
		/// Whether all elements of this vector are imaginary
		/// </summary>
		/// <returns></returns>
		public bool IsImaginary()
		{
			return this.All(c => c.IsImaginary());
		}

		#endregion

		#region Operators

		/// <summary>
		/// Add the two vectors together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static ComplexVector operator +(ComplexVector left, ComplexVector right)
		{
			if (!left.Comparable(right))
				throw new
					IncompatibleOperationException(VectorOperationType.Addition);

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
			if (!left.Comparable(right))
				throw new
					IncompatibleOperationException(VectorOperationType.Inner);

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

		#endregion
	}
}
