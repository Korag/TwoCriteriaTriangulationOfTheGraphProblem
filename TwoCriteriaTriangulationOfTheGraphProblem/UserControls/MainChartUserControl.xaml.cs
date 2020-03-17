using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace TwoCriteriaTriangulationOfTheGraphProblem.UserControls
{
    /// <summary>
    /// Interaction logic for CartesianChartUserCtrl.xaml
    /// </summary>
    public partial class MainChartUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Parameters _Parameters = new Parameters();


        public new string Name2
        {
            get { return _Parameters.Name; }
            set { _Parameters.Name = value; }
        }



        public MainChartUserControl()
        {
            InitializeComponent();

            var r = new Random();

            SeriesCollection = new SeriesCollection();
           // SeriesCollection2 = new SeriesCollection();


            YFormatter = value => value.ToString();

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Avg Function 1",
                Values = new ChartValues<double>(),
                LineSmoothness = 1,
                PointForeground = Brushes.White,
                Stroke = Brushes.Red,
                Fill = Brushes.Transparent
            });

            SeriesCollection.Add(new LineSeries
            {
                Title = "Avg Function 2",
                Values = new ChartValues<double>(),
                LineSmoothness = 1,
                PointForeground = Brushes.White,
                Stroke = Brushes.Blue,
                Fill = Brushes.Transparent
            });
            SeriesCollection.Add(new LineSeries
            {
                Title = "Min Function 1",
                Values = new ChartValues<double>(),
                LineSmoothness = 1,
                PointForeground = Brushes.White,
                Stroke = Brushes.Green,
                Fill = Brushes.Transparent
            });
            SeriesCollection.Add(new LineSeries
            {
                Title = "Min Function 2",
                Values = new ChartValues<double>(),
                LineSmoothness = 1,
                PointForeground = Brushes.White,
                Fill = Brushes.Transparent,
                Stroke = Brushes.Yellow
            });
            //modifying any series values will also animate and update the chart

            DataContext = this;

        }

        public void EditSeriesCollection(double[][] FunctionValueCollection, int Iteration)
        {
            SeriesCollection[0].Values.Add(Function1Average(FunctionValueCollection));
            SeriesCollection[1].Values.Add(Function2Average(FunctionValueCollection));
            SeriesCollection[2].Values.Add(Function1Minimum(FunctionValueCollection));
            SeriesCollection[3].Values.Add(Function2Minimum(FunctionValueCollection));
            Labels = GetStringFromIterations(Iteration+1);
        }

        private string[] GetStringFromIterations(int Iteration)
        {
            string[] ResultTable = new string[Iteration];

            for (int i = 0; i < Iteration; i++)
            {
                ResultTable[i] = (i+1).ToString();
            }

            return ResultTable;
        }

        private double Function1Average(double[][] FunctionValueCollection)
        {
            double Result = 0;

            for (int i = 0; i < FunctionValueCollection.Length; i++)
            {
                Result += FunctionValueCollection[i][0];
            }

            return Math.Round(Result / FunctionValueCollection.Length,2);
        }

        private double Function2Average(double[][] FunctionValueCollection)
        {
            double Result = 0;

            for (int i = 0; i < FunctionValueCollection.Length; i++)
            {
                Result += FunctionValueCollection[i][1];
            }

            return Math.Round(Result / FunctionValueCollection.Length, 2);
        }


        private double Function1Minimum(double[][] FunctionValueCollection)
        {
            double Result = FunctionValueCollection[0][0];

            for (int i = 0; i < FunctionValueCollection.Length; i++)
            {
                if (Result > FunctionValueCollection[i][0])
                {
                    Result = FunctionValueCollection[i][0];
                }
                
            }

            return Math.Round(Result, 2);
        }
        private double Function2Minimum(double[][] FunctionValueCollection)
        {
            double Result = FunctionValueCollection[0][1];

            for (int i = 0; i < FunctionValueCollection.Length; i++)
            {
                if (Result > FunctionValueCollection[i][1])
                {
                    Result = FunctionValueCollection[i][1];
                }

            }

            return Math.Round(Result, 2);
        }


        public SeriesCollection SeriesCollection { get; set; }
       // public SeriesCollection SeriesCollection2 { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


    }
}
