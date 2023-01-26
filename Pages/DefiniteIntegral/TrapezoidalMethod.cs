using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Programing_Labs.Pages.DefiniteIntegral
{
    class TrapezoidalMethod : IEnteredValues, IOutputValue
    {
        #region Properties
        #region IEnteredValues
        public double A { get; set; }
        public double B { get; set; }
        public double N { get; set; }
        public double E { get; set; }
        public Func<double, double> F { get; set; }
        public Func<IEnteredValues, List<Point>> GetFunctionCoordinates { get; set; }
        #endregion
        #region IOutputValue
        public List<Point> FunctionCoordinates => GetFunctionCoordinates(this);

        public List<Point> SplitCoordinates { get; set; }

        public double? OptimalSplitValue => Trapezoidal();


        #endregion
        #endregion
        #region Constructors
        public TrapezoidalMethod()
        {

        }
        public TrapezoidalMethod(IEnteredValues values)
        {
            A = values.A;
            B = values.B;
            E = values.E;
            N = values.N;
            F = values.F;
            GetFunctionCoordinates = values.GetFunctionCoordinates;
        }


        public void SetValues(IEnteredValues values)
        {
            A = values.A;
            B = values.B;
            E = values.E;
            N = values.N;
            F = values.F;
            GetFunctionCoordinates = values.GetFunctionCoordinates;
        }
        #endregion 


        private double? Trapezoidal()
        {
            SplitCoordinates = new List<Point>();
            /*            SplitCoordinates1 = new List<Point>();*/
            double h = (B - A) / N;
            double sum = 0d;
            double xk = 0d, yk = 0d;

            SplitCoordinates.Add(new Point(A, F(A)));

            for (double k = 1; k <= N - 2; k++)
            {
                xk = A + (k * h);

                yk = F(xk);
                sum += yk;
                SplitCoordinates.Add(new Point(xk, yk));

            }

            xk = A + ((N - 1) * h);

            SplitCoordinates.Add(new Point(xk, F(xk)));
            SplitCoordinates.Add(new Point(B, F(B)));

            return h / 2d * (F(A) + F(xk)) + (h * sum);

        }


    }
}
