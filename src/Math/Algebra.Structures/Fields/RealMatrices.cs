using System;
using System.Linq;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.LinearAlgebra;
using Math.Settings;

// ReSharper disable CoVariantArrayConversion

namespace Math.Algebra.Structures.Fields
{
	/// <summary>
	/// The set of real matrices is not technically a field, as multiplication is not commutative,
	/// but since commutativity is in no way implemented in this program, I decided to make it a field.
	/// </summary>
	public class RealMatrices : Field<Matrix<Real>>
	{
		public Integer Size { get; }

		/// <summary>
		/// Constructor for the set of realmatrices with given height and width.
		/// </summary>
		/// <param name="size"></param>
		public RealMatrices(Integer size)
		{
			Size = size;
		}

		/// <summary>
		/// Can be used to create matrix, with as parameters a set of vectors, a
		/// nested array of reals or doubles, with an optional last parameter of
		/// VectorType
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override Matrix<Real> Create(params object[] value)
		{
			VectorType type = value.Length > Size
				? value[Size] as VectorType? ?? ConversionSettings.DefaultVectorType
				: ConversionSettings.DefaultVectorType;

			if (value.Length != Size)
			{
				throw new ArgumentException("Amount of vectors must be equal to the size of the matrices.");
			}

			switch (value) {
				case Vector<Real>[] vectors:
					if (vectors.Any(v => v.Dimension != Size))
					{
						throw new ArgumentException("Length of the vectors must be equal to the size of the matrices.");
					}

					if (LinearMath.LinearlyDependent(vectors))
					{
						throw new ArgumentException("The vectors must not be linearly dependent, or this matrix won't be invertible.");
					}

					return new Matrix<Real>(vectors, type);
				case Real[][] indices:
					if (indices.Any(v => v.Length != Size))
					{
						throw new ArgumentException("Length of the vectors must be equal to the size of the matrices.");
					}

					if (LinearMath.LinearlyDependent(indices.Select(v => new Vector<Real>(v)).ToArray()))
					{
						throw new ArgumentException("The vectors must not be linearly dependent, or this matrix won't be invertible.");
					}

					return new Matrix<Real>(indices.Select(v => new Vector<Real>(v)).ToArray(), type);
				case double[][] doubles:
					if (doubles.Any(v => v.Length != Size))
					{
						throw new ArgumentException("Length of the vectors must be equal to the size of the matrices.");
					}

					if (LinearMath.LinearlyDependent(doubles.Select(v => new RealVector(v)).ToArray()))
					{
						throw new ArgumentException("The vectors must not be linearly dependent, or this matrix won't be invertible.");
					}

					return new Matrix<Real>(doubles.Select(v => new RealVector(v)).ToArray(), type);
			}

			throw new ArgumentException("Given argument must be either a nested array of real numbers, doubles, or an array of vectors.");
		}

		public override Matrix<Real> Null()
		{
			return new Matrix<Real>(Size, Size);
		}

		public override Matrix<Real> Unit()
		{
			return new Matrix<Real>(Size);
		}

		public override bool Contains<T>(T element)
		{
			switch (element)
			{
				case Matrix<Real> r:
					return r.IsSquare() && !r.Determinant().Equals(0);
			}

			return false;
		}
	}
}
