using System;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Groups;
using Math.Algebra.Monoids.Members;
using Math.Algebra.Rings.Members;
using Math.Exceptions;
using Math.Polynomials;

namespace Math.Rationals
{
	/// <summary>
	/// A rational function is any polynomial divided by any polynomial other than the zero polynomial.
	/// </summary>
	public class FractionalFunction : Rational<RealPolynomial>
	{
		public RealPolynomial Num { get; set; }
		public RealPolynomial Den { get; set; }

		public FractionalFunction() : base(new RealPolynomial())
		{
			Num = new RealPolynomial();
			Den = new RealPolynomial(new Vector<Real>(new Real[] {1}));
		}

		public FractionalFunction(RealPolynomial num, RealPolynomial den) : base(num)
		{
			if (((FractionalFunction) den).IsNull())
			{
				throw new DivideByZeroException("Can't divide by the zero polynomial");
			}

			Num = num;
			Den = den;
		}

		#region Test Methods

		public bool TryFactor(out FractionalFunction fractionalFunction)
		{
			List<Complex> numRoots = RootFinder.DurandKerner(Num);
			List<Complex> denRoots = RootFinder.DurandKerner(Den);

			if (!numRoots.Intersect(denRoots).Any())
			{
				// If factoring is impossible, return this same function
				fractionalFunction = new FractionalFunction(Num, Den);
				return false;
			}

			throw new NotImplementedException();
		}

		#endregion

		#region Override Methods

		internal override T Add<T>(T other)
		{
			if (other is Fraction r)
			{
				return (T)(MonoidMember)new FractionalFunction(r.Num * Den + r.Den * Num, r.Den * Den);
			}
			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T Negative<T>()
		{
			return (T)(INegatable)new FractionalFunction(-Num, Den);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Fraction c)
				return (T)(RingMember)new FractionalFunction(Num * c.Num, Den * c.Den);
			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override T Inverse<T>()
		{
			return (T)(IInvertible)new FractionalFunction(Den, Num);
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T)(MonoidMember)new FractionalFunction(new RealPolynomial(new Vector<Real>(new Real[]{0})),
					new RealPolynomial(new Vector<Real>(new Real[]{1})));
			}
			throw new IncorrectSetException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
				return (T)(RingMember)new Fraction(1);
			throw new IncorrectSetException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Num.Equals((RealPolynomial) Null<FractionalFunction>());

		public override bool IsUnit() => Num.Equals(Den);

		[Obsolete]
		public override double ToDouble()
		{
			throw new InvalidOperationException("Fractional functions ");
		}

		public override bool Equals<T>(T other)
		{
			if (other is FractionalFunction r)
			{
				FractionalFunction function1;
				FractionalFunction function2;

				TryFactor(out function1);
				r.TryFactor(out function2);

				return r.Num.Equals(Num) && r.Den.Equals(Den);
			}
			return false;
		}

		#endregion

		public static implicit operator FractionalFunction(RealPolynomial polynomial)
		{
			return new FractionalFunction(polynomial, new RealPolynomial(new Vector<Real>(new Real[]{1})));
		}

		public static explicit operator RealPolynomial(FractionalFunction function)
		{
			if (function.Num.IsNull())
			{
				return new RealPolynomial(new Vector<Real>(new Real[]{0}));
			}

			if (function.Den.IsUnit())
			{
				return function.Num;
			}

			if (function.TryFactor(out FractionalFunction f))
			{
				if (!f.Den.IsUnit()) throw new InvalidCastException("Can't write this fractional function as polynomial");

				return (RealPolynomial) function;
			}

			throw new InvalidCastException("Can't write this fractional function as polynomial");
		}
	}
}
