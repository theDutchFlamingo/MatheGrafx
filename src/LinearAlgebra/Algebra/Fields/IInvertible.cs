using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra.Groups
{
	public interface IInvertible
	{
		T Inverse<T>() where T : IInvertible;
	}
}
