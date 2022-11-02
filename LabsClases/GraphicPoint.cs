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
        /// <summary>
        /// Коллекция значение для изменение значение
        /// </summary>
        public static List<GraphicPoint> EditableList { get; set; }

        public static bool EditMode { get; set; }
        private static int Count { get; set; }
        public int Index { get; set; }
        public double Xi { get; set; }
        public double Yi { get; set; }

        public GraphicPoint()
        {
        }
        public GraphicPoint(double Xi, double Yi)
        {
            this.Index = ++Count;
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
                    GraphicPoints.Remove(el as GraphicPoint);
                }
            }
            var graphicPointsCollection = new ObservableCollection<GraphicPoint>();
            Count = 0;
            foreach (var el in GraphicPoints.ToArray())
            {
                el.Index = ++Count;
                graphicPointsCollection.Add(el);
            }

            GraphicPointsCollection = graphicPointsCollection;
            GraphicPointsView.ItemsSource = GraphicPointsCollection;
            GraphicPointsView.UpdateLayout();

        }

        public static void Clear()
        {
            GraphicPointsView.ItemsSource = new ObservableCollection<GraphicPoint>();
            GraphicPointsCollection.Clear();
            GraphicPoints.Clear();
            Count = 0;
            GraphicPointsView.ItemsSource = GraphicPointsCollection;
            EditableList.Clear();
        }

        public static void PrepareDataForEditing(TextBox[] UITextBoxes, Label LblXi, Label LblYi)
        {

            int _index = (EditableList[0] as GraphicPoint).Index;

            LblXi.Content = $"X({_index})";
            LblYi.Content = $"Y({_index})";

            UITextBoxes[0].Text = (EditableList[0] as GraphicPoint).Xi.ToString();
            UITextBoxes[1].Text = (EditableList[0] as GraphicPoint).Yi.ToString();

        }
        public static void EditValues(double Xi, double Yi)
        {
            GraphicPointsCollection[(EditableList[0] as GraphicPoint).Index - 1].Xi = Xi;
            GraphicPointsCollection[(EditableList[0] as GraphicPoint).Index - 1].Yi = Yi;

            GraphicPoints[(EditableList[0] as GraphicPoint).Index - 1].Xi = Xi;
            GraphicPoints[(EditableList[0] as GraphicPoint).Index - 1].Yi = Yi;

            GraphicPointsView.ItemsSource = new ObservableCollection<GraphicPoint>();
            GraphicPointsView.ItemsSource = GraphicPointsCollection;


        }

        public static void DeleteEditedValue()
        {
           if(EditableList.Count!=0)
            EditableList.RemoveAt(0);
        }
    }
}
