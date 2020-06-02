using LiveCharts.Definitions.Series;
using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Troschuetz.Random.Distributions.Continuous;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods
{
    public class GraphGenerationMethods
    {
        private Parameters _parameters { get; set; }

        public GraphGenerationMethods(Parameters parameters)
        {
            _parameters = parameters;
        }

        public void GenerateBasicGraph()
        {
            MatrixMethod matrixMethod = new MatrixMethod(_parameters);

            _parameters.GeneratedBasicGraph = new Graph(true);//nowy graf
            _parameters.verticesBasicGeneratedGraph = new List<Vertex>();//lista przechowująca wierzchołki

            _parameters.incidenceMatrix = matrixMethod.FillIncidenceMatrix();//macierz incydencji
            _parameters.weightsMatrix = matrixMethod.FillWeightsMatrix();//macierz incydencji

            //wygenerowanie odpowiedniej ilości wierzchołków
            for (int i = 0; i < _parameters.NumberOfVertices; i++)
            {
                _parameters.verticesBasicGeneratedGraph.Add(new Vertex((i + 1).ToString(), i));
                _parameters.GeneratedBasicGraph.AddVertex(_parameters.verticesBasicGeneratedGraph[i]);
            }

            //generowanie krawędzi na podstawie macierzy
            EdgeMethod.GenerateEdges(_parameters.incidenceMatrix, _parameters.verticesBasicGeneratedGraph, _parameters.GeneratedBasicGraph);

            //suma jest zapisana w ostatniej kolumnie macierzy oraz we właściwości obiektu vertex(VertexDegree)<=potrzebne w naprawie
            VertexMethod.CalculateTheSum(_parameters.incidenceMatrix, _parameters.verticesBasicGeneratedGraph);

            //zapisanie wierzołków sąsiadujących ze sobą(potrzebne w naprawie)
            VertexMethod.SetVertexNeighbors(_parameters.incidenceMatrix, _parameters.verticesBasicGeneratedGraph);

            _parameters.UndirectedBasicGraph = new UndirectedBidirectionalGraph<Vertex, Edge>(_parameters.GeneratedBasicGraph);//coś jak canvas

            EdgeMethod.AddWeightsToGraph(_parameters.GeneratedBasicGraph, _parameters.weightsMatrix);
            EdgeMethod.AddWeightsTooltip(_parameters.GeneratedBasicGraph);

            matrixMethod.RefreshMatrixUi(_parameters.GeneratedBasicGraph);
        }

        // Ta metoda wymaga, aby wcześniej wyliczona była nowa macierz incydencji oraz lista wierzchołków grafu
        public void GenerateTriangulationOfGraph()
        {
            _parameters.TriangulationOfGraph = new Graph(true);//nowy graf

            MatrixMethod matrixMethod = new MatrixMethod(_parameters);

            _parameters.verticesTriangulationOfGraph = new List<Vertex>();//lista przechowująca wierzchołki

            //generowanie krawędzi na podstawie macierzy
            //EdgeMethod.GenerateEdges(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph, _parameters.TriangulationOfGraph);

            ////suma jest zapisana w ostatniej kolumnie macierzy oraz we właściwości obiektu vertex(VertexDegree)<=potrzebne w naprawie
            //VertexMethod.CalculateTheSum(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph);

            ////zapisanie wierzołków sąsiadujących ze sobą(potrzebne w naprawie)
            //VertexMethod.SetVertexNeighbors(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph);

            //_parameters.UndirectedTriangulationOfGraph = new UndirectedBidirectionalGraph<Vertex, Edge>(_parameters.TriangulationOfGraph);//coś jak canvas

            //matrixMethod.RefreshMatrixUi(_parameters.TriangulationOfGraph);

            var minimumFitnessGraphIndex = _parameters.FitnessArray.ToList().IndexOf(_parameters.FitnessArray.Min());
            var graphFromCaran = GenerateGraphFromCaranWithCuts(
                _parameters.GeneratedBasicGraph,
                _parameters.Population,
                minimumFitnessGraphIndex
                );
            var groupsVertices = GraphGenerationMethods.GetGroupsVertices(
                _parameters.GeneratedBasicGraph,
                _parameters.Population,
                minimumFitnessGraphIndex);
            VertexMethod.ColorVerticesByGroup(
                graphFromCaran,
                groupsVertices
                );



            //var graphFromCaran = GenerateGraphFromCaran(_parameters.Population).Where(x => x != null).ToList();
            //graphFromCaran.Where(x => x.VertexCount != 0).ToList().ForEach(x => EdgeMethod.ConnectAllVertices(x));
            //ColorGraphsEdges(graphFromCaran);

            //var joinedGraphFromCaran = new Graph();
            //graphFromCaran.ToList().ForEach(x => JoinGraphs(joinedGraphFromCaran, x));

            _parameters.TriangulationOfGraph = graphFromCaran;
            _parameters.verticesTriangulationOfGraph = graphFromCaran.Vertices.ToList();

        }

        static readonly Color[] niceColors = {
            Colors.Red,
            Colors.Green,
            Colors.Blue
        };

        public static void ColorGraphsEdges(List<Graph> graphs)
        {
            graphs.Where(x => x != null).
                Zip(Enumerable.Range(0, 3), (item, index) => (index, item)).ToList().
                ForEach(x =>
                {
                    var colorBrush = new SolidColorBrush(niceColors[x.index]);
                    x.item.Edges.ToList().ForEach(y => y.EdgeColor = colorBrush);
                });
        }

        public static Color GetRandomNiceColor()
        {
            var rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
            return niceColors.OrderBy(x => rnd.Next()).First();
        }

        static void JoinGraphs(Graph graph1, Graph graph2)
        {
            graph1.AddVerticesAndEdgeRange(graph2.Edges);
        }

        public static Dictionary<Vertex, int> ReverseVerticesGroups(Dictionary<int, List<Vertex>> verticesGroups)
        {
            var groupsVertices = new Dictionary<Vertex, int>();
            foreach (var item in verticesGroups)
            {
                foreach (var vertex in item.Value)
                {
                    groupsVertices.Add(vertex, item.Key);
                }
            }
            return groupsVertices;
        }

        public static Dictionary<Vertex, int> GetGroupsVertices(Graph graph, double[][] caranArray, int graphId = 0)
        {
            var vertices = graph.Vertices.ToList();
            var verticesGroups = vertices.GroupBy(x => caranArray[x.Index][graphId])
                .ToDictionary(g => Convert.ToInt32(g.Key), g => g.ToList());
            var groupsVertices = ReverseVerticesGroups(verticesGroups);

            return groupsVertices;
        }


        public static Graph GenerateGraphFromCaranWithCuts(Graph baseGraph, double[][] caranArray, int graphId = 0)
        {
            var graph = baseGraph.Clone();
            var groupsVertices = GetGroupsVertices(graph, caranArray, graphId);

            foreach (var edge in graph.Edges)
            {
                // If group is 0 - skip
                if (groupsVertices[edge.Source] == 0 || groupsVertices[edge.Target] == 0) continue;

                // Different groups - cut
                if (groupsVertices[edge.Source] != groupsVertices[edge.Target])
                {
                    var color = new SolidColorBrush(Colors.Black);
                    color.Opacity = 0.5;
                    edge.EdgeColor = color;
                }

                // Same group - color
                if (groupsVertices[edge.Source] == groupsVertices[edge.Target])
                {
                    var edgeGroup = groupsVertices[edge.Source];
                    edge.EdgeColor = new SolidColorBrush(niceColors[edgeGroup - 1]);
                }
            }

            return graph;
        }

        public static List<Graph> GenerateGraphFromCaran(double[][] caranArray, int graphId = 0)
        {
            List<double> groupArray = new List<double>();
            var output = new List<Graph>(){
                null, //Vertices belonging to group 0
                new Graph(),
                new Graph(),
                new Graph()
            };

            foreach (var popArray in caranArray)
            {
                groupArray.Add(popArray[graphId]);
            }

            for (int i = 0; i < groupArray.Count; i++)
            {
                switch (Convert.ToInt32(groupArray[i]))
                {
                    case 0:
                        break;
                    case 1:
                    case 2:
                    case 3:
                        output[Convert.ToInt32(groupArray[i])].AddVertex(new Vertex((i + 1).ToString(), i));
                        break;
                    default:
                        break;
                }
            }

            return output;

        }
    }
}
