namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimgsv : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServiceDetails", "ListImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServiceDetails", "ListImage");
        }
    }
}
