using System;
using Math.Fields.Members;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Exceptions;
using Math.Fields;

namespace Math.Main
{
	public class RowOperation<T> where T : FieldMember, new()
	{
		public enum RowOperationType
		{
			Substitution, Switching, Scaling
		}

		/// <summary>
		/// For substitution and scaling, how much to scale the row vector by
		/// </summary>
		private T Scale { get; }
		/// <summary>
		/// For substitution and switching, the row to take the vector from
		/// </summary>
		private int From { get; }
		/// <summary>
		/// For subtitution, switching and scaling, the row vector to change
		/// </summary>
		private int Onto { get; }

		private RowOperationType Type { get; }

		public RowOperation(RowOperationType type, int from = -1, int onto = -1, T scale = null)
		{
			Type = type;
			From = from;
			Onto = onto;
			Scale = scale;

			switch (type)
			{
				case RowOperationType.Scaling:
					if (scale.Equals(0)) throw new
						InvalidRowOperationException(InvalidRowOperationType.ScaleZero);
					break;
				case RowOperationType.Substitution:
					if (scale.Equals(0)) throw new
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
		public Matrix<T> ActOn(Matrix<T> m)
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

					Vector<T> from = m[From, VectorType.Row];
					Vector<T> onto = m[Onto, VectorType.Row];

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
		public Matrix<T> ElementaryMatrix(int size)
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

			return ActOn(LinearMath.UnitMatrix<T>(size));
		}

		/// <summary>
		/// Get a scaling RowOperation without having to manually call the constructor
		/// </summary>
		/// <param name="onto"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static RowOperation<T> ScaleOperation(int onto, T scale)
		{
			return new RowOperation<T>(RowOperationType.Scaling, onto:onto, scale:scale);
		}

		/// <summary>
		/// Get a substituting RowOperation without having to manually call the constructor
		/// </summary>
		/// <param name="from"></param>
		/// <param name="onto"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static RowOperation<T> SubstitutionOperation(int from, int onto, T scale)
		{
			return new RowOperation<T>(RowOperationType.Substitution, from, onto, scale);
		}

		/// <summary>
		/// Get a switching RowOperation without having to manually call the constructor
		/// </summary>
		/// <param name="from"></param>
		/// <param name="onto"></param>
		/// <returns></returns>
		public static RowOperation<T> SwitchingOperation(int from, int onto)
		{
			return new RowOperation<T>(RowOperationType.Switching, from, onto);
		}
	}
}
