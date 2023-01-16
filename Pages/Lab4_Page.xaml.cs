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
using ScottPlot;

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
        /*private TextBox TxtBxE { get; set; }
        private TextBox TxtBxFx { get; set; }*/

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

        }
        private void AddBaseControlsValues()
        {
            OnStartControls = new List<Control>() { MenuItemN, /*MenuItemE, MenuItemFx,*/ MenuItemNext };
            DataControls = new List<Control>() { MenuItemXi, MenuItemYi, MenuItemClearOpetations, MenuItemSolveOperation, MenuItemAdd, MenuItemBack };

            TxtBxXi = GetStyleElement(TextBoxXi, "MainTextBox") as TextBox;
            TxtBxYi = GetStyleElement(TextBoxYi, "MainTextBox") as TextBox;
            TxtBxN = GetStyleElement(TextBoxN, "MainTextBox") as TextBox;

            LblXi = GetStyleElement(LabelXi, "MainLabel") as Label;
            LblYi = GetStyleElement(LabelYi, "MainLabel") as Label;

          

            OnStartUITextBoxes = new TextBox[]
            {
               TxtBxN
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
                if (GraphicPointList.Items.Count >= n)
                {
                    MenuItemAdd.Visibility = Visibility.Collapsed;
                    return;
                }
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
        private void ShowGraph(LabsClases.LeastSquareMethod.LinearFunction linearFunction)
        {
            ClearPlot();
            for (int i = 0; i < linearFunction.XValue.Length; ++i)
                WpfPlot1.Plot.AddScatter(
                              new double[] { linearFunction.XValue[i] },
                              new double[] { linearFunction.YValue[i] },
                              color: System.Drawing.Color.FromName("Blue"),
                              markerSize: 7,
                              markerShape: MarkerShape.filledDiamond
                              );

            
            WpfPlot1.Plot.Title(linearFunction.GetFunctionFormula());
            WpfPlot1.Plot.AddScatter(linearFunction.XValue, linearFunction.Ylinear, markerShape: MarkerShape.none, lineWidth: 3, color: System.Drawing.Color.FromName("Red"));

            WpfPlot1.Refresh();


        }
        private void ShowGraph(LabsClases.LeastSquareMethod.QuadraticFunction quadraticFunction)
        {
            ClearPlot();
            for (int i = 0; i < quadraticFunction.XValue.Length; ++i)
                if (i != 0)
                {
                    WpfPlot1.Plot.AddScatter(
                              new double[] { quadraticFunction.XValue[i] },
                              new double[] { quadraticFunction.YValue[i] },
                              color: System.Drawing.Color.FromName("Blue"),
                              markerSize: 7,
                              markerShape: MarkerShape.filledDiamond
                              );
                }
                else
                {
                    WpfPlot1.Plot.AddScatter(
                              new double[] { quadraticFunction.XValue[i] },
                              new double[] { quadraticFunction.YValue[i] },
                              color: System.Drawing.Color.FromName("Blue"),
                              markerSize: 7,
                              markerShape: MarkerShape.filledDiamond,
                              label: "Y"
                              );
                }

            WpfPlot1.Plot.Title(quadraticFunction.GetFunctionFormula());

            double min = LabsClases.GraphicPoint.GraphicPoints[0].Xi;
            double max = LabsClases.GraphicPoint.GraphicPoints[0].Yi;

            LabsClases.GraphicPoint.GraphicPoints.ForEach(el =>
            {
                if (el.Xi > max) max = el.Xi;
                if (el.Xi < min) min = el.Xi;
            });
            if (min > (-max))
                min = -max;
            else if (max < Math.Abs(min))
                max = Math.Abs(min);


            List<double> X = new List<double>();
            List<double> Y = new List<double>();

            for (double i = min; i <= max; i += 0.01)
            {
                Y.Add((quadraticFunction.A * Math.Pow(i, 2)) + (quadraticFunction.B * i) + quadraticFunction.C);
                X.Add(i);
            }

            WpfPlot1.Plot.AddScatter(
                X.ToArray(),
                Y.ToArray(),
                markerShape: MarkerShape.none,
                lineWidth: 3,
                color: System.Drawing.Color.FromName("Red"),
                label: $"Y = {Math.Round(quadraticFunction.A, 5)}*x^2+{Math.Round(quadraticFunction.B, 5)}*x+{Math.Round(quadraticFunction.C, 5)}");


            var legend = WpfPlot1.Plot.Legend();
            legend.FontName = "comic sans ms";
            legend.FontSize = 16;
            legend.FontBold = true;
            legend.FontColor = System.Drawing.Color.DarkBlue;

            WpfPlot1.Refresh();


        }
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            
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

            double.TryParse(TxtBxN.Text, out double N);
            if (Check.CheckTextBoxesValues(DataUITextBoxes))
            {
                //добавить новые данные в списке
                double.TryParse(TxtBxXi.Text, out double Xi);
                double.TryParse(TxtBxYi.Text, out double Yi);

                if (LabsClases.GraphicPoint.GraphicPoints.Count >= N && LabsClases.GraphicPoint.EditableList.Count == 0)
                    return;

                LabsClases.GraphicPoint.EditValues(Xi, Yi);
                //удалить данные которые уже отредактированы
                LabsClases.GraphicPoint.DeleteEditedValue();
            }
            else return;

            if (LabsClases.GraphicPoint.EditableList.Count == 0)
            {
                MenuItemEdit.Visibility = Visibility.Collapsed;

                if (LabsClases.GraphicPoint.GraphicPoints.Count >= N)
                    MenuItemAdd.Visibility = Visibility.Collapsed;
                else MenuItemAdd.Visibility = Visibility.Visible;

                LabsClases.GraphicPoint.EditableList.Clear();
                LabsClases.GraphicPoint.EditMode = false;

                index = (index >= N) ? index : GraphicPointList.Items.Count + 1;
                LblXi.Content = $"X({index})";
                LblYi.Content = $"Y({index})";
                TxtBxXi.Text = string.Empty;
                TxtBxYi.Text = string.Empty;
                return;
            }
            LabsClases.GraphicPoint.PrepareDataForEditing(DataUITextBoxes, LblXi, LblYi);

        }
        private void MenuItemSolveLinear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LabsClases.GraphicPoint.GraphicPoints.Count != 0)
                {
                    LabsClases.LeastSquareMethod.LinearFunction function = new LabsClases.LeastSquareMethod.LinearFunction();
                    function.Solve();
                    ShowGraph(function);

                    /*LabsClases.Excell.StartGreatingExcelFile();
                    LabsClases.Excell.LinearFunction.FillTemplate();
                    LabsClases.Excell.LinearFunction.AddListViewsData(function);
                    LabsClases.Excell.LinearFunction.FillMatrixsValues(function);
                    LabsClases.Excell.SaveFile();
                    LabsClases.Excell.CloseAndQuitFromFile();*/

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private void MenuItemSolveKvadratical_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LabsClases.GraphicPoint.GraphicPoints.Count != 0)
                {
                    LabsClases.LeastSquareMethod.QuadraticFunction function = new LabsClases.LeastSquareMethod.QuadraticFunction();
                    function.Solve();
                    ShowGraph(function);

                   /* LabsClases.Excell.StartGreatingExcelFile();
                    LabsClases.Excell.QuadraticFunction.FillTemplate();
                    LabsClases.Excell.QuadraticFunction.AddListViewsData(function);
                    LabsClases.Excell.QuadraticFunction.FillMatrixsValues(function);
                    LabsClases.Excell.QuadraticFunction.AddChart();
                    LabsClases.Excell.SaveFile();
                    LabsClases.Excell.CloseAndQuitFromFile();*/

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private void MenuItemClearAll_Click(object sender, RoutedEventArgs e)
        {
            OnStartControls.ForEach(el => el.Visibility = Visibility.Visible);
            DataControls.ForEach(el => el.Visibility = Visibility.Collapsed);
            BtnEdit.Visibility = Visibility.Collapsed;
            MenuItemBack.Visibility = Visibility.Collapsed;

            TxtBxN.Text = string.Empty;
            /*TxtBxE.Text = string.Empty;
            TxtBxFx.Text = string.Empty;*/
            TxtBxXi.Text = string.Empty;
            TxtBxYi.Text = string.Empty;

            LabsClases.GraphicPoint.Clear();
            index = 0;
            WpfPlot1.Plot.Title("");
            WpfPlot1.UpdateDefaultStyle();
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
        private void ClearPlot()
        {
            WpfPlot1.Plot.Title("");
            WpfPlot1.UpdateDefaultStyle();
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();
        }
        private void MenuItemClearPlot_Click(object sender, RoutedEventArgs e)
        {
            ClearPlot();
        }
        private void MenuItemClearData_Click(object sender, RoutedEventArgs e)
        {
            OnStartControls.ForEach(el => el.Visibility = Visibility.Collapsed);
            DataControls.ForEach(el => el.Visibility = Visibility.Visible);
            BtnEdit.Visibility = Visibility.Collapsed;
            MenuItemBack.Visibility = Visibility.Collapsed;
            TxtBxXi.Text = string.Empty;
            TxtBxYi.Text = string.Empty;

            LabsClases.GraphicPoint.Clear();
            index = 0;
            WpfPlot1.Plot.Title("");
            WpfPlot1.UpdateDefaultStyle();
            WpfPlot1.Plot.Clear();
            WpfPlot1.Refresh();

        }
    }
}
