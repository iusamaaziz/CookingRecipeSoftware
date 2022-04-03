﻿using System;
using System.Globalization;
using System.Windows;

namespace CookingRecipe
{
	public class NullToVisibilityConvertor : BaseValueConvertor<NullToVisibilityConvertor>
	{
		public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			return value == null ? Visibility.Collapsed : Visibility.Visible;
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
