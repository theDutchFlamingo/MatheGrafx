using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Rings;
using Math.Algebra.Rings.Members;
using Math.ComplexLinearAlgebra;
using Math.Rationals;

namespace Math.Algebra
{
    public class Set
    {
        public static Ring<Integer> Integers = new WholeNumbers();
        public static Field<Real> Reals = new RealNumbers();
        public static Field<Fraction> Fractions = new RationalNumbers();
        public static Field<Complex> ComplexNumbers = new ComplexNumbers();
    }
}