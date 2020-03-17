using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace EvolutionaryAlgorithmApp.UserControls
{
    /// <summary>
    /// Interaction logic for CartesianChartUserCtrl.xaml
    /// </summary>
    public partial class ParetoChartUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Parameters _parameters = ConnectionHelper.ParametersObject;


        private double? F1LeftConstraint;
        private double? F1RightConstraint;
        private double? F2LeftConstraint;
        private double? F2RightConstraint;




        public ChartValues<ObservablePoint> ValuesA { get; set; } = null;
        public ChartValues<ObservablePoint> ValuesB { get; set; } = null;
        public ChartValues<ObservablePoint> ValuesC { get; set; } = null;
        public ChartValues<double> TopPareto { get; set; } = null;
        public ChartValues<double> BottomPareto { get; set; } = null;
        //public ChartValues<ObservablePoint> ValuesC { get; set; }

        public ParetoChartUserControl()
        {
            InitializeComponent();
            ReinitializeVariables();
            var r = new Random();

            BottomPareto = new ChartValues<double>();
            TopPareto = new ChartValues<double>();

            ValuesA = new ChartValues<ObservablePoint>();
            ValuesB = new ChartValues<ObservablePoint>();
            ValuesC = new ChartValues<ObservablePoint>();

          

            SeriesCollection = new SeriesCollection();


            YFormatter = value => value.ToString();

      
            DataContext = this;

        }

        private void ReinitializeVariables()
        {
            //przez to ze strona jest zbindowana do parameters musimy reinicjowac, bo dane z gui sa przesylane tylko do parameters
            F1LeftConstraint = _parameters.F1LeftConstraint;
            F1RightConstraint = _parameters.F1RightConstraint;
            F2LeftConstraint = _parameters.F2LeftConstraint;
            F2RightConstraint = _parameters.F2RightConstraint;

        }

        public void MakeParetoFunctions(double[] interval)
        {
            ReinitializeVariables();
            BottomPareto = CreateBottomPareto(interval);
            TopPareto = CreateTopPareto(interval);
            //Labels = CreateLabels(interval);
        }

        
        public void EditSeriesCollection(ChartValues<ObservablePoint> NewCollection)
        {
            ValuesA = NewCollection;
        }

        public void SetPointsOutsideTheDomain(ChartValues<ObservablePoint> NewCollection)
        {
            //ChartValues<ObservablePoint> tempList = new ChartValues<ObservablePoint>();
            //for (int i = 0; i < NewCollection.Count; i++)
            //{
            //    if (CheckDomain(NewCollection[i], Points[i]))
            //    {
            //        tempList.Add(NewCollection[i]);
            //    }
            //    //if (TopPareto[i] < NewCollection[i].Y)
            //    //{
            //    //    tempList.Add(NewCollection[i]);
            //    //}
            //}
            ValuesC = NewCollection;
        }

        //private bool CheckDomain(ObservablePoint point,double[] Point2)
        //{
        //    bool Result = false;

        //    if (point.Y < Math.Round(((1 + Point2[1]) / Point2[0]),2) )
        //    {
        //        Result = true;
        //    }


        //    return Result;
        //}

        private ChartValues<double> CreateBottomPareto(double[] interval)
        {
            ChartValues<double> tempArray = new ChartValues<double>();
            for (double i = (double)F1LeftConstraint, j  = (double) F2LeftConstraint; i <= (double)F1RightConstraint; i +=0.2, j += 0.3)
            {

                tempArray.Add(((1 + j) / i));
            }

            return tempArray;
        }

        private ChartValues<double> CreateTopPareto(double[] interval)
        {
            ChartValues<double> tempArray = new ChartValues<double>();
            double x = (double)F1LeftConstraint;

            for (double i = (double)F1LeftConstraint, j = (double)F2LeftConstraint; i <= (double)F1RightConstraint; i += 0.2, j += 0.3)
            {

                tempArray.Add((double)F2RightConstraint/(double)F1LeftConstraint + (1 + j) / i);
            }

            //for (double i = (double)F1LeftConstraint, j = (double)F2LeftConstraint; i < F1RightConstraint; i += 0.2, j += 0.2)
            //{
            //    if (i< F1RightConstraint - F1LeftConstraint)
            //    {                   
            //        tempArray.Add(0);
            //    }
            //    else
            //    {

            //        tempArray.Add((1 + j) / x);
            //        x += 0.2;
            //    }

            //}

            return tempArray;
        }

        //private string[] CreateLabels(double[] interval)
        //{
        //    ChartValues<double> tempArray = new ChartValues<double>();
        //    List<string> stringArray = new List<string>();
        //    for (double i = interval[0]; i <= interval[1]; i += 0.01)
        //    {

        //        stringArray.Add(Math.Round(i,2).ToString());
        //    }


        //    return stringArray.ToArray();
        //}

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


    }
}
