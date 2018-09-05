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
using static System.Int32;

namespace Math.Rationals
{
    public class Fraction : Rational<Integer>, INumerical, ITotallyOrdered, IParsable<Fraction>
    {
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

        public Fraction(Integer num, Integer den) : base(num, den)
        {
            
        }

        public Fraction(Fraction i) : this(i.Num, i.Den)
        {
            
        }

		#endregion

		#region Overrides

		internal override T1 Add<T1>(T1 other)
	    {
		    if (other is Fraction r)
		    {
			    return (T1)(MonoidMember)
				    new Fraction(r.Num.Multiply(Den).Add(r.Den.Multiply(Num)), r.Den.Multiply(Den));
		    }
		    throw new IncorrectSetException(GetType(), "added", other.GetType());
	    }

	    public override T1 Negative<T1>()
	    {
		    return (T1)(INegatable)new Fraction(-Num, Den);
	    }

	    internal override T1 Multiply<T1>(T1 other)
	    {
		    if (other is Fraction c)
			    return (T1)(GroupMember)new Fraction(Num.Multiply(c.Num), Den.Multiply(c.Den));
		    throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
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
		    if (other is Fraction f)
		    {
			    return (double) this > (double) f;
		    }

		    if (other is Integer i)
		    {
			    return (double) this > i;
		    }

		    return false;
	    }

	    public bool LessThan<T>(T other) where T : ITotallyOrdered
		{
		    if (other is Fraction f)
		    {
				return (double) this < (double) f;
		    }

		    if (other is Integer d)
		    {
			    return (double) this < d;
		    }

		    return false;
		}

	    public Fraction Parse(string value)
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
	        // ReSharper disable once PossibleLossOfFraction
	        return new Real(System.Math.Round((double) ((int) Num / (int) Den)));
        }

        public INumerical Log10()
        {
	        // ReSharper disable once PossibleLossOfFraction
	        return new Real(System.Math.Log10((int) Num / (int) Den));
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
        /// <param name="fieldMember"></param>
        /// <returns></returns>
        public static Fraction operator -(Fraction fieldMember)
        {
            return fieldMember.Negative<Fraction>();
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

		#endregion

		#region Conversion

		public static explicit operator double(Fraction r)
        {
            return (double)(int)r.Num / r.Den;
        }

        public static explicit operator Fraction(double r)
        {
            int whole = (int) (r > 0 ? System.Math.Floor(r) : System.Math.Ceiling(r));

            int den = 0, num = 0;
            
            return new Fraction(whole * den + num, den);
        }

	    public static implicit operator Fraction(int i)
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