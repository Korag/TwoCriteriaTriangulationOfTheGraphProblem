using LiveCharts;
using LiveCharts.Wpf;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods;
using TwoCriteriaTriangulationOfTheGraphProblem.UserControls;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parameters _parameters { get; set; }
        private BackgroundWorker _bw { get; set; }

        public MainWindow()
        {
            _parameters = new Parameters();
            // Initialize background worker
            _bw = new BackgroundWorker(this._parameters);

            this.DataContext = _parameters;
            this.WindowState = WindowState.Maximized;

            _parameters.MainWindow = this;

            DefaultValue();
            InitializeComponent();
        }

        // w tej klasie będą umieszczone tylko i wyłącznie zdarzenia połączone z frontendem
        // każda operacja musi być przekierowywana do zewnętrznych klas

        //Generowanie basicowego grafu -> akcja wywoływana wyłącznie
        //1 raz dlatego może zablokować frontend

        private void GenerateGraph(object sender, RoutedEventArgs e)
        {
            //Generujemy basic graf, który później nie będzie już zupełnie modyfikowany
            GraphGenerationMethods graphGenerator = new GraphGenerationMethods(_parameters);
            graphGenerator.GenerateBasicGraph();
            this.DataGridIncidence.Columns[0].Visibility = Visibility.Hidden;
            this.DataGridWeights.Columns[0].Visibility = Visibility.Hidden;
        }

        private void StartGeneticAlgorithm(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = false;

            //Uruchamiamy background workera, żeby podczas przetwarzania nie mieć
            //zablokowanego frontendu, który będzie odświeżany co iterację

            //Background worker wykonuje algorytm genetyczny + przegenerowanie grafu
            //+ odświeża frontend

            _bw.worker.RunWorkerAsync();

            //OverallFluctuationChart.EditSeriesCollection(_parameters.FitnessArray.Min(), _parameters.IterationNumber);

            //var test = new GeneticAlgorithmMethods();
            //test.GeneticAlgorithm(_parameters);

            //_parameters.verticesTriangulationOfGraph =
            

        }



        private void ResetCurrentState(object sender, RoutedEventArgs e)
        {
            //Usuwamy wszystkie zapisane dane + wszystko co było generowane
            //Na szybko w sposób toporny, ale działający

            MainWindow mw = new MainWindow();
            this.Close();
            mw.Show();
        }

        private void SaveResults(object sender, RoutedEventArgs e)
        {
            //Obsługa zapisu do pliku
        }

        private void DefaultValue()
        {
            //Ustawiamy dane testowe do kontrolek, aby łatwo i szybko można było testować
            //bez konieczności wpisywania danych

            _parameters.NumberOfVertices = 10;
            _parameters.ProbabilityOfEdgeGeneration = 0.50;

            _parameters.WeightsLowerLimit = 1;
            _parameters.WeightsHigherLimit = 20;

            _parameters.MutationProbabilityValue = 0.40;
            _parameters.CrossoverProbabilityValue = 0.30;

            _parameters.Popsize = 100;
            _parameters.SleepTime = 1;

            _parameters.IterationsLimit = 1;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()+1).ToString();
        }

    }
}
