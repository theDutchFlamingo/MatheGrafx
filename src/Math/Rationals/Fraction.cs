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
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;
using Math.Parsing;
using Math.Settings;
using static System.Int32;

namespace Math.Rationals
{
    public class Fraction : Rational<Integer>, INumerical, ITotallyOrdered, IParsable<Fraction>
    {
		/// <summary>
		/// The amount of digits that will be used when changing a fraction into a real number
		/// </summary>
	    public static Integer Digits = new Integer();

        #region Constructors

        public Fraction()
        {
            
        }

        public Fraction(int num) : base(num, 1)
        {
            
        }

        public Fraction(int num, int den) : base(num, den)
        {
            
        }

	    public Fraction(Natural num) : base(num, 1)
	    {

	    }

	    public Fraction(Natural num, Natural den) : base(num, den)
	    {

	    }

		public Fraction(Integer num) : base(num, 1)
	    {

	    }

        public Fraction(Integer num, Integer den) : base(num, den)
        {
            
        }

        public Fraction(Fraction i) : this(i.Num, i.Den)
        {
            
        }

		#endregion

		#region Overrides

	    public new Fraction Factor()
	    {
		    List<Integer> matchedFactors = Num.Factors<Integer>().Intersect(Den.Factors<Integer>()).ToList();

		    Integer num = Num;
		    Integer den = Den;

		    foreach (var t in matchedFactors)
		    {
			    num = num.Without(t);
			    den = den.Without(t);
		    }

		    return new Fraction(num, den);
	    }

		internal override T1 Add<T1>(T1 other)
	    {
			if (!(other is Fraction r))
		    {
			    throw new IncorrectSetException(GetType(), "added", other.GetType());
		    }

		    Integer num = r.Num.Multiply(Den).Add(r.Den.Multiply(Num));
		    Integer den = r.Den.Multiply(Den);

		    return (T1)(MonoidMember)
			    (NumberSettings.FactorFractions ? new Fraction(num, den).Factor() : new Fraction(num, den));
		}

	    public override T1 Negative<T1>()
	    {
		    return (T1)(INegatable)new Fraction(-Num, Den);
	    }

	    internal override T1 Multiply<T1>(T1 other)
	    {
		    if (!(other is Fraction r))
		    {
			    throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
		    }

		    Integer num = r.Num.Multiply(Num);
		    Integer den = r.Den.Multiply(Den);

		    return (T1)(GroupMember)
			    (NumberSettings.FactorFractions ? new Fraction(num, den).Factor() : new Fraction(num, den));

	    }

	    public override T1 Inverse<T1>()
	    {
		    return (T1)(IInvertible)new Fraction(Den, Num);
	    }

	    public override T1 Null<T1>()
	    {
		    if (typeof(T1) == GetType())
		    {
			    return (T1)(MonoidMember)new Fraction(0);
		    }
		    throw new IncorrectSetException(this, "null", typeof(T1));
	    }

	    public override T1 Unit<T1>()
	    {
		    if (typeof(T1) == GetType())
		    {
			    return (T1)(GroupMember)new Fraction(1);
		    }
		    throw new IncorrectSetException(this, "unit", typeof(T1));
	    }

	    public override bool IsNull() => Num.Equals((Integer)0);

	    public override bool IsUnit() => Num.Equals(Den);

	    public bool GreaterThan<T>(T other) where T : ITotallyOrdered
	    {
		    switch (other) {
			    case Fraction f:
				    return Num * f.Den > f.Num * Den;
			    case Integer i:
				    return Num > i * Den;
		    }

		    return false;
	    }

	    public bool LessThan<T>(T other) where T : ITotallyOrdered
		{
		    switch (other) {
			    case Fraction f:
				    return Num * f.Den < f.Num * Den;
			    case Integer d:
				    return Num < d * Den;
		    }

			return false;
		}

