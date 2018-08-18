using System;
using LinearAlgebra.ComplexLinearAlgebra;
using LinearAlgebra.Fields;
using LinearAlgebra.Main;

namespace LinearAlgebra
{
    public static class Program
    {
	    static void Main()
        {
//	        Complexity();
			Test();
		}

	    private static void Test()
	    {
			Matrix m = new Matrix(4, 4)
			{
				Indices = new Real[,] { { 1, 1, 1, 1 }, { 1, 1, 1, 2 }, { 1, 1, 2, 3 }, { 1, 1, 3, 4} }
			};

		    m = m.Transpose();

			Console.WriteLine(m.Inverse().ToDeterminant(3, true));
	    }

	    private static void Complexity()
	    {
		    Complex result = Complex.I ^ (Complex.I + 3);

		    Complex x = ComplexMath.Exponential(1, Math.PI / 3);

			Console.WriteLine(x);

			Console.WriteLine(result);

			Console.WriteLine(1*(Math.Sin(Math.PI/3) * Complex.I + Math.Cos(Math.PI/3)));

			Console.Write("Input here: ");
		    if (Complex.TryParse(Console.ReadLine(), out var c))
		    {
			    Console.WriteLine();

				for (int i = 0; i < 10; i++) 
					Console.WriteLine(c ^ i);
		    }
	    }

	    private static void Fields()
	    {
			Real r = new Real(5);

			Console.WriteLine(r.ToDouble());
	    }

	    private static void LinAlg()
	    {
		    int[,] ints = new int[2, 5];

		    for (int i = 0; i < 2; i++)
		    {
			    for (int j = 0; j < 5; j++)
			    {
				    ints[i, j] = 2;
			    }
		    }

		    // A matrix with only 2's in it
		    Matrix m = new Matrix(ints);

		    Matrix n = new Matrix(5, 3)
		    {
			    Indices = new Real[,] { { 2, 3, 7 }, { 0, 0, 0 }, { 2, 4, 6 }, { -1, 4, 5 }, { 0, 8, 3 } }
		    };

		    Matrix p = m * n;

			Console.WriteLine(p[VectorType.Column]);

		    Matrix e = (Matrix) n.ToReducedEchelonForm();

		    Matrix u = (Matrix) (n * n.Transpose());

		    Console.WriteLine(e.IsReducedEchelon());
		    Console.WriteLine();
		    Console.WriteLine(u.ToDeterminant(3, true));
		    Console.WriteLine();
		    Console.WriteLine(u.Transpose().ToTable(3));
		    Console.WriteLine(u.IsSymmetric());
		    Console.WriteLine();
		    Console.WriteLine(e);
		    Console.WriteLine(n);
		    Console.WriteLine();
		    Console.WriteLine(LinearMath.NullMatrix(5, 9));
		}
    }
}
