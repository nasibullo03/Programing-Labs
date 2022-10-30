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

        /// <summary>
        /// для хранение значение координаты графика
        /// </summary>
        private List<double> Xpoints = new List<double>();
        private List<double> Ypoints = new List<double>();
        /// <summary>
        /// для хранение координаты точки сходимоты
        /// </summary>
        private List<List<double>> BeginPoints = new List<List<double>>();
        private List<List<double>> lastPoints = new List<List<double>>();

        /// <summary>
        /// для хранение список точки сходимости в графике 
        /// </summary>
        private List<ScottPlot.Plottable.ScatterPlot> BeginScatterPlots = new List<ScottPlot.Plottable.ScatterPlot>();
        private List<ScottPlot.Plottable.ScatterPlot> LastScatterPlots = new List<ScottPlot.Plottable.ScatterPlot>();
        private List<ScottPlot.Plottable.Text> BeginTexts = new List<ScottPlot.Plottable.Text>();
        private List<ScottPlot.Plottable.Text> LastTexts = new List<ScottPlot.Plottable.Text>();
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
            BeginTexts.Clear();
            BeginScatterPlots.Clear();


            if (Check.CheckTextBoxesValues(UITextBoxes, TxtBxA.Text, TxtBxB.Text))
            {
                double.TryParse(TxtBxA.Text, out var a);
                double.TryParse(TxtBxB.Text, out var b);
                double.TryParse(TxtBxE.Text, out var Accuracy);

                double StartPoint = a;
                double EndPoint = b;
                double x1, x2, k1, k2, F1, F2, Result;


                BeginPoints.Add(new List<double>() { a, F(a) });
                lastPoints.Add(new List<double>() { b, F(b) });

                try
                {
                    k2 = (Math.Sqrt(5) - 1) / 2;

                    k1 = 1 - k2;


                    x1 = k2* a + k1 * b;
                    x2 = k1 * a + k2 * b;

                    F1 = F(x1);
                    F2 = F(x2);
                    

                    while (true)
                    {

                        if (F1 < F2)
                        {
                            b = x2;
                            if (Math.Abs(b - a) < Accuracy)
                            {
                                Result = (a + b) / 2;
                                WpfPlot1.Plot.Clear();
                                WpfPlot1.Refresh();
                                ShowGraph(StartPoint, EndPoint, Result);
                                break;
                            }
                            x2 = x1;
                            F2 = F1;
                            x1 = k2 * a + k1 * b;
                            F1 = F(x1);
                            BeginPoints.Add(new List<double>() { a, F(a) });
                            lastPoints.Add(new List<double>() { b, F(b) });
                        }
                        else
                        {
                            a = x1;
                            if (Math.Abs(b - a) < Accuracy)
                            {
                                Result = (a + b) / 2;
                                WpfPlot1.Plot.Clear();
                                WpfPlot1.Refresh();
                                ShowGraph(StartPoint, EndPoint, Result);
                                break;
                            }
                            x1 = x2;
                            F1 = F2;
                            x2 = k1 * a + k2 * b;
                            F2 = F(x2);
                            BeginPoints.Add(new List<double>() { a, F(a) });
                            lastPoints.Add(new List<double>() { b, F(b) });
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
            BeginPoints.Clear();
            lastPoints.Clear();

            BeginTexts.Clear();
            BeginScatterPlots.Clear();
            LastTexts.Clear();
            LastScatterPlots.Clear();

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
            return expression.Eval<double>();
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
        private double ArrowXLength()
        {
            double min = Xpoints[0];
            double max = Xpoints[0];
            foreach (var el in Xpoints)
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
            if (BeginScatterPlots.Count == 0 && ConvergenceIndex <= 0 && BeginTexts.Count == 0)
            {
                return;
            }
            backvard = true;
            if (forvard)
            {
                --ConvergenceIndex;
                forvard = false;
            }
            int index = BeginScatterPlots.Count - 1;
            --ConvergenceIndex;
            if (ConvergenceIndex >= 0 && ConvergenceIndex <= BeginPoints.Count)
            {

                if (ConvergenceIndex >= BeginPoints.Count) ConvergenceIndex = BeginPoints.Count - 1;

                WpfPlot1.Plot.Remove(BeginScatterPlots[index]);
                WpfPlot1.Plot.Remove(BeginTexts[index]);
                WpfPlot1.Plot.Remove(LastScatterPlots[index]);
                WpfPlot1.Plot.Remove(LastTexts[index]);

                BeginScatterPlots.RemoveAt(index);
                BeginTexts.RemoveAt(index);
                LastScatterPlots.RemoveAt(index);
                LastTexts.RemoveAt(index);

                BeginScatterPlots.Add(WpfPlot1.Plot.AddScatter(
               new double[] { BeginPoints[ConvergenceIndex][0] },
               new double[] { BeginPoints[ConvergenceIndex][1] },
               color: System.Drawing.Color.FromName("Blue"),
               markerShape: MarkerShape.filledCircle,
               markerSize: 10
               ));

                LastScatterPlots.Add(WpfPlot1.Plot.AddScatter(
                new double[] { lastPoints[ConvergenceIndex][0] },
                new double[] { lastPoints[ConvergenceIndex][1] },
                color: System.Drawing.Color.FromName("Blue"),
                markerShape: MarkerShape.filledCircle,
                markerSize: 10
                ));

                BeginTexts.Add(WpfPlot1.Plot.AddText($"({Math.Round(BeginPoints[ConvergenceIndex][0], 5)}; " +
                    $"{Math.Round(BeginPoints[ConvergenceIndex][1], 5)})",
                     BeginPoints[ConvergenceIndex][0] - ArrowXLength(),
                     BeginPoints[ConvergenceIndex][1],
                     size: 18,
                     color: System.Drawing.Color.FromName("Black")));
                LastTexts.Add(WpfPlot1.Plot.AddText($"({Math.Round(lastPoints[ConvergenceIndex][0], 5)}; {Math.Round(lastPoints[ConvergenceIndex][1], 5)})",
                    lastPoints[ConvergenceIndex][0], lastPoints[ConvergenceIndex][1],
                    size: 18,
                    color: System.Drawing.Color.FromName("Black")));

                WpfPlot1.Refresh();
            }
            else
            {
                //когда в листе осталась один объект, отчистить все точки и значение точки из графика. Остается только график
                if (BeginScatterPlots.Count == 1)
                {
                    BeginScatterPlots.ForEach(el => WpfPlot1.Plot.Remove(el));
                    LastScatterPlots.ForEach(el => WpfPlot1.Plot.Remove(el));
                    BeginTexts.ForEach(el => WpfPlot1.Plot.Remove(el));
                    LastTexts.ForEach(el => WpfPlot1.Plot.Remove(el));
                    BeginScatterPlots.Clear();
                    LastScatterPlots.Clear();
                    BeginTexts.Clear();
                    LastTexts.Clear();
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

            if (BeginPoints.Count == 0) return;
            forvard = true;
            //после нажатия кнопку назад делать эти операции
            if (backvard)
            {
                if (ConvergenceIndex < 0) ConvergenceIndex = 0;
                else ++ConvergenceIndex;
                backvard = false;
            }

            int index = BeginScatterPlots.Count;

            if (ConvergenceIndex >= 0 && ConvergenceIndex < BeginPoints.Count)
            {
                if (BeginScatterPlots.Count != 0)
                {
                    //удалить указанный элемент из графика
                    WpfPlot1.Plot.Remove(BeginScatterPlots[index - 1]);
                    WpfPlot1.Plot.Remove(BeginTexts[index - 1]);

                    WpfPlot1.Plot.Remove(LastScatterPlots[index - 1]);
                    WpfPlot1.Plot.Remove(LastTexts[index - 1]);
                    //удалить элемент с указанным индексом из списка элементов для удаление
                    BeginScatterPlots.RemoveAt(index - 1);
                    BeginTexts.RemoveAt(index - 1);
                    LastScatterPlots.RemoveAt(index - 1);
                    LastTexts.RemoveAt(index - 1);
                };
                //Добавить точку к заданном координате
                BeginScatterPlots.Add(WpfPlot1.Plot.AddScatter(
                new double[] { BeginPoints[ConvergenceIndex][0] },
                new double[] { BeginPoints[ConvergenceIndex][1] },
                color: System.Drawing.Color.FromName("Blue"),
                markerShape: MarkerShape.filledCircle,
                markerSize: 10
                ));

                LastScatterPlots.Add(WpfPlot1.Plot.AddScatter(
                new double[] { lastPoints[ConvergenceIndex][0] },
                new double[] { lastPoints[ConvergenceIndex][1] },
                color: System.Drawing.Color.FromName("Blue"),
                markerShape: MarkerShape.filledCircle,
                markerSize: 10
                ));
                //Добавить значение точку в виде текста на заданном координате 
                BeginTexts.Add(WpfPlot1.Plot.AddText($"({Math.Round(BeginPoints[ConvergenceIndex][0], 5)}; " +
                    $"{Math.Round(BeginPoints[ConvergenceIndex][1], 5)})",
                    BeginPoints[ConvergenceIndex][0] - ArrowXLength(),
                    BeginPoints[ConvergenceIndex][1],
                    size: 18,
                    color: System.Drawing.Color.FromName("Black")));

                LastTexts.Add(WpfPlot1.Plot.AddText($"({Math.Round(lastPoints[ConvergenceIndex][0], 5)}; " +
                    $"{Math.Round(lastPoints[ConvergenceIndex][1], 5)})",
                    lastPoints[ConvergenceIndex][0],
                    lastPoints[ConvergenceIndex][1],
                    size: 18,
                    color: System.Drawing.Color.FromName("Black")));
                ++ConvergenceIndex;
                //обновить панел для графика
                WpfPlot1.Refresh();
            }

        }
    }
}
