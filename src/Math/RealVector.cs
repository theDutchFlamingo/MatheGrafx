using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Exceptions;
using Math.Exceptions.Operations;

namespace Math
{
	/// <summary>
	/// Specific implementation of VectorBase&lt;Real&gt; because real vectors are by far the most common ones
	/// </summary>
	public class RealVector : Vector<Real>
    {
	    #region Constructors

	    /// <summary>
	    /// Create a vector with a real number array
	    /// </summary>
	    /// <param name="indices"></param>
	    public RealVector(Real[] indices) : base(indices)
	    {
		    
	    }

	    /// <summary>
	    /// Create a vector with a double array
	    /// </summary>
	    /// <param name="indices"></param>
	    public RealVector(double[] indices) : base(indices.Length)
	    {
		    for (int i = 0; i < Dimension; i++)
		    {
			    Indices[i] = indices[i];
		    }
	    }

	    /// <summary>
	    /// Create a vector with an integer array
	    /// </summary>
	    /// <param name="indices"></param>
	    public RealVector(int[] indices) : base(indices.Length)
	    {
		    for (int i = 0; i < Dimension; i++)
		    {
			    Indices[i] = indices[i];
		    }
	    }

		/// <summary>
		/// Create a vector from a VectorBase&lt;Real&gt; object.
		/// </summary>
		/// <param name="vector"></param>
	    public RealVector(Vector<Real> vector) : base(vector)
	    {

	    }

	    /// <summary>
	    /// Create a vector from another vector
	    /// </summary>
	    /// <param name="v"></param>
	    public RealVector(RealVector v) : base(v)
	    {
		    
	    }

	    /// <summary>Creates the (n+1)ᵗʰ unit vector of the given dimension,
	    /// a.k.a. the 1 is at position n</summary>

	    public RealVector(int dimension, int n) : base(dimension, n)
	    {
			
	    }

	    ///<summary>Creates a null vector of given dimension</summary>
	    public RealVector(int dimension) : base(dimension)
	    {

	    }

	    #endregion

		#region Operators

		/// <summary>
		/// Add the two vectors together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealVector operator +(RealVector left, RealVector right)
		{
			if (!left.Comparable(right))
				throw new
					IncompatibleOperationException(VectorOperationType.Addition);

			Real[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] += right[i];
			}

			return new RealVector(indices);
		}

		/// <summary>
		/// The negative of this vector
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static RealVector operator -(RealVector v)
		{
			Real[] indices = new Real[v.Dimension];

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = -v[i];
			}

			return new RealVector(indices);
		}

		/// <summary>
		/// Return the left minus the right vector
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealVector operator -(RealVector left, RealVector right)
		{
			return left + -right;
		}

		/// <summary>
		/// Inner product of two vectors
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static double operator *(RealVector left, RealVector right)
		{
			if (!left.Comparable(right))
				throw new
					IncompatibleOperationException(VectorOperationType.Inner);

			double result = 0;

			for (int i = 0; i < left.Dimension; i++)
			{
				result += left[i] * right[i];
			}

			return result;
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealVector operator *(RealVector left, double right)
		{
			Real[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] *= right;
			}

			return new RealVector(indices);
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealVector operator *(double left, RealVector right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static RealVector operator /(RealVector left, double right)
		{
			return left * (1 / right);
		}

		#endregion
	}
}
