using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programing_Labs.LabsClases
{
    class GraphicPoint
    {
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
        public static void Remove(GraphicPoint item)
        {
            GraphicPoints.Remove(item);
            GraphicPointsCollection.Remove(item);

        }


    }
}
