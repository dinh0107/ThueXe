namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServices", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.CarServices", "TitleMeta", c => c.String(maxLength: 100));
            AddColumn("dbo.CarServices", "DescriptionMeta", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServices", "DescriptionMeta");
            DropColumn("dbo.CarServices", "TitleMeta");
            DropColumn("dbo.CarServices", "Active");
        }
    }
}
