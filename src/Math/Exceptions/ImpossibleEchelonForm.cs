using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Exceptions
{
	/// <summary>
	/// Thrown when a matrix over some ring is 
	/// </summary>
	public class ImpossibleEchelonFormException : Exception
	{
		public ImpossibleEchelonFormException(string message) : base(message)
		{

		}
	}
}
