using System.Data.Entity;

namespace RecipeBook
{
    public class RecipeContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
    }
}
