using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Windows.Controls;
using Programing_Labs.Pages;

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
    public static void Add(double[] Xpoints, double[] Ypoints, int ArraySize, CancellationToken token)
    {
      GraphicPoint data1;

      for (int i = 0; i < ArraySize; ++i)
      {
        data1 = new GraphicPoint(Xpoints[i], Ypoints[i]);
        GraphicPoints.Add(data1);
      }

      token.ThrowIfCancellationRequested();

      GraphicPointsView.Dispatcher.Invoke(() => UpdateCollection());
    }
    public static void UpdateCollection()
    {
      var sortDataCollection = new ObservableCollection<GraphicPoint>();

      Count = 0;

      foreach (var el in GraphicPoints.ToArray())
      {
        el.Index = ++Count;
        sortDataCollection.Add(el);
      }

      GraphicPointsCollection = sortDataCollection;
      GraphicPointsView.ItemsSource = GraphicPointsCollection;
      GraphicPointsView.UpdateLayout();
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
      GraphicPointsCollection?.Clear();
      GraphicPoints?.Clear();
      Count = 0;
      GraphicPointsView.ItemsSource = GraphicPointsCollection;
      EditableList?.Clear();
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
      if (EditableList.Count != 0)
        EditableList.RemoveAt(0);
    }

    public static async void  GenerateData(int ArraySize, CancellationToken token)
    {
      var task = Task.Run(() =>
      {
        Random random = new Random();

        double[] XPoints = new double[ArraySize];
        double[] YPoints = new double[ArraySize];
        for (int i = 0; i < ArraySize; ++i)
        {
          XPoints[i] = random.Next();
          YPoints[i] = random.Next();

          token.ThrowIfCancellationRequested();
        }
        /* Lab5_Page.LoadingLabelText("Идет обработка данных");*/
        GraphicPoints.Clear();
        Add(XPoints, YPoints, ArraySize, token);

        /* Lab5_Page.LoadingPanel1.Dispatcher.Invoke(() => Lab5_Page.LoadingPanel1.Visibility = Visibility.Collapsed);
         Lab5_Page.MenuItemCancell1.Dispatcher.Invoke(() => Lab5_Page.MenuItemCancell1.Visibility = Visibility.Collapsed);*/
      }, token);
      try
      {
        await task;
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(ex.Message);
        /*Lab5_Page.LoadingPanel1.Dispatcher.Invoke(() => Lab5_Page.LoadingPanel1.Visibility = Visibility.Collapsed);
        Lab5_Page.MenuItemCancell1.Dispatcher.Invoke(() => Lab5_Page.MenuItemCancell1.Visibility = Visibility.Collapsed);*/

      }
      finally
      {
        Lab4_Page.cancellationToken.Dispose();

      }
    }

  }
}
}
