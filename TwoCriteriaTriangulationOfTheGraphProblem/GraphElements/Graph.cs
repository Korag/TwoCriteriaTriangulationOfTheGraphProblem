using QuickGraph;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphElements
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
