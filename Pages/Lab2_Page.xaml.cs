using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using org.matheval;
using ScottPlot.WPF;
using ScottPlot;



namespace Programing_Labs.Pages
{
    /// <summary>
    /// Логика взаимодействия для Lab2_Page.xaml
    /// </summary>
    public partial class Lab2_Page : Page
    {
        private TextBox TxtBxA { get; set; }
        private TextBox TxtBxB { get; set; }
        private TextBox TxtBxE { get; set; }
        private TextBox TxtBxFx { get; set; }
        private TextBox[] UITextBoxes { get; set; }
        private List<double> Xpoints = new List<double>();
        private List<double> Ypoints = new List<double>();



        public Lab2_Page()
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
            /*ShowFormula(@"{2+x}/2");*/

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
             Точность  = Accuracy
             */
            double Middle = 0, Result;
            Xpoints.Clear();
            Ypoints.Clear();


            if (Check.CheckTextBoxesValues(UITextBoxes, TxtBxA.Text, TxtBxB.Text))
            {
                double.TryParse(TxtBxA.Text, out var StartPoint);
                double.TryParse(TxtBxB.Text, out var EndPoint);
                double.TryParse(TxtBxE.Text, out var Accuracy);
                double a = StartPoint;
                double b = EndPoint;
                double x1, x2;

                //метод чтобы найти точки минимума
                double IncrementStep = Accuracy / 10;

                int count = 0;
                /*try
                {*/
                while (true)
                {
                    ++count;

                    x1 = (StartPoint + EndPoint - IncrementStep) / 2;
                    x2 = (StartPoint + EndPoint + IncrementStep) / 2;
                    try
                    {
                        if (F(x1) <= F(x2))
                            EndPoint = x2;
                        else
                            StartPoint = x1;

                        Middle = (EndPoint - StartPoint) / 2;

                        if (Middle <= Accuracy)
                        {
                            Result = (StartPoint + EndPoint) / 2;
                            WpfPlot1.Plot.Clear();
                            WpfPlot1.Refresh();
                            ShowGraph(a, b, Result);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        break; ;
                    }
                }
                /* }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }*/


                /* Result = -1 - Math.Pow(Math.Pow(2 * (x - 1) * (x - 7), 2), 1 / 3);*//*
                //метод чтобы найти точки максимума
                int count = 0;
                while (true)
                {
                    ++count;
                    if (Math.Abs(EndPoint - StartPoint) > Accuracy)
                    {
                        Middle = (StartPoint + EndPoint) / 2;

                        if (F(Math.Round(StartPoint, 2)) * F(Math.Round(Middle, 2)) < 0)
                        {
                            EndPoint = Middle;
                        }
                        else
                        {
                            StartPoint = Middle;
                        }
                    }
                    else
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        ShowGraph(a, b, Result);
                        System.Windows.Forms.MessageBox.Show($"Result:{Result} Count:{count}");

                        break;
                    }
                }*/

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
            WpfPlot1.Plot.AddScatter(Xpoints.ToArray(), Ypoints.ToArray(),markerShape: MarkerShape.none, lineWidth: 3);
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

        private void WpfPlot1_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
