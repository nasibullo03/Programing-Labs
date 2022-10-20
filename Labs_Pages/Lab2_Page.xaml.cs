using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using org.matheval;
using ScottPlot;



namespace Programing_Labs.Labs_Pages
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
        List<double> Xpoints = new List<double>();
        List<double> Ypoints = new List<double>();



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


            if (Check.CheckTextBoxesValues(UITextBoxes))
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
                while (true)
                {
                    ++count;

                    x1 = (StartPoint + EndPoint - IncrementStep) / 2;
                    x2 = (StartPoint + EndPoint + IncrementStep) / 2;

                    if (F(x1) <= F(x2))
                        EndPoint = x2;
                    else
                        a = x1;

                    Middle = (EndPoint - StartPoint) / 2;

                    if (Middle <= Accuracy)
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        WpfPlot1.UpdateDefaultStyle();
                        WpfPlot1.Plot.Clear();
                        WpfPlot1.Refresh();
                        ShowGraph(a, b, Result);
                        System.Windows.Forms.MessageBox.Show($"Result:{Result} Count:{count}");
                        System.Windows.Forms.MessageBox.Show(Middle.ToString());
                        break;
                    }
                }


                /* Result = -1 - Math.Pow(Math.Pow(2*(x-1)*(x-7),2), 1 / 3);*/
                //метод чтобы найти точки максимума
                /*int count = 0;
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
            double value = expression.Eval<double>();
            /*System.Windows.Forms.MessageBox.Show(value.ToString());*/
            return value;
        }
        private void ShowGraph(double StartPoint, double EndPoint, double Result)
        {

            for (double i = StartPoint; i <= EndPoint; i += 0.1)
            {
                Xpoints.Add(i);
                Ypoints.Add(F(i));
            }
            WpfPlot1.Plot.AddScatter(Xpoints.ToArray(), Ypoints.ToArray(),markerShape:MarkerShape.none,lineWidth:3);
            WpfPlot1.Plot.AddScatter(
                new double[] { Result },
                new double[] { F(Result) },
                color: System.Drawing.Color.FromName("Red"),
                markerSize: 7,
                label: "Точка минимум");
            WpfPlot1.Refresh();


        }
    }
}
