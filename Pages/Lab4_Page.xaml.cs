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
    /// Логика взаимодействия для Lab4_Page.xaml
    /// </summary>
    /// 
    public partial class Lab4_Page : Page
    {
        private TextBox TxtBxXi { get; set; }
        private TextBox TxtBxYi { get; set; }

        private TextBox TxtBxN { get; set; }
        private TextBox TxtBxE { get; set; }
        private TextBox TxtBxFx { get; set; }

        private Label LblXi { get; set; }
        private Label LblYi { get; set; }
        /// <summary>
        /// Для храниние TextBox-ы которые при загрузки приложение показывается 
        /// </summary>
        private TextBox[] OnStartUITextBoxes { get; set; }
        /// <summary>
        /// Для храниние TextBox-ы которые после нажатия на кнопку Далее показывается
        /// </summary>
        private TextBox[] DataUITextBoxes { get; set; }
        /// <summary>
        /// для хранение элементы управление которые при загрузки приложение показывается
        /// </summary>
        private List<Control> OnStartControls { get; set; }
        /// <summary>
        /// для хранение элементы управление которые после нажатия на кнопку Далее показывается
        /// </summary>
        private List<Control> DataControls { get; set; }
        /// <summary>
        /// значение i  для Xi и Yi 
        /// </summary>
        public int index = 0;

        public Lab4_Page()
        {
            InitializeComponent();
            LabsClases.GraphicPoint.GraphicPointsView = GraphicPointList;
            GraphicPointList.ItemsSource = LabsClases.GraphicPoint.GraphicPointsCollection;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddBaseControlsValues();
            LabsClases.Excell.FillLinearTemplate();
        }

        private void AddBaseControlsValues()
        {
            OnStartControls = new List<Control>() { MenuItemN, MenuItemE, MenuItemFx, MenuItemNext };
            DataControls = new List<Control>() { MenuItemXi, MenuItemYi, MenuItemChooseOperation, MenuItemAdd, MenuItemBack };

            TxtBxXi = GetStyleElement(TextBoxXi, "MainTextBox") as TextBox;
            TxtBxYi = GetStyleElement(TextBoxYi, "MainTextBox") as TextBox;
            TxtBxN = GetStyleElement(TextBoxN, "MainTextBox") as TextBox;
            TxtBxE = GetStyleElement(TextBoxE, "MainTextBox") as TextBox;
            TxtBxFx = GetStyleElement(TextBoxFx, "MainTextBox") as TextBox;

            LblXi = GetStyleElement(LabelXi, "MainLabel") as Label;
            LblYi = GetStyleElement(LabelYi, "MainLabel") as Label;

            TxtBxFx.Tag = "formula";
            TxtBxE.Tag = "e";

            OnStartUITextBoxes = new TextBox[]
            {
               TxtBxN,
               TxtBxE,
               TxtBxFx
            };
            DataUITextBoxes = new TextBox[]
            {
                TxtBxXi,
                TxtBxYi
            };
            foreach (var textBox in OnStartUITextBoxes)
            {
                textBox.Text = "";
                DataObject.AddPastingHandler(textBox, (s, a) => a.CancelCommand());
                if ((string)textBox.Tag != "formula")
                    textBox.PreviewTextInput +=
                new TextCompositionEventHandler(Check.PreviewTextInput);
            }
            foreach (var textBox in DataUITextBoxes)
            {
                textBox.Text = "";
                DataObject.AddPastingHandler(textBox, (s, a) => a.CancelCommand());
                textBox.PreviewTextInput +=
            new TextCompositionEventHandler(Check.PreviewTextInput);
            };

            OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
            DataControls.ForEach(el => el.Visibility = Visibility.Collapsed);
            MenuItemEdit.Visibility = Visibility.Collapsed;
        }

        private object GetStyleElement(Control element, string name) =>
           element.Template.FindName(name, element);


        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Check.CheckTextBoxesValues(DataUITextBoxes))
            {

                double.TryParse(TxtBxXi.Text, out double Xi);
                double.TryParse(TxtBxYi.Text, out double Yi);
                int.TryParse(TxtBxN.Text, out int n);

                LabsClases.GraphicPoint graphicPoint = new LabsClases.GraphicPoint(Xi, Yi);
                await LabsClases.GraphicPoint.Add(graphicPoint);
                if (GraphicPointList.Items.Count >= n)
                {
                    MenuItemAdd.Visibility = Visibility.Collapsed;
                    return;
                }
                ++index;
                LblXi.Content = $"X({index})";
                LblYi.Content = $"Y({index})";

            }
        }

        private void MenuItemSolve_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
            DataControls.ForEach(el => el.Visibility = Visibility.Collapsed);
            MenuItemBack.Visibility = Visibility.Collapsed;

            TxtBxN.Text = string.Empty;
            TxtBxE.Text = string.Empty;
            TxtBxFx.Text = string.Empty;
            TxtBxXi.Text = string.Empty;
            TxtBxYi.Text = string.Empty;

            LabsClases.GraphicPoint.Clear();
            index = 0;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            TxtBxN.Text = "5";
            TxtBxE.Text = "0,0001";
            TxtBxFx.Text = "2*x^2";

            if (Check.CheckTextBoxesValues(OnStartUITextBoxes))
            {
                OnStartControls.ForEach(el => el.Visibility = Visibility.Collapsed);
                DataControls.ForEach(el => el.Visibility = Visibility.Visible);

                int.TryParse(TxtBxN.Text, out int n);
                
                if (LabsClases.GraphicPoint.EditMode)
                    MenuItemBack.Visibility = Visibility.Visible;

                if (GraphicPointList.Items.Count >= n)
                    MenuItemAdd.Visibility = Visibility.Collapsed;

                if (index != 0) return;

                ++index;
                LblXi.Content = $"X({index})";
                LblYi.Content = $"Y({index})";

            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Check.CheckTextBoxesValues(OnStartUITextBoxes))
            {
                OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
                DataControls.ForEach(el => el.Visibility = Visibility.Collapsed);
                MenuItemBack.Visibility = Visibility.Collapsed;
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var focusedItems = GraphicPointList.SelectedItems;

            LabsClases.GraphicPoint.Remove(focusedItems);
            index = GraphicPointList.Items.Count + 1;
            LblXi.Content = $"X({index})";
            LblYi.Content = $"Y({index})";
            MenuItemAdd.Visibility = Visibility.Visible;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MenuItemAdd.Visibility = Visibility.Collapsed;
            MenuItemEdit.Visibility = Visibility.Visible;
            LabsClases.GraphicPoint.EditMode = true;
            System.Collections.IList items = (System.Collections.IList)GraphicPointList.SelectedItems;
            var collection = items.Cast<LabsClases.GraphicPoint>();
            LabsClases.GraphicPoint.EditableList = collection.ToList();

            LabsClases.GraphicPoint.PrepareDataForEditing(DataUITextBoxes, LblXi, LblYi);

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

            if (Check.CheckTextBoxesValues(DataUITextBoxes))
            {
                //добавить новые данные в списке
                double.TryParse(TxtBxXi.Text, out double Xi);
                double.TryParse(TxtBxYi.Text, out double Yi);
                LabsClases.GraphicPoint.EditValues(Xi, Yi);

                //удалить данные которые уже отредактированы
                LabsClases.GraphicPoint.DeleteEditedValue();
            }
            else return;



            if (LabsClases.GraphicPoint.EditableList.Count == 0)
            {
                MenuItemAdd.Visibility = Visibility.Visible;
                MenuItemBack.Visibility = Visibility.Collapsed;

                LabsClases.GraphicPoint.EditableList.Clear();
                LabsClases.GraphicPoint.EditMode = false;
                index = GraphicPointList.Items.Count + 1;
                LblXi.Content = $"X({index})";
                LblYi.Content = $"Y({index})";
                TxtBxXi.Text = string.Empty;
                TxtBxYi.Text = string.Empty;
                return;
            }
            LabsClases.GraphicPoint.PrepareDataForEditing(DataUITextBoxes, LblXi, LblYi);

        }


    }
}
