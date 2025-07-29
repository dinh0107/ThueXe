namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inittypesevice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServiceDetails", "CarServiceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServiceDetails", "CarServiceType");
        }
    }
}
