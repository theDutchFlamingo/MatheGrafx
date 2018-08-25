using System;
using System.Globalization;
using LinearAlgebra.Exceptions;
using LinearAlgebra.Main;

namespace LinearAlgebra.Fields
{
    public class Integer : FieldMember, INumerical
    {
        public new int Value { get; set; } = 0;
        
        #region Constructors

        public Integer() : base(0)
        {
            
        }

        public Integer(int i) : base(i)
        {
            Value = i;
        }

        public Integer(Integer i) : base(i.Value)
        {
            Value = i.Value;
        }

        #endregion

        #region Overrides

        internal override T Add<T>(T other)
        {
            if (other is Integer r)
            {
                return (T)(FieldMember)new Integer(Value + r.Value);
            }
            throw new IncorrectFieldException(GetType(), "added", other.GetType());
        }

        internal override T Negative<T>()
        {
            return (T)(FieldMember)new Integer(-Value);
        }

        internal override T Multiply<T>(T other)
        {
            if (other is Integer c)
                return (T)(FieldMember)new Integer(Value * c.Value);
            throw new IncorrectFieldException(GetType(), "added", other.GetType());
        }

        internal override T Inverse<T>()
        {
            return (T)(FieldMember)new Integer(1 / Value);
        }

        public override T Null<T>()
        {
            if (typeof(T) == GetType())
                return (T)(FieldMember) new Integer(0);
            throw new IncorrectFieldException(this, "null", typeof(T));
        }

        public override T Unit<T>()
        {
            if (typeof(T) == GetType())
                return (T)(FieldMember)new Integer(1);
            throw new IncorrectFieldException(this, "unit", typeof(T));
        }

        public override bool IsNull() => Value.CloseTo(0);

        public override bool IsUnit() => Value.CloseTo(1);

        public override double ToDouble()
        {
            return this;
        }

        public override bool Equals<T>(T other)
        {
            if (other is Integer r)
                return Value.CloseTo(r);
            return false;
        }

        public INumerical Round()
        {
            return new Integer(Value);
        }

        public INumerical Log10()
        {
            return new Real(Math.Log10(Value));
        }

        public INumerical LongestValue()
        {
            return this;
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
        public static Integer operator +(Integer left, Integer right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Negative of this FieldMember
        /// </summary>
        /// <param name="fieldMember"></param>
        /// <returns></returns>
        public static Integer operator -(Integer fieldMember)
        {
            return fieldMember.Negative<Integer>();
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
        public static Integer operator /(Integer left, Integer right)
        {
            return left * right.Inverse<Integer>();
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