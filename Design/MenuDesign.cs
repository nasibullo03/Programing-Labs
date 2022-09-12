using System;
using System.Windows;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Programing_Labs.Design
{
    class MenuDesign:MainWindow
    {
        public static Dictionary<int,int> AmountLabs { get; set; }

        public void AddMenuItems() {
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

            /*foreach (var valuePairs in AmountLabs)
            {
                MenuItem = new RadioButton()
                {
                    Content = $"Lab {valuePairs.Key}",
                    Height = 50,
                    Foreground = (Brush)converter.ConvertFromString("#FFFFFF"),
                    FontSize = 14,
                    Style = 
                    *//*Style = (Style)ResourceDictionary["MenuButtonTheme"],*//*
                    Visibility = Visibility.Visible,

                };
                *//*MenuItem.Checked += RadioButton_Checked;*//*
                MenuItemList.Children.Add(MenuItem);
                MessageBox.Show("");
                
            }*/

        }

    }
}
