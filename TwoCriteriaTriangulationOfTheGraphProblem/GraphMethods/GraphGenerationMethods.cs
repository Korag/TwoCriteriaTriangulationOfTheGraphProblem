using QuickGraph;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

            var graphFromCaran = GenerateGraphFromCaran(_parameters.Population);
            matrixMethod.RefreshMatrixUi(graphFromCaran[1]);
        }

        static List<Graph> GenerateGraphFromCaran(double[][] caranArray)
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
                groupArray.Add(popArray[0]);
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
