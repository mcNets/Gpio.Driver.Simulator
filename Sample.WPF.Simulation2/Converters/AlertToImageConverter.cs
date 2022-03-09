using System;
using System.Globalization;

namespace Sample.WPF.Simulation2.Converters
{
    internal class AlertToImageConverter : ValueConverterBase<AlertToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = new("/Sample.WPF.Simulation1;component/Assets/AlertOff.png", UriKind.Relative);

            if (bool.TryParse(value.ToString(), out var image))
            {
                uri = image switch
                {
                    true => new("/Sample.WPF.Simulation1;component/Assets/AlertOn.png", UriKind.Relative),
                    _ => new("/Sample.WPF.Simulation1;component/Assets/AlertOff.png", UriKind.Relative)
                };
            }
            return uri;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
