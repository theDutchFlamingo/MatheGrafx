using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids;
using Math.Algebra.Structures.Rings;
using Math.Algebra.Structures.Rings.Members;
using Math.ComplexLinearAlgebra;
using Math.Rationals;

namespace Math.Algebra.Structures
{
    public abstract class Set
    {
		public static Monoid<Natural> Naturals = new NaturalNumbers();
        public static Ring<Integer> Integers = new WholeNumbers();
        public static Field<Fraction> Fractions = new RationalNumbers();
        public static Field<Real> Reals = new RealNumbers();
        public static Field<Complex> ComplexNumbers = new ComplexNumbers();

	    public abstract bool Contains<T>(T element) where T : Element;
    }
}