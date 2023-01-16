using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Programing_Labs.Pages.OlypmSort
{
    class SortData
    {
        #region properties
        public static ListView SortDataView { get; set; }
        /// <summary>
        /// Коллекция значение для изменение значение
        /// </summary>
        public static List<SortData> EditableList { get; set; }

        public static bool EditMode { get; set; }
        private static int Count { get; set; }
        public int Index { get; set; }
        public double Xi { get; set; }

        public static List<SortData> GraphicPoints = new List<SortData>();
        public static ObservableCollection<SortData> SortDataCollection = new ObservableCollection<SortData>();
        #endregion
        #region constructors
        public SortData()
        {

        }
        public SortData(double Xi)
        {
            this.Index = ++Count;
            this.Xi = Xi;
        }
        #endregion

        public async static Task Add(SortData sortData)
        {
            GraphicPoints.Add(sortData);
            SortDataCollection.Add(sortData);
            await Task.Yield();
        }

        public static void Remove(System.Collections.IList item)
        {
            foreach (object el in SortDataView.Items)
            {
                if (item.Contains(el))
                {
                    GraphicPoints.Remove(el as SortData);
                }
            }
            var sortDataCollection = new ObservableCollection<SortData>();
            Count = 0;
            foreach (var el in GraphicPoints.ToArray())
            {
                el.Index = ++Count;
                sortDataCollection.Add(el);
            }

            SortDataCollection = sortDataCollection;
            SortDataView.ItemsSource = SortDataCollection;
            SortDataView.UpdateLayout();

        }

        public static void Clear()
        {
            SortDataView.ItemsSource = new ObservableCollection<SortData>();
            SortDataCollection?.Clear();
            GraphicPoints?.Clear();
            Count = 0;
            SortDataView.ItemsSource = SortDataCollection;
            EditableList?.Clear();
        }
        public static void PrepareDataForEditing(TextBox XiTextBox, Label LblXi)
        {

            int _index = (EditableList[0] as SortData).Index;

            LblXi.Content = $"X({_index})";

            XiTextBox.Text = (EditableList[0] as SortData).Xi.ToString();

        }
        public static void EditValues(double Xi)
        {
            SortDataCollection[(EditableList[0] as SortData).Index - 1].Xi = Xi;
            GraphicPoints[(EditableList[0] as SortData).Index - 1].Xi = Xi;

            SortDataView.ItemsSource = new ObservableCollection<SortData>();
            SortDataView.ItemsSource = SortDataCollection;


        }
        public static void DeleteEditedValue()
        {
            if (EditableList.Count != 0)
                EditableList.RemoveAt(0);
        }


    }
}
