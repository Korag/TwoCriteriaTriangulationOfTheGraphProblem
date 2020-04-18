using QuickGraph;
using System.Collections.Generic;
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
            _parameters.GeneratedBasicGraph = new Graph(true);//nowy graf

            MatrixMethod matrixMethod = new MatrixMethod(_parameters);

            _parameters.incidenceMatrix = matrixMethod.FillTheMatrix();//macierz incydencji
            _parameters.verticesBasicGeneratedGraph = new List<Vertex>();//lista przechowująca wierzchołki

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
            EdgeMethod.GenerateEdges(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph, _parameters.TriangulationOfGraph);

            //suma jest zapisana w ostatniej kolumnie macierzy oraz we właściwości obiektu vertex(VertexDegree)<=potrzebne w naprawie
            VertexMethod.CalculateTheSum(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph);

            //zapisanie wierzołków sąsiadujących ze sobą(potrzebne w naprawie)
            VertexMethod.SetVertexNeighbors(_parameters.incidenceMatrix, _parameters.verticesTriangulationOfGraph);

            _parameters.UndirectedTriangulationOfGraph = new UndirectedBidirectionalGraph<Vertex, Edge>(_parameters.TriangulationOfGraph);//coś jak canvas

            matrixMethod.RefreshMatrixUi(_parameters.TriangulationOfGraph);
        }
    }
}
