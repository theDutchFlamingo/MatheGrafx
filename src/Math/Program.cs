using System;
using Math.Algebra.Expressions.Definitions;
using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.ComplexLinearAlgebra;
using Math.Latex;
using Math.LinearAlgebra;
using Math.Polynomials;
using Math.Rationals;

namespace Math
{
    public static class Program
    {
	    private static readonly Complex i = Complex.I;

	    static void Main()
        {
			//			Complexity();
			//			Test();
			//			LinAlg();
			//			ComplexLinAlg();
			//BasicTest();
			Polynomials();

			Console.WriteLine("abc".SetValue(new Real(123)));

			Console.WriteLine("abc".SetValue(12345));

	        Console.WriteLine(Quantities.Constants["pi"]);
		}

		public static void Polynomials()
	    {
			IntegerPolynomial p = new IntegerPolynomial(new Vector<Integer>(new Integer[] { 1, -1 }));

			Console.WriteLine((p ^ 2));
		}
	    
	    private static void ComplexLinAlg()
	    {
			ComplexMatrix c = new ComplexMatrix(3, 3)
			{
				Indices = new [,] {
					{ 1 + i, 1 + i, 1 + i, 1 + i },
					{ i, 2*i, 2*i + 3, 2 - 4*i },
					{ 1 + i, 1 - 2*i, 2 + i, 3 + 5*i },
					{ 1 + 3*i, 2 - 2*i, 3 + 5*i, 4 }
				}
			};

			Console.WriteLine(c.ToDeterminant(3, true, 3));
		    Console.WriteLine();
		    Console.WriteLine(c.Inverse().ToTable(3, 3, true));
	    }

	    private static void Test()
	    {
			RealMatrix m = new RealMatrix(4, 4)
			{
				Indices = new Real[,] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 2 }, { 1, 1, 1, 1, 2, 3 }, { 1, 1, 1, 2, 3, 4}, {1, 1, 2, 3, 4, 5}, {1, 2, 3, 4, 5, 6} }
			};

//		    m[0, VectorType.Row] = m[1, VectorType.Row];

			Console.WriteLine(m.ToDeterminant(3, true));
	    }

	    private static void BasicTest()
	    {
		    RealMatrix m1 = new RealMatrix(5, 3)
		    {
			    Indices = new Real[,] { { 2, 3, 7 }, { 0, 0, 0 }, { 2, 4, 6 }, { -1, 4, 5 }, { 0, 8, 3 } }
		    };

		    RealMatrix m2 = new RealMatrix(4, 4)
		    {
			    Indices = new Real[,] { { 1, 1, 1, 1 }, { 1, 1, 1, 2 }, { 1, 1, 2, 3 }, { 1, 2, 3, 4 } }
		    };

		    Console.WriteLine(m1.ToTable(3));
		    Console.WriteLine();
		    Console.WriteLine(m1.Transpose().ToTable(3));
		    Console.WriteLine();
		    Console.WriteLine((m1 * m1.Transpose()).ToDeterminant(3, true));
		    Console.WriteLine();
		    Console.WriteLine(m2.ToDeterminant(3, true));
	    }

	    private static void Complexity()
	    {
		    Complex result = Complex.I ^ (Complex.I + 3);

		    Complex x = ComplexMath.Exponential(1, System.Math.PI / 3);

			Console.WriteLine(x);

			Console.WriteLine(result);

			Console.WriteLine(1*(System.Math.Sin(System.Math.PI/3) * Complex.I + System.Math.Cos(System.Math.PI/3)));

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

			Console.WriteLine(r);
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
		    RealMatrix m = new RealMatrix(ints);

		    RealMatrix n = new RealMatrix(5, 3)
		    {
			    Indices = new Real[,] { { 2, 3, 7 }, { 0, 0, 0 }, { 2, 4, 6 }, { -1, 4, 5 }, { 0, 8, 3 } }
		    };

		    RealMatrix p = m * n;

			Console.WriteLine(p[VectorType.Column]);

		    RealMatrix e = n.ToReducedEchelonForm().ToRealMatrix();

		    RealMatrix u = n * n.Transpose().ToRealMatrix();

		    Console.WriteLine(e.IsReducedEchelon());
		    Console.WriteLine();
		    Console.WriteLine(u.ToDeterminant(3, true));
		    Console.WriteLine();
		    Console.WriteLine(u.Transpose().ToTable(3));
		    Console.WriteLine(u.IsSymmetric());
		    Console.WriteLine();
		    Console.WriteLine("The matrix e, in all its glory: \n" + e.ToTable(3));
		    Console.WriteLine(n);
		    Console.WriteLine();
		    Console.WriteLine(LinearMath.NullMatrix(5, 9).ToDeterminant(3));
		}
    }
}
