namespace RecipeBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadePersonIdColumnRequiredInRecipeTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recipes", "Person_Id", "dbo.People");
            DropIndex("dbo.Recipes", new[] { "Person_Id" });
            RenameColumn(table: "dbo.Recipes", name: "Person_Id", newName: "PersonId");
            AlterColumn("dbo.Recipes", "PersonId", c => c.Int(nullable: false));
            CreateIndex("dbo.Recipes", "PersonId");
            AddForeignKey("dbo.Recipes", "PersonId", "dbo.People", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "PersonId", "dbo.People");
            DropIndex("dbo.Recipes", new[] { "PersonId" });
            AlterColumn("dbo.Recipes", "PersonId", c => c.Int());
            RenameColumn(table: "dbo.Recipes", name: "PersonId", newName: "Person_Id");
            CreateIndex("dbo.Recipes", "Person_Id");
            AddForeignKey("dbo.Recipes", "Person_Id", "dbo.People", "Id");
        }
    }
}
