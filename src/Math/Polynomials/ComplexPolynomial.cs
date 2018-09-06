using Math.Algebra.Structures.Fields.Members;
using Math.ComplexLinearAlgebra;

namespace Math.Polynomials
{
    public class ComplexPolynomial : Polynomial<Complex>
    {
	    public ComplexPolynomial(ComplexVector coefficients) : base(coefficients)
	    {
		    
	    }
	    
		#region Conversions

	    public static explicit operator ComplexPolynomial(RealPolynomial polynomial)
	    {
		    return new ComplexPolynomial((ComplexVector) polynomial.Coefficients);
	    }

		#endregion
	}
}