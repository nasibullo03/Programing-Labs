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
        private Label LabelAnswer { get; set; }
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
            LabelAnswer = GetStyleElement(LblAnswer, "MainLabel") as Label;

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
            new TextCompositionEventHandler(Lab1_1_Page.PreviewTextInput);
            }
        }



        private object GetStyleElement(Control element, string name)=>
            element.Template.FindName(name, element); 

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var textBox in UITextBoxes)
            textBox.Text = string.Empty;
            LblAnswer.Visibility = Visibility.Hidden;
            LblAnswerName.Visibility = Visibility.Hidden;
        }
        
        private bool CheckTextBoxesValues()
        {
            string message = string.Empty;
            foreach(TextBox textBox in UITextBoxes)
            {
                if(textBox.Text == string.Empty)
                {
                    if(!message.Contains("-Поле не может быть пустым."))
                    message += "-Поле не может быть пустым.\n";
                   
                }
                if (textBox.Text == "0")
                {
                    if (!message.Contains("-Значение поля не может быть нулевым"))
                        message += "-Значение поля не может быть нулевым.\n";
                }

                if (double.TryParse(textBox.Text, out var numberStyles) == true && numberStyles<0)
                {
                    if (!message.Contains("-Значение поля не может быть отрицательным."))
                        message += "-Значение поля не может быть отрицательным.\n";
                }
            }
            if (message != string.Empty)
            {
                MessageBox.Show($"{message}Введите допустимое значение!", "Неверное значение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            

            return true;
        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            if (CheckTextBoxesValues())
            {   
                double.TryParse(CatetA_TextBox.Text, out var CatetA);
                double.TryParse(CatetB_TextBox.Text, out var CatetB);
                double.TryParse(Height_TextBox.Text, out var HeightValue);
                double.TryParse(Weight_TextBox.Text, out var Weight);
                LabelAnswer.Content = 2 * Weight / (CatetA * CatetB * HeightValue);
                LblAnswer.Visibility = Visibility.Visible;
                LblAnswerName.Visibility = Visibility.Visible;
            }
        }
        public static new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }
            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
        

    }
}
