using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LinearAlgebra.ComplexLinearAlgebra;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Main;

namespace LinearAlgebra
{
    public class Vector : IEnumerable<double>
    {
	    protected Complex[] ComplexIndices;

	    public double[] Indices
	    {
		    get
		    {
				double[] indices = new double[ComplexIndices.Length];

			    for (int i = 0; i < indices.Length; i++)
			    {
				    indices[i] = ComplexIndices[i].Real;
			    }

			    return indices;
		    }
		    set
		    {
				ComplexIndices = new Complex[value.Length];

			    for (int i = 0; i < value.Length; i++)
			    {
					ComplexIndices[i] = new Complex(value[i], 0);
			    }

			    Dimension = value.Length;
		    }
	    }

        public int Dimension { get; protected set; }

	    protected Vector(Complex[] indices)
	    {
		    ComplexIndices = indices;
		    Dimension = indices.Length;
	    }

		/// <summary>
		/// Create a vector with a double array
		/// </summary>
		/// <param name="indices"></param>
        public Vector(double[] indices)
        {
            Indices = indices;
        }

		/// <summary>
		/// Create a vector from another vector
		/// </summary>
		/// <param name="v"></param>
		public Vector(Vector v)
	    {
		    Indices = new double[v.Indices.Length];

		    for (int i = 0; i < Dimension; i++)
		    {
			    Indices[i] = v.Indices[i];
		    }
	    }

		/// <summary>Creates the (n+1)ᵗʰ unit vector of the given dimension,
		/// a.k.a. the 1 is at position n</summary>

		public Vector(int dimension, int n)
	    {
			if (n >= dimension) throw new ArgumentException("The index which contains the 1" +
			                                                "must not be greater than the dimension.");

			Indices = new double[dimension];
		    Indices[n] = 1;
	    }

		///<summary>Creates a null vector of given dimension</summary>
		public Vector(int dimension)
	    {
			Indices = new double[dimension];
	    }

		/// <summary>
		/// Whether this is a null vector
		/// </summary>
		/// <returns></returns>
	    public bool IsNull()
	    {
		    return this.All(d => d.CloseTo(0));
	    }

		/// <summary>
		/// Whether this vector has length 1
		/// </summary>
		/// <returns></returns>
	    public bool IsUnit()
	    {
		    return Norm().CloseTo(1);
	    }

		/// <summary>
		/// The length of this vector
		/// </summary>
		/// <returns></returns>
	    public double Norm()
	    {
		    return Math.Sqrt(this * this);
	    }

	    public override string ToString()
	    {
		    string result = "{";

		    foreach (var d in this)
		    {
			    result += d + ", ";
		    }

		    result = result.Remove(result.Length - 2);

			result += "}";

			return result;
	    }

	    public string ToTable(int maxPrecision, VectorType type, int padding = -1)
	    {
			if (maxPrecision < 1) throw new ArgumentException("Precision must be at least one",
				nameof(maxPrecision));

		    string result = "";

		    if (this.Select(d => Math.Log10(Math.Round(d)).ToString(CultureInfo.InvariantCulture).Length)
			    .Any(s => s > maxPrecision)) maxPrecision += 4;

			if (padding == -1) padding = Padding(maxPrecision);

		    switch (type)
		    {
				case VectorType.Column:
					foreach (var d in this)
					{
						result += d.ToString("g" + maxPrecision).PadLeft(padding) + "\n";
					}
					break;
				case VectorType.Row:
					foreach (var d in this)
					{
						result += d.ToString("g" + maxPrecision).PadLeft(padding) + " ";
					}
					break;
				default: throw new ArgumentException("The VectorType was not a valid object",
					nameof(type));
		    }

		    result = result.Remove(result.Length - 1);

		    return result;
	    }

		/// <summary>
		/// Return the amount of padding the strings need which are to be put into the table
		/// </summary>
		/// <param name="maxPrecision"></param>
		/// <returns></returns>
	    protected internal int Padding(int maxPrecision)
	    {
		    int wanted = this.Select(d => d.ToString("g").Length).Max();

			return wanted > maxPrecision ? maxPrecision : wanted;
	    }

	    /// <summary>
		/// Gets the double at the given index i
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
	    public double this[int i]
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
		public static Vector operator +(Vector left, Vector right)
	    {
			if (!Comparable(left, right)) throw new
				IncompatibleOperationException(IncompatibleVectorOperationType.Addition);

		    double[] indices = left.Indices;

		    for (int i = 0; i < indices.Length; i++)
		    {
			    indices[i] += right[i];
		    }

			return new Vector(indices);
	    }

		/// <summary>
		/// The negative of this vector
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
	    public static Vector operator -(Vector v)
	    {
			double[] indices = new double[v.Dimension];

		    for (int i = 0; i < indices.Length; i++)
		    {
			    indices[i] = -v[i];
		    }

			return new Vector(indices);
	    }

		/// <summary>
		/// Return the left minus the right vector
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
	    public static Vector operator -(Vector left, Vector right)
		{
			return left + -right;
		}

		/// <summary>
		/// Inner product of two vectors
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
	    public static double operator *(Vector left, Vector right)
	    {
		    if (!Comparable(left, right)) throw new
			    IncompatibleOperationException(IncompatibleVectorOperationType.Inner);

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
		public static Vector operator *(Vector left, double right)
	    {
		    double[] indices = left.Indices;

		    for (int i = 0; i < indices.Length; i++)
		    {
			    indices[i] *= right;
		    }

		    return new Vector(indices);
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector operator *(double left, Vector right)
		{
			return right * left;
		}

	    /// <summary>
	    /// Scalar division
	    /// </summary>
	    /// <param name="left"></param>
	    /// <param name="right"></param>
	    /// <returns></returns>
	    public static Vector operator /(Vector left, double right)
	    {
		    return left * (1 / right);
	    }

		protected static bool Comparable(Vector left, Vector right)
	    {
		    return left.Dimension == right.Dimension;
	    }

        public static implicit operator double[] (Vector v)
        {
            return v.Indices.ToArray();
        }

        public static implicit operator List<double> (Vector v)
        {
            return new List<double>(v.Indices);
        }

        public static implicit operator Vector (List<double> l)
        {
            return new Vector(l.ToArray());
        }

        public static implicit operator Vector (double[] l)
        {
            return new Vector(l);
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <returns>The IEnumerator object required for IEnumerables</returns>
	    public IEnumerator<double> GetEnumerator()
	    {
		    int position = 0;

		    while (position < Dimension)
		    {
			    yield return this[position];
			    position++;
		    }
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return Indices.GetEnumerator();
	    }
	}
}
