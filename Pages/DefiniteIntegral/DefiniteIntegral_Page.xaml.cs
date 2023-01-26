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

        public enum InputType { TextBoxA, TextBoxB, TextBoxN, TextBoxE, TextBoxFx }
        public enum MethodType { Rectangle, Trepezoida, Simpson }
        private Dictionary<InputType, TextBox> UITextBoxes { get; set; }
        private Dictionary<MethodType, bool> PerformMethods { get; set; } = new Dictionary<MethodType, bool>() {
                {MethodType.Rectangle,true},
                {MethodType.Trepezoida,false},
                {MethodType.Simpson,false}

            };
        private bool FirstTimeLoaded { get; set; } = true;

        #endregion

        public DefiniteIntegral_Page()
        {
            InitializeComponent();
            GraphVisualize.WpfPlot1 = WpfPlot1;
            GraphVisualize.WpfPlot2 = WpfPlot2;
            GraphVisualize.WpfPlot3 = WpfPlot3;

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

            FirstTimeLoaded = false;
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
            /* await Task.Run(() =>
             {

                 TextBoxA.Dispatcher.Invoke(() =>
                 {*/
            if (Check.CheckTextBoxesValues(UITextBoxes))
            {
                /*WpfPlot1.Dispatcher.Invoke(() => */
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
            WpfPlot1.UpdateDefaultStyle();
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

        private void SympsonMethodItem_Checked(object sender, RoutedEventArgs e)
        {
            PerformMethods[MethodType.Simpson] = true;
            if (!FirstTimeLoaded) PanelSizeChanged();
        }

        private void SympsonMethodItem_Unchecked(object sender, RoutedEventArgs e)
        {
            PerformMethods[MethodType.Simpson] = false;
            if (!FirstTimeLoaded) PanelSizeChanged();
        }

        private void TrapezoidalMethodItem_Checked(object sender, RoutedEventArgs e)
        {
            PerformMethods[MethodType.Trepezoida] = true;
            if (!FirstTimeLoaded) PanelSizeChanged(); ;
        }

        private void TrapezoidalMethodItem_Unchecked(object sender, RoutedEventArgs e)
        {
            PerformMethods[MethodType.Trepezoida] = false;
            if (!FirstTimeLoaded) PanelSizeChanged();
        }

        private void RectangleMethodItem_Checked(object sender, RoutedEventArgs e)
        {
            PerformMethods[MethodType.Rectangle] = true;
            if (!FirstTimeLoaded) PanelSizeChanged();
        }

        private void RectangleMethodItem_Unchecked(object sender, RoutedEventArgs e)
        {
            PerformMethods[MethodType.Rectangle] = false;
            if (!FirstTimeLoaded) PanelSizeChanged();
        }

        private void PanelSizeChanged()
        {
            int MetodsCount = 0;
            bool[] values = PerformMethods.Select(a => a.Value).ToArray();

            foreach (bool el in values)
            {
                if (el) ++MetodsCount;
            }

            double GraphWidth;
            if (!values[0] && !values[1] && !values[1])
            {
                PerformMethods[MethodType.Rectangle] = true;
                GraphWidth = this.ActualWidth;
                values[0] = true;
            }
            else
            {
                GraphWidth = this.ActualWidth / MetodsCount;
            }


            WpfPlot1.Width = GraphWidth;
            WpfPlot2.Width = GraphWidth;
            WpfPlot3.Width = GraphWidth;

            WpfPlot1.Visibility = values[0] ? Visibility.Visible : Visibility.Collapsed;
            WpfPlot2.Visibility = values[1] ? Visibility.Visible : Visibility.Collapsed;
            WpfPlot3.Visibility = values[2] ? Visibility.Visible : Visibility.Collapsed;

        }

        private void Perform_Click(object sender, RoutedEventArgs e)
        {
            var value = PerformMethods.Select(a => a.Value).ToArray();


            if (value[0]) PerformMethod(MethodType.Rectangle);
            if (value[1]) PerformMethod(MethodType.Trepezoida);
            if (value[2]) PerformMethod(MethodType.Simpson);

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!FirstTimeLoaded) PanelSizeChanged();
        }

        private void GraphPanel_Loaded(object sender, RoutedEventArgs e)
        {
            PanelSizeChanged();
        }
    }
}
