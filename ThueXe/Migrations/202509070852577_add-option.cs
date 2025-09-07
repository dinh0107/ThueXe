namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addoption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServices", "Image", c => c.String());
            AddColumn("dbo.CarServices", "Firm", c => c.String());
            AddColumn("dbo.CarServices", "Capacity", c => c.String());
            AddColumn("dbo.CarServices", "Type", c => c.String());
            AddColumn("dbo.CarServices", "Speed", c => c.String());
            AddColumn("dbo.CarServices", "Home", c => c.Boolean(nullable: false));
            AddColumn("dbo.CarServices", "Menu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServices", "Menu");
            DropColumn("dbo.CarServices", "Home");
            DropColumn("dbo.CarServices", "Speed");
            DropColumn("dbo.CarServices", "Type");
            DropColumn("dbo.CarServices", "Capacity");
            DropColumn("dbo.CarServices", "Firm");
            DropColumn("dbo.CarServices", "Image");
        }
    }
}
