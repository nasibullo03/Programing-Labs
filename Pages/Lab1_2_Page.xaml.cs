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

namespace Programing_Labs.Pages
{
    /// <summary>
    /// Логика взаимодействия для Lab1_2_Page.xaml
    /// </summary>
    public partial class Lab1_2_Page : Page
    {
        
        private TextBox TxtBxName { get; set; }
        private TextBox TxtBxPhoneNumber { get; set; }
        private TextBox TxtBxYear { get; set; }
        private TextBox[] UITextBoxes { get; set; }
        public static ListView PhoneBooksView { get; set; } 
        public Lab1_2_Page()
        {
            InitializeComponent();
            PhoneBooksView = PhoneBookList;
            PhoneBookList.ItemsSource = PhoneBook.PhoneBooksCollection;

        }
     
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            TxtBxName = GetStyleElement(TextBoxName, "MainTextBox") as TextBox;
            TxtBxPhoneNumber = GetStyleElement(TextBoxPhoneNumber, "MainTextBox") as TextBox;
            TxtBxYear = GetStyleElement(TextBoxYear, "MainTextBox") as TextBox;

            UITextBoxes = new TextBox[]
            {
                TxtBxName, TxtBxPhoneNumber, TxtBxYear
            };
            foreach (TextBox textBox in UITextBoxes)
            {
                DataObject.AddPastingHandler(textBox, (s, a) => a.CancelCommand());
            }
        }
        private object GetStyleElement(Control element, string name) =>
           element.Template.FindName(name, element);
        #region Buttons
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            PhoneBook phoneBook = new  PhoneBook(TxtBxName.Text, TxtBxPhoneNumber.Text, TxtBxYear.Text);
            await PhoneBook.Add(phoneBook);
            


        }
        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {

        }

       
        #endregion

    }
}
