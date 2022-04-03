using System;
using System.Globalization;

namespace CookingRecipe
{
	public class InverseBooleanConvertor : BaseValueConvertor<InverseBooleanConvertor>
	{
		public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return !(bool)value;
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
