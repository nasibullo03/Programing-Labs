using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Programing_Labs.LabsClases
{
    class GraphicPoint
    {
        public static ListView GraphicPointsView { get; set; }
        private static int Count { get; set; }
        public int index { get; set; }
        public double Xi { get; set; }
        public double Yi { get; set; }

        public GraphicPoint()
        {
        }
        public GraphicPoint(double Xi, double Yi)
        {
            this.index = ++Count;
            this.Xi = Xi;
            this.Yi = Yi;
        }

        public static List<GraphicPoint> GraphicPoints = new List<GraphicPoint>();
        public static ObservableCollection<GraphicPoint> GraphicPointsCollection = new ObservableCollection<GraphicPoint>();
        public async static Task Add(GraphicPoint graphicPoint)
        {
            GraphicPoints.Add(graphicPoint);
            GraphicPointsCollection.Add(graphicPoint);
            await Task.Yield();
        }
        public static void Remove(System.Collections.IList item)
        {
            foreach (object el in GraphicPointsView.Items)
            {
                if (item.Contains(el))
                {
                    GraphicPoints.Remove(el as LabsClases.GraphicPoint);
                }
            }
            var graphicPointsCollection = new ObservableCollection<GraphicPoint>();
            Count = 0;
            foreach (var el in GraphicPoints.ToArray())
            {
                el.index = ++Count;
                graphicPointsCollection.Add(el);
            }

            GraphicPointsCollection = graphicPointsCollection;
            GraphicPointsView.ItemsSource = LabsClases.GraphicPoint.GraphicPointsCollection;
            GraphicPointsView.UpdateLayout();

        }


    }
}
