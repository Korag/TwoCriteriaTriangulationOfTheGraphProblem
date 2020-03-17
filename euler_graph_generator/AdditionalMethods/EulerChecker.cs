using euler_graph_generator.GraphElements;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace euler_graph_generator.AdditionalMethods
{
    public static class EulerChecker
    {
        //trzeba było mówić wcześniej że jest na to algorytm bo namieszałem, ale działa

        //sprawdzanie Eulera tutaj zaczynamy
        public static bool CheckIfEuler(Graph graph, List<Edge> edgesToColor, int sleepTime, ref List<int> EulerPath)
        {
            bool result = false;
            //sprawdzanie ścieżki zaczynmy od kazdego wierzchołka aż się uda znaleźć
            foreach (var Edge in graph.Edges)
            {
                //resetujemy krawędzie
                edgesToColor = new List<Edge>();
                foreach (var Edgee in graph.Edges)
                {
                    Edgee.EdgeColor = Brushes.LimeGreen;
                    Edgee.IsVisited = false;
                }
                EulerPath = new List<int>();
                VisitEdge(Edge, -1, edgesToColor, graph);
                //jeśli uda się znaleźć ścieżkę eulera to kończymy przeszukiwanie
                result = graph.Edges.Any(v => v.IsVisited == false);
                if (result == false)
                {
                    ColorEdges(edgesToColor, sleepTime);
                    GenerateEulerPathList(edgesToColor, ref EulerPath);
                    return !result;
                }
            }
            return false;
        }

        //Edge e<-- krawędź którą aktualnie przeszukujemy, int blockedVertex<-- zablokowany wierzchołek, następna krawędź nie może się z nim łączyć
        //List<Edge> edgesToColor<-- to już było opisywane, zachowanie kolejności w rysowaniu
        //Graph graph<- stąd pobieram wszystkie potrzebne dane
        private static void VisitEdge(Edge e, int blockedVertex, List<Edge> edgesToColor, Graph graph)
        {

            if (blockedVertex == -1)
            {
                edgesToColor.Add(e);
            }

            e.IsVisited = true;
            var from = e.Source;
            var to = e.Target;
            var indexVertex = 1;
            //wyznaczanie następnej krawędzi, przez to że te krawędzie dalej są jednokierunkowe(usuneliśmy tylko strzałkę, nie zmieniliśmy logiki) to sprawdzanie jest strasznie dzikie
            //nowy source może być równy następnemu source'owi lub target'owi oraz nie może być odwiedzony, dodatkowo sprawdzamy czy następny wybrany nie łączy się z zablokowanym wierzchołkiem
            var nextEdge = graph.Edges.Where(x => (x.Source.Index == to.Index || x.Source.Index == from.Index || x.Target == to || x.Target == from) && x.IsVisited != true && x.Source.Index != blockedVertex && x.Target.Index != blockedVertex).FirstOrDefault();

            if (nextEdge != null)
            {
                nextEdge.IsVisited = true;
                edgesToColor.Add(nextEdge);
                indexVertex = CheckVertexIndex(e, nextEdge);//wierzchołkowi o tym inedexie sprawdzamy stopień


            }
            //tutaj sprawdzamy czy wierzchołek jest 1 stopnia, jeśli tak to nie ma z niego powrotu
            var vertex = graph.Vertices.Where(x => x.Index == indexVertex).FirstOrDefault();
            if (nextEdge != null && vertex.Neighbors.Count > 1)
            {
                VisitEdge(nextEdge, BlockVertex(e, nextEdge), edgesToColor, graph);
            }

        }

        private static int BlockVertex(Edge prevEdge, Edge nextEdge)
        {
            //blokowanie działa na zasadzie sprawdzania który index wierzchołka powtórzył się
            int BlockedVertexIndex = -1;
            List<int> tempList = new List<int>();
            tempList.Add(prevEdge.Source.Index);
            tempList.Add(prevEdge.Target.Index);
            tempList.Add(nextEdge.Source.Index);
            tempList.Add(nextEdge.Target.Index);

            var duplicates = tempList
            .GroupBy(i => i)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);
            foreach (var d in duplicates)
            {
                BlockedVertexIndex = d;
            }

            return BlockedVertexIndex;
        }
        //kolorowanie
        private static void ColorEdges(List<Edge> edgesToColor, int sleepTime)
        {
            Task ColorEdgeTask = new Task(() =>
            {
                foreach (var edge in edgesToColor)
                {
                    Thread.Sleep(sleepTime * 1000);
                    edge.EdgeColor = Brushes.Red;

                }
            });
            ColorEdgeTask.Start();
        }
        //tym pobieramy index wierzchołka, któremu sprawdzamy stopień
        private static int CheckVertexIndex(Edge prev, Edge next)
        {
            List<int> tempList = new List<int>();
            tempList.Add(prev.Source.Index);
            tempList.Add(prev.Target.Index);

            if (!tempList.Contains(next.Source.Index))
            {
                return next.Source.Index;
            }
            else
            {
                return next.Target.Index;
            }
        }

        private static void GenerateEulerPathList(List<Edge> edgesToColor, ref List<int> EulerPath)
        {
            List<int> prev = new List<int>();
            List<int> next = new List<int>();

            for (int i = 0; i < edgesToColor.Count - 1; i++)
            {
                prev.Add(edgesToColor[i].Source.Index);
                prev.Add(edgesToColor[i].Target.Index);
                next.Add(edgesToColor[i + 1].Source.Index);
                next.Add(edgesToColor[i + 1].Target.Index);

                for (int x = 0; x < 2; x++)
                {
                    if (!next.Contains(prev[x]))
                    {
                        EulerPath.Add(prev[x] + 1);
                    }
                }
                prev = new List<int>();
                next = new List<int>();

            }
            prev = new List<int>();
            next = new List<int>();
            if (edgesToColor.Count > 1)
            {
                prev.Add(edgesToColor[edgesToColor.Count - 2].Source.Index);
                prev.Add(edgesToColor[edgesToColor.Count - 2].Target.Index);

                next.Add(edgesToColor[edgesToColor.Count - 1].Source.Index);
                next.Add(edgesToColor[edgesToColor.Count - 1].Target.Index);
                for (int x = 0; x < 2; x++)
                {
                    if (prev.Contains(next[x]))
                    {
                        EulerPath.Add(next[x] + 1);
                    }
                }
                for (int x = 0; x < 2; x++)
                {
                    if (!prev.Contains(next[x]))
                    {
                        EulerPath.Add(next[x] + 1);
                    }
                }
            }

            //EulerPath.Add


        }
    }
}
