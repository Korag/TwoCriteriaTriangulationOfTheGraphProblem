using GraphSharp.Controls;
using System;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphElements;

namespace TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods
{
    public class GraphLayout : GraphLayout<Vertex, Edge, Graph>
    {
        public GraphLayout()
        {
            base.AnimationLength = TimeSpan.FromMilliseconds(0);
        }
    }
}
