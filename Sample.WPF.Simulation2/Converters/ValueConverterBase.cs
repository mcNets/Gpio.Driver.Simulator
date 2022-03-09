using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Sample.WPF.Simulation2.Converters
{
    public abstract class ValueConverterBase<T> : MarkupExtension, IValueConverter
		where T : class, new()
	{
		#region Private Members

		private static T Converter = null;

		#endregion


		#region Markup Extension Methods

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Converter ?? (Converter = new T());
		}

		#endregion


		#region Value Converter Methods

		public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

		#endregion
	}
}
