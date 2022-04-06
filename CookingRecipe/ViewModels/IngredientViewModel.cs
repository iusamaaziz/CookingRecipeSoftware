using CookingRecipe.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Windows.System;

namespace CookingRecipe
{
	public class IngredientViewModel : BindableBase
	{
		private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

		public IngredientViewModel(Ingredient model = null) => Model = model ?? new Ingredient();

        private Ingredient _model;

        /// <summary>
        /// Gets or sets the underlying Ingredient object.
        /// </summary>
        public Ingredient Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;

                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Ingredient's first name.
        /// </summary>
        public string Name
        {
            get => IsNewIngredient && string.IsNullOrEmpty(Model.Name) ? "New Ingredient" : Model.Name;
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
        /// Gets or sets the Ingredient's supplier.
        /// </summary>
        public string Supplier
        {
            get => Model.Supplier;
            set
            {
                if (value != Model.Supplier)
                {
                    Model.Supplier = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Supplier));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Ingredient's supplier.
        /// </summary>
        public double Units
        {
            get => Model.Units;
            set
            {
                if (value != Model.Units)
                {
                    Model.Units = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Units));
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

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that indicates whether to show a progress bar. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private bool _isNewIngredient;

        /// <summary>
        /// Gets or sets a value that indicates whether this is a new Ingredient.
        /// </summary>
        public bool IsNewIngredient
        {
            get => _isNewIngredient;
            set => Set(ref _isNewIngredient, value);
        }

        private bool _isInEdit = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the Ingredient data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        /// <summary>
        /// Saves Ingredient data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            if (IsNewIngredient)
            {
                IsNewIngredient = false;
                App.ViewModel.IngredientList.Ingredients.Add(this);
            }

            await App.Repository.Ingredients.UpsertAsync(Model);
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the Ingredient data.
        /// </summary>
        public event EventHandler AddNewIngredientCanceled;

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNewIngredient)
            {
                AddNewIngredientCanceled?.Invoke(this, EventArgs.Empty);
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
                await RefreshIngredientAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        /// <summary>
        /// Reloads all of the Ingredient data.
        /// </summary>
        public async Task RefreshIngredientAsync()
        {
            Model = await App.Repository.Ingredients.GetAsync(Model.Id);
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a Ingredient.
        /// </summary>
        public async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a Ingredient.
        /// </summary>
        public async void EndEdit() => await SaveAsync();
    }
}
