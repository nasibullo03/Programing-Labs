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
        public Sort()
        {

        }

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

                            /*if (cancellationToken.Token.IsCancellationRequested) return;*/
                        }
                    }

                }
            else
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    for (int j = 0; j < datas.Length - 1 - i; j++)
                    {
                        if (datas[j] < datas[j + 1])
                        {
                            temp = datas[j + 1];
                            datas[j + 1] = datas[j];
                            datas[j] = temp;

                            /*if (cancellationToken.Token.IsCancellationRequested) return;*/

                        }
                    }

                }
            }


            return datas;
        }
        public static List<Data> InsertSort(List<Data> data, bool reverse)
        {

            return new List<Data>();
        }
        public static List<Data> ShakerSort(List<Data> data, bool reverse)
        {

            return new List<Data>();
        }
        public static List<Data> FastSort(List<Data> data, bool reverse)
        {

            return new List<Data>();
        }
        public static List<Data> BogoSort(List<Data> data, bool reverse)
        {

            return new List<Data>();
        }








    }
}
