using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearAlgebra.Exceptions
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
