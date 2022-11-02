using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.IO;

namespace Programing_Labs.LabsClases
{
    class Excell
    {
        private static Microsoft.Office.Interop.Excel.Application XlApplication { get; set; }
        private static Workbook XlWorkbook { get; set; }
        private static Worksheet XlWorksheet { get; set; }

        private static object missValue = System.Reflection.Missing.Value;

        private static string[] linearTemplateValues = new string[] {
            "x","y","x^2","x*y","y лин","d","d^2","Сумма","x^2","=A1","","=D1"
        };
        private static int LastPointIndex { get; set; }

        public static void StartGreatingExcelFile()
        {
            XlApplication = new Microsoft.Office.Interop.Excel.Application();
            XlWorkbook = XlApplication.Workbooks.Add(missValue);
            XlWorksheet = (Worksheet)XlWorkbook.Worksheets.Item[1];
            
        }

        public static void SaveFile()
        {
            XlWorkbook.SaveAs("d:\\csharp-Excel.xls", XlFileFormat.xlWorkbookNormal, missValue, missValue, missValue, missValue, XlSaveAsAccessMode.xlExclusive, missValue, missValue, missValue, missValue, missValue);
        }
        public static void CloseAndQuitFromFile()
        {
            XlWorkbook.Close(true, missValue, missValue);
            XlApplication.Quit();

            MessageBox.Show("Excel file created , you can find the file d:\\csharp-Excel.xls");

            Marshal.ReleaseComObject(XlWorksheet);
            Marshal.ReleaseComObject(XlWorkbook);
            Marshal.ReleaseComObject(XlApplication);
        }
        public static void FillLinearTemplate()
        {

            for (int i = 1; i <= linearTemplateValues.Length; ++i)
            {
                XlWorksheet.Cells[1, i] = linearTemplateValues[i - 1];
            }

        }
        public static void AddListViewsData(LeastSquareMethod.LinearFunction function)
        {
            int count = 2;
            for(int i=0;i<function.XValue.Length;++i)
            {
                //добавляем значение x и y в таблице
                XlWorksheet.Cells[count, 1] = function.XValue[i];
                XlWorksheet.Cells[count, 2] = function.YValue[i];
                //рассчитаем значение x^2 и x*y
                XlWorksheet.Cells[count, 3] = function.PowX2[i];
                XlWorksheet.Cells[count, 4] = function.MultXY[i];
                ++count;
            }

            ++count;
            //рассчитаем сумма рядов
            XlWorksheet.Cells[count, 1] = LeastSquareMethod.Sum(function.XValue);
            XlWorksheet.Cells[count, 2] = LeastSquareMethod.Sum(function.YValue);
            XlWorksheet.Cells[count, 3] = LeastSquareMethod.Sum(function.PowX2);
            XlWorksheet.Cells[count, 4] = LeastSquareMethod.Sum(function.MultXY);

            //сохраняем последный индекс, чтобы в дальнейшем получить доступ к сумму элементов
            LastPointIndex = count;

        }
        
        public static void FillMatrixsValues(LeastSquareMethod.LinearFunction function)
        {
            XlWorksheet.Cells[2, 9] = function.Matrix1.Value[0,0];
            XlWorksheet.Cells[2, 10] = function.Matrix1.Value[0, 1];
            

            XlWorksheet.Cells[3, 9] = function.Matrix1.Value[1, 0]; 
            XlWorksheet.Cells[3, 10] = function.Matrix1.Value[1, 1];

            XlWorksheet.Cells[2, 12] = function.Matrix2.Value[0,0];
            XlWorksheet.Cells[3, 12] = function.Matrix2.Value[1, 0];

            XlWorksheet.Cells[5, 8] = "Обратная матрица:";
            XlWorksheet.Cells[5, 9] = function.MatrixInverse.Value[0,0];
            XlWorksheet.Cells[5, 10] = function.MatrixInverse.Value[0,1];
            XlWorksheet.Cells[6, 9] = function.MatrixInverse.Value[1,0];
            XlWorksheet.Cells[6, 10] = function.MatrixInverse.Value[1,1];
            XlWorksheet.Cells[6, 10] = function.MatrixInverse.Value[1,1];
            
            
            XlWorksheet.Cells[5, 11] = "a = ";
            XlWorksheet.Cells[6, 11] = "b = ";
            
            XlWorksheet.Cells[5, 12] = function.MatrixResult.Value[0,0];
            XlWorksheet.Cells[6, 12] = function.MatrixResult.Value[1, 0];

            int count = 2;
            for (int i=0; i < function.Ylinear.Length; ++i)
            {
                XlWorksheet.Cells[count, 5] = function.Ylinear[i];
                XlWorksheet.Cells[count, 6] = function.D[i];
                XlWorksheet.Cells[count, 7] = function.PowD2[i];
                ++count;
            }
           
            XlWorksheet.Cells[LastPointIndex, 7] = "Невязка = ";
            XlWorksheet.Cells[LastPointIndex, 8] = LeastSquareMethod.Sum(function.PowD2);



        }




    }
}
