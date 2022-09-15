using System;
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
        private static object MenuItem_Sender = null;
        private static object MenuItem_firstItem = null;
        private static Canvas menuItemBackgroundCanvas = default;
        private static BrushConverter converter = new System.Windows.Media.BrushConverter();
        public MainWindow()
        {

            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowMenuItems();
            ///TODO пока до конца не закончена
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
            
            ApplicationMainGrid.Children.Add(menuItemBackgroundCanvas);
            
        }
        
        private  void ShowMenuItems()
        {
            Dictionary<int, int> AmountLabs;
            /* new Design.MenuDesign().AddMenuItems();*/
            AmountLabs = new Dictionary<int, int>()
            {
                {1,2 },
                {2,2 },
                {3,2 },
                {4,2 },
                {5,2 },
            };
            RadioButton MenuItem;

            foreach (var valuePairs in AmountLabs)
            {
                MenuItem = new RadioButton()
                {
                    Name = $"MemuItem{valuePairs.Key}",
                    Content = $"Lab {valuePairs.Key}",
                    Height = 50,
                    Foreground = (Brush)converter.ConvertFromString("#FFFFFF"),
                    FontSize = 14,
                    Style = (valuePairs.Key != 1) ? (Style)this.FindResource("MenuButtonTheme") : (Style)this.FindResource("MenuButtonThemeClicked"),
                    Visibility = Visibility.Visible,
                    TabIndex = valuePairs.Key,
                    IsChecked = (valuePairs.Key != 1) ? false : true,


                };
                if (valuePairs.Key == 1) MenuItem_firstItem = MenuItem;

                MenuItem.Checked += RadioButton_Checked;
                MenuItemList.Children.Add(MenuItem);


            }
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            radioButton.Style = (Style)this.FindResource("MenuButtonThemeClicked");
           
            if(MenuItem_Sender != null)
            {
                (MenuItem_Sender as RadioButton).Style = (Style)this.FindResource("MenuButtonTheme");
                
                MenuItem_Sender = sender;
            }
            else if (MenuItem_Sender == null)
            {
                (MenuItem_firstItem as RadioButton).Style = (Style)this.FindResource("MenuButtonTheme");
                MenuItem_Sender = sender;
            }

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
    }
}
