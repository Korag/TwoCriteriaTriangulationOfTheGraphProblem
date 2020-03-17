using euler_graph_generator.AdditionalMethods;
using euler_graph_generator.GraphElements;
using euler_graph_generator.GraphMethods;
using GraphSharp.Controls;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;

namespace euler_graph_generator.ViewModels
{
    public class GraphLayout : GraphLayout<Vertex, Edge, Graph>
    {
        public GraphLayout()
        {
            base.AnimationLength = TimeSpan.FromMilliseconds(0);
        }
    }
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Private Data
        //coś się waliło z jedną macierzą dlatego dałem drugą xd
        private double[][] _matrix;//macierz incydencji do obliczeń
        private double[][] _UIMatrix;//macierz do UI
        private List<Vertex> _existingVertices;//lista przechowująca wierzchołki

        private object locker = new object();
        #endregion

        #region Public Data
        public List<int> EulerPath = new List<int>();
        public DataTable DataTable { get; set; } = new DataTable();//macierz w UI
        public DataView DataView { get; private set; }//macierz w UI
        public List<string> LayoutAlgorithmTypes { get; } = new List<string>();//lista z algorytmami rysowania grafów
        public List<Edge> EdgesToColor { get; set; } = new List<Edge>();//Lista która zapewnia prawidłową kolejność rysowania ścieżki/cyklu Eulera
        public readonly BackgroundWorker worker = new BackgroundWorker();//to służy do wykonywania naprawy grafu w odzielnym wątku


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

        private double _probabilityValue;
        public double ProbabilityValue
        {
            get { return _probabilityValue; }
            set
            {
                _probabilityValue = Math.Round(value, 2);
                NotifyPropertyChanged("ProbabilityValue");
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

        private Graph _graph;
        public Graph Graph
        {
            get { return _graph; }
            set
            {
                _graph = value;
                NotifyPropertyChanged("Graph");
            }
        }
        private UndirectedBidirectionalGraph<Vertex, Edge> _undirectedGraph;
        public UndirectedBidirectionalGraph<Vertex, Edge> UndirectedGraph
        {
            get { return _undirectedGraph; }
            set
            {
                _undirectedGraph = value;
                NotifyPropertyChanged("UndirectedGraph");
            }
        }
        #endregion

        #region Constructor
        public MainWindowViewModel()
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


            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
        }
        #endregion


        private void RefreshMatrixUi()
        {
            _UIMatrix = MatrixMethod.GenerateUIMatrix(_UIMatrix, Graph);
            DataTable = new DataTable();
            MatrixMethod.SetMatrixColumns(DataTable);
            MatrixMethod.FillDataTable(_UIMatrix, DataTable);
            DataView = DataTable.DefaultView;
            NotifyPropertyChanged("Graph");
            NotifyPropertyChanged("DataView");
            NotifyPropertyChanged("UndirectedGraph");
        }




        #region Graph repair
        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ////aktualizacja macierzy

            _matrix = MatrixMethod.FillTheSecondHalfOfTheMatrix(_matrix);
            //EdgeMethod.RemoveAllGraphEdges(Graph);

            Graph = EdgeMethod.GenerateEdges(_matrix, _existingVertices, Graph);

            // int edgeCounter = Graph.Edges.Count();
            ////aktualizacja tabeli
            VertexMethod.CalculateTheSum(_matrix, _existingVertices);
            VertexMethod.SetVertexNeighbors(_matrix, _existingVertices);

            RefreshMatrixUi();
        }

