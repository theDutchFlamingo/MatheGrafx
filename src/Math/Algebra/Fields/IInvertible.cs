namespace Math.Algebra.Fields
{
	public interface IInvertible
	{
		T Inverse<T>() where T : IInvertible;
	}
}
