using CookingRecipe.Core;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookingRecipe.Repository
{
    /// <summary>
    /// Defines methods for interacting with the recipes backend.
    /// </summary>
    public interface IRecipeRepository
    {
        /// <summary>
        /// Returns all recipes. 
        /// </summary>
        Task<IEnumerable<Recipe>> GetAsync();

        /// <summary>
        /// Returns the recipe with the given id.
        /// </summary>
        Task<Recipe> GetAsync(Guid recipeId);

        /// <summary>
        /// Returns all recipe with a data field matching the start of the given string. 
        /// </summary>
        Task<IEnumerable<Recipe>> GetAsync(string search);

        /// <summary>
        /// Adds a new recipe if the recipe does not exist, updates the 
        /// existing recipe otherwise.
        /// </summary>
        Task<Recipe> UpsertAsync(Recipe recipe);

        /// <summary>
        /// Deletes an recipe.
        /// </summary>
        Task DeleteAsync(Guid recipeId);

    }
}
