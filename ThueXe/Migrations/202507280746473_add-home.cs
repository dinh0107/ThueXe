namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addhome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleCategories", "Home", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleCategories", "Home");
        }
    }
}
