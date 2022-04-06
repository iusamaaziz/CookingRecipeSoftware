using CookingRecipe.Core;
using CookingRecipe.Repository;
using CookingRecipe.Repository.Rest;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contoso.Repository.Rest
{
    /// <summary>
    /// Contains methods for interacting with the Preparations backend using REST. 
    /// </summary>
    public class RestPreparationRepository : IPreparationRepository
    {
        private readonly HttpHelper _http;

        public RestPreparationRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public Task DeleteAsync(Guid preparationId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Preparation>> GetAsync() =>
            await _http.GetAsync<IEnumerable<Preparation>>("Preparation");

        public async Task<Preparation> GetAsync(Guid id) =>
            await _http.GetAsync<Preparation>($"Preparation/{id}");

        public async Task<IEnumerable<Preparation>> GetAsync(string search) =>
            await _http.GetAsync<IEnumerable<Preparation>>($"Preparation/search?value={search}");

        public Task<Preparation> UpsertAsync(Preparation preparation)
        {
            throw new NotImplementedException();
        }
    }
}
