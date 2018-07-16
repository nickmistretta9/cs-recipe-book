using System.Collections.Generic;

namespace RecipeBook
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> NutritionFacts { get; set; }
    }
}
