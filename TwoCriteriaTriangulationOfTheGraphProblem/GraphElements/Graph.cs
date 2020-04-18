using QuickGraph;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphElements
{
    public class Graph : BidirectionalGraph<Vertex, Edge>
    {
        public Graph() { }

        public Graph(bool allowParallelEdges)
            : base(allowParallelEdges) { }

        public Graph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity) { }
    }
}
