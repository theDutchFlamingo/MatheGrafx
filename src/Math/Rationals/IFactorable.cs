namespace Math.Rationals
{
	public interface IFactorable
	{
		/// <summary>
		/// Note: here prime is more general than the prime in number theory,
		/// this returns true if it cannot be factored into more basic components.
		/// </summary>
		/// <returns></returns>
		bool IsPrime();

		T[] Factors<T>() where T : IFactorable;

		bool TryFactor<T>(out T[] factors) where T : IFactorable;

		T Without<T>(T factor) where T : IFactorable;
	}
}
