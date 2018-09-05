namespace Math.Parsing
{
    public interface IParsable<out T>
    {
        T Parse(string value);
    }
}