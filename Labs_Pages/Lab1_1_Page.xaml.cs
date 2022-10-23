using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Programing_Labs.Labs_Pages
{
    /// <summary>
    /// Логика взаимодействия для Lab1_1_Page.xaml
    /// </summary>
    public partial class Lab1_1_Page : Page
    {

        private TextBox CatetA_TextBox { get; set; }
        private TextBox CatetB_TextBox { get; set; }
        private TextBox Height_TextBox { get; set; }
        private TextBox Weight_TextBox { get; set; }
        private TextBox[] UITextBoxes { get; set; }
        public Lab1_1_Page()
        {
            InitializeComponent();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CatetA_TextBox = GetStyleElement(TextBox_CatetA, "MainTextBox") as TextBox;
            CatetB_TextBox = GetStyleElement(TextBox_CatetB, "MainTextBox") as TextBox;
            Height_TextBox = GetStyleElement(TextBox_Height, "MainTextBox") as TextBox;
            Weight_TextBox = GetStyleElement(TextBox_Weight, "MainTextBox") as TextBox;


            UITextBoxes = new TextBox[]{
                CatetA_TextBox,
                CatetB_TextBox,
                Height_TextBox,
                Weight_TextBox
            };
            foreach (TextBox textBox in UITextBoxes)
            {
                textBox.Text = "";
                textBox.PreviewTextInput +=
            new TextCompositionEventHandler(Check.PreviewTextInput);
                DataObject.AddPastingHandler(textBox, (s, a) => a.CancelCommand());
            }
        }



        private object GetStyleElement(Control element, string name) =>
            element.Template.FindName(name, element);

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var textBox in UITextBoxes)
                textBox.Text = string.Empty;
            AnswerFormulaControl.Formula = "";
            LblAnswerName.Visibility = Visibility.Hidden;
        }



        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            if (Check.CheckTextBoxesValues(UITextBoxes))
            {
                double.TryParse(CatetA_TextBox.Text, out double CatetA);
                double.TryParse(CatetB_TextBox.Text, out double CatetB);
                double.TryParse(Height_TextBox.Text, out double HeightValue);
                double.TryParse(Weight_TextBox.Text, out double Weight);

                AnswerFormulaControl.Formula = @"p=\frac{2 \cdot m}{a \cdot b \cdot h}=\frac{2\cdot " +
                    Weight.ToString() + "}{" + $"{CatetA}\\cdot{CatetB}\\cdot{HeightValue}" + "}=" +
                   (2 * Weight / (CatetA * CatetB * HeightValue)).ToString();
                LblAnswerName.Visibility = Visibility.Visible;

            }
        }
    }
}
