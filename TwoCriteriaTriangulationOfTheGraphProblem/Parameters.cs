using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    public class Parameters : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public ChartValues<ObservablePoint> ListOfPoints { get; set; } = new ChartValues<ObservablePoint>();

        public int NumberOfVertices { get; set; }
        public double ProbabilityOfEdgeGeneration { get; set; }
       
        public double WeightsLowerLimit { get; set; }
        public double WeightsHigherLimit { get; set; }

        public int IterationNumber { get; set; }
        public int IterationsLimit { get; set; }

        public double MutationProbabilityValue { get; set; }
        public double CrossoverProbabilityValue { get; set; }

        public int Popsize { get; set; }
        public int SleepTime{ get; set; }
        public string CountedExtremum { get; set; }

        //  iteracje, X, Y
        public double[][] Population; // [popsize][2]
        public double[][] PopulationAfterSelection;
        public double[][] PopulationAfterMutation;
        public double[][] PopulationAfterCrossover;

        public double[][] PopulationFunctionValue;
        public double[][] PopulationFunctionValueAfterSelection;
        public double[][] PopulationFunctionValueAfterCrossover;

        //minimum dla liczby krawędzi i dla sumy wag
        public double MinAmountOfEdges { get; set; }
        public double MinSumOfEdgesWeights { get; set; }

        public void RewriteThePoints(double[][] tempTab)
        {
            ListOfPoints = new ChartValues<ObservablePoint>();
            for (int i = 0; i < tempTab.Length; i++)
            {
                ListOfPoints.Add(new ObservablePoint(Math.Round(tempTab[i][0],2) , Math.Round(tempTab[i][1],2)));
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
    }
}
