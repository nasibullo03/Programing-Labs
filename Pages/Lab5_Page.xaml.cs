﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private List<Task> Tasks = new List<Task>();
        /// <summary>
        /// значение i  для Xi
        /// </summary>
        public int index = 0;


        #endregion

        public Lab5_Page()
        {
            InitializeComponent();
            OlypmSort.SortData.SortDataView = ArrayData_ListView;
            ArrayData_ListView.ItemsSource = OlypmSort.SortData.SortDataCollection;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddBaseControlsValues();
        }
        private void AddBaseControlsValues()
        {
            OnStartControls = new List<Control>() { MenuItemArraySize, MenuItemFill };
            DataControls_onRandomlyClick = new List<Control>() {
                MenuItemBack,
                MenuItemAdd,
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
        private object GetStyleElement(Control element, string name) =>
          element.Template.FindName(name, element);



        #region Buttons
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
            DataControls_onManuallyClick.ForEach(el => el.Visibility = Visibility.Collapsed);
            MenuItemBack.Visibility = Visibility.Collapsed;
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
            OlypmSort.SortData data = new OlypmSort.SortData(Xi);

            await OlypmSort.SortData.Add(data);
            if (ArrayData_ListView.Items.Count >= n)
            {
                MenuItemAdd.Visibility = Visibility.Collapsed;
                return;
            }
            ++index;
            LblXi.Content = $"X({index})";

        }
        #endregion

        #region MenuItems Sort
        private void MenuItem_BubleSort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_InsertSort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_ShakerSort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_FastSort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_BogoSort_Click(object sender, RoutedEventArgs e)
        {

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

            if (OlypmSort.SortData.EditMode)
                MenuItemBack.Visibility = Visibility.Visible;

            if (ArrayData_ListView.Items.Count >= n)
                MenuItemAdd.Visibility = Visibility.Collapsed;

            if (index != 0) return;

            ++index;
            LblXi.Content = $"X({index})";
        }
        private void MenuItem_RandomGenerate_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(TxtBxArraySize.Text, out int n);
            if (n <= 0)
            {
                MessageBox.Show("Размер массива не может быть меньше или равен нулю!!", "Неверный формат", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            OnStartControls.ForEach(el => el.Visibility = Visibility.Collapsed);
            DataControls_onRandomlyClick.ForEach(el => el.Visibility = Visibility.Visible);

            if (OlypmSort.SortData.EditMode)
                MenuItemBack.Visibility = Visibility.Visible;

            if (ArrayData_ListView.Items.Count >= n)
                MenuItemAdd.Visibility = Visibility.Collapsed;

            if (index != 0) return;
        }
        #endregion

        #region MenuItems Clear 

        private void MenuItemClearAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemClearData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemClearSortData_Click(object sender, RoutedEventArgs e)
        {

        }


        #endregion

        #region DataView context items 
        private void Remove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion


    }
}
