using System.Collections.Generic;

namespace RecipeBook
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Recipe> FavoriteRecipes { get; set; }

        public Person(string name)
        {
            Name = name;
            FavoriteRecipes = new List<Recipe>();
        }

        public Person()
        {
            FavoriteRecipes = new List<Recipe>();
        }
    }
}
