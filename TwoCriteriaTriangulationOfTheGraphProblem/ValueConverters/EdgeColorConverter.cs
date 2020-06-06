using System;
using System.Windows.Data;
using System.Windows.Media;

namespace TwoCriteriaTriangulationOfTheGraphProblem.ValueConverters
{
    //konwerter wykorzystywany w kolorowaniu krawędzi
    public class EdgeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (SolidColorBrush)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
