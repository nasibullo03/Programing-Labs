using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using org.matheval;
using ScottPlot;

namespace Programing_Labs.Pages
{
    /// <summary>
    /// Логика взаимодействия для Lab3_Page.xaml
    /// </summary>
    public partial class Lab3_Page : Page
    {
        private TextBox TxtBxA { get; set; }
        private TextBox TxtBxB { get; set; }
        private TextBox TxtBxE { get; set; }
        private TextBox TxtBxFx { get; set; }
        private TextBox[] UITextBoxes { get; set; }
        List<double> Xpoints = new List<double>();
        List<double> Ypoints = new List<double>();

        public Lab3_Page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


            TxtBxA = GetStyleElement(TextBoxA, "MainTextBox") as TextBox;
            TxtBxB = GetStyleElement(TextBoxB, "MainTextBox") as TextBox;
            TxtBxE = GetStyleElement(TextBoxE, "MainTextBox") as TextBox;
            TxtBxFx = GetStyleElement(TextBoxFx, "MainTextBox") as TextBox;

            TxtBxFx.Tag = "formula";
            TxtBxA.Tag = "a";
            TxtBxB.Tag = "b";
            TxtBxE.Tag = "e";

            UITextBoxes = new TextBox[]{
                TxtBxA,
                TxtBxB,
                TxtBxE,
                TxtBxFx
            };

            foreach (TextBox textBox in UITextBoxes)
            {
                textBox.Text = "";
                DataObject.AddPastingHandler(textBox, (s, a) => a.CancelCommand());
                if ((string)textBox.Tag == "formula") continue;
                else textBox.PreviewTextInput +=
            new TextCompositionEventHandler(Check.PreviewTextInput);

            }
        }
        private object GetStyleElement(Control element, string name) =>
            element.Template.FindName(name, element);


        private void MenuItemSolve_Click(object sender, RoutedEventArgs e)
        {
            /*
             Интервал = [StartPoint, EndPoint]
             Точность(e)  = Accuracy
            Result -ЭТО Х
             */
            Xpoints.Clear();
            Ypoints.Clear();

            if (Check.CheckTextBoxesValues(UITextBoxes, TxtBxA.Text, TxtBxB.Text))
            {
                double.TryParse(TxtBxA.Text, out var StartPoint);
                double.TryParse(TxtBxB.Text, out var EndPoint);
                double.TryParse(TxtBxE.Text, out var Accuracy);

                double a = StartPoint;
                double b = EndPoint;
                double x1, x2, k1, k2, F1, F2, Result;

                int count = 0;

                k2 = (Math.Sqrt(5) - 1) / 2;
                k1 = 1 - k2;

                x1 = StartPoint + k1 * (EndPoint - StartPoint);
                x2 = StartPoint + k2 * (EndPoint - StartPoint);

                try
                {
                    F1 = F(x1);
                    F2 = F(x2);

                    while (true)
                    {
                        ++count;

                        if ((EndPoint - StartPoint) < Accuracy)
                        {
                            Result = (StartPoint + EndPoint) / 2;
                            WpfPlot1.Plot.Clear();
                            WpfPlot1.Refresh();
                            ShowGraph(a, b, Result);
                            break;
                        }
                        else
                        {
                            if (F1 < F2)
                            {
                                EndPoint = x2;
                                x2 = x1;
                                F2 = F1;
                                x1 = StartPoint + k1 * (EndPoint - StartPoint);
                                F1 = F(x1);
                            }
                            else
                            {
                                StartPoint = x1;
                                x1 = x2;
                                F2 = F1;
                                x2 = StartPoint + k2 * (EndPoint - StartPoint);
                                F2 = F(x2);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }


            }
        }
        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {

            TxtBxA.Text = string.Empty;
            TxtBxB.Text = string.Empty;
            TxtBxE.Text = string.Empty;
            TxtBxFx.Text = string.Empty;
            WpfPlot1.UpdateDefaultStyle();
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();

        }
        private double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(TxtBxFx.Text.ToLower());
            expression.Bind("x", X);
            decimal value = expression.Eval<decimal>();
            /*System.Windows.Forms.MessageBox.Show(value.ToString());*/
            return (double)value;
        }
        private void ShowGraph(double StartPoint, double EndPoint, double Result)
        {
            double x = Result;
            double y = F(Result);
            for (double i = StartPoint; i <= EndPoint; i += 0.1)
            {
                Xpoints.Add(i);
                Ypoints.Add(F(i));
            }
            WpfPlot1.Plot.Title($"Точка минимум - min[{Math.Round(x, 5)} ; {Math.Round(y, 5)}]");
            WpfPlot1.Plot.AddScatter(Xpoints.ToArray(), Ypoints.ToArray(), markerShape: MarkerShape.none, lineWidth: 3);
            WpfPlot1.Plot.AddScatter(
                new double[] { x },
                new double[] { y },
                color: System.Drawing.Color.FromName("Green"),
                markerSize: 7);
            WpfPlot1.Plot.AddArrow(x,
                 y, x - 3, y,
                color: System.Drawing.Color.FromName("Red"));
            WpfPlot1.Refresh();
        }



    }
}
