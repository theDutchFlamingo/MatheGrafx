using System;
using System.Globalization;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Main;
using Microsoft.SqlServer.Server;

namespace LinearAlgebra.Fields
{
    public class Rational : FieldMember, INumerical
    {
        public int Num { get; set; } = 0;
        public int Den { get; set; } = 1;
        
        #region Constructors

        public Rational() : base(0)
        {
            
        }

        public Rational(int numerator, int denominator) : base(numerator/(double)denominator)
        {
            if (denominator == 0) throw new DivideByZeroException();
            Num = denominator > 0 ? numerator : -numerator;
            Den = denominator > 0 ? denominator : -denominator;
        }

        public Rational(Rational i) : this(i.Num, i.Den)
        {
            
        }

        #endregion

        #region Overrides

        internal override T Add<T>(T other)
        {
            if (other is Rational r)
            {
                return (T)(FieldMember)new Rational(r.Num * Den + r.Den * Num, r.Den * Den);
            }
            throw new IncorrectFieldException(GetType(), "added", other.GetType());
        }

        internal override T Negative<T>()
        {
            return (T)(FieldMember)new Rational(-Num, Den);
        }

        internal override T Multiply<T>(T other)
        {
            if (other is Rational c)
                return (T)(FieldMember)new Rational(Num * c.Num, Den * c.Den);
            throw new IncorrectFieldException(GetType(), "added", other.GetType());
        }

        internal override T Inverse<T>()
        {
            return (T)(FieldMember)new Rational(Den, Num);
        }

        public override T Null<T>()
        {
            if (typeof(T) == GetType())
                return (T)(FieldMember) new Rational(0);
            throw new IncorrectFieldException(this, "null", typeof(T));
        }

        public override T Unit<T>()
        {
            if (typeof(T) == GetType())
                return (T)(FieldMember)new Rational(1);
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
            if (other is Rational r)
                return ((double)Num / Den).CloseTo((double)r.Num / r.Den);
            return false;
        }

        public INumerical Round()
        {
            return new Real(Math.Round((double)Num / Den));
        }

        public INumerical Log10()
        {
            return new Real(Math.Log10((double)Num / Den));
        }

        public INumerical LongestValue()
        {
            return new Real(Math.Abs(Num) > Math.Abs(Den) ? Num : Den);
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
        public static Rational operator +(Rational left, Rational right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Negative of this FieldMember
        /// </summary>
        /// <param name="fieldMember"></param>
        /// <returns></returns>
        public static Rational operator -(Rational fieldMember)
        {
            return fieldMember.Negative<Rational>();
        }

        /// <summary>
        /// Subtract right from left
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Rational operator -(Rational left, Rational right)
        {
            return left + -right;
        }

        /// <summary>
        /// Multiply the two together
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Rational operator *(Rational left, Rational right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Divide one FieldMember&lt;V&gt; by another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Rational operator /(Rational left, Rational right)
        {
            return left * right.Inverse<Rational>();
        }

        #endregion

        #region Conversion

        public static implicit operator double(Rational r)
        {
            return (double)r.Num / r.Den;
        }

        public static implicit operator Rational(double r)
        {
            int whole = (int) (r > 0 ? Math.Floor(r) : Math.Ceiling(r));

            int den = 0, num = 0;
            
            return new Rational(whole * den + num, den);
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