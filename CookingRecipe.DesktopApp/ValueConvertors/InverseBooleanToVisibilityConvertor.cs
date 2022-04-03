using System;
using System.Globalization;
using System.Windows;

namespace CookingRecipe
{
	public class InverseBooleanToVisibilityConvertor : BaseValueConvertor<InverseBooleanToVisibilityConvertor>
	{
		public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return parameter == null
				? (bool)value ? Visibility.Visible : Visibility.Collapsed
				: (bool)value ? Visibility.Collapsed : Visibility.Visible;
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
