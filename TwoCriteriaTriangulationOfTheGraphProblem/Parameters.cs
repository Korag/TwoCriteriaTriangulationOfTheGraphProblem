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


        public string F1Formula { get; set; } = "Evol";
        public string F2Formula { get; set; }
        public double F1LeftConstraint { get; set; }
        public double F1RightConstraint { get; set; }
        public double F2LeftConstraint { get; set; }
        public double F2RightConstraint { get; set; }
        public int Popsize { get; set; }
        public double PlausOfMutation { get; set; }
        public double PlausOfCrossing { get; set; }
        public string Minimum { get; set; }
        public int SleepTime{ get; set; }
        public int IterationLimit{ get; set; }
        public int IterationNumber { get; set; }

        //  iteracje, X, Y
        public double[][] Population; // [popsize][2]
        public double[][] PopulationAfterSelection;
        public double[][] PopulationAfterMutation;
        public double[][] PopulationAfterCrossing;

        public double[][] PopulationFunctionValue;
        public double[][] PopulationFunctionValueAfterSelection;
        public double[][] PopulationFunctionValueAfterCrossing;

        //minimum dla f1 oraz f2
        public double MinF1 { get; set; }
        public double MinF2 { get; set; }

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
