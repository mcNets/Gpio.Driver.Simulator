using System;
using System.Globalization;

namespace Sample.WPF.Simulation1.Converters
{
    internal class PowerToImageConverter : ValueConverterBase<PowerToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = new("/Sample.WPF.Simulation1;component/Assets/PowerOn.png", UriKind.Relative);

            if (bool.TryParse(value.ToString(), out var image))
            {
                uri = image switch
                {
                    true => new("/Sample.WPF.Simulation1;component/Assets/PowerOn.png", UriKind.Relative),
                    _ => new("/Sample.WPF.Simulation1;component/Assets/PowerOff.png", UriKind.Relative)
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
