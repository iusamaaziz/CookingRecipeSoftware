using CookingRecipe.Repository;

namespace Contoso.Repository.Rest
{
    /// <summary>
    /// Contains methods for interacting with the app backend using REST. 
    /// </summary>
    public class RestCookingRecipeRepository : ICookingRecipeRepository
    {
        private readonly string _url; 

        public RestCookingRecipeRepository(string url)
        {
            _url = url; 
        }

        public IIngredientRepository Ingredients => new RestIngredientRepository(_url); 

        public IRecipeRepository Recipes => new RestRecipeRepository(_url);

        public IPreparationRepository Preparations => new RestPreparationRepository(_url); 
    }
}
