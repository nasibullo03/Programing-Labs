using System;
using System.IO;
using System.Windows.Resources;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

namespace Programing_Labs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow FormMain { get; set; }
        private static object MenuItem_Sender = null;
        private static object MenuSubitem_Sender = null;
        public static BrushConverter converter = new System.Windows.Media.BrushConverter();
        public MainWindow()
        {

            InitializeComponent();
            FormMain = this;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            MenuItem.ShowItems();
            

        }

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            radioButton.Style = (Style)this.FindResource("MenuButtonStyleClicked");
            for (int i = 0; i < MenuItem.MenuItemsRadioButton.Count; ++i)
            {

                if (radioButton != MenuItem.MenuItemsRadioButton[i])
                {
                    MenuItem.SubitemsRadioButton[i][0].IsChecked = false;
                    MenuItem.SubitemsRadioButton[i][0].Style = (Style)this.FindResource("MenuSubitemStyle");
                    foreach (KeyValuePair<int, RadioButton> item in MenuItem.SubitemsRadioButton[i])
                    {
                        item.Value.Visibility = Visibility.Collapsed;
                    }
                }
                if (radioButton == MenuItem.MenuItemsRadioButton[i])
                {
                    MenuItem.SubitemsRadioButton[i][0].IsChecked = true;
                    MenuItem.SubitemsRadioButton[i][0].Style = (Style)this.FindResource("MenuSubitemStyleClicked");
                    if (MenuItem.MenuItemParametrs[i + 1].ItemQuantity <= 1)
                    {
                        MainFrame.Navigate(new Uri(MenuItem.MenuItemParametrs[i + 1].SoursePage[0], UriKind.Relative));
                    }
                    else
                        foreach (KeyValuePair<int, RadioButton> item in MenuItem.SubitemsRadioButton[i])
                        {
                            item.Value.Visibility = Visibility.Visible;

                        }

                }

            }

            if (MenuItem_Sender != null)
            {
                (MenuItem_Sender as RadioButton).Style = (Style)this.FindResource("MenuButtonStyle");

                MenuItem_Sender = sender;
            }
            else if (MenuItem_Sender == null) MenuItem_Sender = sender;

        }
        public void RadioButtonSubitem_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            radioButton.Style = (Style)this.FindResource("MenuSubitemStyleClicked");

            for (int i = 0; i < MenuItem.SubitemsRadioButton.Count; i++)
            {
                foreach (KeyValuePair<int, RadioButton> item in MenuItem.SubitemsRadioButton[i])
                {
                    if (item.Value == radioButton)
                        MainFrame.Navigate(new Uri(MenuItem.MenuItemParametrs[i + 1].SoursePage[item.Key], UriKind.Relative));
                }
            }

            if (MenuSubitem_Sender != null)
            {
                (MenuSubitem_Sender as RadioButton).Style = (Style)this.FindResource("MenuSubitemStyle");

                MenuSubitem_Sender = sender;
            }
            else if (MenuSubitem_Sender == null) MenuSubitem_Sender = sender;

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.GetPosition(Mouse.Captured).Y <= 75)
                {
                    this.DragMove();
                }
                if (e.ClickCount == 2)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        WindowBorder.Margin = new Thickness(0);
                        this.WindowState = WindowState.Normal;

                    }
                    else
                    {
                        WindowBorder.Margin = new Thickness(8);
                        this.WindowState = WindowState.Maximized;
                    }
                }
            }
            catch { }


        }

        

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }


    }
}
