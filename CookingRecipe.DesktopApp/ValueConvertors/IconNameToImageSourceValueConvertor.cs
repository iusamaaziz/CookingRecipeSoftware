using System;
using System.Globalization;
using System.Windows;

namespace CookingRecipe
{
	public class IconNameToImageSourceValueConvertor : BaseValueConvertor<IconNameToImageSourceValueConvertor>
	{
		public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			try
			{
				return Application.Current.FindResource(value as string);
			}
			catch (Exception)
			{
				return Application.Current.FindResource("NotFountIcon");
			}
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
