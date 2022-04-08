using Eddens.App.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Eddens.App
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class IngredientsPage : Page
	{
		public IngredientsPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			NavigationRootPage.Current.NavigationView.Header = "Ingredients";
		}
	}

	public class GroupInfoList : List<object>
	{
		public GroupInfoList(IEnumerable<object> items) : base(items) { }

		public object Key { get; set; }

		public string Title { get; set; }

		public override string ToString()
		{
			return Title;
		}
	}
}
