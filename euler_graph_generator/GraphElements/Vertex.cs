using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euler_graph_generator.GraphElements
{
    public class Vertex
    {
        public List<Vertex> Neighbors;//sąsiedzi wirzchołka

        public string VertexValue { get; set; }//wartość wyświetlana w UI
        public int VertexDegree { get; set; }//stopnie potrzebne w naprawie
        public int Index { get; set; }//nie pamietam po co to było xd, przydało się parę razy przy Eulerze i spójności
        public bool IsVisited { get; set; } = false;//to jest potrzebne do spójności
        public Vertex(string value, int index)
        {
            Neighbors = new List<Vertex>();
            VertexValue = value;
            Index = index;
        }
       
    }
}
