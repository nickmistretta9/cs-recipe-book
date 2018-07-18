namespace RecipeBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredPropertiesToColumns : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ingredients", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.People", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Recipes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recipes", "Name", c => c.String());
            AlterColumn("dbo.People", "Name", c => c.String());
            AlterColumn("dbo.Ingredients", "Name", c => c.String());
        }
    }
}
