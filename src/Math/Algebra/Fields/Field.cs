using Math.Algebra.Fields.Members;
using Math.Algebra.Rings;
using Math.Rationals;

namespace Math.Algebra.Fields
{
	/// <summary>
	/// Represents a set of mathematical objects that obey certain properties
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Field<T> : Ring<T> where T : FieldMember
	{
		public static Field<Real> Reals = new RealNumbers();
		public static Field<Fraction> Fractions = new RationalNumbers();

		public bool Contains(T m)
		{
			return m?.GetType() == typeof(T);
		}
	}
}
