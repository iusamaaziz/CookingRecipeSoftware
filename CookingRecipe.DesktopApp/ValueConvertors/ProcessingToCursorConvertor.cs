using System;
using System.Globalization;
using System.Windows.Input;

namespace CookingRecipe
{
	public class ProcessingToCursorConvertor : BaseValueConvertor<ProcessingToCursorConvertor>
	{
		public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return (bool)value ? Cursors.AppStarting : Cursors.Hand;
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
