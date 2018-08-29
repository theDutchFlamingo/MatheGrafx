using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra.Fields.Members;
using LinearAlgebra.Rationals;

namespace LinearAlgebra.Fields
{
	/// <summary>
	/// Represents a set of mathematical objects that obey certain properties
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Field<T> where T : FieldMember
	{
		public static Field<Real> Reals = new RealNumbers();
		public static Field<Fraction> Fractions = new 

		public abstract T Unit();

		public abstract T Null();

		public abstract T Create(object value);

		public bool Contains(T m)
		{
			return m?.GetType() == typeof(T);
		}
	}
}
