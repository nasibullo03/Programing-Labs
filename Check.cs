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
        /// <summary>
        /// Обрабативает вводимый текст
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        /// <summary>
        /// Для первой лабы <see cref="Labs_Pages.Lab1_1_Page"/>
        /// </summary>
        /// <param name="UITextBoxes"></param>
        /// <returns></returns>
        public static bool CheckTextBoxesValues(TextBox[] UITextBoxes)
        {
            string message = string.Empty;
            foreach (TextBox textBox in UITextBoxes)
            {
                if (textBox.Text == string.Empty)
                {
                    if (!message.Contains("-Поле не может быть пустым."))
                        message += "-Поле не может быть пустым.\n";
                    continue;

                }
                if (textBox.Text == "0")
                {
                    if (!message.Contains("-Значение поля не может быть нулевым"))
                        message += "-Значение поля не может быть нулевым.\n";
                    continue;
                }

                if (double.TryParse(textBox.Text, out var numberStyles) == true && numberStyles < 0)
                {
                    if (!message.Contains("-Значение поля не может быть отрицательным."))
                        message += "-Значение поля не может быть отрицательным.\n";
                    continue;
                }


            }
            if (message != string.Empty)
            {
                MessageBox.Show($"{message}Введите допустимое значение!", "Неверное значение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            return true;
        }
        /// <summary>
        /// Для второй лабы <see cref="Labs_Pages.Lab2_Page"/>
        /// </summary>
        /// <param name="UITextBoxes"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckTextBoxesValues(TextBox[] UITextBoxes, string a, string b)
        {
            string message = string.Empty;
            double.TryParse(a, out double dblA);
            double.TryParse(b, out double dblB);
            foreach (TextBox textBox in UITextBoxes)
            {
                if (textBox.Text == string.Empty)
                {
                    if (!message.Contains("-Поле не может быть пустым."))
                        message += "-Поле не может быть пустым.\n";
                    continue;

                }
                if ((string)textBox.Tag == "e")
                {

                    double.TryParse(textBox.Text, out double dblE);

                    if (dblE <= 0 || dblE >= (dblB - dblA))
                    {
                        if (!message.Contains("-Значение поля \"e\"  должно соответствовать этому условию: 0<e<(a-b)"))
                            message += "-Значение поля \"e\"  должно соответствовать этому условию: 0<e<(a-b)\n";
                        continue;
                    }
                }
                if (textBox.Text == "0" && (string)textBox.Tag != "a" && (string)textBox.Tag != "b")
                {
                    if (!message.Contains("-Значение поля не может быть нулевым"))
                        message += "-Значение поля не может быть нулевым.\n";
                    continue;
                }

                if (double.TryParse(textBox.Text, out var numberStyles) == true && numberStyles < 0 && (string)textBox.Tag != "a" && (string)textBox.Tag != "b")
                {
                    if (!message.Contains("-Значение поля не может быть отрицательным."))
                        message += "-Значение поля не может быть отрицательным.\n";
                    continue;
                }

                if ((string)textBox.Tag == "formula")
                {
                    if (!textBox.Text.Contains('x'))
                    {
                        if (!message.Contains("-F(x) должен иметь свойство x."))
                            message += "-F(x) должен иметь свойство x.\n";
                        continue;
                    }

                    try
                    {
                        org.matheval.Expression expression = new org.matheval.Expression(textBox.Text.ToLower());
                        expression.Bind("x", dblA);
                        double value = expression.Eval<double>();

                        expression.Bind("x", dblB);
                        value = expression.Eval<double>();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Значение было недопустимо малым или недопустимо большим")
                            && !message.Contains("-Для этого F(x), значения a и b не могут быть отрицательными или равными нулю"))
                            message += "-Для этого F(x), значения a и b не могут быть отрицательными или равными нулю.\n";
                        else if (!message.Contains(ex.Message.ToString()))
                            message += "F(x)= " + ex.Message.ToString() + "\n";
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
