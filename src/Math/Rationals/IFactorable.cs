namespace Math.Rationals
{
	public interface IFactorable
	{
		bool TryFactor<T>(out T[] factors) where T : IFactorable;
	}
}
