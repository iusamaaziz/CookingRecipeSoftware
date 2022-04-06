using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipe.Core
{
	public class Recipe : DbObject
	{
		public string Name { get; set; }
		public string Category { get; set; }
		public string Type { get; set; }
		public double Quantity { get; set; }
		public List<Ingredient> Ingredients { get; set; }
		public List<Preparation> Preparations { get; set; }
	}
}
