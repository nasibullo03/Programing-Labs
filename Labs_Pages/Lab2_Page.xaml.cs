using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            double Middle, Result;

            if (Check.CheckTextBoxesValues(UITextBoxes))
            {
                double.TryParse(TxtBxA.Text, out var StartPoint);
                double.TryParse(TxtBxB.Text, out var EndPoint);
                double.TryParse(TxtBxE.Text, out var Accuracy);
                
                int count = 0;
                /*double.TryParse(TxtBxFx.Text, out var Fx);*/
                while (true)
                {
                    ++count;
                    if (Math.Abs(EndPoint - StartPoint) > Accuracy)
                    {
                        Middle = (StartPoint + EndPoint) / 2;

                        if (F(StartPoint) * F(Middle) > 0)
                        {
                            StartPoint = Middle;
                        }
                        else
                        {
                            EndPoint = Middle;
                        }
                    }
                    else
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        ShowGraph(StartPoint, EndPoint, Result);
                        System.Windows.Forms.MessageBox.Show($"Result:{Result} Count:{count}");
                        
                        break;
                    }
                }

            }
        }
        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {

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
            List<double> Xpoints = new List<double>();
            List<double> Ypoints = new List<double>();
            for(double i = StartPoint; i <= EndPoint; ++i)
            {
                Xpoints.Add(i);
                Ypoints.Add(F(i));
            }
            WpfPlot1.Plot.AddScatter(Xpoints.ToArray(),Ypoints.ToArray());
            WpfPlot1.Refresh();

        }
    }
}
