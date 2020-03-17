using System.Linq;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;

namespace TwoCriteriaTriangulationOfTheGraphProblem.AdditionalMethods
{
    public static class ConnectionChecker
    {
        //sprawdzanie spójności(Przeszukiwanie w głąb)
        public static bool DepthFirstSearch(Graph graph)
        {
            bool result = true;
            //resetowanie wierzchołków
            foreach (var v in graph.Vertices)
            {
                v.IsVisited = false;
            }
            //pierwszy wierzchołek
            var vertex = graph.Vertices.FirstOrDefault();
            VisitNode(vertex);
            //sprawdzamy czy wszystkie wierzchołki zostały odwiedzone(w tym przypadku czy jest jakiś nieodwiedzony)
            result = graph.Vertices.Any(v => v.IsVisited == false);
            return !result;
        }

        //odwiedzamy rekurencją od pierwszego wierzchołka, a potem lecimy po jego sąsiadach, sąsiadach jego sąsiadów itd
        private static void VisitNode(Vertex v)
        {
            v.IsVisited = true;
            foreach (var u in v.Neighbors)
            {
                if (u.IsVisited != true)
                {
                    VisitNode(u);
                }
            }
        }
    }
}
