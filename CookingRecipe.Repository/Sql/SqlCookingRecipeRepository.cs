using CookingRecipe.Core;
using CookingRecipe.Repository;

using Microsoft.EntityFrameworkCore;

namespace CookingRecipe.Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the app backend using 
    /// SQL via Entity Framework Core 2.0. 
    /// </summary>
    public class SqlCookingRecipeRepository : ICookingRecipeRepository
    {
        private readonly DbContextOptions<CookingRecipeContext> _dbOptions;

        public SqlCookingRecipeRepository(DbContextOptionsBuilder<CookingRecipeContext>
            dbOptionsBuilder)
        {
            _dbOptions = dbOptionsBuilder.Options;
            using (var db = new CookingRecipeContext(_dbOptions))
            {
                db.Database.EnsureCreated();
            }
        }

        public IRecipeRepository Recipes => new SqlRecipeRepository(
            new CookingRecipeContext(_dbOptions));

        public IPreparationRepository Preparations => new SqlPreparationRepository(
            new CookingRecipeContext(_dbOptions));

        public IIngredientRepository Ingredients => new SqlIngredientRepository(
            new CookingRecipeContext(_dbOptions));

    }
}
