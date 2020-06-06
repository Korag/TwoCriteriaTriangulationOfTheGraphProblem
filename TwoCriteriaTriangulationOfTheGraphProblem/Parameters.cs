using LiveCharts;
using LiveCharts.Defaults;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    public class Parameters : INotifyPropertyChanged
    {
        public MainWindow MainWindow;

        #region UIControls

        private int _numberOfVertices;
        public int NumberOfVertices
        {
            get { return _numberOfVertices; }
            set
            {
                _numberOfVertices = value;
                NotifyPropertyChanged("NumberOfVertices");
            }
        }

        private double _probabilityOfEdgeGeneration;
        public double ProbabilityOfEdgeGeneration
        {
            get { return _probabilityOfEdgeGeneration; }
            set
            {
                _probabilityOfEdgeGeneration = Math.Round(value, 2);
                NotifyPropertyChanged("ProbabilityValue");
            }
        }

        private double _weightsLowerLimit;
        public double WeightsLowerLimit
        {
            get { return _weightsLowerLimit; }
            set
            {
                _weightsLowerLimit = Math.Round(value, 2);
                NotifyPropertyChanged("WeightsLowerLimit");
            }
        }

        private double _weightsHigherLimit;
        public double WeightsHigherLimit
        {
            get { return _weightsHigherLimit; }
            set
            {
                _weightsHigherLimit = Math.Round(value, 2);
                NotifyPropertyChanged("WeightsHigherLimit");
            }
        }


        private int _iterationNumber;
        public int IterationNumber
        {
            get { return _iterationNumber; }
            set
            {
                _iterationNumber = value;
                NotifyPropertyChanged("IterationNumber");
            }
        }

        private int _iterationLimit;
        public int IterationsLimit
        {
            get { return _iterationLimit; }
            set
            {
                _iterationLimit = value;
                NotifyPropertyChanged("IterationsLimit");
            }
        }

        private double _mutationProbabilityValue;
        public double MutationProbabilityValue
        {
            get { return _mutationProbabilityValue; }
            set
            {
                _mutationProbabilityValue = Math.Round(value, 2);
                NotifyPropertyChanged("MutationProbabilityValue");
            }
        }

        private double _crossoverProbabilityValue;
        public double CrossoverProbabilityValue
        {
            get { return _crossoverProbabilityValue; }
            set
            {
                _crossoverProbabilityValue = Math.Round(value, 2);
                NotifyPropertyChanged("CrossoverProbabilityValue");
            }
        }

        private int _popsize;
        public int Popsize
        {
            get { return _popsize; }
            set
            {
                _popsize = value;
                NotifyPropertyChanged("Popsize");
            }
        }

        private int _sleepTime;
        public int SleepTime
        {
            get { return _sleepTime; }
            set
            {
                _sleepTime = value;
                NotifyPropertyChanged("SleepTime");
            }
        }

        private string _countedExtremum;
        public string CountedExtremum
        {
            get { return _countedExtremum; }
            set
            {
                _countedExtremum = value;
                NotifyPropertyChanged("CountedExtremum");
            }
        }

        private string _layoutAlgorithmType;
        public string LayoutAlgorithmType
        {
            get { return _layoutAlgorithmType; }
            set
            {
                _layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }

        public List<string> LayoutAlgorithmTypes { get; } = new List<string>();//lista z algorytmami rysowania grafów

        #endregion

        #region MacierzIncydencji i MacierzWag

        public DataTable DataTableIncidenceMatrix { get; set; } = new DataTable();
        public DataView DataViewIncidenceMatrix { get; set; }

        public DataTable DataTableWeightsMatrix { get; set; } = new DataTable();
        public DataView DataViewWeightsMatrix { get; set; }

        #endregion

        #region ParetoFront

        public string Name { get; set; }
        public ChartValues<ObservablePoint> ListOfPoints { get; set; } = new ChartValues<ObservablePoint>();

        public void RewriteThePoints(double[][] tempTab)
        {
            ListOfPoints = new ChartValues<ObservablePoint>();
            for (int i = 0; i < tempTab.Length; i++)
            {
                ListOfPoints.Add(new ObservablePoint(Math.Round(tempTab[i][0], 2), Math.Round(tempTab[i][1], 2)));
            }
        }

        public void RewriteThePoints(List<double[]> tempTab)
        {
            ListOfPoints = new ChartValues<ObservablePoint>();
            for (int i = 0; i < tempTab.Count; i++)
            {
                ListOfPoints.Add(new ObservablePoint(Math.Round(tempTab[i][0], 2), Math.Round(tempTab[i][1], 2)));
            }
        }

        #endregion


        #region GeneticAlgorithm

        //  iteracje, X, Y
        public double[][] Population; // [popsize][2]
        public double[][] PopulationAfterSelection;
        public double[][] PopulationAfterMutation;
        public double[][] PopulationAfterCrossover;

        public bool SaveToFile;

        public double[][] PopulationFunctionValue;
        public double[][] PopulationFunctionValueAfterSelection;
        public double[][] PopulationFunctionValueAfterCrossover;

        public double[] FitnessGroup1;
        public double[] FitnessGroup2;
        public double[] FitnessGroup3;
        public double[] FitnessArray;

        public List<double[][]> MatrixToSave;
        public List<double[]> FitnessesToSave;

        //minimum dla liczby krawędzi i dla sumy wag
        public double MinAmountOfEdges { get; set; }
        public double MinSumOfEdgesWeights { get; set; }
        #endregion

        #region GraphChart

        public double[][] incidenceMatrix;//macierz incydencji do obliczeń
        public double[][] weightsMatrix;//macierz wag do obliczeń
        public List<Vertex> verticesBasicGeneratedGraph;//lista przechowująca wierzchołki bazowo wygenerowanego grafu
        public List<Vertex> verticesTriangulationOfGraph;//lista przechowująca wierzchołki trójpodziału

        private Graph _generatedBasicGraph;
        public Graph GeneratedBasicGraph
        {
            get { return _generatedBasicGraph; }
            set
            {
                _generatedBasicGraph = value;
                NotifyPropertyChanged("GeneratedBasicGraph");
            }
        }
        private UndirectedBidirectionalGraph<Vertex, Edge> _undirectedBasicGraph;
        public UndirectedBidirectionalGraph<Vertex, Edge> UndirectedBasicGraph
        {
            get { return _undirectedBasicGraph; }
            set
            {
                _undirectedBasicGraph = value;
                NotifyPropertyChanged("UndirectedBasicGraph");
            }
        }

        private Graph _triangulationOfGraph;
        public Graph TriangulationOfGraph
        {
            get { return _triangulationOfGraph; }
            set
            {
                _triangulationOfGraph = value;
                NotifyPropertyChanged("TriangulationOfGraph");
            }
        }
        private UndirectedBidirectionalGraph<Vertex, Edge> _undirectedTriangulationOfGraph;
        public UndirectedBidirectionalGraph<Vertex, Edge> UndirectedTriangulationOfGraph
        {
            get { return _undirectedTriangulationOfGraph; }
            set
            {
                _undirectedTriangulationOfGraph = value;
                NotifyPropertyChanged("UndirectedTriangulationOfGraph");
            }
        }

        #endregion

        // Konstruktor
        public Parameters()
        {
            //Algorytmy rysowania/generowania grafów
            LayoutAlgorithmTypes.Add("BoundedFR");
            LayoutAlgorithmTypes.Add("Circular");
            LayoutAlgorithmTypes.Add("CompoundFDP");
            LayoutAlgorithmTypes.Add("EfficientSugiyama");
            LayoutAlgorithmTypes.Add("FR");
            LayoutAlgorithmTypes.Add("ISOM");
            LayoutAlgorithmTypes.Add("KK");
            LayoutAlgorithmTypes.Add("LinLog");
            LayoutAlgorithmTypes.Add("Tree");

            //Domyślny algorytm
            LayoutAlgorithmType = "Circular";
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion
    }
}
