using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;
using Math.Algebra.Rings.Members;

namespace Math.Rationals
{
    public class Fraction : Rational<Integer>, INumerical
    {
        #region Constructors

        public Fraction()
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
        /// Divide one FieldMember&lt;V&gt; by another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction left, Fraction right)
        {
            return left * right.Inverse<Fraction>();
        }

        #endregion

        #region Conversion

        public static implicit operator double(Fraction r)
        {
            return (double)(int)r.Num / r.Den;
        }

        public static implicit operator Fraction(double r)
        {
            int whole = (int) (r > 0 ? System.Math.Floor(r) : System.Math.Ceiling(r));

            int den = 0, num = 0;
            
            return new Fraction(whole * den + num, den);
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