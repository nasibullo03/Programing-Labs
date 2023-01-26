using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;
using ScottPlot.WPF;


namespace Programing_Labs.Pages.DefiniteIntegral
{
    class GraphVisualize
    {
        public static WpfPlot WpfPlot1 { get; set; }
        public static WpfPlot WpfPlot2 { get; set; }
        public static WpfPlot WpfPlot3 { get; set; }
        
        #region Function and Graph Visualization
        public static void Visualize(RectangleMethod method)
        {
            WpfPlot1.Plot.Title($"S = {method.OptimalSplitValue}");
            VisualizeSplits(method);
            VisualizeGraphLine(method.FunctionCoordinates, DefiniteIntegral_Page.MethodType.Rectangle);
            VisualizeSplitsDots(method, DefiniteIntegral_Page.MethodType.Rectangle);
            WpfPlot1.Refresh();
        }
        public static void Visualize(TrapezoidalMethod method)
        {
            WpfPlot2.Plot.Title($"S = {method.OptimalSplitValue}");
            FillColor(method);
            VisualizeSplits(method);
            VisualizeGraphLine(method.FunctionCoordinates, DefiniteIntegral_Page.MethodType.Trepezoida);
            VisualizeSplitsDots(method, DefiniteIntegral_Page.MethodType.Trepezoida);
            WpfPlot2.Refresh();
        }
        public static void Visualize(SimpsonMethod method)
        {
            WpfPlot3.Plot.Title($"S = {method.OptimalSplitValue}");
            FillColor(method);
            VisualizeSplits(method);
            VisualizeGraphLine(method.FunctionCoordinates, DefiniteIntegral_Page.MethodType.Simpson);
            VisualizeSplitsDots(method, DefiniteIntegral_Page.MethodType.Simpson);
            WpfPlot3.Refresh();
        }

        private static void VisualizeSplits(RectangleMethod value)
        {
            double SplitDistance = (value.SplitCoordinates.Count > 1) ? value.SplitCoordinates[1].X - value.SplitCoordinates[0].X : 1;
            double correctValue = SplitDistance / 2;

            double[] Xvalues = value.SplitCoordinates.Select(a => a.X).ToArray();
            double[] Yvalues = value.SplitCoordinates.Select(a => a.Y).ToArray();

            double[] values = Yvalues;

            for (int i = 0; i < Xvalues.Length; ++i)
            {
                Xvalues[i] -= correctValue;
            }

            // создание гистограмма 
            (double[] probabilities, double[] binEdges) = (Yvalues.ToArray(), Xvalues.ToArray());
            double[] leftEdges = binEdges.Take(binEdges.Length).ToArray();

            // отображение вероятности гистограммы в виде столбчатой диаграммы
            var bar = WpfPlot1.Plot.AddBar(values: probabilities, positions: leftEdges);
            bar.BarWidth = SplitDistance;
            bar.FillColor = ColorTranslator.FromHtml("#9bc3eb");
            bar.BorderColor = ColorTranslator.FromHtml("#82add9");

            // отображение кривой распределения гистограммы в виде линейного графика
            double[] densities = ScottPlot.Statistics.Common.ProbabilityDensity(values, binEdges);

            WpfPlot1.Plot.AddScatterLines(
                xs: binEdges,
                ys: densities,
                lineWidth: 1,
                lineStyle: LineStyle.Dash);
        }
        private static void VisualizeSplits(SimpsonMethod value)
        {
            double[] Xvalues = value.SplitCoordinates.Select(a => a.X).ToArray();
            double[] Yvalues = value.SplitCoordinates.Select(a => a.Y).ToArray();

            double Ymin = Yvalues.Min();

            for (int i = 0; i < Xvalues.Length; ++i)
            {
                WpfPlot3.Plot.AddScatter(
              new double[] { Xvalues[i], Xvalues[i] },
              new double[] { Ymin, Yvalues[i] },
               markerShape: ScottPlot.MarkerShape.none, lineWidth: 4,
               color: ColorTranslator.FromHtml("#82add9"));
            }

        }
        private static void VisualizeSplits(TrapezoidalMethod value)
        {
            double[] Xvalues = value.SplitCoordinates.Select(a => a.X).ToArray();
            double[] Yvalues = value.SplitCoordinates.Select(a => a.Y).ToArray();

            double Ymin = Yvalues.Min();

            for (int i = 0; i < Xvalues.Length; ++i)
            {
                WpfPlot2.Plot.AddScatter(
              new double[] { Xvalues[i], Xvalues[i] },
              new double[] { Ymin, Yvalues[i] },
               markerShape: ScottPlot.MarkerShape.none, lineWidth: 4,
               color: ColorTranslator.FromHtml("#82add9"));
            }
            WpfPlot2.Plot.AddScatter(Xvalues,
              Yvalues,
               markerShape: ScottPlot.MarkerShape.none, lineWidth: 4,
               color: ColorTranslator.FromHtml("#82add9"));

        }

