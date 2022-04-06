using CookingRecipe.Dialogs;
using CookingRecipe.Navigation;

using Microsoft.Toolkit.Uwp.UI.Controls;

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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CookingRecipe.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class IngredientPage : Page
	{
		public IngredientPage()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Used to bind the UI to the data.
		/// </summary>
		public IngredientViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new ingredient record.
        /// </summary>
        private void AddNewIngredientCanceled(object sender, EventArgs e) => Frame.GoBack();

        /// <summary>
        /// Displays the selected ingredient data.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new IngredientViewModel
                {
                    IsNewIngredient = true,
                    IsInEdit = true
                };
                VisualStateManager.GoToState(this, "NewIngredient", false);
            }
            else
            {
                ViewModel = App.ViewModel.IngredientList.Ingredients.Where(
                    ingredient => ingredient.Model.Id == (Guid)e.Parameter).First();
            }

            ViewModel.AddNewIngredientCanceled += AddNewIngredientCanceled;
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
                // Cancel the navigation immediately, otherwise it will continue at the await call. 
                e.Cancel = true;

                void resumeNavigation()
                {
                    if (e.NavigationMode == NavigationMode.Back)
                    {
                        Frame.GoBack();
                    }
                    else
                    {
                        Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
                    }
                }

                var saveDialog = new SaveChangesDialog() { Title = $"Save changes?" };
                await saveDialog.ShowAsync();
                SaveChangesDialogResult result = saveDialog.Result;

                switch (result)
                {
                    case SaveChangesDialogResult.Save:
                        await ViewModel.SaveAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.DontSave:
                        await ViewModel.RevertChangesAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.Cancel:
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
            ViewModel.AddNewIngredientCanceled -= AddNewIngredientCanceled;
            base.OnNavigatedFrom(e);
        }
    }
}
