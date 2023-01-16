using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Programing_Labs.Pages.OlypmSort
{
    class Data
    {
        #region properties
        public static ListView SortDataView { get; set; }
        /// <summary>
        /// Коллекция значение для изменение значение
        /// </summary>
        public static List<Data> EditableList { get; set; }

        public static bool EditMode { get; set; }
        private static int Count { get; set; }
        public int Index { get; set; }
        public double Xi { get; set; }
        public double SortedXi { get; set; }

        public static List<Data> SortDatas = new List<Data>();
        public static ObservableCollection<Data> SortDataCollection = new ObservableCollection<Data>();
        #endregion
        #region constructors
        public Data()
        {

        }
        public Data(double Xi)
        {
            this.Index = ++Count;
            this.Xi = Xi;
        }
        #endregion

        public async static Task Add(Data sortData)
        {
            SortDatas.Add(sortData);
            SortDataCollection.Add(sortData);
            await Task.Yield();
        }

        public static void Remove(System.Collections.IList item)
        {
            foreach (object el in SortDataView.Items)
            {
                if (item.Contains(el))
                {
                    SortDatas.Remove(el as Data);
                }
            }
            var sortDataCollection = new ObservableCollection<Data>();
            Count = 0;
            foreach (var el in SortDatas.ToArray())
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
            SortDataView.ItemsSource = new ObservableCollection<Data>();
            SortDataCollection?.Clear();
            SortDatas?.Clear();
            Count = 0;
            SortDataView.ItemsSource = SortDataCollection;
            EditableList?.Clear();
        }
        public static void PrepareDataForEditing(TextBox XiTextBox, Label LblXi)
        {

            int _index = (EditableList[0] as Data).Index;

            LblXi.Content = $"X({_index})";

            XiTextBox.Text = (EditableList[0] as Data).Xi.ToString();

        }
        public static void EditValues(double Xi)
        {
            SortDataCollection[(EditableList[0] as Data).Index - 1].Xi = Xi;
            SortDatas[(EditableList[0] as Data).Index - 1].Xi = Xi;

            SortDataView.ItemsSource = new ObservableCollection<Data>();
            SortDataView.ItemsSource = SortDataCollection;


        }
        public static void DeleteEditedValue()
        {
            if (EditableList.Count != 0)
                EditableList.RemoveAt(0);
        }


    }
}
