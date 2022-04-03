using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace CookingRecipe
{
	/// <summary>
	/// Converts a static Bitmap path to actual Bitmap
	/// </summary>
	public class PathToBitmapImageConverter : BaseValueConvertor<PathToBitmapImageConverter>
	{
		public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			string? path = value?.ToString();

			if (path == null || !File.Exists(path))
#pragma warning disable CS8603 // Possible null reference return.
				return null;
#pragma warning restore CS8603 // Possible null reference return.

			var bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.CacheOption = BitmapCacheOption.OnLoad;
			bmp.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
			bmp.EndInit();
			return bmp;
		}

		public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			throw new NotImplementedException();
		}
	}
}
