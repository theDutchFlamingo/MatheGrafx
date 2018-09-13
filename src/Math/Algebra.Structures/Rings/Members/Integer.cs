using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Groups;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Ordering;
using Math.Exceptions;
using Math.NumberTheory;
using Math.Rationals;

namespace Math.Algebra.Structures.Rings.Members
{
    public class Integer : RingMember, INumerical, IFactorable, ITotallyOrdered
    {
	    private bool Positive { get; } = true;

		public Natural Absolute { get; } = new Natural();

		public long Value => Positive ? Absolute : -Absolute;

	    #region Constructors

        public Integer()
        {
            
        }

        public Integer(int i)
        {
            Absolute = (uint)System.Math.Abs(i);
	        Positive = i >= 0;
        }

        public Integer(Integer i)
        {
            Absolute = i.Absolute;
	        Positive = i.Positive;
        }

	    public Integer(Natural n) : this(n, true)
	    {
		    
	    }

	    public Integer(Natural n, bool positive)
	    {
			Absolute = n;
		    Positive = positive;
	    }

	    private Integer(byte[] value)
	    {
			Absolute = new Natural(value);
	    }

	    public Integer(string value, bool hex = true)
	    {
		    if (value.StartsWith("-"))
		    {
			    Positive = false;
			    value = value.Substring(1);
		    }

		    Absolute = new Natural(value, hex);
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
			switch (other) {
				case Integer r:
					if (Positive && r.Positive ||
					    !Positive && !r.Positive)
					{
						return (T) (MonoidMember) new Integer(Absolute + r.Absolute, Positive);
					}

					return (T) (MonoidMember) (Positive ? Absolute - r.Absolute : r.Absolute - Absolute);
			}

			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

        public override T Negative<T>()
        {
            return (T)(INegatable)new Integer(Absolute, !Positive);
        }

        internal override T Multiply<T>(T other)
        {
	        if (other is Integer c)
	        {
		        return (T)(GroupMember)new Integer(Absolute * c.Absolute);
	        }

	        throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
        }

        public override T Null<T>()
        {
	        if (typeof(T) == GetType())
	        {
		        return (T)(MonoidMember) new Integer(0);
	        }

	        throw new IncorrectSetException(this, "null", typeof(T));
        }

        public override T Unit<T>()
        {
	        if (typeof(T) == GetType())
	        {
		        return (T)(GroupMember)new Integer(1);
	        }

	        throw new IncorrectSetException(this, "unit", typeof(T));
        }

        public override bool IsNull() => Absolute.Equals(0);

        public override bool IsUnit() => Absolute.Equals(1);

	    [Obsolete]
        public override double ToDouble()
        {
            return this;
        }

	    public bool LessThan<T>(T other) where T : ITotallyOrdered
	    {
		    switch (other) {
				case Natural n:
					return !Positive || Absolute.LessThan(n);
			    case Integer i:
				    if (i.Positive && Positive)
				    {
					    return i.Absolute > Absolute;
				    }

				    if (i.Positive && !Positive)
				    {
					    return true;
				    }

				    if (!i.Positive && Positive)
				    {
					    return false;
				    }

				    return i.Absolute < Absolute;
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
				case Natural n:
					return Positive && n.LessThan(Absolute);
				case Integer i:
					if (i.Positive && Positive)
					{
						return i.Absolute < Absolute;
					}

					if (i.Positive && !Positive)
					{
						return true;
					}

					if (!i.Positive && Positive)
					{
						return false;
					}

					// Else: both are negative
					return i.Absolute > Absolute;
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
		            return Absolute == r;
	            case int i:
		            return Absolute == i;
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
		    return new Integer(Absolute);
	    }

	    public INumerical Log10()
	    {
		    return new Real(System.Math.Log10(Absolute));
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
        /// <param name="integer"></param>
        /// <returns></returns>
        public static Integer operator -(Integer integer)
        {
            return integer.Negative<Integer>();
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
	    /// Subtract right from left
	    /// </summary>
	    /// <param name="left"></param>
	    /// <param name="right"></param>
	    /// <returns></returns>
	    public static Integer operator -(Natural left, Integer right)
	    {
		    return left + -right;
	    }

	    /// <summary>
	    /// Subtract right from left
	    /// </summary>
	    /// <param name="left"></param>
	    /// <param name="right"></param>
	    /// <returns></returns>
	    public static Integer operator -(Integer left, Natural right)
	    {
		    return left + -(Integer)right;
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

	    public static Integer operator %(Integer left, Integer right)
	    {
		    return (Integer) (left / right).Round();
	    }

	    public static bool operator <(Integer left, Integer right)
	    {
		    return left.LessThan(right);
	    }

	    public static bool operator >(Integer left, Integer right)
	    {
		    return left.GreaterThan(right);
	    }

	    public static bool operator <(Integer left, Natural right)
	    {
		    return left.LessThan(right);
	    }

	    public static bool operator >(Integer left, Natural right)
	    {
		    return left.GreaterThan(right);
	    }

	    public static bool operator <(Natural left, Integer right)
	    {
		    return left.LessThan(right);
	    }

	    public static bool operator >(Natural left, Integer right)
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

	    public byte[] GetBytes()
	    {
		    return Absolute.GetBytes();
	    }

		public static implicit operator int(Integer r)
		{
			return r.Positive ? r.Absolute : -(int) r.Absolute;
		}

        public static implicit operator Integer(int r)
        {
            return new Integer(r);
        }

        public override string ToString()
        {
	        return (Positive ? "" : "-") + Absolute;
        }

	    public string ToString(string format)
        {
	        return (Positive ? "" : "-") + Absolute.ToString(format);
		}

		#endregion

	    #region Static

	    public static Integer Pow(Integer b, Natural exp)
	    {
		    Integer result = 1;

		    for (Integer i = 0; i < exp; i++)
		    {
			    result *= b;
		    }

		    return result;
	    }

	    public static Integer Min(Integer left, Integer right)
	    {
		    return left > right ? right : left;
	    }

	    public static Integer Max(Integer left, Integer right)
	    {
		    return left > right ? left : right;
	    }

	    public static Integer Parse(string value, bool positive = true, bool hex = false)
	    {
		    if (!hex && !Regex.IsMatch(value, @"^-?[0-9]+$") ||
		        hex && !Regex.IsMatch(value, @"^-?[0-9a-fA-F]+$"))
		    {
			    throw new ArgumentException($"String was not in correct format: {value}.");
		    }
			
		    if (value.StartsWith("-"))
		    {
			    positive = false;
			    value = value.Substring(1);
		    }

			return new Integer(Natural.Parse(value), positive);
	    }

	    public static bool TryParse(string value, out Integer result, bool positive = true, bool hex = false)
	    {
		    try
		    {
			    result = Parse(value, positive, hex);
			    return true;
		    }
		    catch (FormatException)
		    {
			    result = 0;
			    return false;
		    }
	    }

	    #endregion
	}
}