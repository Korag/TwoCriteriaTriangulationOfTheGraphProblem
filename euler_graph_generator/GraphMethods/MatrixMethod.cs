using euler_graph_generator.GraphElements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euler_graph_generator.GraphMethods
{
    public static class MatrixMethod
    {
        public static int NumberOfVertices { get; set; }
        public static double ProbabilityValue { get; set; }

        //utworzenie nagłówków w macierzy(UI)
        public static void SetMatrixColumns(DataTable dataTable)
        {

            for (int i = -1; i <= NumberOfVertices; i++)
            {
                if (i == NumberOfVertices)
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
        public static void FillDataTable(double[][] matrix, DataTable dataTable)
        {
            dataTable.Rows.Clear();
            for (int i = 0; i < NumberOfVertices; i++)
            {
                var newRow = dataTable.NewRow();

                for (int j = 0; j <= NumberOfVertices; j++)
                {
                    newRow[j + 1] = matrix[i][j];
                }
                newRow[0] = i + 1;
                dataTable.Rows.Add(newRow);
            }
        }
        //wygenerowanie początkowych danych w macierzy na podstawie prawdopodobieństwa
        public static double[][] FillTheMatrix()
        {
            //utworzenie macierzy
            double[][] tempMatrix = new double[NumberOfVertices][];
            tempMatrix = new double[NumberOfVertices][];
            for (int i = 0; i < NumberOfVertices; i++)
            {
                tempMatrix[i] = new double[NumberOfVertices + 1];
            }

            int j = 0;
            double ArraySingleValue = 0;
            Random random = new Random();
            for (int i = 0; i < NumberOfVertices; i++)
            {
                j = i;
                while (j < NumberOfVertices)
                {
                    ArraySingleValue = random.NextDouble();
                    if (i != j)
                    {
                        if (ArraySingleValue <= ProbabilityValue)
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
        public  static double[][] GenerateUIMatrix(double[][] matrixUI, Graph graph)
        {
            matrixUI = new double[NumberOfVertices][];
            for (int i = 0; i < NumberOfVertices; i++)
            {
                matrixUI[i] = new double[NumberOfVertices + 1];
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
        public static void CalculateMatrixSum(double[][] matrix)
        {
            for (int i = 0; i < NumberOfVertices; i++)
            {
                double sum = 0;
                for (int j = 0; j < NumberOfVertices; j++)
                {
                    sum += matrix[i][j];
                }
                matrix[i][NumberOfVertices] = sum;
            }
        }

        //przekopiowanie jednej połowy macierzy na drugą połowę
        public static double[][] FillTheSecondHalfOfTheMatrix(double[][] matrix)
        {
            for (int i = 0; i < NumberOfVertices; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    matrix[i][j] = matrix[j][i];
                }
            }
            return matrix;
        }

    }
}
