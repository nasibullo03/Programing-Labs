using System.Collections.Generic;
using System.Windows;

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
            new Design.MenuDesign().AddMenuItems();

        }
    }
}
