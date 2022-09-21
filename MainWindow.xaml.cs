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
        public static MainWindow FormMain{get;set;}
        private static object MenuItem_Sender = null;
        private static object MenuSubitem_Sender = null;
        private static object ClickedObject = null;
        /*private static Canvas menuItemBackgroundCanvas = default;*/
        public static BrushConverter converter = new System.Windows.Media.BrushConverter();
        public MainWindow()
        {

            InitializeComponent();
            FormMain = this;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            MenuItem.ShowItems();
            /*///TODO пока до конца не закончена
            menuItemBackgroundCanvas = new Canvas()
            {
                Width = MenuItemList.Width,
                Height = MenuItemList.Height
            };
            Canvas Pointer = new Canvas()
            {
                Width = 10,
                Height = 50,
                Background = (Brush)converter.ConvertFromString("#FFFFFF"),
                
            };
            Canvas.SetTop(Pointer, 0);
            Canvas.SetLeft(Pointer, 0);

            menuItemBackgroundCanvas.Children.Add(Pointer);
            
            ApplicationMainGrid.Children.Add(menuItemBackgroundCanvas);*/
            
        }

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            radioButton.Style = (Style)this.FindResource("MenuButtonThemeClicked");
            for(int i=0; i < MenuItem.MenuItemsRadioButton.Count; ++i)
            {
                if(radioButton.Name == MenuItem.MenuItemsRadioButton[i].Name)
                {
                    MenuItem.SubitemsRadioButton[i][0].IsChecked = true;
                    MenuSubitem_Sender = MenuItem.SubitemsRadioButton[i][0];
                }
                break;
            }
                
            if(MenuItem_Sender != null)
            {
                (MenuItem_Sender as RadioButton).Style = (Style)this.FindResource("MenuButtonTheme");
                
                MenuItem_Sender = sender;
            }
            else if (MenuItem_Sender == null) MenuItem_Sender = sender;
            

        }
        public void RadioButtonSubitem_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            radioButton.Style = (Style)this.FindResource("MenuSubitemThemeClicked");

            if (MenuSubitem_Sender != null)
            {
                (MenuSubitem_Sender as RadioButton).Style = (Style)this.FindResource("MenuSubitemTheme");

                MenuSubitem_Sender = sender;
            }
            else if (MenuSubitem_Sender == null) MenuSubitem_Sender = sender;


        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(Mouse.Captured).Y <= 75)
            {
                this.DragMove();
            }
            if (e.ClickCount==2)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                } else
                {
                    this.WindowState = WindowState.Maximized;
                }
            }
            
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        internal void RadioButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(e.)
            RadioButton radioButton = sender as RadioButton;
            if (ClickedObject != null)
            {
                if(radioButton.Name == (ClickedObject as RadioButton).Name)
                radioButton.IsChecked = false;
                ClickedObject = sender;
            }
            else if (ClickedObject == null) ClickedObject = sender;
        }
    }
}
