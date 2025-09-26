namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServices", "ListImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServices", "ListImage");
        }
    }
}
