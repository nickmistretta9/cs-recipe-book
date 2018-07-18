using System.Collections.Generic;

namespace RecipeBook
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public int PersonId { get; set; }
    }
}
