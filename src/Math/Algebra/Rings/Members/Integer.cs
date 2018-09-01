using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Groups;
using Math.Algebra.Groups.Members;
using Math.Algebra.Monoids.Members;
using Math.Exceptions;
using Math.LinearAlgebra;
using Math.NumberTheory;
using Math.Rationals;

namespace Math.Algebra.Rings.Members
{
    public class Integer : RingMember, INumerical, IFactorable
    {
        public int Value { get; }
        
        #region Constructors

        public Integer()
        {
            
        }

        public Integer(int i)
        {
            Value = i;
        }

        public Integer(Integer i)
        {
            Value = i.Value;
        }

		#endregion

		/**
		 * All the overridden methods
		 */
		#region Overrides

		/**
		 * All the overrides from IFactorable
		 */
		#region Factoring

		public bool IsPrime()
		{
			return IntegerMath.IsPrime(this);
		}

	    public T[] Factors<T>() where T : IFactorable
	    {
		    TryFactor(out T[] factors);

		    return factors;
	    }

	    public bool TryFactor<T>(out T[] factors) where T : IFactorable
	    {
		    factors = null;

		    if (typeof(T) == GetType())
		    {
			    if (IsPrime())
			    {
				    factors = new T[0];
				    return false;
			    }
			    
			    List<T> list = new List<T>();

			    for (int i = 2; i < this; i++)
			    {
				    if (this % i == 0)
				    {
					    list.Add((T)(IFactorable)(Integer)i);
				    }
			    }

			    factors = list.ToArray();

			    if (factors.Length != 0)
				    return true;
		    }

		    throw new FactorTypeException();
	    }

	    public T Without<T>(T factor) where T : IFactorable
	    {
		    if (typeof(T) != GetType())
			    throw new FactorTypeException();
		    
		    Integer f = (Integer) (IFactorable) factor;
		    
		    if (f == this)
		    {
			    return (T) (IFactorable) new Integer(1);
		    }
		    
		    List<Integer> factors = Factors<Integer>().ToList();

		    if (factors.Count == 0 || !factors.Contains(f))
			    return (T) (IFactorable) this;

		    factors.Remove(f);

		    Integer result = factors[0];

		    for (int i = 1; i < factors.Count; i++)
		    {
			    result = result.Multiply(factors[i]);
		    }

		    return (T) (IFactorable) result;
	    }

		#endregion

		/**
		 * Contains all the overrides from RingMember
		 */
		#region Operations

		internal override T Add<T>(T other)
        {
            if (other is Integer r)
            {
                return (T)(MonoidMember)new Integer(Value + r.Value);
            }
            throw new IncorrectSetException(GetType(), "added", other.GetType());
        }

        public override T Negative<T>()
        {
            return (T)(INegatable)new Integer(-Value);
        }

        internal override T Multiply<T>(T other)
        {
            if (other is Integer c)
                return (T)(GroupMember)new Integer(Value * c.Value);
            throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
        }

        public override T Null<T>()
        {
            if (typeof(T) == GetType())
                return (T)(MonoidMember) new Integer(0);
            throw new IncorrectSetException(this, "null", typeof(T));
        }

        public override T Unit<T>()
        {
            if (typeof(T) == GetType())
                return (T)(GroupMember)new Integer(1);
            throw new IncorrectSetException(this, "unit", typeof(T));
        }

        public override bool IsNull() => Value.CloseTo(0);

        public override bool IsUnit() => Value.CloseTo(1);

	    [Obsolete]
        public override double ToDouble()
        {
            return this;
        }

        public override bool Equals<T>(T other)
        {
            if (other is Integer r)
                return Value == r;
	        if (other is int i)
		        return Value == i;
            return false;
        }

	    #endregion

		/**
		 * Overrides from INumerical
		 */
		#region INumerical

		public INumerical Round()
	    {
		    return new Integer(Value);
	    }

	    public INumerical Log10()
	    {
		    return new Real(System.Math.Log10(Value));
	    }

	    public INumerical LongestValue()
	    {
		    return this;
	    }

		#endregion

		#endregion

		/**
         * The operators to make adding and subtracting easier
         */
		#region Operators

		/// <summary>
		/// Add the two together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Integer operator +(Integer left, Integer right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Negative of this ringmember
        /// </summary>
        /// <param name="ringMember"></param>
        /// <returns></returns>
        public static Integer operator -(Integer ringMember)
        {
            return ringMember.Negative<Integer>();
        }

        /// <summary>
        /// Subtract right from left
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Integer operator -(Integer left, Integer right)
        {
            return left + -right;
        }

        /// <summary>
        /// Multiply the two together
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Integer operator *(Integer left, Integer right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Divide one FieldMember&lt;V&gt; by another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(Integer left, Integer right)
        {
            return new Fraction(left, right);
        }

        #endregion

        #region Conversion

        public static implicit operator int(Integer r)
        {
            return r.Value;
        }

        public static implicit operator Integer(int r)
        {
            return new Integer(r);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

	    public string ToString(string format)
        {
            return Value.ToString(format);
        }

        #endregion
    }
}