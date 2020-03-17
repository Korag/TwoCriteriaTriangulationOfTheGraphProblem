using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euler_graph_generator.GraphElements
{
    public class Graph : BidirectionalGraph<Vertex, Edge>
    {
        //nic ciekawego
        public Graph() { }

        public Graph(bool allowParallelEdges)
            : base(allowParallelEdges) { }

        public Graph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity) { }
    }
}
