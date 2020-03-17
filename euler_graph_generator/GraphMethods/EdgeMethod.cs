using euler_graph_generator.GraphElements;
using System.Collections.Generic;
using System.Linq;

namespace euler_graph_generator.GraphMethods
{
    public static class EdgeMethod
    {
        //public static void RemoveAllGraphEdges(Graph graph)
        //{
        //    foreach (var edge in graph.Edges)
        //    {
        //        graph.RemoveEdge(edge);
        //    }
        //}

        //generowanie krawędzi na podstawie macierzy(matrix)
        public static Graph GenerateEdges(double[][] matrix, List<Vertex> existingVertices, Graph graph)
        {
            var numberOfVertices = matrix.Length;
            for (int i = 0; i < numberOfVertices; i++)
            {
                int j = i;
                while (j < numberOfVertices)
                {
                    if (matrix[i][j] == 1)
                    {
                        AddNewGraphEdge(existingVertices[i], existingVertices[j], graph);
                    }
                    //else
                    //{
                    //    RemoveTheEdge(graph, i, j);
                    //}
                    j++;
                }
            }
            return graph;
        }

        public static void RemoveTheEdge(Graph graph, int source, int target)
        {
            Edge edge = null;
            if (source != target)
            {
                edge = graph.Edges.Where(e => e.Source.Index == source || e.Source.Index == target
                                        && e.Target.Index == source || e.Target.Index == target).FirstOrDefault();

            }
            if (edge != null)
            {
                graph.RemoveEdge(edge);
            }

        }

        //utworzenie obiektu krawędzi i dodanie go do grafu 
        private static void AddNewGraphEdge(Vertex from, Vertex to, Graph graph)
        {
            string edgeString = string.Format("Connected vertices: {0}-{1}", from.VertexValue, to.VertexValue);
            Edge newEdge = new Edge(edgeString, from, to);

            //sprawdzenie czy określona krawędź istnieje
            if (graph.Edges.Where(x => x.Source == newEdge.Source && x.Target == newEdge.Target).FirstOrDefault() == null)
            {
                graph.AddEdge(newEdge);
            }
        }

    }
}
