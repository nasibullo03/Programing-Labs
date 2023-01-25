using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Programing_Labs.Pages.DefiniteIntegral
{
    /// <summary>
    /// Логика взаимодействия для DefiniteIntegral_Page.xaml
    /// </summary>
    public partial class DefiniteIntegral_Page : Page
    {
        #region Properties

        private enum InputType { TextBoxA, TextBoxB, TextBoxN, TextBoxE, TextBoxFx }
        private Dictionary<InputType, TextBox> UITextBoxes { get; set; }

        #endregion

        public DefiniteIntegral_Page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UITextBoxes = new Dictionary<InputType, TextBox>
            {
                {InputType.TextBoxA, (TextBox)GetStyleElement(TextBoxA,"MainTextBox") },
                {InputType.TextBoxB, (TextBox)GetStyleElement(TextBoxB,"MainTextBox") },
                {InputType.TextBoxN, (TextBox)GetStyleElement(TextBoxN,"MainTextBox") },
                {InputType.TextBoxE, (TextBox)GetStyleElement(TextBoxE,"MainTextBox") },
                {InputType.TextBoxFx, (TextBox)GetStyleElement(TextBoxFx,"MainTextBox") },
            };

        }
        private object GetStyleElement(Control element, string name) =>
          element.Template.FindName(name, element);

        
        #region Buttons_click
        private void RectangleMethod_Click(object sender, RoutedEventArgs e)
        {
            RectangleMethod method = new RectangleMethod(RectangleMethod.RectangleType.Left);

            method.SetValues(GetEnteredValues(method));

            GraphVisualization(method, method);

        }


        private void GraphVisualization(IEnteredValues enteredValues, IOutputValue outputValue)
        {

            WpfPlot1.Plot.Title($"Oптимальное число разбиений: {outputValue.OptimalSplitValue}");
            WpfPlot1.Plot.AddScatter(
                outputValue.FunctionCoordinates.Select(i => i.X).ToArray(),
                outputValue.FunctionCoordinates.Select(i => i.Y).ToArray(),
                markerShape: ScottPlot.MarkerShape.none, lineWidth: 3);

            foreach (Point el in outputValue.SplitCoordinates)
            {
                WpfPlot1.Plot.AddScatter(
                new double[] { el.X },
                new double[] { el.Y },
                color: System.Drawing.Color.FromName("Green"),
                markerSize: 7);

            }

            WpfPlot1.Refresh();
        }

        private double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(UITextBoxes[InputType.TextBoxFx].Text.ToLower());
            expression.Bind("x", X);
            return expression.Eval<double>();
        }
        private List<Point> GetFunctionCoordinates(IEnteredValues enteredValues)
        {
            List<Point> FunctionCoordinates = new List<Point>();
            for (double i = enteredValues.A; i <= enteredValues.B; i += enteredValues.E)
            {
                FunctionCoordinates.Add(new Point(i, F(i)));
            }
            return FunctionCoordinates;
        }

        private IEnteredValues GetEnteredValues(IEnteredValues enteredValues)
        {

            double.TryParse(UITextBoxes[InputType.TextBoxA].Text, out var a);
            double.TryParse(UITextBoxes[InputType.TextBoxB].Text, out var b);
            double.TryParse(UITextBoxes[InputType.TextBoxE].Text, out var e);
            double.TryParse(UITextBoxes[InputType.TextBoxN].Text, out var n);

            enteredValues.A = a;
            enteredValues.B = b;
            enteredValues.E = e;
            enteredValues.N = n;
            enteredValues.F = F;
            enteredValues.GetFunctionCoordinates = GetFunctionCoordinates;
            

            return enteredValues;

        }

        private void TrapezoidalMethod_Click(object sender, RoutedEventArgs e)
        {

        }


        private void SimpsonMethod_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion


    }
}
