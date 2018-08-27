using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Fields;
using LinearAlgebra.Main;

namespace LinearAlgebra.Numeric
{
	/// <summary>
	/// A rational function is any polynomial divided by any polynomial other than the zero polynomial.
	/// </summary>
	public class RationalFunction : FieldMember
	{
		public IntegerPolynomial Num { get; set; }
		public IntegerPolynomial Den { get; set; }

		public RationalFunction() : base(new IntegerPolynomial())
		{
			Num = new IntegerPolynomial();
			Den = new IntegerPolynomial(new Vector<Integer>(new Integer[] {1}));
		}

		public RationalFunction(IntegerPolynomial num, IntegerPolynomial den) : base(num)
		{
			if (((RationalFunction) den).IsNull())
			{
				throw new DivideByZeroException("Can't divide by the zero polynomial");
			}

			Num = num;
			Den = den;
		}

		#region Test Methods

		public bool TryFactor(out RationalFunction rationalFunction)
		{
			List<Complex> numRoots = RootFinder.DurandKerner(Num);
			List<Complex> denRoots = RootFinder.DurandKerner(Den);

			if (!numRoots.Intersect(denRoots).Any())
			{
				// If factoring is impossible, return this same function
				rationalFunction = new RationalFunction(Num, Den);
				return false;
			}

			throw new NotImplementedException();
		}

		#endregion

		#region Override Methods

		internal override T Add<T>(T other)
		{
			if (other is Rational r)
			{
				return (T)(FieldMember)new RationalFunction(r.Num * Den + r.Den * Num, r.Den * Den);
			}
			throw new IncorrectFieldException(GetType(), "added", other.GetType());
		}

		internal override T Negative<T>()
		{
			return (T)(FieldMember)new RationalFunction(-Num, Den);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Rational c)
				return (T)(FieldMember)new RationalFunction(Num * c.Num, Den * c.Den);
			throw new IncorrectFieldException(GetType(), "added", other.GetType());
		}

		internal override T Inverse<T>()
		{
			return (T)(FieldMember)new RationalFunction(Den, Num);
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T)(FieldMember)new RationalFunction(new IntegerPolynomial(new Vector<Integer>(new Integer[]{0})),
					new IntegerPolynomial(new Vector<Integer>(new Integer[]{1})));
			}
			throw new IncorrectFieldException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
				return (T)(FieldMember)new Rational(1);
			throw new IncorrectFieldException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Num.Equals((IntegerPolynomial) Null<RationalFunction>());

		public override bool IsUnit() => Num.Equals(Den);

		public override double ToDouble()
		{
			throw new InvalidOperationException("Fractional functions ");
		}

		public override bool Equals<T>(T other)
		{
			if (other is RationalFunction r)
			{
				RationalFunction function1;
				RationalFunction function2;

				TryFactor(out function1);
				r.TryFactor(out function2);

				return r.Num.Equals(Num) && r.Den.Equals(Den);
			}
			return false;
		}

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

			if (function.TryFactor(out RationalFunction r))
			{
				if (!r.Den.IsUnit())
					throw new InvalidCastException("Can't write this fractional function as polynomial");

				return function.Num;
			}

			throw new InvalidCastException("Can't write this fractional function as polynomial");
		}
	}
}
