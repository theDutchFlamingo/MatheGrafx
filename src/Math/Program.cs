using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Math.Algebra.Expressions.Definitions;
using Math.Algebra.Structures.Fields;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Bytes;
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
			//Complexity();
			//Test();
			//LinAlg();
			//ComplexLinAlg();
			//BasicTest();
			//Polynomials();
//	        Pascal();

			byte[] l = {0, 0, 0,1, 255, 255};
	        byte[] r = {0, 0, 0, 254, 255, 255};

	        byte[] sum = l.Add(r);

	        Console.WriteLine(String.Join(", ", sum));
        }

	    public static void Latex()
	    {
		    Matrix<Matrix<Real>> m = new Matrix<Matrix<Real>>
		    {
			    Indices = new[,]
			    {
				    {
					    new Matrix<Real>
					    {
						    Indices = new Real[,] { {1, 3}, {0, 1} }
					    },
					    new Matrix<Real>
					    {
						    Indices = new Real[,] { {0, 0}, {0, 0} }
					    }
				    },
				    {
					    new Matrix<Real>
					    {
						    Indices = new Real[,] { {1, 2}, {1, 3} }
					    },
					    new Matrix<Real>
					    {
						    Indices = new Real[,] { {1, 0}, {0, 1} }
					    }
				    }
			    }
		    };

		    Console.WriteLine(m.Inverse().ToLatex(mat => mat.ToLatex('[')));
		    Console.WriteLine();

			Console.WriteLine(m.ToLatex('[', mat => mat.ToLatex()));

			Console.WriteLine();
		    Console.WriteLine(m.ToReducedEchelonForm(() => new Matrix<Real>(2, 2), 1).
			    ToLatex(mat => mat.ToLatex()));
	    }

		public static void Polynomials()
	    {
			IntegerPolynomial p = new IntegerPolynomial(1,0,0,0,0,0,0,0,1);
			Polynomial<Real> p2 = new Polynomial<Real>(0, 0, -1, 1, 5, -2);
			Polynomial<Fraction> f = new Polynomial<Fraction>(
				(Integer)1/2,
				(Integer)2/3,
				(Integer)3/4,
				(Integer)4/5,
				(Integer)5/6,
				(Integer)6/7,
				(Integer)7/8				
				);

			Console.WriteLine(new Fraction().Parse("12/5/20/1"));

			Console.WriteLine(f^20);
		}

	    public static void Pascal()
	    {
		    int n = 20;
		    
		    IntegerPolynomial p = new IntegerPolynomial(1, 1);
			IntegerPolynomial curr = new IntegerPolynomial(1);

		    string total = "";

		    int length = String.Join(" ", (p ^ (n - 1)).Coefficients.Select(d => d.ToString())).Length; 

		    for (int i = 0; i < n; i++)
		    {
			    string line = String.Join(" ", (p ^ i).Coefficients.Select(d => d.ToString()));
			    
			    total += line.PadCenter(length) + "\n";
			    
			    curr = curr * p;
		    }
		    
		    Console.WriteLine(total);
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
				Indices = new Real[,] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 2 }, { 1, 1, 1, 1, 2, 3 },
					{ 1, 1, 1, 2, 3, 4}, {1, 1, 2, 3, 4, 5}, {1, 2, 3, 4, 5, 6} }
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

			Console.WriteLine(1*(System.Math.Sin(System.Math.PI/3) * Complex.I +
				System.Math.Cos(System.Math.PI/3)));

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
		}
	    
	    public static string PadCenter(this string source, int length)
	    {
		    int spaces = length - source.Length;
		    int padLeft = spaces/2 + source.Length;
		    return source.PadLeft(padLeft).PadRight(length);

	    }
    }
}
