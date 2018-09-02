using System;

namespace Math.Exceptions
{
	public class InvalidRowOperationException : Exception
	{
		public InvalidRowOperationType Type { get; set; }

		public InvalidRowOperationException(InvalidRowOperationType type)
		{
			Type = type;
		}
	}
}
