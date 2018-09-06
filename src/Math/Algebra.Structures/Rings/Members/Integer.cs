using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Ordering;
using Math.Exceptions;
using Math.LinearAlgebra;
using Math.NumberTheory;
using Math.Rationals;
using static System.BitConverter;

namespace Math.Algebra.Structures.Rings.Members
{
    public class Integer : RingMember, INumerical, IFactorable, ITotallyOrdered
    {
	    private bool Positive { get; } = true;

        private readonly Natural _value = new Natural();

	    public long Value => _value;

	    #region Constructors

        public Integer()
        {
            
        }

        public Integer(int i)
        {
            _value = i;

        }

        public Integer(Integer i)
        {
            _value = i.Value;
        }

	    private Integer(byte[] value)
	    {

	    }

	    public Integer(string value, bool hex = true)
	    {
		    if (value.StartsWith("-"))
		    {
			    Positive = false;
			    value = value.Substring(1);
		    }

		    _value = new Natural(value, hex);
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

		    if (typeof(T) != GetType())
		    {
			    throw new FactorTypeException();
		    }

		    if (IsPrime())
		    {
			    factors = new T[0];
			    return false;
		    }
			    
		    List<T> list = new List<T>();

		    for (int i = 2; i < (int)this / 2; i++)
		    {
			    if (this % i == 0)
			    {
				    list.Add((T)(IFactorable)(Integer)i);
			    }
		    }

		    factors = list.ToArray();

		    if (factors.Length != 0)
		    {
			    return true;
		    }

		    throw new FactorTypeException();
	    }

	    public T Without<T>(T factor) where T : IFactorable
	    {
		    if (typeof(T) != GetType())
		    {
			    throw new FactorTypeException();
		    }
		    
		    Integer f = (Integer) (IFactorable) factor;
		    
		    if (f == this)
		    {
			    return (T) (IFactorable) new Integer(1);
		    }
		    
		    List<Integer> factors = Factors<Integer>().ToList();

		    if (factors.Count == 0 || !factors.Contains(f))
		    {
			    return (T) (IFactorable) this;
		    }

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
                return (T)(MonoidMember)new Integer(_value + r._value);
            }
            throw new IncorrectSetException(GetType(), "added", other.GetType());
        }

        public override T Negative<T>()
        {
            return (T)(INegatable)new Integer(-_value);
        }

        internal override T Multiply<T>(T other)
        {
            if (other is Integer c)
                return (T)(GroupMember)new Integer(_value * c._value);
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

        public override bool IsNull() => _value.Equals(0);

        public override bool IsUnit() => _value.Equals(1);

	    [Obsolete]
        public override double ToDouble()
        {
            return this;
        }

	    public bool LessThan<T>(T other) where T : ITotallyOrdered
	    {
		    switch (other) {
			    case Integer i:
				    return (int)this < (int)i;
			    case Fraction f:
				    return this < (double)f;
				case Real r:
					return this < r;
		    }

		    throw new IncorrectSetException(GetType(), "compared", typeof(T));
	    }

	    public bool GreaterThan<T>(T other) where T : ITotallyOrdered
		{
			switch (other) {
				case Integer integer:
					return (int)this > (int)integer;
				case Fraction f:
					return this > (double)f;
				case Real r:
					return this > r;
			}

			throw new IncorrectSetException(GetType(), "compared", typeof(T));
		}

	    public override bool Equals<T>(T other)
		{
            switch (other) {
	            case Integer r:
		            return _value == r;
	            case int i:
		            return _value == i;
            }

			return false;
        }

	    #endregion

		/**
		 * Overrides from INumerical
		 */
		#region INumerical

		public INumerical Round()
	    {
		    return new Integer(_value);
	    }

	    public INumerical Log10()
	    {
		    return new Real(System.Math.Log10(_value));
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

	    public static bool operator <(Integer left, Integer right)
	    {
		    return left.LessThan(right);
	    }

	    public static bool operator >(Integer left, Integer right)
	    {
		    return left.GreaterThan(right);
	    }

	    public static bool operator <(Integer left, int right)
	    {
		    return (int) left < right;
	    }

	    public static bool operator >(Integer left, int right)
	    {
		    return (int)left > right;
	    }

	    public static bool operator <(int left, Integer right)
	    {
		    return left < (int)right;
	    }

	    public static bool operator >(int left, Integer right)
	    {
		    return left > (int)right;
	    }

		public static bool operator <(Integer left, Fraction right)
	    {
		    return left.LessThan(right);
	    }

	    public static bool operator >(Integer left, Fraction right)
	    {
		    return left.GreaterThan(right);
	    }

	    public static bool operator <(Fraction left, Integer right)
	    {
		    return left.LessThan(right);
	    }

	    public static bool operator >(Fraction left, Integer right)
	    {
		    return left.GreaterThan(right);
	    }

		#endregion

		#region Conversion

		public static implicit operator int(Integer r)
        {
	        if (r.Positive)
	        {
		        return r._value;
	        }

			return -r._value;
        }

        public static implicit operator Integer(int r)
        {
            return new Integer(r);
        }

        public override string ToString()
        {
	        throw new NotImplementedException();
        }

	    public string ToString(string format)
        {
			throw new NotImplementedException();
		}

		#endregion
	}
}