        private static void FillColor(SimpsonMethod value)
        {
            var values = value.FunctionCoordinates;
            double[] Xvalues = values.Select(a => a.X).ToArray();
            double[] Yvalues = values.Select(a => a.Y).ToArray();

            WpfPlot3.Plot.AddFill(Xvalues, Yvalues, color: ColorTranslator.FromHtml("#9bc3eb"));
            WpfPlot3.Plot.SetAxisLimits(xMin: Xvalues.Min(), xMax: Xvalues.Max());
        }
        private static void FillColor(TrapezoidalMethod value)
        {
            var values = value.SplitCoordinates;
            double[] Xvalues = values.Select(a => a.X).ToArray();
            double[] Yvalues = values.Select(a => a.Y).ToArray();

            WpfPlot2.Plot.AddFill(Xvalues, Yvalues, color: ColorTranslator.FromHtml("#9bc3eb"));
            WpfPlot2.Plot.SetAxisLimits(xMin: Xvalues.Min(), xMax: Xvalues.Max());
        }
        private static void VisualizeSplitsDots(IOutputValue outputValue, DefiniteIntegral_Page.MethodType method)
        {
            switch (method)
            {
                case DefiniteIntegral_Page.MethodType.Rectangle:
                    foreach (var el in outputValue.SplitCoordinates)
                    {
                        WpfPlot1.Plot.AddScatter(
                        new double[] { el.X },
                        new double[] { el.Y },
                        color: System.Drawing.Color.FromName("Green"),
                        markerSize: 7);
                    }
                    break;
                case DefiniteIntegral_Page.MethodType.Trepezoida:
                    foreach (var el in outputValue.SplitCoordinates)
                    {
                        WpfPlot2.Plot.AddScatter(
                        new double[] { el.X },
                        new double[] { el.Y },
                        color: System.Drawing.Color.FromName("Green"),
                        markerSize: 7);
                    }
                    break;
                case DefiniteIntegral_Page.MethodType.Simpson:
                    foreach (var el in outputValue.SplitCoordinates)
                    {
                        WpfPlot3.Plot.AddScatter(
                        new double[] { el.X },
                        new double[] { el.Y },
                        color: System.Drawing.Color.FromName("Green"),
                        markerSize: 7);
                    }
                    break;
            }

            

        }
        private static void VisualizeGraphLine(List<System.Windows.Point> points, DefiniteIntegral_Page.MethodType method)
        {
            switch (method)
            {
                case DefiniteIntegral_Page.MethodType.Rectangle:
                    WpfPlot1.Plot.AddScatter(
               points.Select(i => i.X).ToArray(),
               points.Select(i => i.Y).ToArray(),
               markerShape: ScottPlot.MarkerShape.none, lineWidth: 3);
                    break;
                case DefiniteIntegral_Page.MethodType.Trepezoida:
                    WpfPlot2.Plot.AddScatter(
              points.Select(i => i.X).ToArray(),
              points.Select(i => i.Y).ToArray(),
              markerShape: ScottPlot.MarkerShape.none, lineWidth: 3);
                    break;
                case DefiniteIntegral_Page.MethodType.Simpson:
                    WpfPlot3.Plot.AddScatter(
              points.Select(i => i.X).ToArray(),
              points.Select(i => i.Y).ToArray(),
              markerShape: ScottPlot.MarkerShape.none, lineWidth: 3);
                    break;
            }

            
        }
        #endregion
    }
}
