namespace Math.Algebra.Structures.Groups
{
	public interface INegatable
	{
		T Negative<T>() where T : INegatable;
	}
}
