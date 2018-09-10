using System;
using Math.Algebra.Structures.Fields.Members;

namespace Math.LinearAlgebra
{
	public static class Diagonalization
	{
		public static bool IsDiagonalizable<TMat, TResult>(this Matrix<TMat> m)
			where TMat : FieldMember, new() where TResult : FieldMember, new()
		{
			if (m is RealMatrix r)
			{
				RealVector eigenValues = r.RealEigenValues();
			}

			throw new NotImplementedException();
		}
	}
}
