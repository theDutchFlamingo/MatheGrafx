namespace Math.Rationals
{
	public interface IFactorable
	{
		bool IsPrime();

		T[] Factors<T>();

		bool TryFactor<T>(out T[] factors) where T : IFactorable;

		T Without<T>(T factor);
	}
}
