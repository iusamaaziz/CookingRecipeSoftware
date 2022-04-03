﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookingRecipe
{
	/// <summary>
	/// Base value converter for direct XAML usage
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class BaseValueConvertor<T> : MarkupExtension, IValueConverter where T : class, new()
	{

		#region Private Members

		private static T? mConvertor = null;

		#endregion


		#region Markup Value Convertors

		/// <summary>
		/// Provides a static instance of service provider
		/// </summary>
		/// <param name="serviceProvider">The service provider</param>
		/// <returns></returns>
		public override object ProvideValue(IServiceProvider? serviceProvider)
		{
			return mConvertor ??= new T();
		}

		#endregion

		#region Value Converter Methods

		public abstract object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture);

		public abstract object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture);

		#endregion
	}
}
