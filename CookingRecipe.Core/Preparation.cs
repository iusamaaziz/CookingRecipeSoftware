using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipe.Core
{
	public class Preparation : DbObject
	{
		public string Instruction { get; set; }
		public DateTime Time { get; set; }
		public byte[] Thumbnail { get; set; }
		public string VideoUrl { get; set; }

		public Guid RecipeId { get; set; }

	}
}
