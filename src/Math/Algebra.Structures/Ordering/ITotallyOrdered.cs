namespace Math.Algebra.Structures.Ordering
{
	public interface ITotallyOrdered
	{
		bool LessThan<T>(T other);

		bool GreaterThan<T>(T other);

		bool Equals<T>(T other);
	}
}
