using System;
using System.Globalization;

namespace Sample.WPF.Simulation2.Converters
{
    internal class PlayToImageConverter : ValueConverterBase<PlayToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = new("/Sample.WPF.Simulation2;component/Assets/Play.png", UriKind.Relative);

            if (bool.TryParse(value.ToString(), out var image))
            {
                uri = image switch
                {
                    true => new("/Sample.WPF.Simulation2;component/Assets/Pause.png", UriKind.Relative),
                    _ => new("/Sample.WPF.Simulation2;component/Assets/Play.png", UriKind.Relative)
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
