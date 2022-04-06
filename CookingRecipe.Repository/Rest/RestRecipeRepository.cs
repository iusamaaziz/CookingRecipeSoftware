using CookingRecipe.Core;
using CookingRecipe.Repository;
using CookingRecipe.Repository.Rest;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Contoso.Repository.Rest
{
    /// <summary>
    /// Contains methods for interacting with the Recipes backend using REST. 
    /// </summary>
    public class RestRecipeRepository : IRecipeRepository
    {
        private readonly HttpHelper _http;

        public RestRecipeRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public async Task<IEnumerable<Recipe>> GetAsync() =>
            await _http.GetAsync<IEnumerable<Recipe>>("Recipe");

        public async Task<Recipe> GetAsync(Guid id) =>
            await _http.GetAsync<Recipe>($"Recipe/{id}");

        public async Task<IEnumerable<Recipe>> GetForCustomerAsync(Guid customerId) =>
            await _http.GetAsync<IEnumerable<Recipe>>($"Recipe/customer/{customerId}");

        public async Task<IEnumerable<Recipe>> GetAsync(string search) =>
            await _http.GetAsync<IEnumerable<Recipe>>($"Recipe/search?value={search}");

        public async Task<Recipe> UpsertAsync(Recipe Recipe) =>
            await _http.PostAsync<Recipe, Recipe>("Recipe", Recipe);

        public async Task DeleteAsync(Guid RecipeId) =>
            await _http.DeleteAsync("Recipe", RecipeId);
    }
}
