using System;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Structures.Fields.Members;
using Math.Exceptions;

namespace Math.LinearAlgebra
{
	public static class MatrixConversion
	{
		/// <summary>
		/// Put all the null rows on the bottom of the matrix
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		private static Matrix<T> SortNullRows<T>(
			this Matrix<T> m, Func<T> constructor, int toIgnore = 0)
			where  T : FieldMember, new()
		{
			// First clone the matrix so that the original is not changed
			Matrix<T> n = new Matrix<T>(m);

			// Put all null rows on the bottom of the matrix
			while (!n.AreNullRowsBelow(constructor, toIgnore))
			{
				List<int> toSwitch = n.MisplacedNullRows(constructor, toIgnore);

				int next = toSwitch.First();

				// Switch the null row with the last non-null row
				RowOperation<T>.SwitchingOperation(next, n.Height + toSwitch.Count -
				                                      n.AmountOfNullRows(toIgnore) - 1).ActOn(n);
			}

			return n;
		}

		/// <summary>
		/// Put the matrix in sorted form
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor">The constructor for a null element of T</param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		private static Matrix<T> ToSortedForm<T>(
			this Matrix<T> m, Func<T> constructor, int toIgnore = 0)
			where T : FieldMember, new()
		{
			Matrix<T> n = m.SortNullRows(constructor, toIgnore);

			int rowsSorted = -1;

			// Sort all the rows by switching
			while (!n.IsSorted(constructor, toIgnore))
			{
				rowsSorted++;

				List<int> rows = Enumerable.Range(rowsSorted,
						n.Height - n.AmountOfNullRows(toIgnore) - rowsSorted).ToList();
				int min = rows.Select(i => n.GetPivot(i,constructor, toIgnore)).Min();
				int next = rows.First(i => n.GetPivot(i, constructor, toIgnore) == min);

				if (next == rowsSorted)
				{
					continue;
				}

				RowOperation<T>.SwitchingOperation(next, rowsSorted).ActOn(n);
			}

			return n;
		}

		/// <summary>
		/// Converts the given matrix to echelon form
		/// </summary>
		/// <param name="m"></param>
		/// <param name="toIgnore">The amount of rows on the right that should not
		/// be considered when converting to echelon (amount of auxiliary columns)</param>
		/// <returns></returns>
		public static Matrix<T> ToEchelonForm<T>(this Matrix<T> m, int toIgnore = 0)
			where T : FieldMember, new()
		{
			return m.ToEchelonForm(() => new T(), toIgnore);
		}

		public static Matrix<T> ToEchelonForm<T>(
			this Matrix<T> m,
			Func<T> constructor,
			int toIgnore = 0)
			where T : FieldMember, new()
		{
			Matrix<T> n = m.ToSortedForm(constructor);

			int from = -1;

			// Put the rows in echelon form (some substitution will be required)
			while (!n.IsEchelon(constructor, toIgnore))
			{
				from++;
				// Substitution can unsort the rows
				if (!n.IsSorted(constructor, toIgnore))
					n = n.ToSortedForm(constructor, toIgnore);

				List<int> rows = Enumerable.Range(from + 1,
					n.Height - n.AmountOfNullRows(toIgnore) - from - 1).ToList();
				int pivot = n.GetPivot(from, constructor, toIgnore);

				foreach (var to in rows)
				{
					if (n.GetPivot(from, constructor, toIgnore) <
					    n.GetPivot(to, constructor, toIgnore))
					{
						continue;
					}
					if (n.GetPivot(from, constructor, toIgnore) ==
					    n.GetPivot(to, constructor, toIgnore))
					{
						try
						{
							T scale = n[to, pivot].Negative<T>().Multiply(n[from, pivot].Inverse<T>());
							RowOperation<T>.SubstitutionOperation(from, to, scale).ActOn(n);
						}
						catch (IncompatibleOperationException)
						{
							throw new ImpossibleEchelonFormException();
						}
					}
					if (n.GetPivot(from, constructor, toIgnore) >
					    n.GetPivot(to, constructor, toIgnore) &&
					    n.GetPivot(to, constructor, toIgnore) != -1)
					{
						RowOperation<T>.SwitchingOperation(from, to).ActOn(n);
					}
				}
			}

			return n;
		}

