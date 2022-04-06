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
    /// Contains methods for interacting with the Ingredients backend using REST. 
    /// </summary>
    public class RestIngredientRepository : IIngredientRepository
    {
        private readonly HttpHelper _http;
        
        public RestIngredientRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl); 
        }

        public async Task<IEnumerable<Ingredient>> GetAsync() =>
            await _http.GetAsync<IEnumerable<Ingredient>>("Ingredient");

        public async Task<IEnumerable<Ingredient>> GetAsync(string search) => 
            await _http.GetAsync<IEnumerable<Ingredient>>($"Ingredient/search?value={search}");

        public async Task<Ingredient> GetAsync(Guid id) =>
            await _http.GetAsync<Ingredient>($"Ingredient/{id}");

        public async Task<Ingredient> UpsertAsync(Ingredient Ingredient) => 
            await _http.PostAsync<Ingredient, Ingredient>("Ingredient", Ingredient);

        public async Task DeleteAsync(Guid IngredientId) => 
            await _http.DeleteAsync("Ingredient", IngredientId);
    }
}
