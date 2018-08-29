using System;
using Math.Algebra.Fields;
using Math.Main;
using Math.Algebra.Fields.Members;
using Math.Algebra.Groups;
using Math.Algebra.Groups.Members;
using Math.Exceptions;

namespace Math.Rationals
{
    public class FractionCopy : FieldMember, INumerical
    {
        public int Num { get; set; } = 0;
        public int Den { get; set; } = 1;
        
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

        #region Overrides

        internal override T Add<T>(T other)
        {
            if (other is FractionCopy r)
            {
                return (T)(GroupMember)new FractionCopy(r.Num * Den + r.Den * Num, r.Den * Den);
            }
            throw new IncorrectFieldException(GetType(), "added", other.GetType());
        }

        public override T Negative<T>()
        {
            return (T)(INegatable)new FractionCopy(-Num, Den);
        }

        internal override T Multiply<T>(T other)
        {
            if (other is FractionCopy c)
                return (T)(GroupMember)new FractionCopy(Num * c.Num, Den * c.Den);
            throw new IncorrectFieldException(GetType(), "multiplied", other.GetType());
        }

        public override T Inverse<T>()
        {
            return (T)(IInvertible)new FractionCopy(Den, Num);
        }

        public override T Null<T>()
        {
            if (typeof(T) == GetType())
                return (T)(GroupMember) new FractionCopy(0);
            throw new IncorrectFieldException(this, "null", typeof(T));
        }

        public override T Unit<T>()
        {
            if (typeof(T) == GetType())
                return (T)(GroupMember)new FractionCopy(1);
            throw new IncorrectFieldException(this, "unit", typeof(T));
        }

        public override bool IsNull() => Num.CloseTo(0);

        public override bool IsUnit() => Num.CloseTo(Den);

        public override double ToDouble()
        {
            return this;
        }

        public override bool Equals<T>(T other)
        {
            if (other is FractionCopy r)
                return ((double)Num / Den).CloseTo((double)r.Num / r.Den);
            return false;
        }

        public INumerical Round()
        {
            return new Real(System.Math.Round((double)Num / Den));
        }

        public INumerical Log10()
        {
            return new Real(System.Math.Log10((double)Num / Den));
        }

        public INumerical LongestValue()
        {
            return new Real(System.Math.Abs(Num) > System.Math.Abs(Den) ? Num : Den);
        }

        public override bool Equals(FieldMember other)
        {
            if (other is FractionCopy r)
            {
                return r.
            }
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
            return (double)r.Num / r.Den;
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