		/// <summary>
		/// Scale the rows so that each pivot is a 1
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		private static Matrix<T> ToUnitPivots<T>(
			this Matrix<T> m, Func<T> constructor, int toIgnore = 0)
			where T : FieldMember, new()
		{
			Matrix<T> n = m.ToEchelonForm(constructor, toIgnore);

			int i = 0;

			foreach (var vector in n[VectorType.Row])
			{
				if (n.GetPivot(i, constructor, toIgnore) == -1)
				{
					continue;
				}

				try
				{
					T scale = constructor().Unit<T>()
						.Multiply(vector[n.GetPivot(i, constructor, toIgnore)].Inverse<T>());
					RowOperation<T>.ScaleOperation(i, scale).ActOn(n);
				}
				catch (IncompatibleOperationException)
				{
					throw new ImpossibleEchelonFormException();
				}

				i++;
			}

			return n;
		}

		/// <summary>
		/// Convert the Matrix to reduced echelon form
		/// </summary>
		/// <param name="m"></param>
		/// <param name="toIgnore">The amount of rows that needn't be considered when converting.</param>
		/// <returns></returns>
		public static Matrix<T> ToReducedEchelonForm<T>(this Matrix<T> m, int toIgnore = 0)
			where T : FieldMember, new()
		{
			return m.ToReducedEchelonForm(() => new T(), toIgnore);
		}

		/// <summary>
		/// Convert the Matrix to reduced echelon form
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor">The constructor to make a null instance of T</param>
		/// <param name="toIgnore">The amount of rows that needn't be considered when converting.</param>
		/// <returns></returns>
		public static Matrix<T> ToReducedEchelonForm<T>(
			this Matrix<T> m,
			Func<T> constructor,
			int toIgnore = 0)
			where T : FieldMember, new()
		{
			// First convert to normal echelon form
			Matrix<T> n = m.ToUnitPivots(constructor, toIgnore);

			int from = n.Height - n.AmountOfNullRows(toIgnore);

			// Put the rows in echelon form (some substitution will be required)
			while (!n.IsReducedEchelon(constructor, toIgnore))
			{
				from--;

				List<int> rows = Enumerable.Range(0, from).ToList();
				// Go from the bottom to the top
				rows.Reverse();
				int pivot = n.GetPivot(from, constructor, toIgnore);

				foreach (var to in rows)
				{
					if (!n[to,pivot].IsNull())
					{
						try
						{
							T scale = n[to, pivot].Negative<T>().Multiply(n[from, pivot].Inverse<T>());
							RowOperation<T>.SubstitutionOperation(from, to, scale).ActOn(n);
						}
						catch (IncompatibleOperationException)
						{
							throw new ImpossibleEchelonFormException();
						}
					}
				}
			}

			return n;
		}

		/// <summary>
		/// Whether all null rows are on the bottom
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		private static bool AreNullRowsBelow<T>(this Matrix<T> m, Func<T> constructor, int toIgnore)
			where T : FieldMember, new()
		{
			return m.MisplacedNullRows(constructor, toIgnore).Count == 0;
		}

		/// <summary>
		/// Find out if the matrix is sorted, that is, no switching can bring the matrix closer to
		/// echelon form, and substitution must be performed.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		private static bool IsSorted<T>(this Matrix<T> m, Func<T> constructor, int toIgnore)
			where T : FieldMember, new()
		{
			int maxPivot = -1;

			// Make sure pivot positions are increasing
			for (int i = 0; i < m.Height; i++)
			{
				if (m.GetPivot(i, constructor, toIgnore) <
				    maxPivot && m.GetPivot(i, constructor, toIgnore) != -1)
				{
					return false;
				}

				maxPivot = m.GetPivot(i, constructor, toIgnore);
			}

			return m.AreNullRowsBelow(constructor, toIgnore);
		}

