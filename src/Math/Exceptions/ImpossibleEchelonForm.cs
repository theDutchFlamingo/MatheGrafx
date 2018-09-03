using System;

namespace Math.Exceptions
{
	/// <summary>
	/// Thrown when a matrix over some ring is 
	/// </summary>
	public class ImpossibleEchelonFormException : Exception
	{
		public const string Msg = "Inverse of one of the elements does not exist," +
		                          " so the matrix has no (reduced) echelon form.\n" +
		                          "Only if all inner elements have an inverse is the" +
		                          "(reduced) echelon form guaranteed to exist.";

		public ImpossibleEchelonFormException() : base(Msg)
		{

		}
	}
}
