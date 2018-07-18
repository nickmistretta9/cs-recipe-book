namespace RecipeBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeRecipeIdRequiredInIngredientsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ingredients", "Recipe_Id", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "Recipe_Id" });
            RenameColumn(table: "dbo.Ingredients", name: "Recipe_Id", newName: "RecipeId");
            AlterColumn("dbo.Ingredients", "RecipeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ingredients", "RecipeId");
            AddForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "RecipeId" });
            AlterColumn("dbo.Ingredients", "RecipeId", c => c.Int());
            RenameColumn(table: "dbo.Ingredients", name: "RecipeId", newName: "Recipe_Id");
            CreateIndex("dbo.Ingredients", "Recipe_Id");
            AddForeignKey("dbo.Ingredients", "Recipe_Id", "dbo.Recipes", "Id");
        }
    }
}
