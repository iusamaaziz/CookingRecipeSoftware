using Microsoft.Toolkit.Uwp;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.System;

namespace CookingRecipe
{
	public class RecipeListViewModel : BindableBase
	{
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// Creates a new RecipeListViewModel.
        /// </summary>
        public RecipeListViewModel() => Task.Run(GetRecipeListAsync);

        /// <summary>
        /// The collection of Recipes in the list. 
        /// </summary>
        public ObservableCollection<RecipeViewModel> Recipes { get; }
            = new ObservableCollection<RecipeViewModel>();

        private RecipeViewModel _selectedRecipe;

        /// <summary>
        /// Gets or sets the selected recipes, or null if no recipe is selected. 
        /// </summary>
        public RecipeViewModel SelectedRecipe
        {
            get => _selectedRecipe;
            set => Set(ref _selectedRecipe, value);
        }

        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Recipes list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        /// <summary>
        /// Gets the complete list of recipes from the database.
        /// </summary>
        public async Task GetRecipeListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var recipes = await App.Repository.Recipes.GetAsync();
            if (recipes == null)
            {
                return;
            }

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Recipes.Clear();
                foreach (var c in recipes)
                {
                    Recipes.Add(new RecipeViewModel(c));
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified recipes and reloads the recipe list from the database.
        /// </summary>
        public void Sync()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                foreach (var modifiedRecipe in Recipes
                    .Where(recipe => recipe.IsModified).Select(ingredient => ingredient.Model))
                {
                    await App.Repository.Recipes.UpsertAsync(modifiedRecipe);
                }

                await GetRecipeListAsync();
                IsLoading = false;
            });
        }
    }
}
