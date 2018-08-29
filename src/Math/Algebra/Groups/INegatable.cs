namespace LinearAlgebra.Groups
{
	public interface INegatable
	{
		T Negative<T>() where T : INegatable;
	}
}