        //metoda naprawy grafów autorem jest Owner, zrobiłem delikatny tunining-> mniej się sypie xd
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var workerGraph = Graph;
            var selector = VertexMethod.GetVertexDegreeInfo(_matrix, _existingVertices);
            if (Graph.Edges.Count() >= 1)
            {
                while (selector[1].Count > 0)
                {
                    while (selector[0].Count > 0)
                    {

                        if (selector[1].Count >= 2)
                        {
                            var start = selector[0].Pop();
                            var end1 = selector[1].Pop();
                            var end2 = selector[1].Pop();

                            _matrix[start.Index][end1.Index] = 1;
                            _matrix[end1.Index][start.Index] = 1;
                            _matrix[start.Index][end2.Index] = 1;
                            _matrix[end2.Index][start.Index] = 1;
                            Report();
                        }

                        selector = VertexMethod.GetVertexDegreeInfo(_matrix, _existingVertices);
                        //(selector[1].Count == 0 && selector[0].Count > 0)
                        /*                             var start = selector[0].Pop();
                            var end = selector[2].Pop();
                            _matrix[start.Index][end.Index] = 1;
                            _matrix[end.Index][start.Index] = 1;
                            if (end.Neighbors.Count > 0)
                            {
                                _matrix[start.Index][end.Neighbors[0].Index] = 1;
                                _matrix[end.Neighbors[0].Index][start.Index] = 1;
                                _matrix[end.Index][end.Neighbors[0].Index] = 0;
                                _matrix[end.Neighbors[0].Index][end.Index] = 0;
                                Report();
                            }*/

                        if (selector[1].Count == 0 && selector[0].Count > 0)
                        {
                            var start = selector[0].Pop();
                            var end = selector[2].Pop();
                            _matrix[start.Index][end.Index] = 1;
                            _matrix[end.Index][start.Index] = 1;
                            _matrix[start.Index][end.Neighbors[0].Index] = 1;
                            _matrix[end.Neighbors[0].Index][start.Index] = 1;
                            _matrix[end.Index][end.Neighbors[0].Index] = 0;
                            _matrix[end.Neighbors[0].Index][end.Index] = 0;
                            //EdgeMethod.RemoveTheEdge(Graph, end.Index, end.Neighbors[0].Index);


                            Report();
                        }
                        Report();
                    }

                    while (selector[1].Count > 0)
                    {
                        List<Vertex> connection = new List<Vertex>();
                        connection.Add(selector[1].Pop());
                        connection.Add(selector[1].Pop());
                        Report();
                        if (_matrix[connection[0].Index][connection[1].Index] == 1)
                        {
                            if (connection[0].Neighbors.Count == 1 && connection[1].Neighbors.Count == 1)
                            {
                                _matrix[connection[0].Index][connection[1].Index] = 0;
                                _matrix[connection[1].Index][connection[0].Index] = 0;
                                //EdgeMethod.RemoveTheEdge(Graph, connection[0].Index, connection[1].Index);
                                Report();
                            }
                            else
                            {
                                if (connection[0].Neighbors.Count > 1)
                                {
                                    var helpList = connection[0].Neighbors.Where(v => v.Index != connection[1].Index).ToList();

                                    _matrix[connection[0].Index][helpList[0].Index] = 0;
                                    _matrix[helpList[0].Index][connection[0].Index] = 0;
                                    //EdgeMethod.RemoveTheEdge(Graph, connection[0].Index, helpList[0].Index);
                                    Report();
                                }
                                else
                                {
                                    var helpList = connection[1].Neighbors.Where(v => v.Index != connection[0].Index).ToList();

                                    //if (helpList.Count > 0 && connection.Count > 0)
                                    //{
                                    _matrix[connection[1].Index][helpList[0].Index] = 0;
                                    _matrix[helpList[0].Index][connection[1].Index] = 0;
                                    //EdgeMethod.RemoveTheEdge(Graph, connection[1].Index, helpList[0].Index);

                                    Report();
                                    //}

                                }
                            }

                        }
                        else
                        {
                            _matrix[connection[0].Index][connection[1].Index] = 1;
                            _matrix[connection[1].Index][connection[0].Index] = 1;
                            Report();
                        }
                        Report();
                        selector = VertexMethod.GetVertexDegreeInfo(_matrix, _existingVertices);
                    }
                    selector = VertexMethod.GetVertexDegreeInfo(_matrix, _existingVertices);
                    //if (selector[1].Count == 2)
                    //{
                    //    Report();
                    //    break;
                    //}
                    Report();
                }
            }
            else
            {

                for (int i = 0; i < _existingVertices.Count; i++)
                {
                    if (i == 0)
                    {
                        _matrix[i][i + 1] = 1;
                        _matrix[i][_existingVertices.Count - 1] = 1;
                        Report();
                    }
                    else if (i == _existingVertices.Count - 1)
                    {
                        _matrix[i][0] = 1;
                        _matrix[i][_existingVertices.Count - 1] = 1;
                        Report();
                    }
                    else
                    {
                        _matrix[i][i + 1] = 1;
                        _matrix[i][i - 1] = 1;
                        Report();
                    }
                }
            }
            Report();
            RefreshMatrixUi();
            worker.CancelAsync();
        }
        private void Report()
        {

            worker.ReportProgress(0);


            Thread.Sleep(500);
        }

        #endregion

        #region Public methods
        public void ReLayoutGraph()
        {
            //dane potrzebne w metodach obsługujących macierz
            MatrixMethod.NumberOfVertices = _numberOfVertices;
            MatrixMethod.ProbabilityValue = _probabilityValue;

            Graph = new Graph(true);//nowy graf
            _matrix = MatrixMethod.FillTheMatrix();//macierz incydencji
            _existingVertices = new List<Vertex>();//lista przechowująca wierzchołki

            //wygenerowanie odpowiedniej ilości wierzchołków
            for (int i = 0; i < _numberOfVertices; i++)
            {
                _existingVertices.Add(new Vertex((i + 1).ToString(), i));
                Graph.AddVertex(_existingVertices[i]);
            }

            //generowanie krawędzi na podstawie macierzy
            EdgeMethod.GenerateEdges(_matrix, _existingVertices, Graph);

            //suma jest zapisana w ostatniej kolumnie macierzy oraz we właściwości obiektu vertex(VertexDegree)<=potrzebne w naprawie
            VertexMethod.CalculateTheSum(_matrix, _existingVertices);

            //zapisanie wierzołków sąsiadujących ze sobą(potrzebne w naprawie)
            VertexMethod.SetVertexNeighbors(_matrix, _existingVertices);

            UndirectedGraph = new UndirectedBidirectionalGraph<Vertex, Edge>(Graph);//coś jak canvas


            RefreshMatrixUi();//odświeżenie UI
        }

        //sprawdzanie spójności
        public bool DepthFirstSearch()
        {
            return ConnectionChecker.DepthFirstSearch(Graph);
        }
        //sprawdzanie czy eulerowski
        public bool CheckIfEuler()
        {
            VertexMethod.SetVertexNeighbors(_UIMatrix, _existingVertices);
            return EulerChecker.CheckIfEuler(Graph, EdgesToColor, SleepTime, ref EulerPath);
        }



        //resetowanie danych
        public void ResetData()
        {
            Graph = new Graph(true);
            UndirectedGraph = new UndirectedBidirectionalGraph<Vertex, Edge>(Graph);

            _matrix = null;
            _UIMatrix = null;
            _existingVertices = null;
            DataView = null;
            DataTable = null;

            NotifyPropertyChanged("Graph");
            NotifyPropertyChanged("UndirectedGraph");
            NotifyPropertyChanged("DataView");
        }
        //zapis do pliku
        public void SaveToFile(bool isConsistent, string isEuler, string message, bool deleteFile)
        {
            FileSaver.SaveToFile(Graph, _probabilityValue, _UIMatrix, isConsistent, isEuler, message, deleteFile, EulerPath);
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        //tym w WPF'ie odświeżamy UI
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion
    }
}
