using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra.Fields
{
	/// <summary>
	/// Represents a set of mathematical objects that obey certain properties
	/// </summary>
	/// <typeparam name="V"></typeparam>
	public abstract class Field<V>
	{
		public abstract FieldMember<V> Unit();

		public abstract FieldMember<V> Null();

		public abstract FieldMember<V> Create(object value);

		public bool Contains(FieldMember<object> m)
		{
			return m is V;
		}
	}
}
