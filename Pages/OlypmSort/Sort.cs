using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Programing_Labs.Pages.OlypmSort
{
    class Sort
    {
        #region properties
        public static CancellationTokenSource cancellationToken { get; set; }
        private static double temp { get; set; }
        public static ListView SortDataView { get; set; }
        /// <summary>
        /// Коллекция значение для изменение значение
        /// </summary>
        public static List<Sort> EditableList { get; set; }

        public static bool EditMode { get; set; }

        public string sortType { get; set; }
        public string TimerValue { get; set; }
        public string  ArraySize { get; set; }

        public static List<Sort> SortDatas = new List<Sort>();
        public static ObservableCollection<Sort> SortDataCollection = new ObservableCollection<Sort>();

        public enum SortType { Buble, Insert, Shaker, Fast, Bogo }
        public static Dictionary<SortType, string> SortTypeValue = new Dictionary<SortType, string>
        {
            {SortType.Buble, "Пузырковая сортировка"},
            {SortType.Insert, "Сортировка вставками"},
            {SortType.Shaker, "Шейкерная сортировка"},
            {SortType.Fast, "Быстрая сортировка"},
            {SortType.Bogo, "Bogo - сортировка"},
        };

        #endregion
        #region constructor
        public Sort()
        {

        }
        public Sort(SortType type,  long TimerValue, string ArraySize)
        {
            sortType = SortTypeValue[type];
            this. TimerValue = TimerValue.ToString() + " мс";
            this.ArraySize = ArraySize;
        }
        #endregion
        #region ListView methods 
        public async static Task Add(Sort sortData)
        {
            SortDatas.Add(sortData);
            SortDataCollection.Add(sortData);
            await Task.Yield();
        }

        public static void Clear()
        {
            SortDataView.ItemsSource = new ObservableCollection<Sort>();
            SortDataCollection?.Clear();
            SortDatas?.Clear();
            SortDataView.ItemsSource = SortDataCollection;
            EditableList?.Clear();
        }


        #endregion
        #region Sorting Methods
        public static double[] BubleSort(bool reverse)
        {
            double[] datas = Dispatcher.CurrentDispatcher.Invoke(() => Data.GetValues(Data.Value.Xi));

            if (!reverse)
                for (int i = 0; i < datas.Length; i++)
                {
                    for (int j = 0; j < datas.Length - 1 - i; j++)
                    {
                        if (datas[j] > datas[j + 1])
                        {
                            temp = datas[j + 1];
                            datas[j + 1] = datas[j];
                            datas[j] = temp;
                        }
                    }
                }
            else
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    for (int j = 0; j < datas.Length - 1 - i; j++)

                        if (datas[j] < datas[j + 1])
                        {
                            temp = datas[j + 1];
                            datas[j + 1] = datas[j];
                            datas[j] = temp;
                        }


                }
            }


            return datas;
        }
        public static double[] InsertSort(bool reverse)
        {

            return null;
        }
        public static double[] ShakerSort(bool reverse)
        {

            return null;
        }
        public static double[] FastSort(bool reverse)
        {

            return null;
        }
        public static double[] BogoSort(bool reverse)
        {

            return null;
        }

        #endregion



    }
}
