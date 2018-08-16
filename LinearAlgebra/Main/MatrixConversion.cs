using System;
using System.Collections.Generic;
using System.Linq;
using LinearAlgebra.Fields;

namespace LinearAlgebra.Main
{
	public static class MatrixConversion
	{
		/// <summary>
		/// Put all the null rows on the bottom of the matrix
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		private static Matrix SortNullRows(this Matrix m, int amountToIgnore = 0)
		{
			// First clone the matrix so that the original is not changed
			Matrix n = new Matrix(m);

			// Put all null rows on the bottom of the matrix
			while (!n.AreNullRowsBelow(amountToIgnore))
			{
				List<int> toSwitch = n.MisplacedNullRows(amountToIgnore);

				int next = toSwitch.First();

				// Switch the null row with the last non-null row
				RowOperation.SwitchingOperation(next, n.Height + toSwitch.Count -
				                                      n.AmountOfNullRows(amountToIgnore) - 1).ActOn(n);
			}

			return n;
		}

		/// <summary>
		/// Put the matrix in sorted form
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		private static Matrix ToSortedForm(this Matrix m, int amountToIgnore = 0)
		{
			Matrix n = m.SortNullRows(amountToIgnore);

			int rowsSorted = -1;

			// Sort all the rows by switching
			while (!n.IsSorted(amountToIgnore))
			{
				rowsSorted++;

				List<int> rows = Enumerable.Range(rowsSorted,
						n.Height - n.AmountOfNullRows(amountToIgnore) - rowsSorted).ToList();
				int min = rows.Select(i => n.GetPivot(i, amountToIgnore)).Min();
				int next = rows.First(i => n.GetPivot(i, amountToIgnore) == min);

				if (next == rowsSorted)
					continue;

				RowOperation.SwitchingOperation(next, rowsSorted).ActOn(n);
			}

			return n;
		}

		/// <summary>
		/// Converts the given matrix to echelon form
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore">The amount of rows on the right that should not
		/// be considered when converting to echelon (amount of auxiliary columns)</param>
		/// <returns></returns>
		public static Matrix ToEchelonForm(this Matrix m, int amountToIgnore = 0)
		{
			Matrix n = m.ToSortedForm();

			int from = -1;

			// Put the rows in echelon form (some substitution will be required)
			while (!n.IsEchelon(amountToIgnore))
			{
				from++;
				// Substitution can unsort the rows
				if (!n.IsSorted(amountToIgnore)) n = n.ToSortedForm(amountToIgnore);

				List<int> rows = Enumerable.Range(from + 1,
						n.Height - n.AmountOfNullRows(amountToIgnore) - from - 1).ToList();
				int pivot = n.GetPivot(from, amountToIgnore);

				foreach (var to in rows)
				{
					if (n.GetPivot(from, amountToIgnore) < n.GetPivot(to, amountToIgnore))
						continue;
					if (n.GetPivot(from, amountToIgnore) == n.GetPivot(to, amountToIgnore))
					{
						Real scale = (Real) (-n[to, pivot] / n[from, pivot]);

						RowOperation.SubstitutionOperation(from, to, scale).ActOn(n);
					}
					if (n.GetPivot(from, amountToIgnore) > n.GetPivot(to, amountToIgnore) &&
					    n.GetPivot(to, amountToIgnore) != -1)
					{
						RowOperation.SwitchingOperation(from, to).ActOn(n);
					}
				}
			}

			return n;
		}

		/// <summary>
		/// Scale the rows so that each pivot is a 1
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		private static Matrix ToUnitPivots(this Matrix m, int amountToIgnore = 0)
		{
			Matrix n = m.ToEchelonForm(amountToIgnore);

			int i = 0;

			foreach (var vector in n[VectorType.Row])
			{
				if (n.GetPivot(i, amountToIgnore) == -1) continue;

				double scale = 1 / vector[n.GetPivot(i, amountToIgnore)];

				RowOperation.ScaleOperation(i, scale).ActOn(n);

				i++;
			}

			return n;
		}

		/// <summary>
		/// Convert the Matrix to reduced echelon form
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		public static Matrix ToReducedEchelonForm(this Matrix m, int amountToIgnore = 0)
		{
			// First convert to normal echelon form
			Matrix n = m.ToUnitPivots(amountToIgnore);

			int from = n.Height - n.AmountOfNullRows(amountToIgnore);

			// Put the rows in echelon form (some substitution will be required)
			while (!n.IsReducedEchelon(amountToIgnore))
			{
				from--;

				List<int> rows = Enumerable.Range(0, from).ToList();
				// Go from the bottom to the top
				rows.Reverse();
				int pivot = n.GetPivot(from, amountToIgnore);

				foreach (var to in rows)
				{
					if (!n[to,pivot].IsNull)
					{
						Real scale = (Real) (-n[to, pivot] / n[from, pivot]);

						RowOperation.SubstitutionOperation(from, to, scale).ActOn(n);
					}
				}
			}

			return n;
		}

