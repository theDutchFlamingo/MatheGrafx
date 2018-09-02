using Math.Algebra.Structures.Rings.Members;

namespace Math.NumberTheory
{
    public static class IntegerMath
    {
        public static bool IsPrime(Integer i)
        {
            if (i <= 1) return false;
            if (i <= 3) return true;
            if (i % 2 == 0 || i % 3 == 0) return false;

            Integer n = 5;

            while (n < System.Math.Sqrt(i))
            {
                if (i % n == 0 || i % (n + 1) == 0)
                    return false;

                n += 6;
            }

            return true;
        }
    }
}