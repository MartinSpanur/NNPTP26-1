using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Threading;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{

    using System;


    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Digest arguments and prepare bitmap
            ProgramArguments arguments = new ProgramArguments(args);
            Bitmap bmp = new Bitmap((int)arguments.Width, (int)arguments.Height); // "bmp" feels like conventional name so I keep iterationCount

            // Creation of polynomial for creation of newton fractal
            Polynomial polynomial = new Polynomial();
            polynomial.Coefficients.Add(new ComplexNumber() { Real = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { Real = 1 });

            Polynomial polynomialDerivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(polynomialDerivative);

            List<ComplexNumber> roots = new List<ComplexNumber>();

            // Colouring the bitmap
            NewtonColourer.ColourBitmap(bmp, arguments, roots, polynomial, polynomialDerivative);

            SaveOutput(arguments.Output, bmp);
        }

        private static void SaveOutput(string output, Bitmap bmp)
        {
            bmp.Save(output ?? "../../../out.png");
        }
    }
}
