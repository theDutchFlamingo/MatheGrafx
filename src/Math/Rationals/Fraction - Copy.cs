using System;
using System.Collections.Generic;
using System.Linq;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Groups;
using Math.Algebra.Groups.Members;
using Math.Algebra.Monoids.Members;
using Math.Algebra.Rings.Members;
using Math.Exceptions;

namespace Math.Rationals
{
    public class FractionCopy : FieldMember, INumerical
    {
        public Integer Num { get; set; } = 0;
        public Integer Den { get; set; } = 1;
        
        #region Constructors

        public FractionCopy()
        {
            
        }

        public FractionCopy(int numerator, int denominator)
        {
            if (denominator == 0) throw new DivideByZeroException();
            Num = denominator > 0 ? numerator : -numerator;
            Den = denominator > 0 ? denominator : -denominator;
        }

        public FractionCopy(FractionCopy i) : this(i.Num, i.Den)
        {
            
        }

		#endregion

	    /// <summary>
	    /// Check if the numerator and denominators match, without trying to factor
	    /// </summary>
	    /// <param name="other"></param>
	    /// <returns></returns>
	    public bool EqualsExactly(FractionCopy other)
	    {
		    return Num == other.Num && Den == other.Den;
	    }

		#region Overrides

		internal override T Add<T>(T other)
        {
            if (other is FractionCopy r)
            {
                return (T)(MonoidMember)new FractionCopy(r.Num * Den + r.Den * Num, r.Den * Den);
            }
            throw new IncorrectSetException(GetType(), "added", other.GetType());
        }

        public override T Negative<T>()
        {
            return (T)(INegatable)new FractionCopy(-Num, Den);
        }

        internal override T Multiply<T>(T other)
        {
            if (other is FractionCopy c)
                return (T)(GroupMember)new FractionCopy(Num * c.Num, Den * c.Den);
            throw new IncorrectSetException(GetType(), "multiplied", other.GetType());
        }

        public override T Inverse<T>()
        {
            return (T)(IInvertible)new FractionCopy(Den, Num);
        }

        public override T Null<T>()
        {
            if (typeof(T) == GetType())
                return (T)(MonoidMember) new FractionCopy(0);
            throw new IncorrectSetException(this, "null", typeof(T));
        }

        public override T Unit<T>()
        {
            if (typeof(T) == GetType())
                return (T)(GroupMember)new FractionCopy(1);
            throw new IncorrectSetException(this, "unit", typeof(T));
        }

        public override bool IsNull() => Num.Equals(0);

        public override bool IsUnit() => Num.Equals(Den);

        [Obsolete]
        public override double ToDouble()
        {
            return this;
        }

        public override bool Equals<T>(T other)
        {
            if (other is FractionCopy r)
                return Equals((MonoidMember) r);
            return false;
        }

        public INumerical Round()
        {
            return new Real(System.Math.Round((double)(int)Num / Den));
        }

        public INumerical Log10()
        {
            return new Real(System.Math.Log10((double)(int)Num / Den));
        }

        public INumerical LongestValue()
        {
            return new Real(System.Math.Abs(Num) > System.Math.Abs(Den) ? Num : Den);
        }

		#endregion

		#region Factoring

		public FractionCopy Factor()
		{
			List<Integer> matchedFactors = Num.Factors<Integer>().Intersect(Den.Factors<Integer>()).ToList();

			Integer num = Num;
			Integer den = Den;

			foreach (var t in matchedFactors)
			{
				num = num.Without(t);
				den = den.Without(t);
			}

			return new FractionCopy(num, den);
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
		public static FractionCopy operator +(FractionCopy left, FractionCopy right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Negative of this FieldMember
        /// </summary>
        /// <param name="fieldMember"></param>
        /// <returns></returns>
        public static FractionCopy operator -(FractionCopy fieldMember)
        {
            return fieldMember.Negative<FractionCopy>();
        }

        /// <summary>
        /// Subtract right from left
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FractionCopy operator -(FractionCopy left, FractionCopy right)
        {
            return left + -right;
        }

        /// <summary>
        /// Multiply the two together
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FractionCopy operator *(FractionCopy left, FractionCopy right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Divide one FieldMember&lt;V&gt; by another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FractionCopy operator /(FractionCopy left, FractionCopy right)
        {
            return left * right.Inverse<FractionCopy>();
        }

        #endregion

        #region Conversion

        public static implicit operator double(FractionCopy r)
        {
            return (double)(int)r.Num / r.Den;
        }

        public static implicit operator FractionCopy(double r)
        {
            int whole = (int) (r > 0 ? System.Math.Floor(r) : System.Math.Ceiling(r));

            int den = 0, num = 0;
            
            return new FractionCopy(whole * den + num, den);
        }

        public override string ToString()
        {
            return $"{Num}/{Den}";
        }

        public string ToString(string format)
        {
            return $"{Num.ToString(format)}/{Den.ToString(format)}";
        }

        #endregion
    }
}