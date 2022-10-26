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
        private List<double> Xpoints = new List<double>();
        private List<double> Ypoints = new List<double>();
        /// <summary>
        /// для хранение список точки сходимости в графике 
        /// </summary>
        private List<ScottPlot.Plottable.ScatterPlot> ScatterPlots = new List<ScottPlot.Plottable.ScatterPlot>();
        private List<ScottPlot.Plottable.Text> Texts = new List<ScottPlot.Plottable.Text>();
        private int _index = 0;
        /// <summary>
        /// Пошаговый индекс точки сходимости
        /// </summary>
        private int ConvergenceIndex = 0;

        private bool forvard;
        private bool backvard;

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
            Texts.Clear();
            ScatterPlots.Clear();
            
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

        private void ClearStartdatrtParams()
        {
            
            Xpoints.Clear();
            Ypoints.Clear();
            Texts.Clear();
            ScatterPlots.Clear();

            ConvergenceIndex = 0;

            forvard = false;
            backvard = false;

            WpfPlot1.UpdateDefaultStyle();
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
        /// <summary>
        /// Кнопка отчиска 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            TxtBxA.Text = string.Empty;
            TxtBxB.Text = string.Empty;
            TxtBxE.Text = string.Empty;
            TxtBxFx.Text = string.Empty;
            ClearStartdatrtParams();

        }
        private double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(TxtBxFx.Text.ToLower());
            expression.Bind("x", X);
            decimal value = expression.Eval<decimal>();
            return (double)value;
        }
        /// <summary>
        /// Возвращает 20%  от расстояние между Max(y) и Min(x).
        /// </summary>
        /// <returns></returns>
        private double ArrowYLength()
        {
            double min = Ypoints[0];
            double max = Ypoints[0];
            foreach (var el in Ypoints)
            {
                if (el < min) min = el;
                if (el > max) max = el;
            }
            return (max - min) * 0.2;
        }
        /// <summary>
        /// Метод который показывает график принимая значение a , b  и  x.  
        /// a = <paramref name="StartPoint"/>, 
        /// b= <paramref name="EndPoint"/>, 
        /// x = <paramref name="Result"/>.
        /// Метод автоматический находит значение F(x)
        /// </summary>
        /// <param name="StartPoint"></param>
        /// <param name="EndPoint"></param>
        /// <param name="Result"></param>
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
                 y, x, y - ArrowYLength(),
                color: System.Drawing.Color.FromName("Red"));
            WpfPlot1.Refresh();
        }
        /// <summary>
        /// Метод для кнопки назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (Xpoints.Count == 0) return;
            if (ScatterPlots.Count == 0 && ConvergenceIndex <= 0 && Texts.Count==0)
            {
                return;
            }
            backvard = true;
            if (forvard)
            {
                --ConvergenceIndex;
                forvard = false;
            }
            int index = ScatterPlots.Count - 1;
            --ConvergenceIndex;
            if (ConvergenceIndex >= 0 && ConvergenceIndex <= Xpoints.Count)
            {
                
                if (ConvergenceIndex >= Xpoints.Count) ConvergenceIndex = Xpoints.Count - 1;
                
                WpfPlot1.Plot.Remove(ScatterPlots[index]);
                WpfPlot1.Plot.Remove(Texts[index]);
                ScatterPlots.RemoveAt(index);
                Texts.RemoveAt(index);
                
                ScatterPlots.Add(WpfPlot1.Plot.AddScatter(
                new double[] { Xpoints[ConvergenceIndex] },
                new double[] { Ypoints[ConvergenceIndex] },
                color: System.Drawing.Color.FromName("Blue"),
                markerShape: MarkerShape.filledCircle,
                markerSize: 10
                ));
                Texts.Add(WpfPlot1.Plot.AddText($"({Math.Round(Xpoints[ConvergenceIndex], 4)}; {Math.Round(Ypoints[ConvergenceIndex], 4)})",
                    Xpoints[ConvergenceIndex], Ypoints[ConvergenceIndex],
                    size: 18,
                    color: System.Drawing.Color.FromName("Black")));

                WpfPlot1.Refresh();
            } else
            {
                //когда в листе осталась один объект, отчистить все точки и значение точки из графика. Остается только график
                if (ScatterPlots.Count == 1)
                { 
                    ScatterPlots.ForEach(el => WpfPlot1.Plot.Remove(el));
                    Texts.ForEach(el => WpfPlot1.Plot.Remove(el));
                    ScatterPlots.Clear();
                    Texts.Clear();
                    WpfPlot1.Refresh();
                }
            }

        }
        /// <summary>
        /// метод для кнопка вперед
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Forward_Click(object sender, RoutedEventArgs e)
        {

            if (Xpoints.Count == 0) return;
            forvard = true;
            //после нажатия кнопку назад делать эти операции
            if (backvard)
            {
                if (ConvergenceIndex < 0) ConvergenceIndex = 0;
                else ++ConvergenceIndex;
                backvard = false;
            }
            
            int index = ScatterPlots.Count;
            if (ConvergenceIndex >= 0 && ConvergenceIndex < Xpoints.Count)
            {
                if (index != 0)
                {
                    //удалить указанный элемент из графика
                    WpfPlot1.Plot.Remove(ScatterPlots[index - 1]);
                    WpfPlot1.Plot.Remove(Texts[index - 1]);
                    //удалить элемент с указанным индексом из списка элементов для удаление
                    ScatterPlots.RemoveAt(index - 1);
                    Texts.RemoveAt(index - 1);
                };
                //Добавить точку к заданном координате
                ScatterPlots.Add(WpfPlot1.Plot.AddScatter(
                new double[] { Xpoints[ConvergenceIndex] },
                new double[] { Ypoints[ConvergenceIndex] },
                color: System.Drawing.Color.FromName("Blue"),
                markerShape: MarkerShape.filledCircle,
                markerSize: 10
                ));
                //Добавить значение точку в виде текста на заданном координате 
                Texts.Add(WpfPlot1.Plot.AddText($"({Math.Round(Xpoints[ConvergenceIndex], 4)}; {Math.Round(Ypoints[ConvergenceIndex], 4)})",
                    Xpoints[ConvergenceIndex], Ypoints[ConvergenceIndex],
                    size: 18,
                    color: System.Drawing.Color.FromName("Black")));
                ++ConvergenceIndex;
                //обновить панел для графика
                WpfPlot1.Refresh();
            }

        }
    }
}
