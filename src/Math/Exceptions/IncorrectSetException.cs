using System;

namespace Math.Exceptions
{
	class IncorrectSetException : Exception
	{
		public Type Left { get; set; }
		public Type Right { get; set; }

		/// <summary>
		/// For binary operators, when the two fields to be added don't match
		/// </summary>
		/// <param name="left"></param>
		/// <param name="operation"></param>
		/// <param name="right"></param>
		public IncorrectSetException(Type left, string operation, Type right) :
			base($@"A set member of type '{left.Name}' cannot be {operation} " +
						$@"to set members of type'{right.Name}'.")
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
		public IncorrectSetException(object field, string property, Type type) :
			base($@"A set of type '{field.GetType()}' does not have a '{property}' property of type '{type}'.")
		{
			Left = field.GetType();
			Right = type;
		}
	}
}
