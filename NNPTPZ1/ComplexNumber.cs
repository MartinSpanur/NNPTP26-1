using System;

namespace NNPTPZ1
{

    namespace Mathematics
    {
        public class ComplexNumber
        {
            public double Real { get; set; }
            public double Imaginary { get; set; } // TODO: changed from float to double => why there was float?

            public override bool Equals(object obj)
            {
                if (obj is ComplexNumber)
                {
                    ComplexNumber x = obj as ComplexNumber;
                    return x.Real == Real && x.Imaginary == Imaginary; // TODO: comparison of floating point numbers with "=="? that doesnt seem right
                }
                return base.Equals(obj);
            }

            public readonly static ComplexNumber Zero = new ComplexNumber()
            {
                Real = 0,
                Imaginary = 0
            };

            public ComplexNumber Multiply(ComplexNumber otherNumber)
            {
                ComplexNumber thisNumber = this;
                // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
                return new ComplexNumber()
                {
                    Real = thisNumber.Real * otherNumber.Real - thisNumber.Imaginary * otherNumber.Imaginary,
                    Imaginary = thisNumber.Real * otherNumber.Imaginary + thisNumber.Imaginary * otherNumber.Real
                };
            }
            public double GetAbs()
            {
                return Math.Sqrt( Real * Real + Imaginary * Imaginary);
            }

            public ComplexNumber Add(ComplexNumber otherNumber)
            {
                ComplexNumber thisNumber = this;
                return new ComplexNumber()
                {
                    Real = thisNumber.Real + otherNumber.Real,
                    Imaginary = thisNumber.Imaginary + otherNumber.Imaginary
                };
            }
            public double GetAngleInRadians()
            {
                return Math.Atan(Imaginary / Real);
            }
            public ComplexNumber Subtract(ComplexNumber otherNumber)
            {
                ComplexNumber thisNumber = this;
                return new ComplexNumber()
                {
                    Real = thisNumber.Real - otherNumber.Real,
                    Imaginary = thisNumber.Imaginary - otherNumber.Imaginary
                };
            }

            public override string ToString()
            {
                return $"({Real} + {Imaginary}i)";
            }

            public ComplexNumber Divide(ComplexNumber otherNumber)
            {
                // (aRe + aIm*i) / (bRe + bIm*i)
                // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
                //  bRe*bRe - bIm*bIm*i*i
                ComplexNumber numerator = this.Multiply(new ComplexNumber() { Real = otherNumber.Real, Imaginary = -otherNumber.Imaginary });
                double denominator = otherNumber.Real * otherNumber.Real + otherNumber.Imaginary * otherNumber.Imaginary;

                return new ComplexNumber()
                {
                    Real = numerator.Real / denominator,
                    Imaginary = numerator.Imaginary / denominator
                };
            }
        }
    }
}
