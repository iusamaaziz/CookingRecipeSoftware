using CookingRecipe.Core;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookingRecipe.Repository
{
    /// <summary>
    /// Defines methods for interacting with the ingredients backend.
    /// </summary>
    public interface IIngredientRepository
    {
        /// <summary>
        /// Returns all ingredients. 
        /// </summary>
        Task<IEnumerable<Ingredient>> GetAsync();

        /// <summary>
        /// Returns all ingredients with a data field matching the start of the given string. 
        /// </summary>
        Task<IEnumerable<Ingredient>> GetAsync(string search);

        /// <summary>
        /// Returns the ingredient with the given id. 
        /// </summary>
        Task<Ingredient> GetAsync(Guid id);

        /// <summary>
        /// Adds a new ingredient if the ingredient does not exist, updates the 
        /// existing ingredient otherwise.
        /// </summary>
        Task<Ingredient> UpsertAsync(Ingredient ingredient);

        /// <summary>
        /// Deletes a ingredient.
        /// </summary>
        Task DeleteAsync(Guid ingredientId);
    }
}