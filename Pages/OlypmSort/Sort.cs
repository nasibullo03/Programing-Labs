using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Programing_Labs.Pages.OlypmSort
{
    class Sort
    {
        public static CancellationTokenSource cancellationToken { get; set; }
        private static Data temp { get; set; }
        public Sort()
        {

        }

        public static void BubleSort(bool reverse) 
        {
            
            for (int i = 0; i < Data.SortDatas.Count; i++)
            {
                for (int j = 0; j < Data.SortDatas.Count - 1 - i; j++)
                {
                    if (!reverse && Data.SortDatas[j].Xi > Data.SortDatas[j + 1].Xi || reverse && Data.SortDatas[j].Xi < Data.SortDatas[j + 1].Xi)
                    {
                        Data.SortDatas[j].SortedXi = Data.SortDatas[j + 1].Xi;
                        Data.SortDatas[j + 1].SortedXi = Data.SortDatas[j].Xi;
                        if (cancellationToken.Token.IsCancellationRequested) return;
                        
                    }
                }
            }
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
