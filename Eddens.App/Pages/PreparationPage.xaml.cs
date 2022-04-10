using CookingRecipe.Core;
using Eddens.App.Dialogs;
using Eddens.App.Navigation;
using Eddens.App.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using toolkit = Microsoft.Toolkit.Uwp.UI.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Eddens.App.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PreparationPage : Page
	{
		public PreparationPage()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Used to bind the UI to the data.
		/// </summary>
		public PreparationViewModel ViewModel { get; set; }

		/// <summary>
		/// Navigate to the previous page when the user cancels the creation of a new ingredient record.
		/// </summary>
		private void AddNewPreparationCanceled(object sender, EventArgs e) => Frame.GoBack();

		/// <summary>
		/// Displays the selected ingredient data.
		/// </summary>
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			var guid = (Guid)e.Parameter;
			var recipe = App.ViewModel.RecipeList.Recipes.Where(r => r.Model.Id == guid).FirstOrDefault();

			if (recipe != null)
			{
				// Ingredient is a new ingredient
				ViewModel = new PreparationViewModel(new Preparation(recipe.Model)) { IsNewPreparation = true, IsInEdit = true };
			}
			else
			{
				// Ingredient is an existing.
				var ingredient = await App.Repository.Preparations.GetAsync(guid);
				ViewModel = new PreparationViewModel(ingredient);
			}

			ViewModel.AddNewPreparationCanceled += AddNewPreparationCanceled;
			base.OnNavigatedTo(e);

			NavigationRootPage.Current.NavigationView.Header = string.Empty;
		}

		/// <summary>
		/// Check whether there are unsaved changes and warn the user.
		/// </summary>
		protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			if (ViewModel.IsModified)
			{
				var saveDialog = new SaveChangesDialog()
				{
					Title = $"Save changes to Instruction?",
					Content = $"Instruction # {ViewModel.Instruction} " +
						"has unsaved changes that will be lost. Do you want to save your changes?"
				};

				await saveDialog.ShowAsync();
				SaveChangesDialogResult result = saveDialog.Result;

				switch (result)
				{
					case SaveChangesDialogResult.Save:
						await ViewModel.SaveAsync();
						break;
					case SaveChangesDialogResult.DontSave:
						break;
					case SaveChangesDialogResult.Cancel:
						if (e.NavigationMode == NavigationMode.Back)
						{
							Frame.GoForward();
						}
						else
						{
							Frame.GoBack();
						}
						e.Cancel = true;

						// This flag gets cleared on navigation, so restore it. 
						ViewModel.IsModified = true;
						break;
				}
			}

			base.OnNavigatingFrom(e);
		}

		/// <summary>
		/// Disconnects the AddNewIngredientCanceled event handler from the ViewModel 
		/// when the parent frame navigates to a different page.
		/// </summary>
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			ViewModel.AddNewPreparationCanceled -= AddNewPreparationCanceled;
			base.OnNavigatedFrom(e);
		}

		private void BrowsePhotoButton_Click(object sender, RoutedEventArgs e)
		{
			Notification.Show(ViewModel.Time.ToString(), 4000);
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			PlayVideoTeachingTip.IsOpen = true;
		}
	}
}
