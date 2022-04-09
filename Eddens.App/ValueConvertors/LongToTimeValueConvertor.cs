using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Data;

namespace Eddens.App.ValueConvertors
{
	internal class LongToTimeValueConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			long ticks =  (long)value;
			TimeSpan span = TimeSpan.FromTicks(ticks);
			return span;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
