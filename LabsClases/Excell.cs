using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Windows.Forms;

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


        public static void FillLinearTemplate()
        {
            
            XlApplication = new Microsoft.Office.Interop.Excel.Application();

            XlWorkbook = XlApplication.Workbooks.Add(missValue);
            XlWorksheet = (Worksheet)XlWorkbook.Worksheets.Item[1];
            
            for(int i = 1; i <= linearTemplateValues.Length; ++i)
            {
                XlWorksheet.Cells[1, i] = linearTemplateValues[i-1];
            }

            XlWorkbook.SaveAs("d:\\csharp-Excel.xls", XlFileFormat.xlWorkbookNormal, missValue, missValue, missValue, missValue, XlSaveAsAccessMode.xlExclusive, missValue, missValue, missValue, missValue, missValue);
            XlWorkbook.Close(true, missValue, missValue);
            XlApplication.Quit();

            Marshal.ReleaseComObject(XlWorksheet);
            Marshal.ReleaseComObject(XlWorkbook);
            Marshal.ReleaseComObject(XlApplication);
            MessageBox.Show("Excel file created , you can find the file d:\\csharp-Excel.xls");

        }


    }
}
