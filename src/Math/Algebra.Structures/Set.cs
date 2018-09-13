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
		/// <summary>
		/// Set of all 2x2 matrices
		/// </summary>
	    public static Field<Matrix<Real>> RealMatrices2 = new RealMatrices(2);
		/// <summary>
		/// Set of all 3x3 matrices
		/// </summary>
	    public static Field<Matrix<Real>> RealMatrices3 = new RealMatrices(3);

		/// <summary>
		/// Whether this set contains given element T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="element"></param>
		/// <returns></returns>
		public abstract bool Contains<T>(T element) where T : Element;
    }
}