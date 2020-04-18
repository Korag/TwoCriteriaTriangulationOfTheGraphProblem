using System;
using System.ComponentModel;
using System.Data;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods
{
    public class MatrixMethod
    {
        private Parameters _parameters { get; set; }

        public MatrixMethod(Parameters parameters)
        {
            _parameters = parameters;
        }

        // Funkcja pozwala odświeżyć macierz incydencji i wag dla najlepszego trójpodziału obecnej iteracji
        public void RefreshMatrixUi(Graph graph)
        {
            _parameters.incidenceMatrix = GenerateUIMatrix(_parameters.incidenceMatrix, graph);
            _parameters.DataTableIncidenceMatrix = new DataTable();

            SetMatrixColumns(_parameters.DataTableIncidenceMatrix);
            FillDataTable(_parameters.incidenceMatrix, _parameters.DataTableIncidenceMatrix);

            _parameters.DataViewIncidenceMatrix = _parameters.DataTableIncidenceMatrix.DefaultView;

            // TODO: ponowne wyliczanie macierzy wag

            NotifyPropertyChanged("DataViewIncidenceMatrix");
            //NotifyPropertyChanged("DataViewWeightsMatrix");

            NotifyPropertyChanged("TriangulationOfGraph");
            NotifyPropertyChanged("UndirectedTriangulationOfGraph");
        }

        //utworzenie nagłówków w macierzy(UI)
        public void SetMatrixColumns(DataTable dataTable)
        {
            for (int i = -1; i <= _parameters.NumberOfVertices; i++)
            {
                if (i == _parameters.NumberOfVertices)
                {
                    dataTable.Columns.Add(new DataColumn("Suma"));
                }
                else if (i == -1)
                {
                    dataTable.Columns.Add(new DataColumn("Wierzchołki"));
                }
                else
                {
                    dataTable.Columns.Add(new DataColumn((i + 1).ToString()));
                }
            }
        }

        //wiersze z danymi w macierzy(UI)
        public void FillDataTable(double[][] matrix, DataTable dataTable)
        {
            dataTable.Rows.Clear();
            for (int i = 0; i < _parameters.NumberOfVertices; i++)
            {
                var newRow = dataTable.NewRow();

                for (int j = 0; j <= _parameters.NumberOfVertices; j++)
                {
                    newRow[j + 1] = matrix[i][j];
                }
                newRow[0] = i + 1;
                dataTable.Rows.Add(newRow);
            }
        }

        //wygenerowanie początkowych danych w macierzy na podstawie prawdopodobieństwa
        public double[][] FillTheMatrix()
        {
            //utworzenie macierzy
            double[][] tempMatrix = new double[_parameters.NumberOfVertices][];
            tempMatrix = new double[_parameters.NumberOfVertices][];
            for (int i = 0; i < _parameters.NumberOfVertices; i++)
            {
                tempMatrix[i] = new double[_parameters.NumberOfVertices + 1];
            }

            int j = 0;
            double ArraySingleValue = 0;
            Random random = new Random();
            for (int i = 0; i < _parameters.NumberOfVertices; i++)
            {
                j = i;
                while (j < _parameters.NumberOfVertices)
                {
                    ArraySingleValue = random.NextDouble();
                    if (i != j)
                    {
                        if (ArraySingleValue <= _parameters.ProbabilityOfEdgeGeneration)
                        {
                            ArraySingleValue = 1;

                        }
                        else
                        {
                            ArraySingleValue = 0;
                        }
                    }
                    else
                    {
                        ArraySingleValue = 0;
                    }
                    tempMatrix[i][j] = ArraySingleValue;
                    j++;
                }

            }
            //przekopiowanie jednej połowy macierzy na drugą połowę
            tempMatrix = FillTheSecondHalfOfTheMatrix(tempMatrix);
            return tempMatrix;
        }

        //wygenerowanie macierzy(UI) na podstawie krawędzi w grafie
        public double[][] GenerateUIMatrix(double[][] matrixUI, Graph graph)
        {
            matrixUI = new double[_parameters.NumberOfVertices][];
            for (int i = 0; i < _parameters.NumberOfVertices; i++)
            {
                matrixUI[i] = new double[_parameters.NumberOfVertices + 1];
            }
            foreach (var Edge in graph.Edges)
            {
                matrixUI[Edge.Source.Index][Edge.Target.Index] = 1;
                matrixUI[Edge.Target.Index][Edge.Source.Index] = 1;
            }
            CalculateMatrixSum(matrixUI);
            return matrixUI;
        }

        //obliczenie sumy(stopnia) wierzchołków i zapisanie ich w ostatniej kolumnie
        public void CalculateMatrixSum(double[][] matrix)
        {
            for (int i = 0; i < _parameters.NumberOfVertices; i++)
            {
                double sum = 0;
                for (int j = 0; j < _parameters.NumberOfVertices; j++)
                {
                    sum += matrix[i][j];
                }
                matrix[i][_parameters.NumberOfVertices] = sum;
            }
        }

        //przekopiowanie jednej połowy macierzy na drugą połowę
        public double[][] FillTheSecondHalfOfTheMatrix(double[][] matrix)
        {
            for (int i = 0; i < _parameters.NumberOfVertices; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    matrix[i][j] = matrix[j][i];
                }
            }
            return matrix;
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
