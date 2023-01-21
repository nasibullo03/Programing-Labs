using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation

namespace Programing_Labs.Pages.OlympSort
{
    class Sort
    {
        #region properties
        public static CancellationTokenSource cancellationToken { get; set; }
        private static double temp { get; set; }
        public static ListView SortDataView { get; set; }
        public string sortType { get; set; }
        public string TimerValue { get; set; }
        public string ArraySize { get; set; }

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
        public Sort(SortType type, long TimerValue, string ArraySize)
        {
            sortType = SortTypeValue[type];
            this.TimerValue = TimerValue.ToString() + " мс";
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
            double[] datas = Dispatcher.CurrentDispatcher.Invoke(() => Data.GetValues(Data.Value.Xi));
            if (!reverse)
                for (int i = 1; i < datas.Length; ++i)
                {
                    double temp = datas[i];
                    int j = i - 1;
                    while (j >= 0 && datas[j] > temp)
                    {
                        datas[j + 1] = datas[j];
                        j = j - 1;
                    }

                    datas[j + 1] = temp;

                }
            else
                for (int i = 1; i < datas.Length; ++i)
                {
                    double temp = datas[i];
                    int j = i - 1;
                    while (j >= 0 && datas[j] < temp)
                    {
                        datas[j + 1] = datas[j];
                        j = j - 1;
                    }

                    datas[j + 1] = temp;

                }
            return datas;
        }
        public static double[] ShakerSort(bool reverse)
        {
            double[] datas = Dispatcher.CurrentDispatcher.Invoke(() => Data.GetValues(Data.Value.Xi));
            if (!reverse)
                for (var i = 0; i < datas.Length / 2; i++)
                {
                    var swapFlag = false;

                    for (var j = i; j < datas.Length - i - 1; j++)
                    {
                        if (datas[j] > datas[j + 1])
                        {
                            (datas[j], datas[j + 1]) = (datas[j + 1], datas[j]);
                            swapFlag = true;
                        }
                    }

                    for (var j = datas.Length - 2 - i; j > i; j--)
                    {
                        if (datas[j - 1] > datas[j])
                        {
                            (datas[j - 1], datas[j]) = (datas[j], datas[j - 1]);
                            swapFlag = true;
                        }
                    }

                    if (!swapFlag)
                    {
                        break;
                    }
                }
            else
                for (var i = 0; i < datas.Length / 2; i++)
                {
                    var swapFlag = false;

                    for (var j = i; j < datas.Length - i - 1; j++)
                    {
                        if (datas[j] < datas[j + 1])
                        {
                            (datas[j], datas[j + 1]) = (datas[j + 1], datas[j]);
                            swapFlag = true;
                        }
                    }

                    for (var j = datas.Length - 2 - i; j > i; j--)
                    {
                        if (datas[j - 1] < datas[j])
                        {
                            (datas[j - 1], datas[j]) = (datas[j], datas[j - 1]);
                            swapFlag = true;
                        }
                    }

                    if (!swapFlag)
                    {
                        break;
                    }
                }

            return datas;
        }
        public static double[] FastSort(double[] datas, int LeftIndex, int RightIndex, bool reverse)
        {
            int i = LeftIndex;
            int j = RightIndex;
            double bar = datas[(LeftIndex+RightIndex)/2];

            if (!reverse)
                while (i <= j)
                {
                    while (datas[i] < bar) ++i;

                    while (datas[j] > bar) --j;

                    if (i <= j)
                    {
                        (datas[i], datas[j]) = (datas[j], datas[i]);

                        ++i; --j;

                    }
                }

            else
                while (i <= j)
                {
                    while (datas[i] > bar) ++i;

                    while (datas[j] < bar) --j;

                    if (i <= j)
                    {
                        (datas[i], datas[j]) = (datas[j], datas[i]);

                        ++i; --j;

                    }
                }


            if (LeftIndex < j)
                FastSort(datas, LeftIndex, j, reverse);

            if (i < RightIndex)
                FastSort(datas, i, RightIndex, reverse);

            return datas;
        }
        public static double[] BogoSort(bool reverse)
        {
            double[] datas = Dispatcher.CurrentDispatcher.Invoke(() => Data.GetValues(Data.Value.Xi));
            Random random = new Random();
            if (!reverse)
                while (!IsSorted(datas))
                {
                    for (int i = datas.Length - 1; i > 0; --i)
                    {
                        int n = random.Next(i + 1);
                        (datas[i], datas[n]) = (datas[n], datas[i]);
                    }
                }
            else
                while (!IsSorted_Revese(datas))
                {
                    for (int i = datas.Length - 1; i > 0; --i)
                    {
                        int n = random.Next(i + 1);
                        (datas[i], datas[n]) = (datas[n], datas[i]);
                    }
                }
            return datas;
        }
        static bool IsSorted(double[] datas)
        {
            for (int i = 0; i < datas.Length - 1; i++)
                if (datas[i] > datas[i + 1])
                    return false;

            return true;
        }
        static bool IsSorted_Revese(double[] datas)
        {
            for (int i = 0; i < datas.Length - 1; i++)
                if (datas[i] < datas[i + 1])
                    return false;

            return true;
        }

        #endregion



    }
}
