using CookingRecipe.Core;
using CookingRecipe.Repository;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookingRecipe.Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the Recipes backend using 
    /// SQL via Entity Framework Core 2.0.
    /// </summary>
    public class SqlRecipeRepository : IRecipeRepository
    {
        private readonly CookingRecipeContext _db;

        public SqlRecipeRepository(CookingRecipeContext db) => _db = db;

        public async Task<IEnumerable<Recipe>> GetAsync() =>
            await _db.Recipes
                .Include(Recipe => Recipe.Ingredients)
                .Include(Recipe => Recipe.Preparations)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Recipe> GetAsync(Guid id) =>
            await _db.Recipes
                .Include(Recipe => Recipe.Ingredients)
                .Include(Recipe => Recipe.Preparations)
                .AsNoTracking()
                .FirstOrDefaultAsync(Recipe => Recipe.Id == id);

        public async Task<IEnumerable<Recipe>> GetAsync(string value)
        {
            string[] parameters = value.Split(' ');
            return await _db.Recipes
                .Include(Recipe => Recipe.Ingredients)
                .Include(Recipe => Recipe.Preparations)
                .Where(Recipe => parameters
                    .Any(parameter =>
                        Recipe.Name.StartsWith(parameter) ||
                        Recipe.Category.StartsWith(parameter) ||
                        Recipe.Type.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Recipe> UpsertAsync(Recipe Recipe)
        {
            var existing = await _db.Recipes.FirstOrDefaultAsync(_Recipe => _Recipe.Id == Recipe.Id);
            if (null == existing)
            {
                _db.Recipes.Add(Recipe);
            }
            else
            {
                _db.Entry(existing).CurrentValues.SetValues(Recipe);
            }
            await _db.SaveChangesAsync();
            return Recipe;
        }

        public async Task DeleteAsync(Guid RecipeId)
        {
            var match = await _db.Recipes.FindAsync(RecipeId);
            if (match != null)
            {
                _db.Recipes.Remove(match);
            }
            await _db.SaveChangesAsync();
        }
    }
}
