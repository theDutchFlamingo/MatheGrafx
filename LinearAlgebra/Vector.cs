using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LinearAlgebra.ComplexLinearAlgebra;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Fields;
using LinearAlgebra.Main;

namespace LinearAlgebra
{
    public class Vector : VectorBase<Real>
    {
	    /// <summary>
		/// Create a vector with a double array
		/// </summary>
		/// <param name="indices"></param>
        public Vector(double[] indices) : base(indices.Length)
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
		public Vector(Vector v) : base(v)
	    {
		    
	    }

		/// <summary>Creates the (n+1)ᵗʰ unit vector of the given dimension,
		/// a.k.a. the 1 is at position n</summary>

		public Vector(int dimension, int n) : base(dimension, n)
	    {
			
	    }

		///<summary>Creates a null vector of given dimension</summary>
		public Vector(int dimension) : base(dimension)
	    {

	    }
	}
}
