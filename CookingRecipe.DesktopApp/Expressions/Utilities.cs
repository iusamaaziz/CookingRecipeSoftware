using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CookingRecipe
{
	internal class Utilities
	{
		public static byte[] ConvertBitmapSourceToByteArray(BitmapEncoder encoder, ImageSource imageSource)
		{
			byte[] bytes = null;
			var bitmapSource = imageSource as BitmapSource;

			if (bitmapSource != null)
			{
				encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

				using (var stream = new MemoryStream())
				{
					encoder.Save(stream);
					bytes = stream.ToArray();
				}
			}

			return bytes;
		}
		public static byte[] ConvertBitmapSourceToByteArray(BitmapSource image)
		{
			byte[] data;
			BitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			using (MemoryStream ms = new())
			{
				encoder.Save(ms);
				data = ms.ToArray();
			}
			return data;
		}
		public static byte[] ConvertBitmapToByteArray(Bitmap img)
		{
			using var stream = new MemoryStream();
			img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
			return stream.ToArray();
		}
		public static byte[] ConvertBitmapSourceToByteArray(ImageSource imageSource)
		{
			var image = imageSource as BitmapSource;
			byte[] data;
			BitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			using (MemoryStream ms = new())
			{
				encoder.Save(ms);
				data = ms.ToArray();
			}
			return data;
		}
		public static byte[] ConvertBitmapSourceToByteArray(Uri uri)
		{
			var image = new BitmapImage(uri);
			byte[] data;
			BitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			using (MemoryStream ms = new())
			{
				encoder.Save(ms);
				data = ms.ToArray();
			}
			return data;
		}
		public static byte[] ConvertBitmapSourceToByteArray(string filepath)
		{
			var image = new BitmapImage(new Uri(filepath));
			byte[] data;
			BitmapEncoder encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			using (MemoryStream ms = new())
			{
				encoder.Save(ms);
				data = ms.ToArray();
			}
			return data;
		}
		public static bool IsZenStudio(string injectorName) => injectorName == "ZenStudio" || injectorName == "zen_studio_latest";
		public static BitmapImage ConvertBitmapSourceToBitmapImage(string filePath)
		{
			byte[] stream = ConvertBitmapSourceToByteArray(filePath);
			return ConvertByteArrayToBitmapImage(stream);
		}
		public static BitmapImage ConvertByteArrayToBitmapImage(byte[] bytes)
		{
			if (bytes == null) return null;
			var stream = new MemoryStream(bytes);
			stream.Seek(0, SeekOrigin.Begin);
			var image = new BitmapImage();
			image.BeginInit();
			image.StreamSource = stream;
			image.EndInit();
			return image;
		}
		public static string RandomString(int length = 20)
		{
			const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			Random rand = new();
			return new string(Enumerable.Repeat(characters, length).Select(s => s[rand.Next(s.Length)]).ToArray());
		}

	}
}
