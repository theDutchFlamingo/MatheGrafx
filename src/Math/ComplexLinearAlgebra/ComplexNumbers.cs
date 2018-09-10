using System;
using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Rationals;

namespace Math.ComplexLinearAlgebra
{
    public class ComplexNumbers : Field<Complex>
    {
        public override Complex Create(params object[] value)
        {
	        if (value == null || value.Length != 1 && value.Length != 2)
	        {
		        throw new ArgumentException("Exactly two parameters must be given to create a fraction.");
	        }

	        if (!(value[0] is double num))
	        {
		        throw new ArgumentException("The two parameters must be of integer type.");
	        }

	        if (value[1] is double den)
	        {
		        return new Complex(num, den);
	        }

	        return new Complex(num, 1);
        }

        public override Complex Null()
        {
            return new Complex();
        }

        public override Complex Unit()
        {
            return new Complex(1, 0);
        }

	    public override bool Contains<T>(T element)
	    {
			switch (element)
		    {
				case Complex _:
			    case Real _:
			    case Fraction _:
			    case Integer _:
			    case Natural _:
				    return true;
		    }

		    return false;
		}
    }
}