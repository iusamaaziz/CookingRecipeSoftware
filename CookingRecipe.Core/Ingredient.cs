using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipe.Core
{
	public class Ingredient : DbObject
	{
		public string Name { get; set; }
		public string Supplier { get; set; }
		public double Units { get; set; }
		public Guid RecipeId { get; set; } = Guid.NewGuid();
		public Recipe Recipe { get; set; }

		public Ingredient()
		{ }

		public Ingredient(Recipe recipe) : this()
		{
			RecipeId = recipe.Id;
		}
	}
}
