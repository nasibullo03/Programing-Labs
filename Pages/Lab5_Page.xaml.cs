﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace Programing_Labs.Pages
{
  /// <summary>
  /// Логика взаимодействия для Lab5_Page.xaml
  /// </summary>
  public partial class Lab5_Page : Page
  {
    #region properties
    private TextBox TxtBxXi { get; set; }
    private TextBox TxtBxArraySize { get; set; }
    private Label LblXi { get; set; }
    private bool Reverse { get; set; } = false;
    private List<Control> OnStartControls { get; set; }
    private List<Control> DataControls_onRandomlyClick { get; set; }
    private List<Control> DataControls_onManuallyClick { get; set; }

    public static CancellationTokenSource cancellationToken { get; set; } = new CancellationTokenSource();
    public static List<Task> tasks = new List<Task>();
    public static StackPanel LoadingPanel1 { get; set; }
    public static Label LoadingLabel1 { get; set; }
    public static Control MenuItemCancell1 { get; set; }

    public int index = 0;
    private Stopwatch Timer { get; set; }
    private int EndedTaskCount = 0;
    private bool AllTasks = false;
    #endregion

    public Lab5_Page()
    {
      InitializeComponent();
      OlympSort.Data.SortDataView = ArrayData_ListView;
      OlympSort.Sort.SortDataView = ArrayData_ListView1;

      ArrayData_ListView.ItemsSource = OlympSort.Data.SortDataCollection;
      ArrayData_ListView1.ItemsSource = OlympSort.Sort.SortDataCollection;

      OlympSort.Sort.Token = cancellationToken.Token;

      LoadingPanel1 = loadingPanel;
      LoadingLabel1 = LoadingLabel;
      MenuItemCancell1 = MenuItemCancel;
    }

    #region OnStartMethods
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      AddBaseControlsValues();
    }
    private void AddBaseControlsValues()
    {
      OnStartControls = new List<Control>() { MenuItemArraySize, MenuItemFill };
      DataControls_onRandomlyClick = new List<Control>() {
                MenuItemBack,
                MenuItemCheckBox,
                MenuItemSortOperation,
                MenuItemClearOpetations,
            };

      DataControls_onManuallyClick = new List<Control>() {
                MenuItemXi,
                MenuItemBack,
                MenuItemAdd,
                MenuItemCheckBox,
                MenuItemSortOperation,
                MenuItemClearOpetations,

            };

      TxtBxXi = GetStyleElement(TextBoxXi, "MainTextBox") as TextBox;
      TxtBxArraySize = GetStyleElement(TextBoxArraySize, "MainTextBox") as TextBox;

      LblXi = GetStyleElement(LabelXi, "MainLabel") as Label;

      TxtBxXi.PreviewTextInput += new TextCompositionEventHandler(Check.PreviewTextInput);
      TxtBxArraySize.PreviewTextInput += new TextCompositionEventHandler(Check.PreviewTextInput);

      DataObject.AddPastingHandler(TxtBxXi, (s, a) => a.CancelCommand());
      DataObject.AddPastingHandler(TxtBxArraySize, (s, a) => a.CancelCommand());

      OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
      DataControls_onManuallyClick.ForEach(el => el.Visibility = Visibility.Collapsed);
      MenuItemEdit.Visibility = Visibility.Collapsed;
    }
    private void OnStartControrsVisibility()
    {
      OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
      DataControls_onManuallyClick.ForEach(el => el.Visibility = Visibility.Collapsed);
      if (OlympSort.Data.SortDatas.Count > 0)
        MenuItemNext.Visibility = Visibility.Visible;
      MenuItemBack.Visibility = Visibility.Collapsed;
    }
    private object GetStyleElement(Control element, string name) =>
      element.Template.FindName(name, element);
    #endregion

    #region Buttons
    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
      double.TryParse(TxtBxArraySize.Text, out double N);
      if (N > 0)
      {
        //добавить новые данные в списке
        double.TryParse(TxtBxXi.Text, out double Xi);

        if (OlympSort.Data.SortDatas.Count >= N && OlympSort.Data.EditableList.Count == 0)
          return;

        OlympSort.Data.EditValues(Xi);
        //удалить данные которые уже отредактированы
        OlympSort.Data.DeleteEditedValue();
      }
      else return;

      if (OlympSort.Data.EditableList.Count == 0)
      {
        MenuItemEdit.Visibility = Visibility.Collapsed;

        if (OlympSort.Data.SortDatas.Count >= N)
          MenuItemAdd.Visibility = Visibility.Collapsed;
        else MenuItemAdd.Visibility = Visibility.Visible;

        OlympSort.Data.EditableList.Clear();
        OlympSort.Data.EditMode = false;

        index = (index >= N) ? index : ArrayData_ListView.Items.Count + 1;
        LblXi.Content = $"X({index})";
        TxtBxXi.Text = string.Empty;
        return;
      }
      OlympSort.Data.PrepareDataForEditing(TxtBxXi, LblXi);
    }
    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
      OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
      DataControls_onManuallyClick.ForEach(el => el.Visibility = Visibility.Collapsed);
      MenuItemBack.Visibility = Visibility.Collapsed;
      if (OlympSort.Data.SortDatas.Count > 0)
        MenuItemNext.Visibility = Visibility.Visible;

    }
    private async void BtnAdd_Click(object sender, RoutedEventArgs e)
    {

      double.TryParse(TxtBxXi.Text, out double Xi);
      double.TryParse(TxtBxArraySize.Text, out double n);

      if (ArrayData_ListView.Items.Count >= n)
      {
        MenuItemAdd.Visibility = Visibility.Collapsed;
        return;
      }
      OlympSort.Data data = new OlympSort.Data(Xi);

      await OlympSort.Data.Add(data);
      if (ArrayData_ListView.Items.Count >= n)
      {
        MenuItemAdd.Visibility = Visibility.Collapsed;
        return;
      }
      ++index;
      LblXi.Content = $"X({index})";

    }
    private void BtnNext_Click(object sender, RoutedEventArgs e)
    {
      int.TryParse(TxtBxArraySize.Text, out int n);
      OnStartControls.ForEach(el => el.Visibility = Visibility.Collapsed);
      DataControls_onRandomlyClick.ForEach(el => el.Visibility = Visibility.Visible);
      MenuItemNext.Visibility = Visibility.Collapsed;

      if (OlympSort.Data.EditMode)
        MenuItemBack.Visibility = Visibility.Visible;

      if (ArrayData_ListView.Items.Count >= n)
        MenuItemAdd.Visibility = Visibility.Collapsed;
    }
    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
      cancellationToken.Cancel();
      OlympSort.Sort.Token = cancellationToken.Token;
    }
    #endregion

    private double[] SetTimer(Func<double[]> action, OlympSort.Sort.SortType sortType)
    {
      double[] data = null;

      cancellationToken.Token.ThrowIfCancellationRequested();

      LoadingLabelText("Идет запуск таймера");

      try
      {
        Timer = new Stopwatch();
        Timer.Start();
        data = action.Invoke();
        Timer.Stop();
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3}",
    Timer.Elapsed.Hours, Timer.Elapsed.Minutes, Timer.Elapsed.Seconds,
    Timer.Elapsed.Milliseconds);

        LoadingLabelText("Идет обработка данных");

        /*Здесь @"hh\:mm\:ss\.fff" - это строка формата, которая задает необходимый вид вывода времени(часы: минуты:секунды: миллисекунды).Символы \ служат для экранирования знаков : и.в строке формата, т.к.они имеют специальное значение.*/

        TxtBxArraySize.Dispatcher.Invoke(async () =>
            await OlympSort.Sort.Add(new OlympSort.Sort(sortType, elapsedTime, TxtBxArraySize.Text)));

      }
      catch /*(Exception ex)*/
      {
        /*MessageBox.Show(ex.Message);*/

      }

      return data;

    }
    private void LoadingAnimation(Visibility visibility)
    {
      loadingPanel.Dispatcher.Invoke(() =>
      {
        loadingPanel.Visibility = visibility;
      });

    }
    private async void LoadingAnimation(Action action)
    {
      try
      {
        MenuItemCancel.Dispatcher.Invoke(() => MenuItemCancel.Visibility = Visibility.Visible);
        LoadingAnimation(Visibility.Visible);
        await Task.Run(action);
      }
      catch /*(Exception ex)*/
      {
        /*System.Windows.Forms.MessageBox.Show(ex.Message);*/
      }
      finally
      {
        LoadingAnimation(Visibility.Collapsed);
        MenuItemCancel.Dispatcher.Invoke(() => MenuItemCancel.Visibility = Visibility.Collapsed);
        
      }



    }

    private double[] Sort(OlympSort.Sort.SortType sortType)
    {
      LoadingLabelText("Идет сортировка данных");
      cancellationToken.Token.ThrowIfCancellationRequested();

      switch (sortType)
      {
        case OlympSort.Sort.SortType.Buble:
          return OlympSort.Sort.BubleSort(Reverse);
        case OlympSort.Sort.SortType.Insert:
          return OlympSort.Sort.InsertSort(Reverse);
        case OlympSort.Sort.SortType.Shaker:
          return OlympSort.Sort.ShakerSort(Reverse);
        case OlympSort.Sort.SortType.Fast:
          double[] data = OlympSort.Data.SortDatas.Select(a => a.Xi).ToArray();
          return OlympSort.Sort.FastSort(data, 0, data.Length - 1, Reverse);
        case OlympSort.Sort.SortType.Bogo:
          return OlympSort.Sort.BogoSort(Reverse);
        default:
          return null;

      }
    }
    private  void SortingProcces(OlympSort.Sort.SortType sortType, CancellationToken token)
    {
     
        token.ThrowIfCancellationRequested();

        OlympSort.Data.SetValues(
                    SetTimer(() => Sort(sortType), sortType),
                    OlympSort.Data.Value.SortedXi, token);
     

    }
    public static async void LoadingLabelText(string text)
    {
      await Task.Run(() => LoadingLabel1.Dispatcher.Invoke(() => LoadingLabel1.Content = text));
    }

    #region MenuItems Sort
    private void MenuItem_BubleSort_Click(object sender, RoutedEventArgs e)
    {
      cancellationToken = new CancellationTokenSource();
      OlympSort.Sort.Token = cancellationToken.Token;
      LoadingAnimation(() => SortingProcces(OlympSort.Sort.SortType.Buble, cancellationToken.Token));

    }

    private void MenuItem_InsertSort_Click(object sender, RoutedEventArgs e)
    {
      cancellationToken = new CancellationTokenSource();
      OlympSort.Sort.Token = cancellationToken.Token;
      LoadingAnimation(() => SortingProcces(OlympSort.Sort.SortType.Insert, cancellationToken.Token));

    }

    private void MenuItem_ShakerSort_Click(object sender, RoutedEventArgs e)
    {
      cancellationToken = new CancellationTokenSource();
      OlympSort.Sort.Token = cancellationToken.Token;
      LoadingAnimation(() => SortingProcces(OlympSort.Sort.SortType.Shaker, cancellationToken.Token));

    }

    private void MenuItem_FastSort_Click(object sender, RoutedEventArgs e)
    {
      cancellationToken = new CancellationTokenSource();
      OlympSort.Sort.Token = cancellationToken.Token;
      LoadingAnimation(() =>
          SortingProcces(OlympSort.Sort.SortType.Fast, cancellationToken.Token));

    }

    private void MenuItem_BogoSort_Click(object sender, RoutedEventArgs e)
    {
      cancellationToken = new CancellationTokenSource();
      OlympSort.Sort.Token = cancellationToken.Token;
      LoadingAnimation(() =>
          SortingProcces(OlympSort.Sort.SortType.Bogo, cancellationToken.Token));
    }
    private void MenuItem_StartSortingTypes_Click(object sender, RoutedEventArgs e)
    {
      cancellationToken = new CancellationTokenSource();
      OlympSort.Sort.Token = cancellationToken.Token;
      CancellationToken token = cancellationToken.Token;

      AllTasks = true;
      LoadingAnimation(() =>
      {
       
        Parallel.Invoke(
                  () => SortingProcces(OlympSort.Sort.SortType.Buble, token),
                  () => SortingProcces(OlympSort.Sort.SortType.Fast, token),
                  () => SortingProcces(OlympSort.Sort.SortType.Insert, token),
                  () => SortingProcces(OlympSort.Sort.SortType.Bogo, token),
                  () => SortingProcces(OlympSort.Sort.SortType.Shaker, token)
                  );
      
      });

    }
    #endregion

    #region Reverse CheckBox
    private void ReverseCheckBox_Checked(object sender, RoutedEventArgs e)
    {
      Reverse = true;
    }

    private void ReverseCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
      Reverse = false;
    }
    #endregion

    #region Fill Data

    private void MenuItem_FillManually_Click(object sender, RoutedEventArgs e)
    {
      int.TryParse(TxtBxArraySize.Text, out int n);

      if (n <= 0)
      {
        MessageBox.Show("Размер массива не может быть меньше или равен нулю!!", "Неверный формат", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      OnStartControls.ForEach(el => el.Visibility = Visibility.Collapsed);
      DataControls_onManuallyClick.ForEach(el => el.Visibility = Visibility.Visible);
      MenuItemNext.Visibility = Visibility.Collapsed;

      if (OlympSort.Data.EditMode)
        MenuItemBack.Visibility = Visibility.Visible;

      if (ArrayData_ListView.Items.Count >= n)
        MenuItemAdd.Visibility = Visibility.Collapsed;

      if (index != 0) return;

      ++index;
      LblXi.Content = $"X({index})";
    }
    private void MenuItem_RandomGenerate_Click(object sender, RoutedEventArgs e)
    {
      LoadingLabelText("Идет проверка данных");
      loadingPanel.Visibility = Visibility.Visible;
      MenuItemCancel.Visibility = Visibility.Visible;

      int.TryParse(TxtBxArraySize.Text, out int n);

      if (n <= 0)
      {
        MessageBox.Show("Размер массива не может быть меньше или равен нулю!!", "Неверный формат", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      OnStartControls.ForEach(el => el.Visibility = Visibility.Collapsed);
      DataControls_onRandomlyClick.ForEach(el => el.Visibility = Visibility.Visible);
      MenuItemNext.Visibility = Visibility.Collapsed;

      if (OlympSort.Data.EditMode)
        MenuItemBack.Visibility = Visibility.Visible;

      if (ArrayData_ListView.Items.Count >= n)
        MenuItemAdd.Visibility = Visibility.Collapsed;

      OlympSort.Data.Clear();

      cancellationToken = new CancellationTokenSource();

      LoadingLabelText("Выполняется генерация данных");

      CancellationToken token = cancellationToken.Token;

      OlympSort.Data.GererateData(n, token);


    }
    #endregion

    #region MenuItems Clear 

    private void MenuItemClearAll_Click(object sender, RoutedEventArgs e)
    {
      OlympSort.Data.Clear();
      OlympSort.Sort.Clear();
      OnStartControrsVisibility();
    }

    private void MenuItemClearData_Click(object sender, RoutedEventArgs e)
    {
      OlympSort.Data.Clear();
      OnStartControrsVisibility();


    }

    private void MenuItemClearSortData_Click(object sender, RoutedEventArgs e)
    {
      OlympSort.Sort.Clear();
    }


    #endregion

    #region DataView context items 
    private void Remove_Click(object sender, RoutedEventArgs e)
    {
      var focusedItems = ArrayData_ListView.SelectedItems;

      OlympSort.Data.Remove(focusedItems);
      index = ArrayData_ListView.Items.Count + 1;
      LblXi.Content = $"X({index})";
      MenuItemAdd.Visibility = Visibility.Visible;
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
      MenuItemAdd.Visibility = Visibility.Collapsed;
      MenuItemEdit.Visibility = Visibility.Visible;

      System.Collections.IList items = (System.Collections.IList)ArrayData_ListView.SelectedItems;

      var collection = items.Cast<OlympSort.Data>();

      OlympSort.Data.EditMode = true;
      OlympSort.Data.EditableList = collection.ToList();
      OlympSort.Data.PrepareDataForEditing(TxtBxXi, LblXi);
    }




    #endregion


  }
}
