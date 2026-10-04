namespace NNPTPZ1


{

    using System;
    using System.Globalization;

    public struct ProgramArguments
    {
        public double Width { get; }
        public double Height { get; }
        public double XMin { get; }
        public double XMax { get; }
        public double YMin { get; }
        public double YMax { get; }
        public string Output { get; }

        public double XStep => (XMax - XMin) / Width;
        public double YStep => (YMax - YMin) / Height;

        public ProgramArguments(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length != 7)
            {
                throw new ArgumentException(
                    $"Expected 7 arguments, but got {args.Length}. " +
                    "Expected: width height xmin xmax ymin ymax output",
                    nameof(args));
            }

            Width = ParseDouble(args[0], "width");
            Height = ParseDouble(args[1], "height");
            XMin = ParseDouble(args[2], "xmin");
            XMax = ParseDouble(args[3], "xmax");
            YMin = ParseDouble(args[4], "ymin");
            YMax = ParseDouble(args[5], "ymax");

            if (string.IsNullOrWhiteSpace(args[6]))
            {
                throw new ArgumentException(
                    "The output argument cannot be empty.",
                    nameof(args));
            }

            Output = args[6];
        }

        private static double ParseDouble(string value, string argumentName)
        {
            if (!double.TryParse(
                    value,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double result))
            {
                throw new FormatException(
                    $"Argument '{argumentName}' must be a valid number, but was '{value}'.");
            }

            return result;
        }
    }
}
