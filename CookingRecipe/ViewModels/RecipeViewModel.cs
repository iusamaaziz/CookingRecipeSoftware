using CookingRecipe.Core;

using Microsoft.Toolkit.Uwp;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.System;
using Windows.UI.Xaml.Controls;

namespace CookingRecipe
{
	public class RecipeViewModel : BindableBase
	{
		private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

		public RecipeViewModel(Recipe model = null) => Model = model ?? new Recipe();

        private Recipe _model;

        /// <summary>
        /// Gets or sets the underlying Recipe object.
        /// </summary>
        public Recipe Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    RefreshIngredients();

                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the recipe's name.
        /// </summary>
        public string Name
        {
            get => IsNewRecipe && string.IsNullOrEmpty(Model.Name) ? "New Recipe" : Model.Name;
            set
            {
                if (value != Model.Name)
                {
                    Model.Name = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Gets or sets the recipe's Category.
        /// </summary>
        public string Category
        {
            get => Model.Category;
            set
            {
                if (value != Model.Category)
                {
                    Model.Category = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        /// <summary>
        /// Gets or sets the recipe's Type.
        /// </summary>
        public string Type
        {
            get => Model.Type;
            set
            {
                if (value != Model.Type)
                {
                    Model.Type = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        /// <summary>
        /// Gets or sets the recipe's quantity.
        /// </summary>
        public double Quantity
        {
            get => Model.Quantity;
            set
            {
                if (value != Model.Quantity)
                {
                    Model.Quantity = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the underlying model has been modified. 
        /// </summary>
        /// <remarks>
        /// Used when sync'ing with the server to reduce load and only upload the models that have changed.
        /// </remarks>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets the collection of the recipe's Ingredients.
        /// </summary>
        public ObservableCollection<Ingredient> Ingredients { get; } = new ObservableCollection<Ingredient>();

        private Ingredient _selectedIngredient;

        /// <summary>
        /// Gets or sets the currently selected order.
        /// </summary>
        public Ingredient SelectedIngredient
        {
            get => _selectedIngredient;
            set => Set(ref _selectedIngredient, value);
        }

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that indicates whether to show a progress bar. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private bool _isNewRecipe;

        /// <summary>
        /// Gets or sets a value that indicates whether this is a new recipe.
        /// </summary>
        public bool IsNewRecipe
        {
            get => _isNewRecipe;
            set => Set(ref _isNewRecipe, value);
        }

        private bool _isInEdit = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the recipe data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        /// <summary>
        /// Saves recipe data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            if (IsNewRecipe)
            {
                IsNewRecipe = false;
                App.ViewModel.RecipeList.Recipes.Add(this);
            }

            await App.Repository.Recipes.UpsertAsync(Model);
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the recipe data.
        /// </summary>
        public event EventHandler AddNewRecipeCanceled;

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNewRecipe)
            {
                AddNewRecipeCanceled?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                await RevertChangesAsync();
            }
        }

        /// <summary>
        /// Discards any edits that have been made, restoring the original values.
        /// </summary>
        public async Task RevertChangesAsync()
        {
            IsInEdit = false;
            if (IsModified)
            {
                await RefreshRecipeAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        /// <summary>
        /// Reloads all of the recipe data.
        /// </summary>
        public async Task RefreshRecipeAsync()
        {
            RefreshIngredients();
            Model = await App.Repository.Recipes.GetAsync(Model.Id);
        }

        /// <summary>
        /// Resets the recipe detail fields to the current values.
        /// </summary>
        public void RefreshIngredients() => Task.Run(LoadIngredientsAsync);

        /// <summary>
        /// Loads the order data for the recipe.
        /// </summary>
        public async Task LoadIngredientsAsync()
        {
            await dispatcherQueue.EnqueueAsync(() =>
            {
                IsLoading = true;
            });

            var ingredient = await App.Repository.Ingredients.GetForRecipeAsync(Model.Id);

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Ingredients.Clear();
                foreach (var order in ingredient)
                {
                    Ingredients.Add(order);
                }

                IsLoading = false;
            });
        }

        /// <summary>
        /// Called when a bound DataGrid control causes the recipe to enter edit mode.
        /// </summary>
        public void BeginEdit()
        {
            // Not used.
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a recipe.
        /// </summary>
        public async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a recipe.
        /// </summary>
        public async void EndEdit() => await SaveAsync();
    }
}