	    public Fraction FromString(string value)
	    {
		    TryParse(Regex.Match(value, @"(\d+)/").Groups[1].Value, out int num);
		    if (!TryParse(Regex.Match(value, @"/(\d+)").Groups[1].Value, out int den))
		    {
			    den = 1;
		    }

		    return new Fraction(num, den);
	    }

		public INumerical Round()
        {
			// TODO implement better
	        if (GreaterThan(new Integer(0)))
	        {
		        if (LessThan((Integer) 1 / 2))
		        {
			        return new Integer(0);
				}

		        
	        }
	        else
	        {
				if (GreaterThan((Integer)(-1) / 2))
		        {
					return new Integer(0);
		        }

		        if (LessThan((Integer) 1))
		        {

		        }
	        }

			int i = (int)Num / (int)Den;
			return new Integer((int)System.Math.Round((double)i));
        }

        public INumerical Log10()
        {
	        return new Real(System.Math.Log10((double)(int) Num / Den));
        }

        public INumerical LongestValue()
        {
            return new Real(System.Math.Abs(Num) > System.Math.Abs(Den) ? Num : Den);
        }

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
        public static Fraction operator +(Fraction left, Fraction right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Negative of this FieldMember
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Fraction operator -(Fraction f)
        {
            return f.Negative<Fraction>();
        }

        /// <summary>
        /// Subtract right from left
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator -(Fraction left, Fraction right)
        {
            return left + -right;
        }

        /// <summary>
        /// Multiply the two together
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator *(Fraction left, Fraction right)
        {
            return left.Multiply(right);
        }

	    /// <summary>
	    /// Multiply the two together
	    /// </summary>
	    /// <param name="left"></param>
	    /// <param name="right"></param>
	    /// <returns></returns>
	    public static Fraction operator *(Integer left, Fraction right)
	    {
		    return new Fraction(left * right.Num, right.Den);
	    }

	    /// <summary>
	    /// Multiply the two together
	    /// </summary>
	    /// <param name="left"></param>
	    /// <param name="right"></param>
	    /// <returns></returns>
	    public static Fraction operator *(Fraction left, Integer right)
	    {
		    return right * left;
	    }

		/// <summary>
		/// Divide one Fraction by another
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Fraction operator /(Fraction left, Fraction right)
        {
            return left * right.Inverse<Fraction>();
        }

	    /// <summary>
	    /// Divide one Fraction by another
	    /// </summary>
	    /// <param name="left"></param>
	    /// <param name="right"></param>
	    /// <returns></returns>
	    public static Fraction operator /(Fraction left, Integer right)
	    {
		    return left * (1 / right);
	    }

	    public static Fraction operator /(Integer left, Fraction right)
	    {
		    return left * ((Fraction) 1 / right);
	    }

	    public static bool operator ==(Fraction left, Fraction right)
	    {
		    return left?.Equals(right) ?? right?.Den == null;
	    }
	    
	    public static bool operator !=(Fraction left, Fraction right)
	    {
		    return !(left?.Equals(right) ?? right == null);
	    }

		#endregion

		#region Conversion

	    public static explicit operator Real(Fraction f)
	    {
		    throw new NotImplementedException();
	    }

		public static explicit operator double(Fraction f)
        {
            return (double)(int)f.Num / f.Den;
        }

        public static explicit operator Fraction(double r)
        {
            int whole = (int) (r > 0 ? System.Math.Floor(r) : System.Math.Ceiling(r));

            int den = 0, num = 0;

			// TODO finish this
            
            return new Fraction(whole * den + num, den);
        }

	    public static explicit operator Fraction(int i)
	    {
			return new Fraction(i);
	    }

	    public override string ToString()
        {
            return Num == 0 || Den == 1 ? $"{Num}" : $"{Num}/{Den}";
        }

        public string ToString(string format)
        {
            return $"{Num.ToString(format)}/{Den.ToString(format)}";
        }

        #endregion
    }
}