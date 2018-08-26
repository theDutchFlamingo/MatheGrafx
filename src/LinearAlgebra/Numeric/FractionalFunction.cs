using System;
using System.Collections.Generic;
using System.Linq;
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
	public class FractionalFunction : FieldMember
	{
		public RealPolynomial Num { get; set; }
		public RealPolynomial Den { get; set; }

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

		public bool TryFactor(out FractionalFunction FractionalFunction)
		{
			List<Complex> numRoots = RootFinder.DurandKerner();
		}

		#endregion

		#region Override Methods

		internal override T Add<T>(T other)
		{
			if (other is Rational r)
			{
				return (T)(FieldMember)new FractionalFunction(r.Num * Den + r.Den * Num, r.Den * Den);
			}
			throw new IncorrectFieldException(GetType(), "added", other.GetType());
		}

		internal override T Negative<T>()
		{
			return (T)(FieldMember)new FractionalFunction(-Num, Den);
		}

		internal override T Multiply<T>(T other)
		{
			if (other is Rational c)
				return (T)(FieldMember)new FractionalFunction(Num * c.Num, Den * c.Den);
			throw new IncorrectFieldException(GetType(), "added", other.GetType());
		}

		internal override T Inverse<T>()
		{
			return (T)(FieldMember)new FractionalFunction(Den, Num);
		}

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
			{
				return (T)(FieldMember)new FractionalFunction(new RealPolynomial(new Vector<Real>(new Real[]{0})),
					new RealPolynomial(new Vector<Real>(new Real[]{1})));
			}
			throw new IncorrectFieldException(this, "null", typeof(T));
		}

		public override T Unit<T>()
		{
			if (typeof(T) == GetType())
				return (T)(FieldMember)new Rational(1);
			throw new IncorrectFieldException(this, "unit", typeof(T));
		}

		public override bool IsNull() => Num.Equals((RealPolynomial) Null<FractionalFunction>());

		public override bool IsUnit() => Num.Equals(Den);

		public override double ToDouble()
		{
			return this;
		}

		public override bool Equals<T>(T other)
		{
			if (other is FractionalFunction r)
				return (Num / Den).CloseTo(r.Num / r.Den);
			return false;
		}

		#endregion

		public static implicit operator FractionalFunction(RealPolynomial polynomial)
		{
			return new FractionalFunction(polynomial, new RealPolynomial(new Vector<Real>(new Real[]{1})));
		}

		public static explicit operator RealPolynomial(FractionalFunction function)
		{
			if (function.IsNull())
			{
				return new RealPolynomial(new Vector<Real>(new Real[]{0}));
			}

			if (function.IsUnit())
			{
				return new RealPolynomial(new Vector<Real>(new Real[]{1}));
			}

			try
			{
				return 
			}
			catch ()
			{
				throw new InvalidCastException("Cannot cast this function to an Real polynomial");
			}
		}
	}
}
