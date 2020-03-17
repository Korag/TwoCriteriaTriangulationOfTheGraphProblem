using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace TwoCriteriaTriangulationOfTheGraphProblem.UserControls
{
    /// <summary>
    /// Interaction logic for CartesianChartUserCtrl.xaml
    /// </summary>
    public partial class CartesianChartUserCtrl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ChartValues<ObservablePoint> TempCollection;
        private Parameters _Parameters = new Parameters();


        public new string Name2
        {
            get { return _Parameters.Name; }
            set { _Parameters.Name = value; }
        }

        public ChartValues<ObservablePoint> PointSeries
        {
            get { return _Parameters.ListOfPoints; }
            set { _Parameters.ListOfPoints = value; }
        }
        public ChartValues<ObservablePoint> ValuesA { get; set; } = null;
        public ChartValues<ObservablePoint> ValuesB { get; set; } = null;

        public ChartValues<ObservablePoint> ValuesC { get; set; } = null;
        //public ChartValues<ObservablePoint> ValuesC { get; set; }

        public CartesianChartUserCtrl()
        {
            InitializeComponent();

            var r = new Random();
            ValuesA = new ChartValues<ObservablePoint>();
            ValuesB = new ChartValues<ObservablePoint>();
            ValuesC = new ChartValues<ObservablePoint>();

            
            
            DataContext = this;

        }

        public void EditASeriesCollection(ChartValues<ObservablePoint> NewCollection)
        {
            ValuesA = NewCollection;
            if (TempCollection!=null)
            {
                ValuesB = TempCollection;
            }
            TempCollection = NewCollection;
        }

        public void SetPointsOutsideTheDomain(ChartValues<ObservablePoint> NewCollection)
        {
            ValuesC = NewCollection;
        }
        public void EditBSeriesCollection(ChartValues<ObservablePoint> NewCollection)
        {
            ValuesA = NewCollection;
        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


    }
}
