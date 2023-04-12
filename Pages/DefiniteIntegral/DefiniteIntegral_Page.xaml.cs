using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Programing_Labs.Pages.DefiniteIntegral
{
    /// <summary>
    /// Логика взаимодействия для DefiniteIntegral_Page.xaml
    /// </summary>
    public partial class DefiniteIntegral_Page : Page
    {
        #region Properties

        public enum InputType { TextBoxA, TextBoxB, TextBoxN, TextBoxE, TextBoxFx }
        public enum MethodType { Rectangle, Trepezoida, Simpson }
        private Dictionary<InputType, TextBox> UITextBoxes { get; set; }

        
        #endregion

        public DefiniteIntegral_Page()
        {
            InitializeComponent();
            GraphVisualize.WpfPlot1 = WpfPlot1;

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
            UITextBoxes[InputType.TextBoxA].Tag = "TextBoxA";
            UITextBoxes[InputType.TextBoxB].Tag = "TextBoxB";
            UITextBoxes[InputType.TextBoxE].Tag = "TextBoxE";
            UITextBoxes[InputType.TextBoxN].Tag = "TextBoxN";
            UITextBoxes[InputType.TextBoxFx].Tag = "TextBoxFx";

            foreach (TextBox textBox in UITextBoxes.Values)
            {
                if ((string)textBox.Tag != "TextBoxFx")
                {
                    DataObject.AddPastingHandler(textBox, (s, a) => a.CancelCommand());
                    textBox.PreviewTextInput += new TextCompositionEventHandler(Check.PreviewTextInput);
                }
            }


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

        private void PerformMethod(MethodType type)
        {
            if (Check.CheckTextBoxesValues(UITextBoxes))
            {
                ClearGraph();
                switch (type)
                {
                    case MethodType.Rectangle:
                        RectangleMethod method = new RectangleMethod(RectangleMethod.RectangleType.Left);
                        method.SetValues(GetEnteredValues(method));
                        GraphVisualize.Visualize(method);
                        break;
                    case MethodType.Trepezoida:
                        TrapezoidalMethod method1 = new TrapezoidalMethod();
                        method1.SetValues(GetEnteredValues(method1));
                        GraphVisualize.Visualize(method1);
                        break;
                    case MethodType.Simpson:
                        SimpsonMethod method2 = new SimpsonMethod();
                        method2.SetValues(GetEnteredValues(method2));
                        GraphVisualize.Visualize(method2);
                        break;
                }
            }
        }

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
            WpfPlot1.Plot.Title("");
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
        private void clearEnteredValues()
        {

            foreach (var textBox in UITextBoxes.Values)
                textBox.Text = string.Empty;

        }
        #endregion

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

        private void WpfPlot1_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void RectangleMethodItem_Click(object sender, RoutedEventArgs e)
        {
            PerformMethod(MethodType.Rectangle);
        }

        private void TrapezoidalMethodItem_Click(object sender, RoutedEventArgs e)
        {
            PerformMethod(MethodType.Trepezoida);
        }

        private void SympsonMethodItem_Click(object sender, RoutedEventArgs e)
        {
            PerformMethod(MethodType.Simpson);
        }
    }
}
