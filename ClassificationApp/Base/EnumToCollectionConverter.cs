using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using AttributesExtraction;
using Classification.Metrics;

namespace ClassificationApp.Base
{
    [ValueConversion(typeof(Enum), typeof(IEnumerable<string>))]
    public class EnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(ExtractorType))
                return ExtractorTypeEnumHelper.GetAllValues(value.GetType());
            if(value.GetType() == typeof(MetricType))
                return MetricTypeEnumHelper.GetAllValues(value.GetType());
            
            throw new NotSupportedException($"Cannot convert to this type: { value.GetType()}");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
