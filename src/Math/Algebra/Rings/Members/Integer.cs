using System;
using System.Collections.Generic;
using System.Globalization;
using Math.Fields.Members;
using Math.Main;
using Math.Algebra.Fields.Members;
using Math.Exceptions;
using Math.Fields;
using Math.Groups;
using Math.Rationals;

namespace Math.Algebra.Rings.Members
{
    public class Integer : RingMember, INumerical, IFactorable
    {
        public int Value { get; set; }
        
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

        #region Overrides

        internal override T Add<T>(T other)
        {
            if (other is Integer r)
            {
                return (T)(GroupMember)new Integer(Value + r.Value);
            }
            throw new IncorrectFieldException(GetType(), "added", other.GetType());
        }

        public override T Negative<T>()
        {
            return (T)(RingMember)new Integer(-Value);
        }

        internal override T Multiply<T>(T other)
        {
            if (other is Integer c)
                return (T)(GroupMember)new Integer(Value * c.Value);
            throw new IncorrectFieldException(GetType(), "multiplied", other.GetType());
        }

        public override T Inverse<T>()
        {
            return (T)(IInvertible)new Integer(1 / Value);
        }

        public override T Null<T>()
        {
            if (typeof(T) == GetType())
                return (T)(GroupMember) new Integer(0);
            throw new IncorrectFieldException(this, "null", typeof(T));
        }

        public override T Unit<T>()
        {
            if (typeof(T) == GetType())
                return (T)(GroupMember)new Integer(1);
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
            return new Real(System.Math.Log10(Value));
        }

        public INumerical LongestValue()
        {
            return this;
        }

        public override bool Equals(FieldMember other)
        {
            if (other is Integer i)
            {
                return i.Value == Value;
            }

            return false;
        }

	    public bool TryFactor<T>(out T[] factors) where T : IFactorable
	    {
		    factors = null;

			if (typeof(T) == GetType())
		    {
				List<T> list = new List<T>();

			    for (int i = 0; i < this; i++)
			    {
				    if ((ToDouble() / i % 1).CloseTo(0))
				    {
						list.Add((T) (IFactorable) (Integer)i);
				    }
			    }

			    factors = list.ToArray();

				if (factors.Length != 0)
					return true;
		    }

		    throw new FactorTypeException();
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