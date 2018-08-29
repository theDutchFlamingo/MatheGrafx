using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Math.Main;
using Math.Algebra.Rings.Members;
using Math.Exceptions;
using Math.Fields;
using Math.Groups;

namespace Math.Algebra.Fields
{
	public class Vector<T> : IEnumerable<T> where T : RingMember, IInvertible, new()
	{
		#region Fields & Properties

		private T[] _indices;

		public T[] Indices
		{
			get => _indices;
			set
			{
				_indices = new T[value.Length];

				for (int i = 0; i < value.Length; i++)
				{
					_indices[i] = value[i];
				}

				Dimension = value.Length;
			}
		}

		public int Dimension { get; protected set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Create a vector with a double array
		/// </summary>
		/// <param name="indices"></param>
		public Vector(T[] indices)
		{
			Indices = indices;
		}

		/// <summary>
		/// Create a vector from another vector
		/// </summary>
		/// <param name="v"></param>
		public Vector(Vector<T> v)
		{
			Indices = new T[v.Indices.Length];

			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = v.Indices[i];
			}
		}

		/// <summary>Creates the (n+1)ᵗʰ unit vector of the given dimension,
		/// a.k.a. the 1 is at position n</summary>

		public Vector(int dimension, int n)
		{
			if (n >= dimension)
				throw new ArgumentException("The index which contains the 1" +
				                            "must not be greater than the dimension.");

			Indices = new T[dimension];

			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = new T();
			}

			Indices[n] = new T().Unit<T>();
		}

		///<summary>Creates a null vector of given dimension</summary>
		public Vector(int dimension)
		{
			Indices = new T[dimension];

			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = new T();
			}
		}

		#endregion

		/**
		 * Whether a vector is null, unit, or of the same dimension as another
		 */
		#region Tests

		/// <summary>
		/// Whether this is a null vector
		/// </summary>
		/// <returns></returns>
		public bool IsNull()
		{
			return this.All(d => d.IsNull());
		}

		/// <summary>
		/// Whether this vector has length 1
		/// </summary>
		/// <returns></returns>
		public bool IsUnit()
		{
			return Norm().CloseTo(1);
		}

		protected bool Comparable(Vector<T> right)
		{
			return Dimension == right.Dimension;
		}

		#endregion

		#region Norm

		/// <summary>
		/// The length of this vector
		/// </summary>
		/// <returns></returns>
		public double Norm()
		{
			return System.Math.Sqrt((double) (this * this));
		}

		#endregion

		#region String Conversion

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

		public string ToTable(int maxPrecision, VectorType type, int padding = -1, int spacing = 1, bool wholeNumbers = false)
		{
			if (maxPrecision < 1)
				throw new ArgumentException("Precision must be at least one", nameof(maxPrecision));

			if (new T() is INumerical) return ToNumberTable(maxPrecision, type, padding, spacing, wholeNumbers);

			List<string> newStrings = this.Select(t => t.ToString()).ToList();

			if (padding == -1)
				padding = Padding(maxPrecision);

			string result = "";

			switch (type)
			{
				case VectorType.Column:
					foreach (var str in newStrings)
					{
						result += str.PadLeft(padding) + "\n";
					}

					break;
				case VectorType.Row:
					foreach (var str in newStrings)
					{
						result += str.PadLeft(padding) + String.Concat(Enumerable.Repeat(" ", spacing));
					}
					break;
			}

			result = result.Remove(result.Length - 1);

			return result;
		}

		/// <summary>
		/// When T is a number type
		/// </summary>
		/// <param name="maxPrecision"></param>
		/// <param name="type"></param>
		/// <param name="padding"></param>
		/// <param name="spacing"></param>
		/// <returns></returns>
		private string ToNumberTable(int maxPrecision, VectorType type, int padding, int spacing, bool wholeNumbers)
		{
			string result = "";

			INumerical[] indices = new INumerical[Dimension];

			for (int i = 0; i < Dimension; i++)
			{
				indices[i] = (INumerical) Indices[i];
			}

			if (indices.Select(d => d.LongestValue().Log10().Round().ToString().Length)
				.Any(s => s > maxPrecision))
				maxPrecision += 4;

			if (padding == -1)
				padding = Padding(maxPrecision);

			switch (type)
			{
				case VectorType.Column:
					foreach (var d in this)
					{
						if (wholeNumbers)
						{
							result += ((INumerical)d).Round().ToString("g" + maxPrecision).PadLeft(padding) + "\n";
							continue;
						}

						result += ((INumerical)d).ToString("g" + maxPrecision).PadLeft(padding) + "\n";
					}
					break;
				case VectorType.Row:
					foreach (var d in this)
					{
						if (wholeNumbers)
						{
							result += ((INumerical) d).Round().ToString("g" + maxPrecision).PadLeft(padding) +
							          String.Concat(Enumerable.Repeat(" ", spacing));
							continue;
						}
						
						result += ((INumerical) d).ToString("g" + maxPrecision).PadLeft(padding) +
						          String.Concat(Enumerable.Repeat(" ", spacing));
					}
					break;
				default:
					throw new ArgumentException("The VectorType was not a valid object",
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
			int wanted;

			if (new T() is INumerical)
			{
				wanted = this.Select(d => ((INumerical) d).ToString("g").Length).Max();
				return wanted > maxPrecision ? maxPrecision : wanted;
			}
			
			return this.Select(d => d.ToString().Length).Max();
		}

		#endregion

		#region Indexing

		/// <summary>
		/// Gets the double at the given index i
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public T this[int i]
		{
			get => Indices[i];
			set => Indices[i] = value;
		}

		#endregion

		#region Operators

		/// <summary>
		/// Add the two vectors together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector<T> operator +(Vector<T> left, Vector<T> right)
		{
			if (!left.Comparable(right))
				throw new IncompatibleOperationException(VectorOperationType.Addition);

			T[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = indices[i].Add(right[i]);
			}

			return new Vector<T>(indices);
		}

		/// <summary>
		/// The negative of this vector
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static Vector<T> operator -(Vector<T> v)
		{
			T[] indices = new T[v.Dimension];

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = v[i].Negative<T>();
			}

			return new Vector<T>(indices);
		}

		/// <summary>
		/// Return the left minus the right vector
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector<T> operator -(Vector<T> left, Vector<T> right)
		{
			return left + -right;
		}

		/// <summary>
		/// Inner product of two vectors
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static T operator *(Vector<T> left, Vector<T> right)
		{
			if (!left.Comparable(right))
				throw new IncompatibleOperationException(VectorOperationType.Inner);

			T result = new T();

			for (int i = 0; i < left.Dimension; i++)
			{
				result = result.Add(left[i].Multiply(right[i]));
			}

			return result;
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>&lt;T&gt;</returns>
		public static Vector<T> operator *(Vector<T> left, T right)
		{
			T[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = indices[i].Multiply(right);
			}

			return new Vector<T>(indices);
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector<T> operator *(T left, Vector<T> right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector<T> operator /(Vector<T> left, T right)
		{
			return left * right.Inverse<T>();
		}

		#endregion

		#region Cast Operators

		public static explicit operator T[] (Vector<T> v)
		{
			return v.Indices.ToArray();
		}

		public static explicit operator List<T>(Vector<T> v)
		{
			return new List<T>(v.Indices);
		}

		public static explicit operator Vector<T>(List<T> l)
		{
			return new Vector<T>(l.ToArray());
		}

		public static explicit operator Vector<T>(T[] l)
		{
			return new Vector<T>(l);
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The IEnumerator object required for IEnumerables</returns>
		public IEnumerator<T> GetEnumerator()
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

		#endregion
	}
}
