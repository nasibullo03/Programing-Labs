using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
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
        public static void AddListViewsData()
        {
            int count = 2;
            GraphicPoint.GraphicPoints.ForEach(el =>
            {
                //добавляем значение x и y в таблице
                XlWorksheet.Cells[count, 1] = el.Xi;
                XlWorksheet.Cells[count, 2] = el.Yi;
                //рассчитаем значение x^2 и x*y
                XlWorksheet.Cells[count, 3] = $"=A{count}^2";
                XlWorksheet.Cells[count, 4] = $"=A{count}*B{count}";
                ++count;
            });

            ++count;
            //рассчитаем сумма рядов
            XlWorksheet.Cells[count, 1] = XlApplication.WorksheetFunction.Sum(XlWorksheet.Range["A2",$"A{count-1}"]);
            XlWorksheet.Cells[count, 2] = XlApplication.WorksheetFunction.Sum(XlWorksheet.Range["B2", $"B{count-1}"]);
            XlWorksheet.Cells[count, 3] = XlApplication.WorksheetFunction.Sum(XlWorksheet.Range["C2", $"C{count-1}"]);
            XlWorksheet.Cells[count, 4] = XlApplication.WorksheetFunction.Sum(XlWorksheet.Range["D2", $"D{count-1}"]);

            //сохраняем последный индекс, чтобы в дальнейшем получить доступ к сумму элементов
            LastPointIndex = count;

        }
        
        public static void FillMatrixsValues()
        {
            XlWorksheet.Cells[2, 9] = $"=C{LastPointIndex}";
            XlWorksheet.Cells[2, 10] = $"=A{LastPointIndex}";
            XlWorksheet.Cells[2, 12] = $"=D{LastPointIndex}";

            XlWorksheet.Cells[3, 9] = $"=A{LastPointIndex}";
            XlWorksheet.Cells[3, 10] = GraphicPoint.GraphicPoints.Count;
            XlWorksheet.Cells[3, 12] = $"=B{LastPointIndex}";
            
            XlWorksheet.Cells[5, 8] = "Обратная матрица:";
            XlWorksheet.Cells["I5", "J7"] = XlApplication.WorksheetFunction.MInverse(XlWorksheet.Range["I2", "J3"]);
            /*            XlWorksheet.Cells[5, 10] = "=C10";
             *            XlWorksheet.Cells[6, 9] = "=C10";
                        XlWorksheet.Cells[6, 10] = "=C10";*/

            XlWorksheet.Cells[5, 11] = "a = ";
            XlWorksheet.Cells[6, 11] = "b = ";

            XlWorksheet.Cells[5, 12] = XlApplication.WorksheetFunction.DProduct(XlWorksheet.Range["L5", "L6"], XlWorksheet.Range["I5", "J6"], XlWorksheet.Range["L2", "L3"]);
            /*XlWorksheet.Cells[6, 12] = "{=МУМНОЖ(I5:J6;L2:L3)}";*/

            for (int i=2; i <= LastPointIndex; ++i)
            {
                XlWorksheet.Cells[i, 5] = $"=$L$5*A{i}+$L$6";
                XlWorksheet.Cells[i, 6] = $"=B{i}-E{i}";
                XlWorksheet.Cells[i, 7] = $"=F{i}^2";
            }


            XlWorksheet.Cells[LastPointIndex, 7] = "невязка";
            XlWorksheet.Cells[LastPointIndex, 8] = $"=СУММ(G2:G{LastPointIndex-1})";



        }




    }
}