		/// <summary>
		/// Whether all null rows are on the bottom
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		private static bool AreNullRowsBelow(this Matrix m, int amountToIgnore)
		{
			return m.MisplacedNullRows(amountToIgnore).Count == 0;
		}

		/// <summary>
		/// Find out if the matrix is sorted, that is, no switching can bring the matrix closer to
		/// echelon form, and substitution must be performed.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		private static bool IsSorted(this Matrix m, int amountToIgnore)
		{
			int maxPivot = -1;

			// Make sure pivot positions are increasing
			for (int i = 0; i < m.Height; i++)
			{
				if (m.GetPivot(i, amountToIgnore) < maxPivot && m.GetPivot(i, amountToIgnore) != -1)
					return false;

				maxPivot = m.GetPivot(i, amountToIgnore);
			}

			return m.AreNullRowsBelow(amountToIgnore);
		}

		/// <summary>
		/// List of row indices with a null row that should be lower
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		private static List<int> MisplacedNullRows(this Matrix m, int amountToIgnore)
		{
			List<int> result = new List<int>();

			for (int i = 0; i < m.Height - m.AmountOfNullRows(amountToIgnore); i++)
			{
				if (m.GetPivot(i, amountToIgnore) == -1) result.Add(i);
			}

			return result;
		}

		/// <summary>
		/// Whether the matrix is in echelon form, that is, whether increasing the row number
		/// also increases the pivot position
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		public static bool IsEchelon(this Matrix m, int amountToIgnore = 0)
		{
			int maxPivot = -1;

			// Make sure pivot positions are strictly increasing
			for (int i = 0; i < m.Height; i++)
			{
				if (m.GetPivot(i, amountToIgnore) <= maxPivot && m.GetPivot(i, amountToIgnore) != -1)
					return false;

				maxPivot = m.GetPivot(i, amountToIgnore);
			}

			return m.AreNullRowsBelow(amountToIgnore);
		}

		/// <summary>
		/// Whether the matrix is in reduced echelon form, that is, whether increasing the row number
		/// also increases the pivot position, and all pivots are 1, and whether all the indices above
		/// each pivot are zeroes.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		public static bool IsReducedEchelon(this Matrix m, int amountToIgnore = 0)
		{
			if (!m.IsEchelon(amountToIgnore)) return false;

			// Make sure pivots are 1 and above the pivots there are only zeroes.
			for (int i = 0; i < m.Height; i++)
			{
				if (m.GetPivot(i, amountToIgnore) != -1 &&
				    (!m[i, VectorType.Row][m.GetPivot(i, amountToIgnore)].IsUnit ||
				    ((Matrix) m.Transpose()).GetPivot(m.GetPivot(i, amountToIgnore)) != i))
					return false;
			}

			return m.AreNullRowsBelow(amountToIgnore);
		}

		/// <summary>
		/// Returns the amount of rows in the given matrix which are completely zero
		/// </summary>
		/// <param name="m"></param>
		/// <param name="amountToIgnore"></param>
		/// <returns></returns>
		public static int AmountOfNullRows(this Matrix m, int amountToIgnore = 0)
		{
			Matrix n = new Matrix(m);

			for (int j = m.Width - 1; j >= m.Width - amountToIgnore; j--)
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
		private static Matrix WithoutColumn(this Matrix m, int n)
		{
			double[,] indices = new double[m.Height, m.Width - 1];

			if (n >= m.Width)
				throw new IndexOutOfRangeException();

			for (int i = 0; i < m.Height; i++)
			{
				for (int j = 0; j < m.Width; j++)
				{
					if (j == n)
						continue;

					indices[i, j > n ? j - 1 : j] = m.Indices[i, j];
				}
			}

			return new Matrix(indices);
		}

		/// <summary>
		/// Get the pivot, i.e. the first non-zero value of the row indicated by row.
		/// Returns -1 if this row has no pivot
		/// </summary>
		/// <param name="m">The matrix to evaluate</param>
		/// <param name="i">Which row to get a pivot from</param>
		/// <param name="amountToIgnore">Amount of columns on the right
		/// to ignore when searching non-zero values</param>
		/// <returns></returns>
		public static int GetPivot(this Matrix m, int i, int amountToIgnore = 0)
		{
			if (i >= m.Height)
				throw new IndexOutOfRangeException(
					"Row index was greater than or equal to the height of the matrix");

			for (int j = 0; j < m.Width - amountToIgnore; j++)
			{
				if (!m[i, j].CloseTo(0)) return j;
			}

			return -1;
		}
	}
}
