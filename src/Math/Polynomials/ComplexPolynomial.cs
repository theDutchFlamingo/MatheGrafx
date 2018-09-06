using Math.Algebra.Structures.Fields.Members;

namespace Math.Polynomials
{
    public class ComplexPolynomial : Polynomial<Complex>
    {
		#region Conversions

	    public static explicit operator ComplexPolynomial(RealPolynomial polynomial)
	    {

	    }

		#endregion
	}
}