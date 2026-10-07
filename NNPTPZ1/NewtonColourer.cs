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

                    int iterationCount;
                    int rootId;
                    maxRootId = SolvePoint(roots, polynomial, polynomialDerivative, maxRootId, y, x, out iterationCount, out rootId);

                    // colorize pixel according to root number
                    Color pixelColor = colorMap[rootId % colorMap.Length];
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

            ComplexNumber currentPoint = CreateStartingPoint(x, y);

            currentPoint = PerformNewtonIteration(currentPoint, polynomial, polynomialDerivative, out iterationCount);

            // find solution root number
            maxRootId = FindOrAddRoot( roots, currentPoint, out rootId, maxRootId);

            return maxRootId;
        }

        // Create starting point for newton's method
        private static ComplexNumber CreateStartingPoint(double x, double y)
        {
            const double OriginOffset = 0.0001;

            if (x == 0)
            {
                x = OriginOffset;
            }
            
            if (y == 0)
            {
                y = OriginOffset;
            }
            
            return new ComplexNumber() { Real = x, Imaginary = y };
        }

        // Performs Newton's iteration and returns the resulting point
        private static ComplexNumber PerformNewtonIteration( ComplexNumber currentPoint, Polynomial polynomial, Polynomial polynomialDerivative, out int iterationCount)
        {
            const int RequiredIterations = 30;
            const double DifferenceSquaredTolerance = 0.5;
            iterationCount = 0;

            for (int i = 0; i < RequiredIterations; i++)
            {
                ComplexNumber difference = polynomial .Eval(currentPoint) .Divide(polynomialDerivative.Eval(currentPoint));
                currentPoint = currentPoint.Subtract(difference);
                double differenceSquared = difference.Real * difference.Real + difference.Imaginary * difference.Imaginary;
                if (differenceSquared >= DifferenceSquaredTolerance)
                {
                    i--;
                }
                iterationCount++;
            }
            
            return currentPoint;
        }

        // Finds solution root number
        private static int FindOrAddRoot( List<ComplexNumber> roots, ComplexNumber currentPoint, out int rootId, int maxRootId)
        {
            const double RootSquaredTolerance = 0.01;
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