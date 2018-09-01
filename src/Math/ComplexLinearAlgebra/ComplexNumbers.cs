using System;
using Math.Algebra.Fields;
using Math.Algebra.Fields.Members;

namespace Math.ComplexLinearAlgebra
{
    public class ComplexNumbers : Field<Complex>
    {
        public override Complex Create(params object[] value)
        {
            if (value == null || value.Length != 2)
                throw new ArgumentException("Exactly two parameters must be given to create a fraction.");
            if (value[0] is double num)
            {
                if (value[1] is double den)
                    return new Complex(num, den);
                return new Complex(num, 1);
            }
            throw new ArgumentException("The two parameters must be of integer type.");
        }

        public override Complex Null()
        {
            return new Complex();
        }

        public override Complex Unit()
        {
            return new Complex(1, 0);
        }
    }
}