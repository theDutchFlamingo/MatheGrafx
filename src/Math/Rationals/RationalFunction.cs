using System;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Groups;
using Math.Algebra.Rings.Members;
using Math.Exceptions;
using Math.Numeric;
using Math.Polynomials;

namespace Math.Rationals
{
	/// <summary>
	/// A rational function is any polynomial divided by any polynomial other than the zero polynomial.
	/// </summary>
	public class RationalFunction : Rational<IntegerPolynomial>
	{
		public RationalFunction() : base(new IntegerPolynomial())
		{
			
		}

		public RationalFunction(IntegerPolynomial num, IntegerPolynomial den) : base(num, den)
		{
			
		}

		#region Test Methods

		#endregion

		public static implicit operator RationalFunction(IntegerPolynomial polynomial)
		{
			return new RationalFunction(polynomial, new IntegerPolynomial(new Vector<Integer>(new Integer[]{1})));
		}

		public static explicit operator IntegerPolynomial(RationalFunction function)
		{
			if (function.Num.IsNull())
			{
				return new IntegerPolynomial(new Vector<Integer>(new Integer[] { 0 }));
			}

			if (function.Den.IsUnit())
			{
				return function.Num;
			}

			if (function.TryFactor(out Rational<IntegerPolynomial> r))
			{
				if (!r.Den.IsUnit())
					throw new InvalidCastException("Can't write this fractional function as polynomial");

				return function.Num;
			}

			throw new InvalidCastException("Can't write this fractional function as polynomial");
		}
	}
}
