using System;

namespace Math.Exceptions.Operations
{
	/**
	 * When two matrices are multiplied or added, or a determinant or an inverse is taken,
	 * and that operation isn't allowed
	 */
	public class IncompatibleOperationException : Exception
	{
		public MatrixOperationType MatrixType { get; }
		public VectorOperationType VectorType { get; }

		public IncompatibleOperationException(MatrixOperationType type) : base(GetMessage(type))
		{
			MatrixType = type;
		}

		public IncompatibleOperationException(MatrixOperationType type, string message)
			: base(message)
		{
			MatrixType = type;
		}

		public IncompatibleOperationException(VectorOperationType type) : base(GetMessage(type))
		{
			VectorType = type;
		}

		public IncompatibleOperationException(VectorOperationType type, string message)
			: base(message)
		{
			VectorType = type;
		}

		public static string GetMessage(MatrixOperationType matrixType)
		{
			switch (matrixType)
			{
				case MatrixOperationType.Addition:
					return "The two matrices could not be added because their dimensions were unequal.";
				case MatrixOperationType.Determinant:
					return "The determinant of a non-square matrix does not exist.";
				case MatrixOperationType.Division:
					return "A matrix cannot be divided by the zero element of its field.";
				case MatrixOperationType.Eigenvalue:
					return "Only square matrices have eigenvalues.";
				case MatrixOperationType.Eigenvector:
					return "Only square matrices have eigenvectors.";
				case MatrixOperationType.Inverse:
					return "Only square matrices with non-zero determinant have an inverse.";
				case MatrixOperationType.Multiplication:
					return "The two matrices could not be multiplied, the width of the first" +
					       "should be equal to the height of the second.";
				case MatrixOperationType.Trace:
					return "The trace of a matrix exists only if the matrix is square.";
				case MatrixOperationType.Unit:
					return "Only square matrices can be a unit matrix.";
			}

			throw new ArgumentException("The given MatrixOperationType was " +
			                            "not a valid object.");
		}

		public static string GetMessage(VectorOperationType vectorType)
		{
			switch (vectorType)
			{
				case VectorOperationType.Addition:
					return "The two vectors could not be added because their dimensions were unequal.";
				case VectorOperationType.Dimension:
					return "Two vectors with unequal dimension cannot be compared.";
				case VectorOperationType.Division:
					return "Division by a scalar is only supported if the scalar is invertible.";
				case VectorOperationType.Inner:
					return "The inner product cannot be taken of vectors with unequal dimensions.";
				case VectorOperationType.MatrixVector:
					return "The matrix-vector product couldn't be computed.";
				case VectorOperationType.Outer:
					return "";
			}

			throw new ArgumentException("The given VectorOperationType was " +
			                            "not a valid object.");
		}
	}
}
