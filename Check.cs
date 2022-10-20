using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Programing_Labs
{
    class Check
    {
        public static void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ",")
            {
                if (!((TextBox)sender).Text.Contains(","))
                    approvedDecimalPoint = true;
                
            } else if (e.Text == "-")
            {
                if (!((TextBox)sender).Text.Contains("-"))
                    approvedDecimalPoint = true;
                if (!(((TextBox)sender).Text.Length>0))
                        approvedDecimalPoint = true;
            }
            
            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint ))
                e.Handled = true;
            
        }

        
        public static bool CheckTextBoxesValues(TextBox[] UITextBoxes)
        {
            string message = string.Empty;
            foreach (TextBox textBox in UITextBoxes)
            {
                if (textBox.Text == string.Empty)
                {
                    if (!message.Contains("-Поле не может быть пустым."))
                        message += "-Поле не может быть пустым.\n";

                }
                if (textBox.Text == "0")
                {
                    if (!message.Contains("-Значение поля не может быть нулевым"))
                        message += "-Значение поля не может быть нулевым.\n";
                }

                if (double.TryParse(textBox.Text, out var numberStyles) == true && numberStyles < 0)
                {
                    if (!message.Contains("-Значение поля не может быть отрицательным."))
                        message += "-Значение поля не может быть отрицательным.\n";
                }
                if ((string)textBox.Tag == "formula" && !textBox.Text.Contains('x'))
                {
                    if (!message.Contains("-F(x) должен иметь свойство x."))
                        message += "-F(x) должен иметь свойство x.\n";
                }
                if ((string)textBox.Tag == "formula")
                {
                    try
                    {
                        org.matheval.Expression expression = new org.matheval.Expression(textBox.Text.ToLower());
                        expression.Bind("x", 0.5);
                        double value = expression.Eval<double>();
                    }
                    catch(Exception ex)
                    {
                        if (!message.Contains(ex.Message.ToString()))
                            message += ex.Message.ToString()+"\n";
                    }
                    
                }
                
            }
            if (message != string.Empty)
            {
                MessageBox.Show($"{message}Введите допустимое значение!", "Неверное значение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            return true;
        }
        
        public void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
    }
}
