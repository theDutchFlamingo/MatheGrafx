namespace Math.Algebra.Structures.Ordering
{
	public interface ITotallyOrdered
	{
		bool LessThan<T>(T other) where T : ITotallyOrdered;

		bool GreaterThan<T>(T other) where T : ITotallyOrdered;

		bool Equals<T>(T other);
	}
}
