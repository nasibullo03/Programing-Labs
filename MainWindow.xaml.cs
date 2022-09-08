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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           /* var converter = new System.Windows.Media.BrushConverter();
            Rectangle rect = new Rectangle(){
                Width = 10, Height=20,
                StrokeThickness = 1,
                HorizontalAlignment = HorizontalAlignment.Left,
                Fill = (Brush)converter.ConvertFromString("#48465e"),
                Visibility = Visibility.Visible,
        };
            
            MenuItemList.Children.Add(rect);

           
            *//*< Rectangle x: Name = "MenuItemRectangle"
                                       Width = "10"
                                       StrokeThickness = "1"
                                       Fill = "#48465e"
                                       HorizontalAlignment = "Left"
                            />*/
        }
    }
}
