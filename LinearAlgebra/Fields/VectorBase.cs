using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Main;

namespace LinearAlgebra.Fields
{
	public class VectorBase<T> : IEnumerable<T> where T : FieldMember<T>, new()
	{
		private FieldMember<T>[] _indices;

		public T[] Indices
		{
			get => _indices.Select(t => new T()).ToArray();
			set
			{
				_indices = new FieldMember<T>[value.Length];

				for (int i = 0; i < value.Length; i++)
				{
					_indices[i] = new T{Value = value[i]};
				}

				Dimension = value.Length;
			}
		}

		public int Dimension { get; protected set; }

		/// <summary>
		/// Create a vector with a double array
		/// </summary>
		/// <param name="indices"></param>
		public VectorBase(T[] indices)
		{
			Indices = indices;
		}

		/// <summary>
		/// Create a vector from another vector
		/// </summary>
		/// <param name="v"></param>
		public VectorBase(VectorBase<T> v)
		{
			Indices = new T[v.Indices.Length];

			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = v.Indices[i];
			}
		}

		/// <summary>Creates the (n+1)ᵗʰ unit vector of the given dimension,
		/// a.k.a. the 1 is at position n</summary>

		public VectorBase(int dimension, int n)
		{
			if (n >= dimension)
				throw new ArgumentException("The index which contains the 1" +
											"must not be greater than the dimension.");

			Indices = new T[dimension];

			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = new T();
			}

			Indices[n] = new T().Unit().Value;
		}

		///<summary>Creates a null vector of given dimension</summary>
		public VectorBase(int dimension)
		{
			Indices = new T[dimension];

			for (int i = 0; i < Dimension; i++)
			{
				Indices[i] = new T();
			}
		}

		/// <summary>
		/// Whether this is a null vector
		/// </summary>
		/// <returns></returns>
		public bool IsNull()
		{
			return this.All(d => d.IsNull);
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
			return Math.Sqrt((double) (this * this));
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
			if (maxPrecision < 1)
				throw new ArgumentException("Precision must be at least one", nameof(maxPrecision));

			if (new T() is INumerical n) return ToNumberTable(maxPrecision, type, padding);

			List<string> newStrings = this.Select(t => t.ToString()).ToList();

			if (!newStrings.All(s => s.Length < maxPrecision))
			{
				newStrings = newStrings.Select(s => s.Substring(0, maxPrecision)).ToList();
			}

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
						result += str.PadLeft(padding) + " ";
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
		/// <returns></returns>
		private string ToNumberTable(int maxPrecision, VectorType type, int padding)
		{
			string result = "";

			INumerical[] indices = new INumerical[Dimension];

			for (int i = 0; i < Dimension; i++)
			{
				indices[i] = (INumerical) Indices[i];
			}

			if (indices.Select(d => d.Round().Log10().ToString().Length)
				.Any(s => s > maxPrecision))
				maxPrecision += 4;

			if (padding == -1)
				padding = Padding(maxPrecision);

			switch (type)
			{
				case VectorType.Column:
					foreach (var d in this)
					{
						result += ((INumerical)d).ToString("g" + maxPrecision).PadLeft(padding) + "\n";
					}
					break;
				case VectorType.Row:
					foreach (var d in this)
					{
						result += ((INumerical) d).ToString("g" + maxPrecision).PadLeft(padding) + " ";
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
			int wanted = this.Select(d => ((INumerical) d).ToString("g").Length).Max();

			return wanted > maxPrecision ? maxPrecision : wanted;
		}

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

		/// <summary>
		/// Add the two vectors together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static VectorBase<T> operator +(VectorBase<T> left, VectorBase<T> right)
		{
			if (!Comparable(left, right))
				throw new IncompatibleOperationException(IncompatibleVectorOperationType.Addition);

			T[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = (indices[i] + right[i]).Value;
			}

			return new VectorBase<T>(indices);
		}

		/// <summary>
		/// The negative of this vector
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static VectorBase<T> operator -(VectorBase<T> v)
		{
			T[] indices = new T[v.Dimension];

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = (-v[i]).Value;
			}

			return new VectorBase<T>(indices);
		}

		/// <summary>
		/// Return the left minus the right vector
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static VectorBase<T> operator -(VectorBase<T> left, VectorBase<T> right)
		{
			return left + -right;
		}

		/// <summary>
		/// Inner product of two vectors
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static T operator *(VectorBase<T> left, VectorBase<T> right)
		{
			if (!Comparable(left, right))
				throw new IncompatibleOperationException(IncompatibleVectorOperationType.Inner);

			T result = new T();

			for (int i = 0; i < left.Dimension; i++)
			{
				result = (result + left[i] * right[i]).Value;
			}

			return result;
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the right
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></VectorBase<T>>
		public static VectorBase<T> operator *(VectorBase<T> left, T right)
		{
			T[] indices = left.Indices;

			for (int i = 0; i < indices.Length; i++)
			{
				indices[i] = (indices[i].Inner(right)).Value;
			}

			return new VectorBase<T>(indices);
		}

		/// <summary>
		/// Scalar multiplication with the scalar on the left
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static VectorBase<T> operator *(T left, VectorBase<T> right)
		{
			return right * left;
		}

		/// <summary>
		/// Scalar division
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static VectorBase<T> operator /(VectorBase<T> left, T right)
		{
			return left * right.MultiplicativeInverse().Value;
		}

		protected static bool Comparable(VectorBase<T> left, VectorBase<T> right)
		{
			return left.Dimension == right.Dimension;
		}

		public static explicit operator T[] (VectorBase<T> v)
		{
			return v.Indices.ToArray();
		}

		public static explicit operator List<T>(VectorBase<T> v)
		{
			return new List<T>(v.Indices);
		}

		public static explicit operator VectorBase<T>(List<T> l)
		{
			return new VectorBase<T>(l.ToArray());
		}

		public static explicit operator VectorBase<T>(T[] l)
		{
			return new VectorBase<T>(l);
		}

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
	}
}
