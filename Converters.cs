using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media;
using PlanBeh.Models;

namespace PlanBeh
{
    [ValueConversion(typeof(string), typeof(NodeModel))]
    public class StringToNodeModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = value.ToString();
            NodeModel temp = new NodeModel();
            //temp.IconPath = "../Icons/" + name + ".png";
            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(Stroke))]
    public class BoolToConnectionActiveStroke : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new BrushConverter().ConvertFrom("#FF00D1FF") : new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class BoolToNodeActiveBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new BrushConverter().ConvertFrom("#FF00D1FF") : new BrushConverter().ConvertFrom("#FF323232");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
