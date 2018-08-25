using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearAlgebra
{
	/// <summary>
	/// Used in conversions between matrices and arrays of vectors. This simple enum
	/// determines whether that array of vectors represents the columns or the rows
	/// of a matrix.
	/// </summary>
	public enum VectorType
	{
		Column, Row
	}
}
