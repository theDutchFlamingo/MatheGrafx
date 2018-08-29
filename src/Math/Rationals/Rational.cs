using System;
using System.Collections.Generic;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Groups;
using Math.Algebra.Groups.Members;
using Math.Exceptions;

namespace Math.Rationals
{
    public class Rational<T> : FieldMember where T : GroupMember, INegatable, IFactorable, new()
    {
        public T Num { get; set; } = new T();
        public T Den { get; set; } = new T().Unit<T>();

		/// <summary>
		/// The functions which converts this rational to a double. Should be set on initialization.
		/// </summary>
	    public virtual Func<Rational<T>, double> DoubleConvert { get; set; }

	    public Rational()
	    {

	    }

	    public Rational(T num)
	    {
		    Num = num;
	    }

		public Rational(T num, T den)
		{
			Num = num;
			Den = den;
		}

	    #region Factoring

	    

	    #endregion

		#region Overrides

		internal override T1 Add<T1>(T1 other)
        {
            if (other is Rational<T> r)
            {
                return (T1)(GroupMember)
	                new Rational<T>(r.Num.Multiply(Den).Add(r.Den.Multiply(Num)), r.Den.Multiply(Den));
            }
            throw new IncorrectFieldException(GetType(), "added", other.GetType());
        }

        public override T1 Negative<T1>()
        {
            return (T1)(INegatable)new Rational<T>(Num.Negative<T>(), Den);
        }

        internal override T1 Multiply<T1>(T1 other)
        {
            if (other is Rational<T> c)
                return (T1)(GroupMember)new Rational<T>(Num.Multiply(c.Num), Den.Multiply(c.Den));
            throw new IncorrectFieldException(GetType(), "multiplied", other.GetType());
        }

        public override T1 Inverse<T1>()
        {
            return (T1)(IInvertible)new Rational<T>(Den, Num);
        }

        public override T1 Null<T1>()
        {
            if (typeof(T1) == GetType())
                return (T1)(GroupMember) new Rational<T>(new T());
            throw new IncorrectFieldException(this, "null", typeof(T1));
        }

        public override T1 Unit<T1>()
        {
            if (typeof(T1) == GetType())
                return (T1)(GroupMember)new Rational<T>(new T().Unit<T>());
            throw new IncorrectFieldException(this, "unit", typeof(T1));
        }

        public override bool IsNull() => Num.Equals(new T());

        public override bool IsUnit() => Num.Equals(Den);

		/// <summary>
		/// This method cannot be implemented yet, but to make this class non-abstract,
		/// it throws a NotImplementedException
		/// </summary>
		/// <returns></returns>
	    public override double ToDouble()
		{
			return DoubleConvert(this);
		}

        public override bool Equals<T1>(T1 other)
        {
	        if (other is Rational<T> r)
	        {
				List<T> matchedFactors = new List<T>();

		        foreach (var VARIABLE in r.)
		        {
			        
		        }

		        return ((double)Num / Den).CloseTo((double)r.Num / r.Den);
	        }

            return false;
        }

        public override bool Equals(FieldMember other)
        {
            if (other is Fraction r)
            {
                return r.
            }
        }

        #endregion
    }
}