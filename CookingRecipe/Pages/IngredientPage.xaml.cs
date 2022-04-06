using CookingRecipe.Core;
using CookingRecipe.Dialogs;
using CookingRecipe.Navigation;

using Microsoft.Toolkit.Uwp.UI.Controls;

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

		//public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Navigate to the previous page when the user cancels the creation of a new ingredient record.
		/// </summary>
		private void AddNewIngredientCanceled(object sender, EventArgs e) => Frame.GoBack();

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
                ViewModel = new IngredientViewModel(new Ingredient(recipe.Model)) { IsNewIngredient = true, IsInEdit = true };
            }//guid is of ingredient
            else
            {
                // Ingredient is an existing.
                var ingredient = await App.Repository.Ingredients.GetAsync(guid);
                ViewModel = new IngredientViewModel(ingredient);
            }

            //ViewModel.AddNewIngredientCanceled += AddNewIngredientCanceled;
            base.OnNavigatedTo(e);

            NavigationRootPage.Current.NavigationView.Header = string.Empty;
        }

        /// <summary>
        /// Check whether there are unsaved changes and warn the user.
        /// </summary>
        //protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        //{
        //    if (ViewModel.IsModified)
        //    {
        //        var saveDialog = new SaveChangesDialog()
        //        {
        //            Title = $"Save changes to Ingredient # {ViewModel.Name}?",
        //            Content = $"Ingredient # {ViewModel.Name} " +
        //                "has unsaved changes that will be lost. Do you want to save your changes?"
        //        };

        //        await saveDialog.ShowAsync();
        //        SaveChangesDialogResult result = saveDialog.Result;

        //        switch (result)
        //        {
        //            case SaveChangesDialogResult.Save:
        //                await ViewModel.SaveAsync();
        //                break;
        //            case SaveChangesDialogResult.DontSave:
        //                break;
        //            case SaveChangesDialogResult.Cancel:
        //                if (e.NavigationMode == NavigationMode.Back)
        //                {
        //                    Frame.GoForward();
        //                }
        //                else
        //                {
        //                    Frame.GoBack();
        //                }
        //                e.Cancel = true;

        //                // This flag gets cleared on navigation, so restore it. 
        //                ViewModel.IsModified = true;
        //                break;
        //        }
        //    }

        //    base.OnNavigatingFrom(e);
        //}

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
