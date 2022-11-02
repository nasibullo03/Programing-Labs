using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programing_Labs.LabsClases
{
    class LeastSquareMethod
    {

        private static double[] X { get; set; }
        private static double[] Y { get; set; }
        private static int Count { get; set; }
        private static void FillValues(List<GraphicPoint> graphicPoints)
        {
            Count = graphicPoints.Count;
            X = new double[Count];
            Y = new double[Count];
            int count = 0;
            graphicPoints.ForEach(el =>
            {
                X[count++] = el.Xi;
                Y[count++] = el.Yi;
            });
        }


        public static double Sum(double[] values)
        {
            double sum = 0;
            foreach (double value in values)
            {
                sum += value;
            }
            return sum; ;
        }

        public static void Clear()
        {
            X = null;
            Y = null;
            LinearFunction.Clear();
        }
        public class LinearFunction
        {
            private static double[] PowX2 { get; set; }
            private static double[] MultXY { get; set; }
            private static double[] Ylinear { get; set; }
            private static double[] D { get; set; }
            private static double[] PowD2 { get; set; }
            private static Matrix Matrix1 { get; set; }
            private static Matrix Matrix2 { get; set; }

            public static void FillBasicValues()
            {
                FillValues(GraphicPoint.GraphicPoints);
                PowX2 = new double[Count];
                MultXY = new double[Count];
                for (int i = 0; i < Count; ++i)
                {
                    PowX2[i] = Math.Pow(X[i], 2);
                    MultXY[i] = X[i] * Y[i];
                }

                Matrix1 = new Matrix(new double[,] {
                    { Sum(PowX2), Sum(X)},
                    { Sum(X),Count}
                });
                
            }

            public static void Clear()
            {
                PowX2 = null;
                MultXY = null;
                Ylinear = null;
                D = null;
                PowD2 = null;
            }




        }
        public class QuadraticFunction
        {

        }

        public class Matrix
        {
            public double[,] Value { get; set; }
            public Matrix()
            {

            }
            public Matrix(double[,] Value)
            {
                this.Value = Value;
            }





        }
    }
}
