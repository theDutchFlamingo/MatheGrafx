using Math.LinearAlgebra;

namespace Math.VectorSpaces
{
	public class Basis
	{
		public RealVector[] BasisVectors { get; }

		public Basis(RealVector[] basisVectors)
		{
			// Remove enough vectors to have no linear dependence
			BasisVectors = basisVectors;
		}

		public RealVector[] GetIndependentVectors(RealVector[] vectors)
		{
			return vectors;
		}
	}
}
