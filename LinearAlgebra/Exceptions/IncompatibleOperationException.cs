using System;
using System.Collections.Generic;

namespace LinearAlgebra.Exceptions
{
	/**
	 * When two matrices are multiplied or added, or a determinant or an inverse is taken,
	 * and that operation isn't allowed
	 */
	public class IncompatibleOperationException : Exception
	{
		public IncompatibleMatrixOperationType MatrixType { get; }
		public IncompatibleVectorOperationType VectorType { get; }

		public IncompatibleOperationException(IncompatibleMatrixOperationType type) : base(GetMessage(type))
		{
			MatrixType = type;
		}

		public IncompatibleOperationException(IncompatibleMatrixOperationType type, string message)
			: base(message)
		{
			MatrixType = type;
		}

		public IncompatibleOperationException(IncompatibleVectorOperationType type) : base(GetMessage(type))
		{
			VectorType = type;
		}

		public IncompatibleOperationException(IncompatibleVectorOperationType type, string message)
			: base(message)
		{
			VectorType = type;
		}

		public static string GetMessage(IncompatibleMatrixOperationType matrixType)
		{
			

			switch (matrixType)
			{
				case IncompatibleMatrixOperationType.Addition:
					return "The two matrices could not be added because their dimensions were unequal.";
				case IncompatibleMatrixOperationType.Determinant:
					return "The determinant of a non-square matrix does not exist.";
				case IncompatibleMatrixOperationType.Eigenvalue:
					return "Only square matrices have eigenvalues.";
				case IncompatibleMatrixOperationType.Eigenvector:
					return "Only square matrices have eigenvectors.";
				case IncompatibleMatrixOperationType.Inverse:
					return "Only square matrices have an inverse.";
				case IncompatibleMatrixOperationType.Multiplication:
					return "The two matrices could not be multiplied, the width of the first" +
					       "should be equal to the height of the second.";
			}

			throw new ArgumentException("This exception has neither a MatrixType" +
			                            "nor VectorType initialized.");
		}

		public static string GetMessage(IncompatibleVectorOperationType vectorType)
		{
			switch (vectorType)
			{
				case IncompatibleVectorOperationType.Addition:
					return "The two vectors could not be added because their dimensions were unequal.";
				case IncompatibleVectorOperationType.Dimension:
					return "Two vectors with unequal dimension cannot be compared.";
				case IncompatibleVectorOperationType.Inner:
					return "The inner product cannot be taken of vectors with unequal dimensions.";
				case IncompatibleVectorOperationType.MatrixVector:
					return "The matrix-vector product couldn't be computed.";
				case IncompatibleVectorOperationType.Outer:
					return "";
			}

			throw new ArgumentException("The given IncompatibleVectorOperationType was" +
			                            "not a valid object.");
		}
	}
}
