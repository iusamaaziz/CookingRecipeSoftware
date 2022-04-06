using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CookingRecipe.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class RecipeListPage : Page
	{
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

		public RecipeListPage()
		{
			this.InitializeComponent();
		}

        /// <summary>
        /// Gets the app-wide ViewModel instance.
        /// </summary>
        public RecipeListViewModel ViewModel => App.ViewModel.RecipeList;

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void RecipeSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (RecipeSearchBox != null)
            {
                RecipeSearchBox.AutoSuggestBox.QuerySubmitted += RecipeSearchBox_QuerySubmitted;
                RecipeSearchBox.AutoSuggestBox.TextChanged += RecipeSearchBox_TextChanged;
                RecipeSearchBox.AutoSuggestBox.PlaceholderText = "Search recipes...";
            }
        }

        /// <summary>
        /// Updates the search box items source when the user changes the search text.
        /// </summary>
        private async void RecipeSearchBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // If no search query is entered, refresh the complete list.
                if (String.IsNullOrEmpty(sender.Text))
                {
                    await dispatcherQueue.EnqueueAsync(async () =>
                        await ViewModel.GetRecipeListAsync());
                    sender.ItemsSource = null;
                }
                else
                {
                    string[] parameters = sender.Text.Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    sender.ItemsSource = ViewModel.Recipes.Where(recipe => parameters
                        .Any(parameter =>
                            recipe.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                            recipe.Category.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                            recipe.Type.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                        .OrderByDescending(recipe => parameters.Count(parameter =>
                            recipe.Name.StartsWith(parameter) ||
                            recipe.Category.StartsWith(parameter) ||
                            recipe.Type.StartsWith(parameter)))
                        .Select(rec => $"{rec.Name}");
                }
            }
        }

        /// Filters or resets the customer list based on the search text.
        /// </summary>
        private async void RecipeSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.QueryText))
            {
                await ResetRecipeList();
            }
            else
            {
                await FilterRecipeList(args.QueryText);
            }
        }

        /// <summary>
        /// Resets the customer list.
        /// </summary>
        private async Task ResetRecipeList()
        {
            await dispatcherQueue.EnqueueAsync(async () =>
                await ViewModel.GetRecipeListAsync());
        }

        /// <summary>
        /// Filters the customer list based on the search text.
        /// </summary>
        private async Task FilterRecipeList(string text)
        {
            string[] parameters = text.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            var matches = ViewModel.Recipes.Where(recipe => parameters
                .Any(parameter =>
                    recipe.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    recipe.Category.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    recipe.Type.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(recipe => parameters.Count(parameter =>
                    recipe.Name.StartsWith(parameter) ||
                    recipe.Category.StartsWith(parameter) ||
                    recipe.Type.StartsWith(parameter)))
                .ToList();

            await dispatcherQueue.EnqueueAsync(() =>
            {
                ViewModel.Recipes.Clear();
                foreach (var match in matches)
                {
                    ViewModel.Recipes.Add(match);
                }
            });
        }

        /// <summary>
        /// Resets the customer list when leaving the page.
        /// </summary>
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await ResetRecipeList();
        }

        /// <summary>
        /// Applies any existing filter when navigating to the page.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RecipeSearchBox.AutoSuggestBox.Text))
            {
                await FilterRecipeList(RecipeSearchBox.AutoSuggestBox.Text);
            }
        }

        /// <summary>
        /// Menu flyout click control for selecting a customer and displaying details.
        /// </summary>
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedRecipe != null)
            {
                Frame.Navigate(typeof(RecipePage), ViewModel.SelectedRecipe.Model.Id,
                    new DrillInNavigationTransitionInfo());
            }
        }

        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(RecipePage), ViewModel.SelectedRecipe.Model.Id,
                    new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Navigates to a blank customer details page for the user to fill in.
        /// </summary>
        private void CreateRecipe_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(RecipePage), null, new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Reverts all changes to the row if the row has changes but a cell is not currently in edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape &&
                ViewModel.SelectedRecipe != null &&
                ViewModel.SelectedRecipe.IsModified &&
                !ViewModel.SelectedRecipe.IsInEdit)
            {
                (sender as DataGrid).CancelEdit(DataGridEditingUnit.Row);
            }
        }

        /// <summary>
        /// Selects the tapped customer. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedRecipe = (e.OriginalSource as FrameworkElement).DataContext as RecipeViewModel;

        /// <summary>
        /// Opens the order detail page for the user to create an order for the selected customer.
        /// </summary>
        private void AddIngredient_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(RecipePage), ViewModel.SelectedRecipe.Model.Id);

    }
}
