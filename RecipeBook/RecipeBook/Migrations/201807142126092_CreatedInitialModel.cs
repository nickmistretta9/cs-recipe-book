namespace RecipeBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedInitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Recipe_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id)
                .Index(t => t.Recipe_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "Person_Id", "dbo.People");
            DropForeignKey("dbo.Ingredients", "Recipe_Id", "dbo.Recipes");
            DropIndex("dbo.Recipes", new[] { "Person_Id" });
            DropIndex("dbo.Ingredients", new[] { "Recipe_Id" });
            DropTable("dbo.Recipes");
            DropTable("dbo.People");
            DropTable("dbo.Ingredients");
        }
    }
}
