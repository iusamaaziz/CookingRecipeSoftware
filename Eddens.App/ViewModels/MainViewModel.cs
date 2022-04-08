using Eddens.App.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.System;

namespace Eddens.App
{
	public class MainViewModel : BindableBase
	{
		private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

		/// <summary>
		/// Creates a new MainViewModel.
		/// </summary>
		public MainViewModel()
		{

		}

		public RecipeListViewModel RecipeList { get; set; } = new RecipeListViewModel();

		public IngredientListViewModel IngredientList { get; set; } = new IngredientListViewModel();

		public List<PreparationViewModel> Preparations { get; set; } = new List<PreparationViewModel>();
	}
}
