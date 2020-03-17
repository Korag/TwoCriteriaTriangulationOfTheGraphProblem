using QuickGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace euler_graph_generator.GraphElements
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


        public Edge(string id, Vertex source, Vertex target)
            : base(source, target)
        {
            ID = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


    }
}
