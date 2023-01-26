using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
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

#if DEBUG 
            UITextBoxes[InputType.TextBoxA].Text = "1";
            UITextBoxes[InputType.TextBoxB].Text = "50";
            UITextBoxes[InputType.TextBoxE].Text = "0,1";
            UITextBoxes[InputType.TextBoxN].Text = "50";
            UITextBoxes[InputType.TextBoxFx].Text = "cos(x)+x^3";
#endif

        }
        private object GetStyleElement(Control element, string name) =>
          element.Template.FindName(name, element);

        
        #region Buttons_click
        private void RectangleMethod_Click(object sender, RoutedEventArgs e)
        {
            ClearGraph();

            RectangleMethod method = new RectangleMethod(RectangleMethod.RectangleType.Left);

            method.SetValues(GetEnteredValues(method));

            GraphVisualization(method);

        }

        private void TrapezoidalMethod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SimpsonMethod_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region ClearItems_click and clear Methods

        private void ClearGraphItem_Click(object sender, RoutedEventArgs e)
        {
            ClearGraph();
        }

        private void ClearItem_Click(object sender, RoutedEventArgs e)
        {
            clearEnteredValues();
            ClearGraph();
            GC.Collect();
        }
        private void ClearGraph()
        {
            WpfPlot1.UpdateDefaultStyle();
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
        private void clearEnteredValues()
        {
            foreach (var textBox in UITextBoxes.Values)
                textBox.Text = string.Empty;
        }
        #endregion

        #region Function and Graph Visualization
        private void GraphVisualization(IOutputValue outputValue)
        {


            WpfPlot1.Plot.Title($"Oптимальное число разбиений: {outputValue.OptimalSplitValue}");
            VisualizeSplits(outputValue);
            WpfPlot1.Plot.AddScatter(
                outputValue.FunctionCoordinates.Select(i => i.X).ToArray(),
                outputValue.FunctionCoordinates.Select(i => i.Y).ToArray(),
                markerShape: ScottPlot.MarkerShape.none, lineWidth: 3);

            foreach (var el in outputValue.SplitCoordinates)
            {
                WpfPlot1.Plot.AddScatter(
                new double[] { el.X },
                new double[] { el.Y },
                color: System.Drawing.Color.FromName("Green"),
                markerSize: 7);

            }

            WpfPlot1.Refresh();
        }
        private void VisualizeSplits(IOutputValue outputValue)
        {


            double SplitDistance = (outputValue.SplitCoordinates.Count > 1) ? outputValue.SplitCoordinates[1].X - outputValue.SplitCoordinates[0].X : 1;
            double correctValue = SplitDistance / 2;

            double[] Xvalues = outputValue.SplitCoordinates.Select(a => a.X).ToArray();
            double[] Yvalues = outputValue.SplitCoordinates.Select(a => a.Y).ToArray();
            
            double[] values = Yvalues;
            
            for(int i=0; i < Xvalues.Length; ++i)
            {
                Xvalues[i] -= correctValue;
            }
            
            // create a histogram
            (double[] probabilities, double[] binEdges) = (Yvalues.ToArray(), Xvalues.ToArray());
            double[] leftEdges = binEdges.Take(binEdges.Length).ToArray();

            // display histogram probabability as a bar plot
            var bar = WpfPlot1.Plot.AddBar(values: probabilities, positions: leftEdges);
            bar.BarWidth = SplitDistance;
            bar.FillColor = ColorTranslator.FromHtml("#9bc3eb");
            bar.BorderColor = ColorTranslator.FromHtml("#82add9");

            // display histogram distribution curve as a line plot
            double[] densities = ScottPlot.Statistics.Common.ProbabilityDensity(values, binEdges);
            WpfPlot1.Plot.AddScatterLines(
                xs: binEdges,
                ys: densities,
                color: System.Drawing.Color.Black,
                lineWidth: 2,
                lineStyle: LineStyle.Dash);
        }

        private double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(UITextBoxes[InputType.TextBoxFx].Text.ToLower());
            expression.Bind("x", X);
            return expression.Eval<double>();
        }
        private List<System.Windows.Point> GetFunctionCoordinates(IEnteredValues enteredValues)
        {
            List<System.Windows.Point> FunctionCoordinates = new List<System.Windows.Point>();
            for (double i = enteredValues.A; i <= enteredValues.B; i += enteredValues.E)
            {
                FunctionCoordinates.Add(new System.Windows.Point(i, F(i)));
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

        #endregion


    }
}
