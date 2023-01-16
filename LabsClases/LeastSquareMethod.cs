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
            public LinearFunction()
            {
            }
            public double[] XValue { get; private set; }
            public double[] YValue { get; private set; }
            public double[] PowX2 { get; private set; }
            public double[] MultXY { get; private set; }
            public double[] Ylinear { get; private set; }
            public double[] D { get; private set; }
            public double A { get; private set; }
            public double B { get; private set; }
            public double S { get; private set; }
            public double[] PowD2 { get; private set; }
            public Matrix Matrix1 { get; private set; }
            public Matrix Matrix2 { get; private set; }
            public Matrix MatrixInverse { get; private set; }
            public Matrix MatrixResult { get; private set; }
            public void Solve()
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
            /// <summary>
            /// 
            /// </summary>
            /// <returns>Возвращает функцию в виде строка</returns>
            public string GetFunctionFormula()
            {
                string result = $"{Math.Round(A)} * x ";
                result += (B >= 0) ? "+ " : "";
                result += Math.Round(B,5).ToString();
                return result;

            }

        }
        public class QuadraticFunction
        {
            public double[] XValue { get; private set; }
            public double[] YValue { get; private set; }
            public double[] PowX2 { get; private set; }
            public double[] PowX3 { get; private set; }
            public double[] PowX4 { get; private set; }
            public double[] PowX2MultY { get; private set; }
            public double[] MultXY { get; private set; }
            public double[] YQuadratical { get; private set; }
            public double[] D { get; private set; }
            public double A { get; private set; }
            public double B { get; private set; }
            public double C { get; private set; }
            public double S { get; private set; }
            public double[] PowD2 { get; private set; }
            public double SumXValue { get; private set; }
            public double SumYValue { get; private set; }
            public double SumPowX2 { get; private set; }
            public double SumPowX3 { get; private set; }
            public double SumPowX4 { get; private set; }
            public double SumPowX2MultY { get; private set; }
            public double SumMultXY { get; private set; }

            public Matrix Matrix1 { get; private set; }
            public Matrix Matrix2 { get; private set; }
            public Matrix MatrixInverse { get; private set; }
            public Matrix MatrixResult { get; private set; }


            public void Solve()
            {

                FillValues(GraphicPoint.GraphicPoints);
                XValue = X;
                YValue = Y;

                PowX2 = new double[Count];
                PowX3 = new double[Count];
                PowX4 = new double[Count];
                PowX2MultY = new double[Count];
                MultXY = new double[Count];
                YQuadratical = new double[Count];
                D = new double[Count];
                PowD2 = new double[Count];
                //заполняем значение нижеуказанных рядов
                for (int i = 0; i < Count; ++i)
                {
                    PowX2[i] = Math.Pow(XValue[i], 2);
                    MultXY[i] = X[i] * YValue[i];
                    PowX3[i] = Math.Pow(XValue[i], 3);
                    PowX4[i] = Math.Pow(XValue[i], 4);
                    PowX2MultY[i] = Math.Pow(XValue[i], 2) * YValue[i];
                }
                //находим суммму всех рядов который нам нужны
                SumXValue = Sum(XValue);
                SumYValue = Sum(YValue);
                SumPowX2 = Sum(PowX2);
                SumPowX3 = Sum(PowX3);
                SumPowX4 = Sum(PowX4);
                SumPowX2MultY = Sum(PowX2MultY);
                SumMultXY = Sum(MultXY);

                Matrix1 = new Matrix(new double[,] {
                    { SumPowX4, SumPowX3,SumPowX2 },
                    { SumPowX3,SumPowX2,SumXValue},
                    { SumPowX2, SumXValue, Count}
                });

                Matrix2 = new Matrix(new double[,] { { SumPowX2MultY }, { SumMultXY }, { SumYValue } });
                MatrixInverse = new Matrix(Matrix.FindInverseMatrix(Matrix1.Value));
                MatrixResult = MatrixInverse * Matrix2;

                A = MatrixResult.Value[0, 0];
                B = MatrixResult.Value[1, 0];
                C = MatrixResult.Value[2, 0];

                for (int i = 0; i < Count; ++i)
                {
                    YQuadratical[i] = (A * PowX2[i]) + (B * XValue[i]) + C;
                    D[i] = Y[i] - YQuadratical[i];
                    PowD2[i] = Math.Pow(D[i], 2);
                }

                S = Sum(PowD2);

            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns>Возвращает функцию в виде строка</returns>
            public string GetFunctionFormula()
            {
                string result = $"{Math.Round(A,5)} * x^2 ";
                result += (B >= 0) ? "+ " : "";
                result += Math.Round(B,5).ToString();
                return result;

            }
        }


    }
}
