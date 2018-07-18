using System.Data.Entity;

namespace RecipeBook
{
    public class RecipeContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<Recipe>()
                .Property(r => r.PersonId)
                .IsRequired();

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Name)
                .IsRequired();

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.RecipeId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
