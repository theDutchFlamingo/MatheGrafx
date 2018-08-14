namespace LinearAlgebra.VectorSpaces
{
	public class Basis
	{
		public Vector[] BasisVectors { get; }

		public Basis(Vector[] basisVectors)
		{
			// Remove enough vectors to have no linear dependence
			BasisVectors = basisVectors;
		}

		public Vector[] GetIndependentVectors(Vector[] vectors)
		{
			return vectors;
		}
	}
}
