using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods
{
    public static class EdgeMethod
    {
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

        public static void AddWeightsToGraph(Graph graph, double[][] weightsMatrix)
        {
            //for (int i = 0; i < weightsMatrix.Length; i++)
            //{
            //    for (int j = 0; j < weightsMatrix[i].Length; j++)
            //    {
            //        var edgeWeight = graph.Edges
            //            .Where(x => x.Source.Index == i && x.Target.Index == j)
            //            .FirstOrDefault()
            //            .Weight;
            //        edgeWeight = weightsMatrix[i][j];
            //    }
            //}

            foreach (var edge in graph.Edges)
            {
                edge.Weight = weightsMatrix[edge.Source.Index][edge.Target.Index];
            }
        }

        public static void AddWeightsTooltip(Graph graph)
        {
            foreach (var edge in graph.Edges)
            {
                edge.ID = string.Format("Connected vertices: {0}-{1}", edge.Source.Index, edge.Target.Index)
                    + $", Weight: {edge.Weight}";
            }
        }


        public static List<double> GetCutsWeightsSum(Graph basicGraph, double[][] caranArray)
        {
            var result = new List<double>();
            for (int i = 0; i < caranArray[0].Length; i++)
            {
                var groupsVertices = GraphGenerationMethods
                    .GetGroupsVertices(basicGraph, caranArray, i)
                    //.Where(x => x.Value != 0)
                    .ToDictionary(x => x.Key, x => x.Value);
                var cuts = basicGraph.Edges.Where(x => groupsVertices[x.Target] != groupsVertices[x.Source]);
                result.Add(cuts.Select(x => x.Weight).Sum());
            }

            return result;
        }

        public static List<double> GetCutsCount(Graph basicGraph, double[][] caranArray)
        {
            var result = new List<double>();
            for (int i = 0; i < caranArray[0].Length; i++)
            {
                var groupsVertices = GraphGenerationMethods
                    .GetGroupsVertices(basicGraph, caranArray, i)
                    //.Where(x => x.Value != 0)
                    .ToDictionary(x => x.Key, x => x.Value);
                var cuts = basicGraph.Edges.Where(x => groupsVertices[x.Target] != groupsVertices[x.Source]);
                result.Add(cuts.Count());
            }

            return result;
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

        public static void ConnectAllVertices(Graph graph)
        {
            graph.Vertices.Take(graph.Vertices.Count() - 1)
                          .Zip(graph.Vertices.Skip(1), (item, next) => (item, next))
                          .ToList()
                          .ForEach(x => AddNewGraphEdge(x.item, x.next, graph));
            //Connect last vertex to first
            AddNewGraphEdge(graph.Vertices.Last(), graph.Vertices.FirstOrDefault(), graph);
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
