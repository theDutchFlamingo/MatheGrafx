using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Rings;
using Math.Algebra.Structures.Rings.Members;
using Math.ComplexLinearAlgebra;
using Math.Rationals;

namespace Math.Algebra.Structures
{
    public class Set
    {
        public static Ring<Integer> Integers = new WholeNumbers();
        public static Field<Real> Reals = new RealNumbers();
        public static Field<Fraction> Fractions = new RationalNumbers();
        public static Field<Complex> ComplexNumbers = new ComplexNumbers();
    }
}