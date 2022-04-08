using CookingRecipe.Core;
using Eddens.App.Dialogs;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Eddens.App.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class RecipePage : Page
	{
		public RecipePage()
		{
			this.InitializeComponent();
		}

        /// <summary>
        /// Used to bind the UI to the data.
        /// </summary>
        public RecipeViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new recipe record.
        /// </summary>
        private void AddNewRecipeCanceled(object sender, EventArgs e) => Frame.GoBack();

        /// <summary>
        /// Displays the selected recipe data.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new RecipeViewModel
                {
                    IsNewRecipe = true,
                    IsInEdit = true
                };
                VisualStateManager.GoToState(this, "NewRecipe", false);
            }
            else
            {
                ViewModel = App.ViewModel.RecipeList.Recipes.Where(
                    recipe => recipe.Model.Id == (Guid)e.Parameter).First();
            }

            NavigationRootPage.Current.NavigationView.Header = string.Empty;
            ViewModel.AddNewRecipeCanceled += AddNewRecipeCanceled;
            base.OnNavigatedTo(e);
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
        /// Disconnects the AddNewRecipeCanceled event handler from the ViewModel 
        /// when the parent frame navigates to a different page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewRecipeCanceled -= AddNewRecipeCanceled;
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void RecipeSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += RecipeSearchBox_QuerySubmitted;
                searchBox.AutoSuggestBox.TextChanged += RecipeSearchBox_TextChanged;
                searchBox.AutoSuggestBox.PlaceholderText = "Search recipes...";
            }
        }

        /// <summary>
        /// Queries the database for a recipe result matching the search text entered.
        /// </summary>
        private async void RecipeSearchBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // If no search query is entered, refresh the complete list.
                if (string.IsNullOrEmpty(sender.Text))
                {
                    sender.ItemsSource = null;
                }
                else
                {
                    sender.ItemsSource = await App.Repository.Recipes.GetAsync(sender.Text);
                }
            }
        }

        /// <summary>
        /// Search by recipe name, email, or phone number, then display results.
        /// </summary>
        private void RecipeSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is Recipe recipe)
            {
                Frame.Navigate(typeof(RecipePage), recipe.Id);
            }
        }

        /// <summary>
        /// Navigates to the ingredient page for the recipe.
        /// </summary>
        private void ViewIngredientButton_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(IngredientPage), ((sender as FrameworkElement).DataContext as Ingredient).Id,
                new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Adds a new ingredient for the recipe.
        /// </summary>
        private void AddIngredient_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(IngredientPage), ViewModel.Model.Id);
    }
}
