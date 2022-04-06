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
    /// Contains methods for interacting with the Ingredients backend using 
    /// SQL via Entity Framework Core 2.0.
    /// </summary>
    public class SqlIngredientRepository : IIngredientRepository
    {
        private readonly CookingRecipeContext _db;

        public SqlIngredientRepository(CookingRecipeContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Ingredient>> GetAsync()
        {
            return await _db.Ingredients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Ingredient> GetAsync(Guid id)
        {
            return await _db.Ingredients
                .AsNoTracking()
                .FirstOrDefaultAsync(Ingredient => Ingredient.Id == id);
        }

        public async Task<IEnumerable<Ingredient>> GetAsync(string value)
        {
            string[] parameters = value.Split(' ');
            return await _db.Ingredients
                .Where(Ingredient =>
                    parameters.Any(parameter =>
                        Ingredient.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        Ingredient.Supplier.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(Ingredient =>
                    parameters.Count(parameter =>
                        Ingredient.Name.StartsWith(parameter) ||
                        Ingredient.Supplier.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Ingredient> UpsertAsync(Ingredient Ingredient)
        {
            var current = await _db.Ingredients.FirstOrDefaultAsync(_Ingredient => _Ingredient.Id == Ingredient.Id);
            if (null == current)
            {
                _db.Ingredients.Add(Ingredient);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(Ingredient);
            }
			try
			{
				await _db.SaveChangesAsync();
			}
			catch (Exception e)
			{
                _ = e;
				throw;
			}
            return Ingredient;
        }

        public async Task DeleteAsync(Guid id)
        {
            var Ingredient = await _db.Ingredients.FirstOrDefaultAsync(_Ingredient => _Ingredient.Id == id);
            if (null != Ingredient)
            {
                _db.Ingredients.Remove(Ingredient);
                await _db.SaveChangesAsync();
            }
        }

		public async Task<IEnumerable<Ingredient>> GetForRecipeAsync(Guid recipeId) =>
            await _db.Ingredients
                .Where(ing => ing.RecipeId == recipeId)
                .AsNoTracking()
                .ToListAsync();
    }
}
