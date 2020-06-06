using QuickGraph;
using System.ComponentModel;
using System.Windows.Media;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphElements
{
    public class Edge : Edge<Vertex>, INotifyPropertyChanged
    {
        //ID == opis(wyświetla się w UI jak najedzie się na krawędź)
        private string id;
        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("ID");
            }
        }

        public bool IsVisited { get; set; } = false;//do ścieżki/cyklu Eulera
        public SolidColorBrush EdgeColor { get; set; } = Brushes.LimeGreen;//do ścieżki/cyklu Eulera

        public double Weight;

        public Edge(string id, Vertex source, Vertex target)
            : base(source, target)
        {
            ID = id;
        }

        public Edge(string id, Vertex source, Vertex target, double weight)
            : base(source, target)
        {
            ID = id;
            Weight = weight;
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
