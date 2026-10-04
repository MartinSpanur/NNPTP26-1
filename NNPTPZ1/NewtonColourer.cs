using NNPTPZ1.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NNPTPZ1
{
    internal static class NewtonColourer
    {

        // Helper array that helps to map number to color
        private static Color[] colorMap = new Color[]
        {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
        };

        public static void ColourBitmap(Bitmap bmp, ProgramArguments arguments, List<ComplexNumber> roots, Polynomial polynomial, Polynomial polynomialDerivative)
        {
            int maxRootId = 0;

            // Color for every pixel in image
            for (int i = 0; i < arguments.Height; i++)
            {
                for (int j = 0; j < arguments.Width; j++)
                {
                    // find "world" coordinates of pixel
                    double y = arguments.YMin + i * arguments.YStep;
                    double x = arguments.XMin + j * arguments.XStep;

                    int iterationCount, rootId;
                    maxRootId = SolvePoint(roots, polynomial, polynomialDerivative, maxRootId, y, x, out iterationCount, out rootId);

                    // colorize pixel according to root number
                    Color pixelColor = colorMap[rootId % colorMap.Length];
                    pixelColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    pixelColor = Color.FromArgb(
                        Math.Min(Math.Max(0, pixelColor.R - iterationCount * 2), 255),
                        Math.Min(Math.Max(0, pixelColor.G - iterationCount * 2), 255),
                        Math.Min(Math.Max(0, pixelColor.B - iterationCount * 2), 255)
                        );
                    bmp.SetPixel(j, i, pixelColor);
                }
            }
        }

        // Solves one point from newton fractal
        private static int SolvePoint(
            List<ComplexNumber> roots, 
            Polynomial polynomial, 
            Polynomial polynomialDerivative, 
            int maxRootId,
            double y, 
            double x, 
            out int iterationCount, 
            out int rootId)
        {
            const double OriginOffset = 0.0001;
            const int RequiredIterations = 30;
            const double DifferenceSquaredTolerance = 0.5;
            const double RootSquaredTolerance = 0.01;

            ComplexNumber currentPoint = new ComplexNumber()
            {
                Real = x,
                Imaginary = y
            };

            if (currentPoint.Real == 0)
            {
                currentPoint.Real = OriginOffset;
            }

            if (currentPoint.Imaginary == 0)
            {
                currentPoint.Imaginary = OriginOffset;
            }

            // find solution of equation using newton's iteration
            iterationCount = 0;
            for (int i = 0; i < RequiredIterations; i++)
            {
                ComplexNumber difference = polynomial.Eval(currentPoint).Divide(polynomialDerivative.Eval(currentPoint));
                currentPoint = currentPoint.Subtract(difference);

                if (Math.Pow(difference.Real, 2) + Math.Pow(difference.Imaginary, 2) >= DifferenceSquaredTolerance)
                {
                    i--;
                }
                iterationCount++;
            }

            // find solution root number
            bool known = false;
            rootId = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                if (Math.Pow(currentPoint.Real - roots[i].Real, 2) + Math.Pow(currentPoint.Imaginary - roots[i].Imaginary, 2) <= RootSquaredTolerance)
                {
                    known = true;
                    rootId = i;
                }
            }
            if (!known)
            {
                roots.Add(currentPoint);
                rootId = roots.Count;
                maxRootId = rootId + 1;
            }

            return maxRootId;
        }
    }
}