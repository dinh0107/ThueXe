namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addredirect : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleCategories", "Redirect", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleCategories", "Redirect");
        }
    }
}
