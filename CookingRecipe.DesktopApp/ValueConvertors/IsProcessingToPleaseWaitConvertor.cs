using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipe
{
	internal class IsProcessingToPleaseWaitConvertor : BaseValueConvertor<IsProcessingToPleaseWaitConvertor>
	{
		public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
		{
#pragma warning disable CS8603 // Possible null reference return.
			return (bool)value ? "Please wait..." : parameter?.ToString();
#pragma warning restore CS8603 // Possible null reference return.
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
