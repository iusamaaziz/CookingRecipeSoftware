using System;
using System.Globalization;
using System.Windows;

namespace CookingRecipe
{
	public class BooleanToVisibilityConvertor : BaseValueConvertor<BooleanToVisibilityConvertor>
	{
		public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return parameter == null
				? (bool)value ? Visibility.Collapsed : Visibility.Visible
				: (bool)value ? Visibility.Visible : Visibility.Collapsed;
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
