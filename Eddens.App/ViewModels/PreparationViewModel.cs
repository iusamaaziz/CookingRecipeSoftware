using CookingRecipe.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.System;

namespace Eddens.App.ViewModels
{
	public class PreparationViewModel : BindableBase
	{
		private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

		public PreparationViewModel(Preparation model = null) => Model = model ?? new Preparation();

		private Preparation _model;

		/// <summary>
		/// Gets or sets the underlying Preparation object.
		/// </summary>
		public Preparation Model
		{
			get { return _model; }
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
		/// Gets or sets the instruction.
		/// </summary>
		public string Instruction
		{
			get => IsNewPreparation && string.IsNullOrEmpty(Model.Instruction) ? $"New Step" : Model.Instruction;
			set
			{
				if (value != Model.Instruction)
				{
					Model.Instruction = value;
					IsModified = true;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Instruction));
				}
			}
		}

		/// <summary>
		/// Gets or sets the time.
		/// </summary>
		public TimeSpan Time
		{
			get => TimeSpan.FromTicks(Model.Time);
			set
			{
				if (value != TimeSpan.FromTicks(Model.Time))
				{
					Model.Time = value.Ticks;
					IsModified = true;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Time));
				}
			}
		}

		/// <summary>
		/// Gets or sets the VideoLink.
		/// </summary>
		public string VideoUrl
		{
			get => Model.VideoUrl;
			set
			{
				if (value != Model.VideoUrl)
				{
					Model.VideoUrl = value;
					IsModified = true;
					OnPropertyChanged();
					OnPropertyChanged(nameof(VideoUrl));
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

        private bool _isNewPreparation;

        /// <summary>
        /// Gets or sets a value that indicates whether this is a new preparation.
        /// </summary>
        public bool IsNewPreparation
        {
            get => _isNewPreparation;
            set => Set(ref _isNewPreparation, value);
        }

        private bool _isInEdit = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the preparation data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        /// <summary>
        /// Saves Preparation data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            if (IsNewPreparation)
            {
                IsNewPreparation = false;
                App.ViewModel.Preparations.Add(this);
            }

            await App.Repository.Preparations.UpsertAsync(Model);
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the Ingredient data.
        /// </summary>
        public event EventHandler AddNewPreparationCanceled;

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNewPreparation)
            {
                AddNewPreparationCanceled?.Invoke(this, EventArgs.Empty);
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
                await RefreshPreparationsAsync();
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
        public async Task RefreshPreparationsAsync()
        {
            Model = await App.Repository.Preparations.GetAsync(Model.Id);
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
