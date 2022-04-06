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
	public class IngredientListViewModel : BindableBase
	{
		private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

		public IngredientListViewModel() => Task.Run(GetIngredientListAsync);

        /// <summary>
        /// The collection of ingredients in the list. 
        /// </summary>
        public ObservableCollection<IngredientViewModel> Ingredients { get; }
			= new ObservableCollection<IngredientViewModel>();

		private IngredientViewModel _selectedIngredient;

        /// <summary>
        /// Gets or sets the selected ingredient, or null if no ingredient is selected. 
        /// </summary>
        public IngredientViewModel SelectedIngredient
        {
            get => _selectedIngredient;
            set => Set(ref _selectedIngredient, value);
        }

        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the ingredients list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        /// <summary>
        /// Gets the complete list of ingredients from the database.
        /// </summary>
        public async Task GetIngredientListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var ingredients = await App.Repository.Ingredients.GetAsync();
            if (ingredients == null)
            {
                return;
            }

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Ingredients.Clear();
                foreach (var c in ingredients)
                {
                    Ingredients.Add(new IngredientViewModel(c));
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified ingredients and reloads the ingredient list from the database.
        /// </summary>
        public void Sync()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                foreach (var modifiedingredient in Ingredients
                    .Where(ingredient => ingredient.IsModified).Select(ingredient => ingredient.Model))
                {
                    await App.Repository.Ingredients.UpsertAsync(modifiedingredient);
                }

                await GetIngredientListAsync();
                IsLoading = false;
            });
        }
    }
}
