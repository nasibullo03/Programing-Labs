using System;
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

namespace Programing_Labs.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ModernSmallUserInputControl.xaml
    /// </summary>
    public partial class ModernSmallUserInputControl : UserControl
    {
        public ModernSmallUserInputControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }
        public Brush ForegroundColor { get; set; }
        public float FontSizeValue { get; set; }

    }
}
