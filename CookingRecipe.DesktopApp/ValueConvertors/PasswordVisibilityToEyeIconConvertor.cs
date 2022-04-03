using System;
using System.Globalization;

namespace CookingRecipe
{
	public class PasswordVisibilityToEyeIconConvertor : BaseValueConvertor<PasswordVisibilityToEyeIconConvertor>
	{

		public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return (bool)value ? "Eye" : "EyeOff";
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
