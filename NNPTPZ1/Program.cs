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

            ProgramArguments arguments = new ProgramArguments(args);
            Bitmap bmp = new Bitmap((int)arguments.Width, (int)arguments.Height); // "bmp" feels like conventional name so I keep it

            List<ComplexNumber> roots = new List<ComplexNumber>();

            // TODO: poly should be parameterised?
            Polynomial polynomial = new Polynomial();
            polynomial.Coefficients.Add(new ComplexNumber() { Real = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { Real = 1 });

            Polynomial ptmp = polynomial; // TODO unused temporary!!!
            Polynomial polynomialDerivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(polynomialDerivative);

            var colorMap = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            ColorBitmap(bmp, arguments, roots, polynomial, polynomialDerivative, colorMap);

            SaveOutput(arguments.Output, bmp);
        }

        private static void ColorBitmap(Bitmap bmp, ProgramArguments arguments, List<ComplexNumber> koreny, Polynomial polynomial, Polynomial polynomialDerivative, Color[] clrs)
        {
            var maxid = 0;

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < arguments.Height; i++)
            {
                for (int j = 0; j < arguments.Width; j++)
                {
                    // find "world" coordinates of pixel
                    double y = arguments.YMin + i * arguments.YStep;
                    double x = arguments.XMin + j * arguments.XStep;

                    ComplexNumber ox = new ComplexNumber()
                    {
                        Real = x,
                        Imaginary = y
                    };

                    if (ox.Real == 0)
                    {
                        ox.Real = 0.0001;
                    }

                    if (ox.Imaginary == 0)
                    {
                        ox.Imaginary = 0.0001f;
                    }

                    // find solution of equation using newton's iteration
                    int it = 0;
                    for (int q = 0; q < 30; q++)
                    {
                        ComplexNumber diff = polynomial.Eval(ox).Divide(polynomialDerivative.Eval(ox));
                        ox = ox.Subtract(diff);

                        if (Math.Pow(diff.Real, 2) + Math.Pow(diff.Imaginary, 2) >= 0.5)
                        {
                            q--;
                        }
                        it++;
                    }

                    // find solution root number
                    var known = false;
                    var id = 0;
                    for (int w = 0; w < koreny.Count; w++)
                    {
                        if (Math.Pow(ox.Real - koreny[w].Real, 2) + Math.Pow(ox.Imaginary - koreny[w].Imaginary, 2) <= 0.01)
                        {
                            known = true;
                            id = w;
                        }
                    }
                    if (!known)
                    {
                        koreny.Add(ox);
                        id = koreny.Count;
                        maxid = id + 1;
                    }

                    // colorize pixel according to root number
                    var vv = clrs[id % clrs.Length];
                    vv = Color.FromArgb(vv.R, vv.G, vv.B);
                    vv = Color.FromArgb(
                        Math.Min(Math.Max(0, vv.R - it * 2), 255),
                        Math.Min(Math.Max(0, vv.G - it * 2), 255),
                        Math.Min(Math.Max(0, vv.B - it * 2), 255)
                        );
                    bmp.SetPixel(j, i, vv);
                }
            }
        }

        private static void SaveOutput(string output, Bitmap bmp)
        {
            bmp.Save(output ?? "../../../out.png");
        }
    }
}
