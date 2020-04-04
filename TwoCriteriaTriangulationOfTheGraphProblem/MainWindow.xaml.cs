using System;
using System.Collections.Generic;
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

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parameters _parameters { get; set; }

        public MainWindow()
        {
            this.DataContext = _parameters;
            InitializeComponent();
        }

        private void Generuj_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartGeneticAlgorithm(object sender, RoutedEventArgs e)
        {

        }

        private void ResetCurrentState(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateGraph(object sender, RoutedEventArgs e)
        {

        }

        private void SaveResults(object sender, RoutedEventArgs e)
        {

        }
    }
}
