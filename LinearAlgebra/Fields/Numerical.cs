using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra.Fields
{
	/// <summary>
	/// For classes that support a conversion to string with specified parameters, like double
	/// </summary>
	public interface INumerical
	{
		string ToString();

		string ToString(string format);
	}
}
