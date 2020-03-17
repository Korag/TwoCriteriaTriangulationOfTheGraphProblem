using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace euler_graph_generator.ValueConverters
{
    //konwerter wykorzystywany przez framework do generowania kształtu i położenia krawędzi
    //(kod został wyciągnięty z source kodu a następnie został zmodyfikowany żeby nie generował strzałek)
    public class ConvertPath : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.Assert(values != null && values.Length == 9, "EdgeRouteToPathConverter should have 9 parameters: pos (1,2), size (3,4) of source; pos (5,6), size (7,8) of target; routeInformation (9).");


            //get the position of the source
            Point sourcePos = new Point()
            {
                X = (values[0] != DependencyProperty.UnsetValue ? (double)values[0] : 0.0),
                Y = (values[1] != DependencyProperty.UnsetValue ? (double)values[1] : 0.0)
            };
            //get the size of the source
            Size sourceSize = new Size()
            {
                Width = (values[2] != DependencyProperty.UnsetValue ? (double)values[2] : 0.0),
                Height = (values[3] != DependencyProperty.UnsetValue ? (double)values[3] : 0.0)
            };
            //get the position of the target
            Point targetPos = new Point()
            {
                X = (values[4] != DependencyProperty.UnsetValue ? (double)values[4] : 0.0),
                Y = (values[5] != DependencyProperty.UnsetValue ? (double)values[5] : 0.0)
            };
            //get the size of the target
            Size targetSize = new Size()
            {
                Width = (values[6] != DependencyProperty.UnsetValue ? (double)values[6] : 0.0),
                Height = (values[7] != DependencyProperty.UnsetValue ? (double)values[7] : 0.0)
            };



            //get the position of the source

            Point[] routeInformation = (values[8] != DependencyProperty.UnsetValue ? (Point[])values[8] : null);

            bool hasRouteInfo = routeInformation != null && routeInformation.Length > 0;

            //
            // Create the path
            //
            Point p1 = GraphConverterHelper.CalculateAttachPoint(sourcePos, sourceSize, (hasRouteInfo ? routeInformation[0] : targetPos));
            Point p2 = GraphConverterHelper.CalculateAttachPoint(targetPos, targetSize, (hasRouteInfo ? routeInformation[routeInformation.Length - 1] : sourcePos));


            PathSegment[] segments = new PathSegment[1 + (hasRouteInfo ? routeInformation.Length : 0)];
            if (hasRouteInfo)
                //append route points
                for (int i = 0; i < routeInformation.Length; i++)
                    segments[i] = new LineSegment(routeInformation[i], true);

            Point pLast = (hasRouteInfo ? routeInformation[routeInformation.Length - 1] : p1);
            Vector v = pLast - p2;
            v = v / v.Length * 0.01;
            Vector n = new Vector(-v.Y, v.X) * 0.4;

            segments[segments.Length - 1] = new LineSegment(p2 + v, true);

            PathFigureCollection pfc = new PathFigureCollection(2);
            pfc.Add(new PathFigure(p1, segments, false));
            //pfc.Add(new PathFigure(p2,
            //                         new PathSegment[] {
            //                                            new LineSegment(p2 + v - n, true),
            //                                            new LineSegment(p2 + v + n, true)}, true));

            return pfc;
        }
        private class GraphConverterHelper
        {
            public static Point CalculateAttachPoint(Point s, Size sourceSize, Point t)
            {
                double[] sides = new double[4];
                sides[0] = (s.X - sourceSize.Width / 2.0 - t.X) / (s.X - t.X);
                sides[1] = (s.Y - sourceSize.Height / 2.0 - t.Y) / (s.Y - t.Y);
                sides[2] = (s.X + sourceSize.Width / 2.0 - t.X) / (s.X - t.X);
                sides[3] = (s.Y + sourceSize.Height / 2.0 - t.Y) / (s.Y - t.Y);

                double fi = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (sides[i] <= 1)
                        fi = Math.Max(fi, sides[i]);
                }

                return t + fi * (s - t);
            }
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
