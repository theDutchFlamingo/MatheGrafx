using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Fields;
using Math.Fields.Members;
using Math.Algebra.Fields.Members;

namespace Math.Exceptions
{
	class IncorrectFieldException : Exception
	{
		public Type Left { get; set; }
		public Type Right { get; set; }

		/// <summary>
		/// For binary operators, when the two fields to be added don't match
		/// </summary>
		/// <param name="left"></param>
		/// <param name="operation"></param>
		/// <param name="right"></param>
		public IncorrectFieldException(Type left, string operation, Type right) :
			base($@"A field member of type '{left.Name}' cannot be {operation} " +
						$@"to field members of type'{right.Name}'.")
		{
			Right = right;
			Left = left;
		}

		/// <summary>
		/// For unary operators, if the supplied generic type does not correspond to the type on which it is called
		/// </summary>
		/// <param name="field"></param>
		/// <param name="property"></param>
		/// <param name="type"></param>
		public IncorrectFieldException(FieldMember field, string property, Type type) :
			base($@"A field of type '{field.GetType()}' does not have a '{property}' property of type '{type}'.")
		{
			Left = field.GetType();
			Right = type;
		}
	}
}
