using CookingRecipe.Core;

namespace CookingRecipe.Repository
{
    /// <summary>
    /// Defines methods for interacting with the app backend.
    /// </summary>
    public interface ICookingRecipeRepository
    {
        /// <summary>
        /// Returns the ingredients repository.
        /// </summary>
        IIngredientRepository Ingredients { get; }

        /// <summary>
        /// Returns the recipes repository.
        /// </summary>
        IRecipeRepository Recipes { get; }

        /// <summary>
        /// Returns the preparations repository.
        /// </summary>
        IPreparationRepository Preparations { get;  }
    }
}
