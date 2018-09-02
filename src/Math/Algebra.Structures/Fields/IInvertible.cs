namespace Math.Algebra.Structures.Fields
{
	public interface IInvertible
	{
		T Inverse<T>() where T : IInvertible;
	}
}
