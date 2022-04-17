using CookingRecipe.Core;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

namespace CookingRecipe.Repository.Sql
{
    /// <summary>
    /// Entity Framework Core DbContext for Contoso.
    /// </summary>
    public class CookingRecipeContext : DbContext
    {
		public CookingRecipeContext()
		{

		}

        /// <summary>
        /// Creates a new Recipe DbContext.
        /// </summary>
        public CookingRecipeContext(DbContextOptions<CookingRecipeContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite(@"Data Source= D:\Code\Fiverr\CookingRecipeSoftware\Eddens.App\Assets\Recipe.db");
				//optionsBuilder.UseSqlServer(@"Server=.;Database=Unreal-DB;Trusted_Connection=True;");
			}
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
