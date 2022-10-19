using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using org.matheval;
using WpfMath;

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
                ShowFormula(@"f(x)="+TxtBxFx.Text);
                int count=0;
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
        private void ShowFormula(string value)
        {
            
            const string fileName = @"T:\Temp\formula.png";

            var parser = new TexFormulaParser();
            var formula = parser.Parse(value);
            
            var renderer = formula.GetRenderer(TexStyle.Display, 60.0, "Arial",foreground:Brushes.White);
            var bitmapSource = renderer.RenderToBitmap(0.0, 0.0);
            ImgFormula.Source = bitmapSource;

           /* var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            using (var target = new FileStream(fileName, FileMode.Create))
            {
                encoder.Save(target);
                Console.WriteLine($"File saved to {fileName}");
            }*/
        }


    }
}
