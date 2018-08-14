using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearAlgebra.Exceptions
{
	public class IncompatibleRowOperationException : Exception
	{
		public IncompatibleRowOperationException(string message) : base(message)
		{

		}
	}
}
