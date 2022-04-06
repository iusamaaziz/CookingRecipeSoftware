using CookingRecipe.Navigation;

using Microsoft.Toolkit.Uwp.UI.Controls;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using static System.Net.WebRequestMethods;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CookingRecipe.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class IngredientListPage : Page
	{
		private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
		public IngredientListPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			NavigationRootPage.Current.NavigationView.Header = string.Empty;
		}

		/// <summary>
		/// Gets the app-wide ViewModel instance.
		/// </summary>
		public IngredientListViewModel ViewModel => App.ViewModel.IngredientList;

		/// <summary>
		/// Menu flyout click control for selecting a ingredient and displaying details.
		/// </summary>
		private void ViewDetails_Click(object sender, RoutedEventArgs e)
		{
			if (ViewModel.SelectedIngredient != null)
			{
				Frame.Navigate(typeof(IngredientPage), ViewModel.SelectedIngredient.Model.Id,
					new DrillInNavigationTransitionInfo());
			}
		}

		/// <summary>
		/// Navigates to a blank ingredient details page for the user to fill in.
		/// </summary>
		private void CreateIngredient_Click(object sender, RoutedEventArgs e) =>
			Frame.Navigate(typeof(IngredientPage), null, new DrillInNavigationTransitionInfo());

		/// <summary>
		/// Reverts all changes to the row if the row has changes but a cell is not currently in edit mode.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Escape &&
				ViewModel.SelectedIngredient != null &&
				ViewModel.SelectedIngredient.IsModified &&
				!ViewModel.SelectedIngredient.IsInEdit)
			{
				(sender as DataGrid).CancelEdit(DataGridEditingUnit.Row);
			}
		}

		/// <summary>
		/// Selects the tapped ingredient. 
		/// </summary>
		private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
			ViewModel.SelectedIngredient = (e.OriginalSource as FrameworkElement).DataContext as IngredientViewModel;

		private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
			Frame.Navigate(typeof(IngredientPage), ViewModel.SelectedIngredient.Model.Id,
					new DrillInNavigationTransitionInfo());
	}
}
