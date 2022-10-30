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

        private TextBox[] OnStartUITextBoxes { get; set; }
        private TextBox[] DataUITextBoxes { get; set; }

        private List<Control> OnStartControls { get; set; }
        private List<Control> DataControls { get; set; }
        private int index = 0;
        public static ListView GraphicPointsView { get; set; }


        public Lab4_Page()
        {
            InitializeComponent();
            GraphicPointsView = GraphicPointList;
            GraphicPointList.ItemsSource = LabsClases.GraphicPoint.GraphicPointsCollection;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddBaseControlsValues();

        }

        private void AddBaseControlsValues()
        {
            OnStartControls = new List<Control>() { MenuItemN, MenuItemE, MenuItemFx, BtnNext };
            DataControls = new List<Control>() { MenuItemXi, MenuItemYi, MenuItemChooseOperation, BtnAdd };

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
            foreach(var textBox in OnStartUITextBoxes)
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
        }

        private object GetStyleElement(Control element, string name) =>
           element.Template.FindName(name, element);


        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Check.CheckTextBoxesValues(DataUITextBoxes))
            {
                double.TryParse(TxtBxXi.Text, out double Xi);
                double.TryParse(TxtBxYi.Text, out double Yi);

                LabsClases.GraphicPoint graphicPoint = new LabsClases.GraphicPoint(Xi, Yi);
                await LabsClases.GraphicPoint.Add(graphicPoint);
            }
        }

        private void MenuItemSolve_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
            DataControls.ForEach(el => el.Visibility = Visibility.Collapsed);
            TxtBxN.Text = string.Empty;
            TxtBxE.Text = string.Empty;
            TxtBxFx.Text = string.Empty;
            TxtBxXi.Text = string.Empty;
            TxtBxYi.Text = string.Empty;
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
            }


        }
    }
}
