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
                X[count] = el.Xi;
                Y[count] = el.Yi;
                ++count;
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
            private static Matrix MatrixInverse { get; set; }
            
                
            public static void FillBasicValues(out double A,out double B, out double S)
            {
                FillValues(GraphicPoint.GraphicPoints);
                PowX2 = new double[Count];
                MultXY = new double[Count];
                Ylinear = new double[Count];
                D = new double[Count];
                PowD2 = new double[Count];

                for (int i = 0; i < Count; ++i)
                {
                    PowX2[i] = Math.Pow(X[i], 2);
                    MultXY[i] = X[i] * Y[i];
                }

                Matrix1 = new Matrix(new double[,] {
                    { Sum(PowX2), Sum(X)},
                    { Sum(X),Count}
                });
                MatrixInverse = new Matrix(Matrix.FindInverseMatrix(Matrix1.Value));

                Matrix matrixC = MatrixInverse * new Matrix(new double[,] { { Sum(MultXY) }, { Sum(Y) } });
                A = matrixC.Value[0, 0];
                B = matrixC.Value[1, 0];


                for (int i = 0; i < Count; ++i)
                {
                    Ylinear[i] = A * X[i] + B;
                    D[i] = Y[i] - Ylinear[i];
                    PowD2[i] = Math.Pow(D[i], 2);
                }
                S = Sum(PowD2);
                

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


    }
}
