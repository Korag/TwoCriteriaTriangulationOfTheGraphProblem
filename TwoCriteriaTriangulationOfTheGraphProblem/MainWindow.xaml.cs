using Denxorz.ZoomControl;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parameters _parameters { get; set; }
        private BackgroundWorker _bw { get; set; }


        const double ZoomControlTranslateX = 120;
        const double ZoomControlTranslateY = 80;

        public MainWindow()
        {
            _parameters = new Parameters();

            // Initialize background worker
            _bw = new BackgroundWorker(this._parameters);

            this.DataContext = _parameters;
            this.WindowState = WindowState.Maximized;

            _parameters.MainWindow = this;
            _parameters.SaveToFile = false;

            DefaultValue();
            InitializeComponent();
            _parameters.MainWindow.Save.IsEnabled = false;
        }

        private void GenerateGraph(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = true;

            //Generujemy basic graf, który później nie będzie już zupełnie modyfikowany
            GraphGenerationMethods graphGenerator = new GraphGenerationMethods(_parameters);
            graphGenerator.GenerateBasicGraph();

            ResetZoomControl(BasicGraphZoomControl);
        }

        public void ResetZoomControl(ZoomControl zoomControl)
        {
            zoomControl.Mode = Denxorz.ZoomControl.ZoomControlModes.Custom;
            zoomControl.Zoom = 0.8;

            if (zoomControl == BasicGraphZoomControl)
            {
                zoomControl.TranslateX = 120;
                zoomControl.TranslateY = 80;
            }
            else if (zoomControl == TriangulationGraphZoomControl)
            {
                zoomControl.TranslateX = 150;
                zoomControl.TranslateY = 65;
            }

            zoomControl.Mode = Denxorz.ZoomControl.ZoomControlModes.Fill;
        }

        private void StartGeneticAlgorithm(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = false;
            ProgressBar.Maximum = _parameters.IterationsLimit;

            //Reset charts, iteration count
            OverallFluctuationChart.ResetAll();
            SecondChart.ResetAll();
            _parameters.IterationNumber = 0;

            _bw.worker.RunWorkerAsync();
        }

        private void ResetCurrentState(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            this.Close();
            mw.Show();
        }

        private void SaveResults(object sender, RoutedEventArgs e)
        {
            var FileSaver = new FileSaver();
            FileSaver.SaveToFileAsync(_parameters);
        }

        private void DefaultValue()
        {
            //Ustawiamy dane testowe do kontrolek, aby łatwo i szybko można było testować
            //bez konieczności wpisywania danych

            _parameters.NumberOfVertices = 12;
            _parameters.ProbabilityOfEdgeGeneration = 0.50;

            _parameters.WeightsLowerLimit = 1;
            _parameters.WeightsHigherLimit = 20;

            _parameters.MutationProbabilityValue = 0.40;
            _parameters.CrossoverProbabilityValue = 0.30;

            _parameters.Popsize = 100;
            _parameters.SleepTime = 1;

            _parameters.IterationsLimit = 20;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void GroupList_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var detailsWindow = new GroupListDetails();

            //.OrderBy(x => x.Key.Index).ToList().ForEach(x => groupsVerticesString += $"V: {x.Key.Index + 1}, G: {x.Value}; ");
            //_parameters.MainWindow.GroupList.Content = groupsVerticesString;

            var groupsVertices = _parameters.CurrentGroupsVertices.OrderBy(x => x.Key.Index);
            // Create the Table...
            var table1 = new Table();

            var flowDoc = new FlowDocument();
            detailsWindow.FlowDocViewer.Document = flowDoc;
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(table1);

            // Set some global formatting properties for the table.
            table1.CellSpacing = 0;
            table1.Background = Brushes.White;

            //// Create 6 columns and add them to the table's Columns collection.
            //int numberOfColumns = 6;
            //for (int x = 0; x < numberOfColumns; x++)
            //{
            //    table1.Columns.Add(new TableColumn());

            //    // Set alternating background colors for the middle colums.
            //    if (x % 2 == 0)
            //        table1.Columns[x].Background = Brushes.Beige;
            //    else
            //        table1.Columns[x].Background = Brushes.LightSteelBlue;
            //}

            table1.Columns.Add(new TableColumn());
            foreach (var vertex in groupsVertices)
            {
                table1.Columns.Add(new TableColumn());
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            //table1.RowGroups[0].Rows.Add(new TableRow());

            //// Alias the current working row for easy reference.
            TableRow currentRow;

            // Global formatting for the title row.
            //currentRow.Background = Brushes.Silver;
            //currentRow.FontSize = 40;
            //currentRow.FontWeight = System.Windows.FontWeights.Bold;

            //// Add the header row with content,
            //currentRow.Cells.Add(new TableCell(new Paragraph(new Run("2004 Sales Project"))));
            //// and set the row to span all 6 columns.
            //currentRow.Cells[0].ColumnSpan = 6;

            // Add the second (header) row.
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[0];

            // Global formatting for the header row.
            //currentRow.FontSize = 18;
            //currentRow.FontWeight = FontWeights.Bold;

            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Vertex"))));
            foreach (var vertex in groupsVertices)
            {
                var run = new Run(vertex.Key.VertexValue);
                var paragraph = new Paragraph(run);
                paragraph.TextAlignment = TextAlignment.Right;
                var tableCell = new TableCell(paragraph);
                currentRow.Cells.Add(tableCell);
            }
            // Bold the first cell.
            currentRow.Cells[0].FontWeight = FontWeights.Bold;

            // Add the third row.
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[1];

            //// Global formatting for the row.
            //currentRow.FontSize = 12;
            //currentRow.FontWeight = FontWeights.Normal;

            // Add cells with content to the third row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Group"))));
            foreach (var vertex in groupsVertices)
            {
                var run = new Run(vertex.Value.ToString());
                var paragraph = new Paragraph(run);
                paragraph.TextAlignment = TextAlignment.Right;
                var tableCell = new TableCell(paragraph);
                currentRow.Cells.Add(tableCell);
            }

            // Bold the first cell.
            currentRow.Cells[0].FontWeight = FontWeights.Bold;

            //table1.RowGroups[0].Rows.Add(new TableRow());
            //currentRow = table1.RowGroups[0].Rows[3];

            //// Global formatting for the footer row.
            //currentRow.Background = Brushes.LightGray;
            //currentRow.FontSize = 18;
            //currentRow.FontWeight = System.Windows.FontWeights.Normal;

            //// Add the header row with content,
            //currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Projected 2004 Revenue: $810,000"))));
            //// and set the row to span all 6 columns.
            //currentRow.Cells[0].ColumnSpan = 6;

            // Add table borders
            table1.BorderBrush = Brushes.Black;
            table1.BorderThickness = new Thickness(1);
            foreach (var row in table1.RowGroups[0].Rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.BorderThickness = new Thickness(1, 1, 1, 1);
                    cell.BorderBrush = Brushes.Black;

                    cell.Padding = new Thickness(5);
                }
            }

            table1.Columns[0].Width = new GridLength(120);
            if (table1.Columns.Count > 15)
            {
                flowDoc.PageWidth = table1.Columns.Count * 40;
            }

            //detailsWindow.DetailsBlock.Text = (string)GroupList.Content.ToString().Clone();


            //detailsWindow.DetailsTable = table1;

            detailsWindow.Show();



        }
    }
}
