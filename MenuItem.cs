using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Programing_Labs
{
    public class MenuItem
    {
        public static List<RadioButton[]> SubitemsRadioButton {get;set;} = new List<RadioButton[]>();
        public static List<RadioButton> MenuItemsRadioButton { get; set; } = new List<RadioButton>();
        public static BrushConverter converter = new System.Windows.Media.BrushConverter();
        /// <summary>
        /// для хранение параметров MenuItems
        /// </summary>
        public static Dictionary<int, MenuItem> MenuItemParametrs;
        public static int Count { get; set; }
        /// <summary>
        /// для хранение количесво задание в одном Item 
        /// </summary>
        public int ItemQuantity { get; set; }
        /// <summary>
        /// для хранение имя задании в одном Item, при ItemQuantity=0 получает значение null
        /// </summary>
        public string[] ItemsName { get; set; }
        public string MenuName { get; set; }
        /// <summary>
        /// для хранение путь к страницу
        /// </summary>
        public string[] SoursePage { get; set; }

        /// <summary>
        /// Заполняет MenuItemParametrs
        /// </summary>
        private static void FillMenuItemParametrs(){
            MenuItemParametrs = new Dictionary<int, MenuItem>()
            {
                 {1,new MenuItem(){
                    ItemQuantity=2,
                    ItemsName= new[]{"Заданяя №1", "Заданяя №2" },
                    SoursePage = new[] {"Labs_Pages\\Lab1_1_Page.xaml","Labs_Pages\\Lab1_1_Page.xaml"},
                    }
                },
                {2,new MenuItem(){
                    ItemQuantity=1,
                    SoursePage =  new[] {"Labs_Pages\\Lab1_1_Page.xaml"},
                    }
                },
            };

        }
        
        private static RadioButton NewMenuItem() => new RadioButton() {
                Height = 50,
                Foreground = (Brush)converter.ConvertFromString("#FFFFFF"),
                FontSize = 14,
                Style = (Style)MainWindow.FormMain.FindResource("MenuButtonTheme"),
                Visibility = Visibility.Visible,
                IsChecked = false,
                GroupName = "Items"
        };
        private static RadioButton NewMenuSubitem() => new RadioButton()
        {
            Height = 20,
            Foreground = (Brush)converter.ConvertFromString("#FFFFFF"),
            FontSize = 14,
            Style = (Style)MainWindow.FormMain.FindResource("MenuSubitemTheme"),
            Visibility = Visibility.Visible,
            IsChecked = false,
            GroupName = "Subitems"
        };
        private static void AddSubitems(MenuItem menuItem, int count )
        {
            RadioButton Subitem;
            RadioButton[] SubitemsList = new RadioButton[menuItem.ItemQuantity];
            
            for (int i=0; i< menuItem.ItemQuantity; ++i)
            {
                Subitem = NewMenuSubitem();
                Subitem.Name = $"MenuSubitem{count}";
                Subitem.Content = menuItem.ItemsName[i];
                Subitem.TabIndex = count;
                Subitem.Checked += MainWindow.FormMain.RadioButtonSubitem_Checked;
                SubitemsList[i] = Subitem;
                MainWindow.FormMain.MenuItemList.Children.Add(Subitem);
                if (count == 1 && i==0) Subitem.IsChecked = true;

            }
            SubitemsRadioButton.Add(SubitemsList);
        }
        public static void ShowItems()
        {
            FillMenuItemParametrs();
            RadioButton Item;
            foreach (var valuePairs in MenuItemParametrs)
            {
                Item = NewMenuItem();
                Item.Name = $"MenuItem{valuePairs.Key}";
                Item.Content = $"Lab {valuePairs.Key}";
                Item.TabIndex = valuePairs.Key;
                Item.Checked += MainWindow.FormMain.RadioButton_Checked;
                
                if (valuePairs.Key == 1) Item.IsChecked = true;
                MainWindow.FormMain.MenuItemList.Children.Add(Item);
                MenuItemsRadioButton.Add(Item);
                if (valuePairs.Value.ItemQuantity > 1)
                    AddSubitems(valuePairs.Value, valuePairs.Key);
                else
                {
                    SubitemsRadioButton.Add(new RadioButton[1]);
                }

            }
        }

    }
}
