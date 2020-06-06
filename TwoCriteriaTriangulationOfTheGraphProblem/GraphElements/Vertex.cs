using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphElements
{
    public class Vertex
    {
        public List<Vertex> Neighbors;//sąsiedzi wierzchołka

        public string VertexValue { get; set; }//wartość wyświetlana w UI
        public int VertexDegree { get; set; }//stopnie potrzebne w naprawie
        public int Index { get; set; }//nie pamietam po co to było xd, przydało się parę razy przy Eulerze i spójności
        public bool IsVisited { get; set; } = false;//to jest potrzebne do spójności

        private string tooltip;
        public string Tooltip
        {
            get { return tooltip; }
            set
            {
                tooltip = value;
                NotifyPropertyChanged("Tooltip");
            }
        }

        public SolidColorBrush Color { get; set; } = Brushes.Red;//do ścieżki/cyklu Eulera

        public Vertex(string value, int index)
        {
            Neighbors = new List<Vertex>();
            VertexValue = value;
            Index = index;
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