		/// <summary>
		/// List of row indices with a null row that should be lower
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor">The constructor for a null element of T</param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		private static List<int> MisplacedNullRows<T>(
			this Matrix<T> m, Func<T> constructor, int toIgnore)
			where T : FieldMember, new()
		{
			List<int> result = new List<int>();

			for (int i = 0; i < m.Height - m.AmountOfNullRows(toIgnore); i++)
			{
				if (m.GetPivot(i, constructor, toIgnore) == -1)
				{
					result.Add(i);
				}
			}

			return result;
		}

		/// <summary>
		/// Whether the matrix is in echelon form, that is, whether increasing the row number
		/// also increases the pivot position
		/// </summary>
		/// <param name="m"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		public static bool IsEchelon<T>(this Matrix<T> m, int toIgnore = 0)
			where T : FieldMember, new()
		{
			return m.IsEchelon(() => new T(), toIgnore);
		}

		/// <summary>
		/// Whether the matrix is in echelon form, that is, whether increasing the row number
		/// also increases the pivot position
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		public static bool IsEchelon<T>(this Matrix<T> m, Func<T> constructor, int toIgnore = 0)
			where T : FieldMember, new()
		{
			int maxPivot = -1;

			// Make sure pivot positions are strictly increasing
			for (int i = 0; i < m.Height; i++)
			{
				if (m.GetPivot(i, constructor, toIgnore) <=
				    maxPivot && m.GetPivot(i, constructor, toIgnore) != -1)
				{
					return false;
				}

				maxPivot = m.GetPivot(i, constructor, toIgnore);
			}

			return m.AreNullRowsBelow(constructor, toIgnore);
		}

		/// <summary>
		/// Whether the matrix is in reduced echelon form, that is, whether increasing the row number
		/// also increases the pivot position, and all pivots are 1, and whether all the indices above
		/// each pivot are zeroes.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		public static bool IsReducedEchelon<T>(this Matrix<T> m, int toIgnore = 0)
			where T : FieldMember, new()
		{
			return m.IsReducedEchelon(() => new T(), toIgnore);
		}

		/// <summary>
		/// Whether the matrix is in reduced echelon form, that is, whether increasing the row number
		/// also increases the pivot position, and all pivots are 1, and whether all the indices above
		/// each pivot are zeroes.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="constructor"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		public static bool IsReducedEchelon<T>(this Matrix<T> m, Func<T> constructor, int toIgnore = 0)
			where T : FieldMember, new()
		{
			if (!m.IsEchelon(constructor, toIgnore))
			{
				return false;
			}

			// Make sure pivots are 1 and above the pivots there are only zeroes.
			for (int i = 0; i < m.Height; i++)
			{
				if (m.GetPivot(i, constructor, toIgnore) != -1 &&
				    (!m[i, VectorType.Row][m.GetPivot(i, constructor, toIgnore)].IsUnit() ||
				     m.Transpose().GetPivot(m.GetPivot(i, constructor, toIgnore), constructor) != i))
				{
					return false;
				}
			}

			return m.AreNullRowsBelow(constructor, toIgnore);
		}

		/// <summary>
		/// Returns the amount of rows in the given matrix which are completely zero
		/// </summary>
		/// <param name="m"></param>
		/// <param name="toIgnore"></param>
		/// <returns></returns>
		public static int AmountOfNullRows<T>(this Matrix<T> m, int toIgnore = 0)
			where T : FieldMember, new()
		{
			Matrix<T> n = new Matrix<T>(m);

			for (int j = m.Width - 1; j >= m.Width - toIgnore; j--)
			{
				n = n.WithoutColumn(j);
			}

			return n[VectorType.Row].Count(v => v.IsNull());
		}

		/// <summary>
		/// Returns the matrix without column n
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private static Matrix<T> WithoutColumn<T>(this Matrix<T> m, int n)
			where T : FieldMember, new()
		{
			T[,] indices = new T[m.Height, m.Width - 1];

			if (n >= m.Width)
			{
				throw new IndexOutOfRangeException();
			}

			for (int i = 0; i < m.Height; i++)
			{
				for (int j = 0; j < m.Width; j++)
				{
					if (j == n)
					{
						continue;
					}

					indices[i, j > n ? j - 1 : j] = m.Indices[i, j];
				}
			}

			return new Matrix<T>(indices);
		}

		/// <summary>
		/// Get the pivot, i.e. the first non-zero value of the row indicated by row.
		/// Returns -1 if this row has no pivot
		/// </summary>
		/// <param name="m">The matrix to evaluate</param>
		/// <param name="i">Which row to get a pivot from</param>
		/// <param name="toIgnore">Amount of columns on the right
		/// to ignore when searching non-zero values</param>
		/// <returns></returns>
		public static int GetPivot<T>(this Matrix<T> m, int i, int toIgnore = 0)
			where T : FieldMember, new()
		{
			return m.GetPivot(i, () => new T(), toIgnore);
		}

		public static int GetPivot<T>(this Matrix<T> m, int i, Func<T> constructor, int toIgnore = 0)
			where T : FieldMember, new()
		{
			if (i >= m.Height)
			{
				throw new IndexOutOfRangeException(
					"Row index was greater than or equal to the height of the matrix");}

			for (int j = 0; j < m.Width - toIgnore; j++)
			{
				if (!m[i, j].Equals(constructor()))
				{
					return j;
				}
			}

			return -1;
		}
	}
}
