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
                XlWorksheet.Cells[count, 1] = el.Xi.ToString();
                XlWorksheet.Cells[count, 2] = el.Yi.ToString();
                //рассчитаем значение x^2 и x*y
                XlWorksheet.Cells[count, 3] = $"=A{count}^2";
                XlWorksheet.Cells[count, 4] = $"=A{count}*B{count}";
                ++count;
            });
            ++count;

            //рассчитаем сумма рядов
            XlWorksheet.Cells[count, 1] = $"=СУММ(A2:A{count})";
            XlWorksheet.Cells[count, 2] = $"=СУММ(B2:A{count})";
            XlWorksheet.Cells[count, 3] = $"=СУММ(C2:A{count})";
            XlWorksheet.Cells[count, 4] = $"=СУММ(D2:A{count})";

            //сохраняем последный индекс, чтобы в дальнейшем получить доступ к сумму элементов
            LastPointIndex = count;

        }


    }
}
