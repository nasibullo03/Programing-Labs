using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation

namespace Programing_Labs.Pages.OlympSort
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
        public enum Value { Xi, SortedXi }

        #endregion
        #region constructors
        public Data(double Xi)
        {
            this.Index = ++Count;
            this.Xi = Xi;
        }

        #endregion

        public static async Task Add(Data sortData)
        {
            SortDatas.Add(sortData);
            SortDataCollection.Add(sortData);
            await Task.Yield();
        }
        public static void Add(double[] datas, CancellationToken token)
        {
            Data data1;

            foreach (double data in datas)
            {
                data1 = new Data(data);
                SortDatas.Add(data1);
            }

            token.ThrowIfCancellationRequested();

            SortDataView.Dispatcher.Invoke(() => UpdateCollection());

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
            SortDataView.Items.Refresh();

        }

        public static void UpdateCollection()
        {
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
            SortDataCollection?.Clear();
            SortDatas?.Clear();
            Count = 0;
            EditableList?.Clear();
            SortDataView.Items.Refresh();
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

        public static double[] GetValues(Value value)
        {
            double[] data = new double[SortDatas.Count];

            switch (value)
            {
                case Value.Xi:
                    for (int i = 0; i < SortDatas.Count; ++i)
                        data[i] = SortDatas[i].Xi;
                    break;
                case Value.SortedXi:
                    for (int i = 0; i < SortDatas.Count; ++i)
                        data[i] = SortDatas[i].SortedXi;
                    break;
            }

            return data;

        }
        public static void SetValues(double[] data, Value value, CancellationToken token)
        {
            Lab5_Page.LoadingLabelText("Идет обработка данных");

            token.ThrowIfCancellationRequested();

            if (data == null) return;

            switch (value)
            {
                case Value.Xi:

                    for (int i = 0; i < SortDatas.Count; ++i)
                    {
                        SortDatas[i].Xi = data[i];
                    }
                    break;
                case Value.SortedXi:
                    for (int i = 0; i < SortDatas.Count; ++i)
                    {
                        SortDatas[i].SortedXi = data[i];
                    }

                    break;
            }

            SortDataView.Dispatcher.Invoke(() => UpdateCollection());
        }
        public static async void GererateData(int ArraySize, CancellationToken token)
        {
            var task = Task.Run(() =>
           {
               Random random = new Random();
               double[] data = new double[ArraySize];
               for (int i = 0; i < ArraySize; ++i)
               {
                   data[i] = random.Next();

                   token.ThrowIfCancellationRequested();
               }
               Lab5_Page.LoadingLabelText("Идет обработка данных");
               Add(data, token);

               Lab5_Page.LoadingPanel1.Dispatcher.Invoke(() => Lab5_Page.LoadingPanel1.Visibility = Visibility.Collapsed);
               Lab5_Page.MenuItemCancell1.Dispatcher.Invoke(() => Lab5_Page.MenuItemCancell1.Visibility = Visibility.Collapsed);
           }, token);
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Lab5_Page.LoadingPanel1.Dispatcher.Invoke(() => Lab5_Page.LoadingPanel1.Visibility = Visibility.Collapsed);
                Lab5_Page.MenuItemCancell1.Dispatcher.Invoke(() => Lab5_Page.MenuItemCancell1.Visibility = Visibility.Collapsed);

            }
            finally
            {
                Lab5_Page.cancellationToken.Dispose();

            }
        }

    }
}
