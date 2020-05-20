﻿using System.Collections.Generic;
using System.Linq;
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
