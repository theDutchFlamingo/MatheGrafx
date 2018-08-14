using System;
using LinearAlgebra.Exceptions;

namespace LinearAlgebra.Main
{
	public class RowOperation
	{
		public enum RowOperationType
		{
			Substitution, Switching, Scaling
		}

		/// <summary>
		/// For substitution and scaling, how much to scale the row vector by
		/// </summary>
		private double Scale { get; }
		/// <summary>
		/// For substitution and switching, the row to take the vector from
		/// </summary>
		private int From { get; }
		/// <summary>
		/// For subtitution, switching and scaling, the row vector to change
		/// </summary>
		private int Onto { get; }

		private RowOperationType Type { get; }

		public RowOperation(RowOperationType type, int from = -1, int onto = -1, double scale = 1)
		{
			Type = type;
			From = from;
			Onto = onto;
			Scale = scale;

			switch (type)
			{
				case RowOperationType.Scaling:
					if (scale.CloseTo(0)) throw new
						InvalidRowOperationException(InvalidRowOperationType.ScaleZero);
					break;
				case RowOperationType.Substitution:
					if (scale.CloseTo(0)) throw new
						InvalidRowOperationException(InvalidRowOperationType.ScaleZero);
					if (from == onto) throw new
						InvalidRowOperationException(InvalidRowOperationType.SameRow);
					if (from < 0 || onto < 0) throw new
						InvalidRowOperationException(InvalidRowOperationType.InvalidRow);
					break;
				case RowOperationType.Switching:
					if (from == onto)
						throw new
							InvalidRowOperationException(InvalidRowOperationType.SameRow);
					if (from < 0 || onto < 0)
						throw new
							InvalidRowOperationException(InvalidRowOperationType.InvalidRow);
					break;
				default:
					throw new ArgumentException("Invalid type of InvalidRowOperationType");
			}
		}

		/// <summary>
		/// Perform this row operation on the given matrix
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public Matrix ActOn(Matrix m)
		{
			switch (Type)
			{
				case RowOperationType.Scaling:
					if (Onto >= m.Height)
						throw new IncompatibleRowOperationException(
							"Index of one of the rows was greater than or equal to the height of the matrix.");

					m[Onto, VectorType.Row] = m[Onto, VectorType.Row] * Scale;
					break;
				case RowOperationType.Substitution:
					if (Onto >= m.Height || From >= m.Height)
						throw new IncompatibleRowOperationException(
							"Index of one of the rows was greater than or equal to the height of the matrix.");

					m[Onto, VectorType.Row] += m[From, VectorType.Row] * Scale;
					break;
				case RowOperationType.Switching:
					if (Onto >= m.Height || From >= m.Height)
						throw new IncompatibleRowOperationException(
						"Index of one of the rows was was greater than or equal to the height of the matrix.");

					Vector from = m[From, VectorType.Row];
					Vector onto = m[Onto, VectorType.Row];

					m[Onto, VectorType.Row] = from;
					m[From, VectorType.Row] = onto;
					break;
			}

			return m;
		}

		/// <summary>
		/// Returns the elementary matrix which corresponds to this operation
		/// </summary>
		/// <returns></returns>
		public Matrix ElementaryMatrix(int size)
		{
			switch (Type)
			{
				case RowOperationType.Scaling:
					if (Onto >= size)
						throw new IncompatibleRowOperationException(
							"Index of one of the rows was greater than or equal to the height of the matrix.");
					break;
				case RowOperationType.Substitution:
					if (Onto >= size || From >= size)
						throw new IncompatibleRowOperationException(
							"Index of one of the rows was greater than or equal to the height of the matrix.");
					break;
				case RowOperationType.Switching:
					if (Onto >= size || From >= size)
						throw new IncompatibleRowOperationException(
							"Index of one of the rows was was greater than or equal to the height of the matrix.");
					break;
			}

			return ActOn(Main.LinearMath.UnitMatrix(size));
		}

		/// <summary>
		/// Get a scaling RowOperation without having to manually call the constructor
		/// </summary>
		/// <param name="onto"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static RowOperation ScaleOperation(int onto, double scale)
		{
			return new RowOperation(RowOperationType.Scaling, onto:onto, scale:scale);
		}

		/// <summary>
		/// Get a substituting RowOperation without having to manually call the constructor
		/// </summary>
		/// <param name="from"></param>
		/// <param name="onto"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static RowOperation SubstitutionOperation(int from, int onto, double scale)
		{
			return new RowOperation(RowOperationType.Substitution, from, onto, scale);
		}

		/// <summary>
		/// Get a switching RowOperation without having to manually call the constructor
		/// </summary>
		/// <param name="from"></param>
		/// <param name="onto"></param>
		/// <returns></returns>
		public static RowOperation SwitchingOperation(int from, int onto)
		{
			return new RowOperation(RowOperationType.Switching, from, onto);
		}
	}
}
