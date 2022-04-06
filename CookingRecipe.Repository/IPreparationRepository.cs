using CookingRecipe.Core;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookingRecipe.Repository
{
    /// <summary>
    /// Defines methods for interacting with the products backend.
    /// </summary>
    public interface IPreparationRepository
    {
        /// <summary>
        /// Returns all preparations. 
        /// </summary>
        Task<IEnumerable<Preparation>> GetAsync();

        /// <summary>
        /// Returns the preparation with the given id. 
        /// </summary>
        Task<Preparation> GetAsync(Guid id);

        /// <summary>
        /// Adds a new preparation if the preparation does not exist, updates the 
        /// existing preparation otherwise.
        /// </summary>
        Task<Preparation> UpsertAsync(Preparation preparation);

        /// <summary>
        /// Deletes a preparation.
        /// </summary>
        Task DeleteAsync(Guid preparationId);
    }
}
