using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Math.Exceptions
{
	public class IncompatibleRowOperationException : Exception
	{
		public IncompatibleRowOperationException(string message) : base(message)
		{

		}
	}
}
