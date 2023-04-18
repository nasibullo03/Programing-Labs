using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation

namespace Programing_Labs.Pages.OlympSort
{
  public class Sort
  {
    #region properties

    private static double TemproraryDouble { get; set; }
    public string sortType { get; set; }
    public string TimerValue { get; set; }
    public string ArraySize { get; set; }
    public static ListView SortDataView { get; set; }
    public static List<Sort> SortDatas { get; set; } = new List<Sort>();
    public static ObservableCollection<Sort> SortDataCollection { get; set; } = new ObservableCollection<Sort>();
    public static CancellationToken Token { get; set; }
    public enum SortType { Buble, Insert, Shaker, Fast, Bogo, AllTypes }
    public static Dictionary<SortType, string> SortTypeValue { get; set; } = new Dictionary<SortType, string>
        {
            {SortType.Buble, "Пузырковая сортировка"},
            {SortType.Insert, "Сортировка вставками"},
            {SortType.Shaker, "Шейкерная сортировка"},
            {SortType.Fast, "Быстрая сортировка"},
            {SortType.Bogo, "Bogo - сортировка"},
        };
    #endregion
    #region constructor
    public Sort(SortType type, string TimerValue, string ArraySize)
    {
      sortType = SortTypeValue[type];
      this.TimerValue = TimerValue;
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
      SortDataCollection?.Clear();
      SortDatas?.Clear();
      SortDataView.Items.Refresh();
    }

    #endregion
    #region Sorting Methods
    public static double[] BubleSort(bool reverse)
    {
      double[] datas = Data.GetValues(Data.Value.Xi).Clone() as double[];

      if (!reverse)
        for (int i = 0; i < datas.Length; i++)
        {
          for (int j = 0; j < datas.Length - 1 - i; j++)
          {
            if (datas[j] > datas[j + 1])
            {
              TemproraryDouble = datas[j + 1];
              datas[j + 1] = datas[j];
              datas[j] = TemproraryDouble;
            }
            Token.ThrowIfCancellationRequested();
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
              TemproraryDouble = datas[j + 1];
              datas[j + 1] = datas[j];
              datas[j] = TemproraryDouble;
            }
            Token.ThrowIfCancellationRequested();
          }
        }
      }

      return datas;
    }
    public static double[] InsertSort(bool reverse)
    {
      double[] datas = Data.GetValues(Data.Value.Xi).Clone() as double[];
      if (!reverse)
        for (int i = 1; i < datas.Length; ++i)
        {
          double temp = datas[i];
          int j = i - 1;
          while (j >= 0 && datas[j] > temp)
          {
            datas[j + 1] = datas[j];
            j--;
            Token.ThrowIfCancellationRequested();
          }
          Token.ThrowIfCancellationRequested();
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
            j--;

            Token.ThrowIfCancellationRequested();
          }

          Token.ThrowIfCancellationRequested();

          datas[j + 1] = temp;

        }
      return datas;
    }
    public static double[] ShakerSort(bool reverse)
    {
      double[] datas = Data.GetValues(Data.Value.Xi).Clone() as double[];
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
            Token.ThrowIfCancellationRequested();
          }

          for (var j = datas.Length - 2 - i; j > i; j--)
          {
            if (datas[j - 1] > datas[j])
            {
              (datas[j - 1], datas[j]) = (datas[j], datas[j - 1]);
              swapFlag = true;
            }
            Token.ThrowIfCancellationRequested();
          }

          if (!swapFlag)
            break;

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
            Token.ThrowIfCancellationRequested();
          }

          for (var j = datas.Length - 2 - i; j > i; j--)
          {
            if (datas[j - 1] < datas[j])
            {
              (datas[j - 1], datas[j]) = (datas[j], datas[j - 1]);
              swapFlag = true;
            }
            Token.ThrowIfCancellationRequested();
          }

          if (!swapFlag)
            break;
        }

      return datas;
    }
    public static double[] FastSort(double[] datas, int LeftIndex, int RightIndex, bool reverse)
    {
      int i = LeftIndex;
      int j = RightIndex;
      double bar = datas[(LeftIndex + RightIndex) / 2];

      Token.ThrowIfCancellationRequested();

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

          Token.ThrowIfCancellationRequested();
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

          Token.ThrowIfCancellationRequested();
        }


      if (LeftIndex < j)
        FastSort(datas, LeftIndex, j, reverse);

      if (i < RightIndex)
        FastSort(datas, i, RightIndex, reverse);

      return datas;
    }
    public static double[] BogoSort(bool reverse)
    {
      double[] datas = Data.GetValues(Data.Value.Xi).Clone() as double[];

      if (!reverse)
        while (!IsSorted(datas))
        {
          datas = RandomPermutation(datas);
          Token.ThrowIfCancellationRequested();
        }
      else
        while (!IsSorted_Revese(datas))
        {
          datas = RandomPermutation(datas);
          Token.ThrowIfCancellationRequested();
        }

      return datas;
    }
    private static bool IsSorted_Revese(double[] datas)
    {
      for (int i = 0; i < datas.Length - 1; i++)
      {
        if (datas[i] < datas[i + 1])
          return false;
        Token.ThrowIfCancellationRequested();

      }

      return true;
    }
    private static bool IsSorted(double[] datas)
    {
      for (int i = 0; i < datas.Length - 1; i++)
      {
        if (datas[i] > datas[i + 1])
          return false;
        Token.ThrowIfCancellationRequested();
      }

      return true;
    }

    /// <summary>
    /// перемешивание элементов массива
    /// </summary>
    /// <param name="datas"></param>
    /// <returns></returns>
    private static double[] RandomPermutation(double[] datas)
    {
      Token.ThrowIfCancellationRequested();
      Random random = new Random();
      var n = datas.Length;
      while (n > 1)
      {
        n--;
        var i = random.Next(n + 1);
        var temp = datas[i];
        datas[i] = datas[n];
        datas[n] = temp;
        if (Token.IsCancellationRequested)
          Token.ThrowIfCancellationRequested(); ;
      }

      return datas;
    }

    #endregion



  }
}
