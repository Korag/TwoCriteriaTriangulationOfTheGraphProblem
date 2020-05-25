using QuickGraph;
using System;
using System.Linq;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphElements
{
    public class Graph : BidirectionalGraph<Vertex, Edge>
    {
        public Graph() { }

        public Graph(bool allowParallelEdges)
            : base(allowParallelEdges) { }

        public Graph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity) { }

        public new Graph Clone()
        {
            var result = new Graph();
            foreach (var vertex in Vertices)
            {
                var newVertex = new Vertex(vertex.VertexValue, vertex.Index);
                result.AddVertex(newVertex);
            }
            foreach (var edge in Edges)
            {
                var edgeSource = result.Vertices.Where(x => x.Index == edge.Source.Index).FirstOrDefault();
                var edgeTarget = result.Vertices.Where(x => x.Index == edge.Target.Index).FirstOrDefault();
                var newEdge = new Edge(edge.ID, edgeSource, edgeTarget);
                result.AddEdge(newEdge);
            }

            return result;
        }

    }
}
