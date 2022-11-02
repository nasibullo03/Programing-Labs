using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programing_Labs.LabsClases
{
    class Matrix
    {
        public double[,] Value { get; set; }
        public Matrix()
        {

        }
        public Matrix(double[,] Value)
        {
            this.Value = Value;
        }
        private static double Determinant3x3(double[,] MainMatrix)
        {
            double a = MainMatrix[0, 0],
                b = MainMatrix[0, 1],
                c = MainMatrix[0, 2],
                d = MainMatrix[1, 0],
                e = MainMatrix[1, 1],
                f = MainMatrix[1, 2],
                g = MainMatrix[2, 0],
                h = MainMatrix[2, 1],
                i = MainMatrix[2, 2];

            return a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g);
        }

        private static double DeterminantOver3(double[,] MainMatrix)
        {
            if (MainMatrix.GetLength(0) == 3)
            {
                return Determinant3x3(MainMatrix);
            }

            int DetSize = MainMatrix.GetLength(0) - 1,
                RowIndex = 0,
                ColIndex = 0,
                MatIndex = MainMatrix.GetLength(0);
            double det = 0;

            double[,] DetMatrix = new double[DetSize, DetSize];

            for (int FirstColInd = 0; FirstColInd < MatIndex; ++FirstColInd)
            {
                for (int MatrixColumn = 1; MatrixColumn < MatIndex; ++MatrixColumn)
                {
                    for (int MatrixRow = 0; MatrixRow < MatIndex; ++MatrixRow)
                    {
                        if (MatrixRow == FirstColInd)
                        {
                            continue;
                        }
                        else
                        {
                            DetMatrix[ColIndex, RowIndex++] = MainMatrix[MatrixColumn, MatrixRow];
                        }
                    }
                    RowIndex = 0;
                    ++ColIndex;
                }
                ColIndex = 0;
                RowIndex = 0;
                if (FirstColInd % 2 == 0)
                {
                    det += MainMatrix[0, FirstColInd] * DeterminantOver3(DetMatrix);
                }
                else if (FirstColInd % 2 == 1)
                {
                    det -= MainMatrix[0, FirstColInd] * DeterminantOver3(DetMatrix);
                }

                DetMatrix = new double[DetSize, DetSize];
            }
            return det;
        }

        public static double FindDeterminant(double[,] MainMatrix)
        {
            double det = default;
            int Size = MainMatrix.GetLength(0);
            if (Size == 2)
            {
                det = MainMatrix[0, 0] * MainMatrix[1, 1] - MainMatrix[1, 0] * MainMatrix[0, 1];
            }
            else if (Size == 3)
            {
                det = Determinant3x3(MainMatrix);
            }
            else if (Size > 3)
            {
                det = DeterminantOver3(MainMatrix);
            }

            return det;
        }

        public static double[,] FindTranspose(double[,] MainMatrix)
        {
            int Size = MainMatrix.GetLength(0);
            double[,] Transpose = new double[Size, Size];

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    Transpose[i, j] = MainMatrix[j, i];
                }
            }

            return Transpose;
        }

        private static double[,] MinorOf3x3(double[,] MainMatrix)
        {
            /*
                | a11 a12 a13 |
            A =	| a21 a22 a23 |
                | a31 a32 a33 |
                        | a22 a23 |
            a11 = M11 =	| a32 a33 | = a22*a33 - a23*a32
            */
            double a11 = MainMatrix[0, 0],
                a12 = MainMatrix[0, 1],
                a13 = MainMatrix[0, 2],
                a21 = MainMatrix[1, 0],
                a22 = MainMatrix[1, 1],
                a23 = MainMatrix[1, 2],
                a31 = MainMatrix[2, 0],
                a32 = MainMatrix[2, 1],
                a33 = MainMatrix[2, 2];

            double M11 = a22 * a33 - a23 * a32,
                M12 = a21 * a33 - a23 * a31,
                M13 = a21 * a32 - a22 * a31,
                M21 = a12 * a33 - a13 * a32,
                M22 = a11 * a33 - a13 * a31,
                M23 = a11 * a32 - a12 * a31,
                M31 = a12 * a23 - a13 * a22,
                M32 = a11 * a23 - a13 * a21,
                M33 = a11 * a22 - a12 * a21;

            double[,] MinorMatrix = { { M11, M12, M13 }, { M21, M22, M23 }, { M31, M32, M33 } };

            return MinorMatrix;
        }

        private static double[,] MinorOfGreaterThan3(double[,] MainMatrix)
        {
            /*
                | a11 a12 a13 |
            A =	| a21 a22 a23 |
                | a31 a32 a33 |
                        | a22 a23 |
            a11 = M11 =	| a32 a33 | = a22*a33 - a23*a32
            */
            if (MainMatrix.GetLength(0) == 3)
            {
                return MinorOf3x3(MainMatrix);
            }

            int DetSize = MainMatrix.GetLength(0) - 1,

                RowIndex = 0,
                ColIndex = 0,
                MatIndex = MainMatrix.GetLength(0);

            double[,] Minor = new double[MatIndex, MatIndex];
            double[,] DetMatrix = new double[DetSize, DetSize];

            for (int MainMatrixCol = 0; MainMatrixCol < MatIndex; ++MainMatrixCol)
            {
                for (int MainMatrixRow = 0; MainMatrixRow < MatIndex; ++MainMatrixRow)
                {
                    for (int DetMatrixCol = 0; DetMatrixCol < MatIndex; ++DetMatrixCol)
                    {
                        if (MainMatrixCol == DetMatrixCol)
                            continue;
                        for (int DetMatrixRow = 0; DetMatrixRow < MatIndex; ++DetMatrixRow)
                        {
                            if (MainMatrixRow == DetMatrixRow)
                                continue;
                            else
                            {
                                DetMatrix[ColIndex, RowIndex++] = MainMatrix[DetMatrixCol, DetMatrixRow];
                                if (RowIndex >= DetSize)
                                    RowIndex = 0;
                            }
                        }
                        RowIndex = 0;
                        ++ColIndex;
                    }
                    RowIndex = 0;
                    ColIndex = 0;

                    Minor[MainMatrixCol, MainMatrixRow] = DeterminantOver3(DetMatrix);

                    DetMatrix = new double[DetSize, DetSize];
                }
            }

            return Minor;
        }

        public static double[,] FindMinor(double[,] MainMatrix)
        {
            int Size = MainMatrix.GetLength(0);
            double[,] Minor = new double[Size, Size];

            if (Size == 2)
            {
                Minor[0, 0] = MainMatrix[1, 1];
                Minor[0, 1] = MainMatrix[1, 0];
                Minor[1, 0] = MainMatrix[0, 1];
                Minor[1, 1] = MainMatrix[0, 0];
            }
            else if (Size == 3)
            {
                Minor = MinorOf3x3(MainMatrix);
            }
            else if (Size > 3)
            {
                Minor = MinorOfGreaterThan3(MainMatrix);
            }

            return Minor;
        }

        public static double[,] FindInverseMatrix(double[,] MainMatrix)
        {
            int Size = MainMatrix.GetLength(0);
            double[,] Inverse = new double[Size, Size];
            double[,] Transpose = FindTranspose(MainMatrix);
            double Determinant = FindDeterminant(MainMatrix);
            double rr = 1 / Determinant;

            for (int ColIndex = 0; ColIndex < Size; ++ColIndex)
            {
                for (int RowIndex = 0; RowIndex < Size; ++RowIndex)
                {
                    Inverse[ColIndex, RowIndex] = Transpose[ColIndex, RowIndex] * rr;
                }
            }
            return Inverse;
        }
        public static Matrix operator *(Matrix MatrixA, Matrix MatrixB)
        {

            int rA = MatrixA.Value.GetLength(0);
            int cA = MatrixB.Value.GetLength(1);
            int rB = MatrixB.Value.GetLength(0);
            int cB = MatrixB.Value.GetLength(1);
            double temp = 0;
            double[,] MatrixC = new double[rA, cB];
            
            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    temp = 0;
                    for (int k = 0; k < rB; k++)
                    {
                        temp += Math.Round(MatrixA.Value[i, k],6) * Math.Round(MatrixB.Value[k, j],6);
                    }
                    MatrixC[i, j] = temp;
                }
            }

            return new Matrix(MatrixC);
        }
    }
}
