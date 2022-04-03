using System;
using System.Globalization;

namespace CookingRecipe
{
	public class NullToTitleHeightConvertor : BaseValueConvertor<NullToTitleHeightConvertor>
	{
		public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return value == null ? 0 : 35;
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
