using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Programing_Labs.CustomControls.MatrixViewer
{
    /// <summary>
    /// Логика взаимодействия для MatrixViewer.xaml
    /// </summary>
    public partial class MatrixViewer : UserControl
    {
        public ObservableCollection<ListView> MatrixCollection { get; set; }
        public ObservableCollection<TextBox> MatrixRowCollection { get; set; }

        public enum ViewType { Input, Output}
        public ViewType? viewType { get; set; }
        
        public MatrixViewer()
        {
            InitializeComponent();
            ListView listView = new ListView();
            TextBox textBox = new TextBox()
            {
                Style = (Style)Resources["MatrixValueTextBox"],
            };
            MatrixRowCollection = new ObservableCollection<TextBox> { textBox };
            listView.ItemsSource = MatrixRowCollection;
            MatrixCollection = new ObservableCollection<ListView>()
            {
                listView
            };
            ListViewMatrix.ItemsSource = MatrixCollection;
        }

        public override void OnApplyTemplate()
        {
            switch (viewType)
            {
                case ViewType.Input:
                   
                    OutputMatrix_StackPanel.Visibility = Visibility.Collapsed;
                    Scobe.Visibility = Visibility.Collapsed;
                    ClosseScobe.Visibility = Visibility.Collapsed;

                    break;
                case ViewType.Output:
                    break;
                default:
                    break;
            }

            base.OnApplyTemplate();
        }
        public void Fill(int[,] values)
        {

        }
        public void Fill(double[,] values)
        {

        }
        public void Fill(float[,] values)
        {

        }
        public void Fill(string[,] values)
        {

        }

        public double[,] GetValue()
        {

            return null;
        }

        private void textbox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ",")
            {
                if (!((TextBox)sender).Text.Contains(","))
                    approvedDecimalPoint = true;

            }
            else if (e.Text == "-")
            {
                if (!((TextBox)sender).Text.Contains("-"))
                    approvedDecimalPoint = true;
                if (!(((TextBox)sender).Text.Length > 0))
                    approvedDecimalPoint = true;
            }
            
            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
    }
}
