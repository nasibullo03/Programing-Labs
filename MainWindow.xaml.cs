using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Programing_Labs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
           
            InitializeComponent();
            

        }
        
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
            var converter = new System.Windows.Media.BrushConverter();
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
                    Style = (Style)this.FindResource("MenuButtonTheme"),
                    Visibility = Visibility.Visible,
                    TabIndex = valuePairs.Key,
                    

                };
                MenuItem.Checked += RadioButton_Checked;
                MenuItemList.Children.Add(MenuItem);
                

            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
