using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Programing_Labs.Pages.DefiniteIntegral
{
    class SimpsonMethod : IEnteredValues, IOutputValue
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
        #region IoutputValues
        public List<Point> FunctionCoordinates => GetFunctionCoordinates(this);

        public List<Point> SplitCoordinates { get; private set; }
        public List<Point> ParabolsCoordinates => FindParabolsCoordinates();

        public double? OptimalSplitValue => Simpson();
        #endregion

        #endregion

        #region Constructors
        public SimpsonMethod()
        {

        }
        public SimpsonMethod(IEnteredValues values)
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


        private double Simpson()
        {
            SplitCoordinates = new List<Point>();
            double h = (B - A) / N;
            double sum1 = 0d;
            double sum2 = 0d;
            double xk = 0d, yk = 0d, xk_1 = 0d;

            SplitCoordinates.Add(new Point(A, F(A)));

            for (double k = 1; k <= N; k++)
            {
                xk = A + (k * h);

                if (k <= N - 1)
                {
                    yk = F(xk);
                    sum1 += yk;
                    SplitCoordinates.Add(new Point(xk, yk));
                }

                xk_1 = A + ((k - 1) * h);
                sum2 += F((xk + xk_1) / 2);
            }

            SplitCoordinates.Add(new Point(B, F(B)));

            double result = h / 3d * ((1d / 2d * F(A)) + sum1 + (2 * sum2) + (1d / 2d * F(B)));

            return result;
        }

        private List<Point> FindParabolsCoordinates()
        {
            double h = (B - A) / N;
            double xk = 0d;
            double xk1 = 0d;
            double xk2 = 0d;
            double y = 0d;
            double y1 = 0d;
            double y2 = 0d;

            List<double[]> FuncValues = new List<double[]>();
            List<double[]> FuncXCoordinates = new List<double[]>();

            List<Point> FuncsCoordinates = new List<Point>();

            double[] values;
            for (double k = 1; k <= N; k += 2)
            {
                values = new double[3];
                xk = A + (k * h);
                xk1 = A + ((k + 1) * h);
                xk2 = A + ((k + 2) * h);

                y = F(xk);
                y1 = F(xk1);
                y2 = F(xk2);

                values[0] = 0.5 * ((y - (2d * y1) + y2) / Math.Pow(h, 2d));
                values[1] = 0.5 * ((3d * y - (4d * y1) + y2) / h);
                values[2] = y;

                FuncValues.Add(values);
                FuncXCoordinates.Add(new double[] { xk, xk2 });
            }

            /*List<Point> points;*/
            
            for(int i = 0; i< FuncXCoordinates.Count;++i)
            {
               /* points = new List<Point>();*/

                for(double j = FuncXCoordinates[i][0]; j< FuncXCoordinates[i][1]; j+=E)
                {
                    FuncsCoordinates.Add(new Point()
                    {
                        X = j,
                        Y = FuncValues[i][0] * Math.Pow(j, 2) + (FuncValues[i][1] * j) + FuncValues[i][2]
                    });
                   /* System.Windows.Forms.MessageBox.Show($"a = {FuncValues[i][0]}  b= {FuncValues[i][1]} c = {FuncValues[i][2]}");*/

                }

                /*FuncsCoordinates.Add(points);*/
                
            }
            return FuncsCoordinates;
        }



    }
}
