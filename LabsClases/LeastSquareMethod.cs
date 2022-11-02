using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programing_Labs.LabsClases
{
    class LeastSquareMethod
    {

        public static double[] X { get; private set; }
        public static double[] Y { get; private set; }
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

       
        public class LinearFunction
        {
            public double[]XValue { get; private set; }
            public double[]YValue { get; private set; }
            public  double[] PowX2 { get; private set; }
            public  double[] MultXY { get; private set; }
            public  double[] Ylinear { get; private set; }
            public  double[] D { get; private set; }
            public  double A { get; private set; }
            public  double B { get; private set; }
            public  double S { get; private set; }
            public  double[] PowD2 { get; private  set; }
            public  Matrix Matrix1 { get; private set; }
            public  Matrix Matrix2 { get; private set; }
            public  Matrix MatrixInverse { get; private set; }
            public  Matrix MatrixResult { get; private set; }
            
            
            public void FillBasicValues()
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
                Matrix2 = new Matrix(new double[,] { { Sum(MultXY) }, { Sum(Y) } });

                MatrixInverse = new Matrix(Matrix.FindInverseMatrix(Matrix1.Value));

                MatrixResult = MatrixInverse * Matrix2;

                A = MatrixResult.Value[0, 0];
                B = MatrixResult.Value[1, 0];

                for (int i = 0; i < Count; ++i)
                {
                    Ylinear[i] = A * X[i] + B;
                    D[i] = Y[i] - Ylinear[i];
                    PowD2[i] = Math.Pow(D[i], 2);
                }
                S = Sum(PowD2);
                XValue = X;
                YValue = Y;
            }

            




        }
        public class QuadraticFunction
        {

        }


    }
}
