using CookingRecipe.Core;
using Microsoft.EntityFrameworkCore;

namespace CookingRecipe.Repository.Sql
{
    /// <summary>
    /// Entity Framework Core DbContext for Contoso.
    /// </summary>
    public class CookingRecipeContext : DbContext
    {
        /// <summary>
        /// Creates a new Recipe DbContext.
        /// </summary>
        public CookingRecipeContext(DbContextOptions<CookingRecipeContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets the Ingredients DbSet.
        /// </summary>
        public DbSet<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Gets the Preparations DbSet.
        /// </summary>
        public DbSet<Preparation> Preparations { get; set; }

        /// <summary>
        /// Gets the Recipes DbSet.
        /// </summary>
        public DbSet<Recipe> Recipes { get; set; }

    }
}
