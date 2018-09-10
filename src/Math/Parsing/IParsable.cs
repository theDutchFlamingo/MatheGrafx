namespace Math.Parsing
{
    public interface IParsable<out T>
    {
        T FromString(string value);
    }
}