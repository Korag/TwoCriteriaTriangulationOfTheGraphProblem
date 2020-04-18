using System.ComponentModel;
using System.Threading;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    public class BackgroundWorker
    {
        public readonly System.ComponentModel.BackgroundWorker worker;//to służy do wykonywania naprawy grafu w odzielnym wątku
        private Parameters _parameters { get; set; }

        public BackgroundWorker(Parameters parameters)
        {
            _parameters = parameters;
            worker = new System.ComponentModel.BackgroundWorker();

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
        }

        private void worker_Report()
        {
            worker.ReportProgress(0);
            Thread.Sleep(500);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Aktualizujemy frontend

            //aktualizacja macierzy incydencji
            MatrixMethod matrixMethod = new MatrixMethod(_parameters);

            _parameters.incidenceMatrix = matrixMethod.FillTheSecondHalfOfTheMatrix(_parameters.incidenceMatrix);

            _parameters.GeneratedBasicGraph = EdgeMethod.GenerateEdges(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph, _parameters.TriangulationOfGraph);

            //aktualizacja macierzy wag
            //TODO

            //aktualizacja wyświetlanej macierzy incydencji i wag (frontend)
            VertexMethod.CalculateTheSum(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph);
            VertexMethod.SetVertexNeighbors(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph);

            //WAGI
            //TODO 

            matrixMethod.RefreshMatrixUi(_parameters.TriangulationOfGraph);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Przebieg algorytmu genetycznego
            

            //Kiedy potrzebujemy odświeżyć UI
            //worker.Report();


            worker.CancelAsync();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Metoda zostaje wywołana zawsze po zakończeniu pracy przez BackgroundWorkera
        }

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
