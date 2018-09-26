using System;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;
using Math.Settings;

namespace Math.Rationals
{
    public class Rational<T> : FieldMember where T : RingMember, IFactorable, new()
    {
        public T Num { get; } = new T();
        public T Den { get; } = new T().Unit<T>();

		/// <summary>
		/// The functions which converts this rational to a double. Should be set on initialization.
		/// </summary>
	    public virtual Func<Rational<T>, double> DoubleConvert { protected get; set; }

	    public Rational()
	    {

	    }

	    public Rational(T num)
	    {
		    Num = num;
	    }

		public Rational(T num, T den)
		{
			if (den.IsNull())
			{
				throw new DivideByZeroException("Denominator of a rational can't be the zero member");
			}
			
			Num = num;
			Den = den;
		}

	    /// <summary>
	    /// Check if the numerator and denominators match, without trying to factor
	    /// </summary>
	    /// <param name="other"></param>
	    /// <returns></returns>
	    public bool EqualsExactly(Rational<T> other)
	    {
		    return Num == other.Num && Den == other.Den;
	    }

		#region Factoring

		/// <summary>
		/// Whether this rational can be factored, factors it even if it is already in lowest terms
		/// </summary>
		/// <param name="factored"></param>
		/// <returns></returns>
		public bool TryFactor(out Rational<T> factored)
		{
			factored = Factor();
			return factored.EqualsExactly(this);
		}

	    public virtual Rational<T> Factor()
	    {
		    List<T> matchedFactors = Num.Factors<T>().Intersect(Den.Factors<T>()).ToList();

		    T num = Num;
		    T den = Den;

		    foreach (var t in matchedFactors)
		    {
			    num = num.Without(t);
			    den = den.Without(t);
		    }

			return new Rational<T>(num, den);
	    }

	    #endregion

		#region Overrides

		internal override T1 Add<T1>(T1 other)
        {
	        if (!(other is Rational<T> r))
	        {
		        throw new IncorrectSetException(GetType(), "added", other.GetType());
	        }

			T num = r.Num.Multiply(Den).Add(r.Den.Multiply(Num));
			T den = r.Den.Multiply(Den);

			return (T1)(MonoidMember)
		        (NumberSettings.FactorFractions ? new Rational<T>(num, den).Factor() : new Rational<T>(num, den));
        }

        public override T1 Negative<T1>()
        {
            return (T1)(INegatable)new Rational<T>(Num.Negative<T>(), Den);
        }

        internal override T1 Multiply<T1>(T1 other)
        {
	        if (!(other is Rational<T> c))
	        {
		        throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
	        }

	        T num = Num.Multiply(c.Num);
	        T den = Den.Multiply(c.Den);

	        return (T1)(GroupMember)
		        (NumberSettings.FactorFractions ? new Rational<T>(num, den).Factor() : new Rational<T>(num, den));

        }

        public override T1 Inverse<T1>()
        {
            return (T1)(IInvertible)new Rational<T>(Den, Num);
        }

        public override T1 Null<T1>()
        {
	        if (typeof(T1).IsSubclassOfGeneric(GetType()))
	        {
		        return (T1)(MonoidMember) new Rational<T>(new T());
	        }

            throw new IncorrectSetException(this, "null", typeof(T1));
        }

        public override T1 Unit<T1>()
        {
	        if (typeof(T1).IsSubclassOfGeneric(GetType()))
	        {
		        return (T1)(GroupMember)new Rational<T>(new T().Unit<T>());
	        }

            throw new IncorrectSetException(this, "unit", typeof(T1));
        }

        public override bool IsNull() => Num.Equals(new T());

        public override bool IsUnit() => Num.Equals(Den);
	    
		/// <summary>
		/// Try finding the closest 
		/// </summary>
		/// <returns></returns>
		[Obsolete]
	    public override double ToDouble()
		{
			return DoubleConvert(this);
		}

        public override bool Equals<T1>(T1 other)
        {
	        if (other is Rational<T> r)
	        {
		        return Factor().EqualsExactly(r.Factor());
	        }

            return false;
        }

        #endregion
    }
}