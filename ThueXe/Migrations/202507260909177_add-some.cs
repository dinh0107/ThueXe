namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsome : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Trips", "PriceChange", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Trips", "Pile", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Trips", "TypeCar", c => c.Int(nullable: false));
            AddColumn("dbo.Trips", "TypeTrip", c => c.Int(nullable: false));
            AddColumn("dbo.Trips", "Source", c => c.Int(nullable: false));
            AddColumn("dbo.Trips", "DriverId", c => c.Int());
            CreateIndex("dbo.Trips", "DriverId");
            AddForeignKey("dbo.Trips", "DriverId", "dbo.Drivers", "Id");
            DropColumn("dbo.Trips", "Gas");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trips", "Gas", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.Trips", "DriverId", "dbo.Drivers");
            DropIndex("dbo.Trips", new[] { "DriverId" });
            DropColumn("dbo.Trips", "DriverId");
            DropColumn("dbo.Trips", "Source");
            DropColumn("dbo.Trips", "TypeTrip");
            DropColumn("dbo.Trips", "TypeCar");
            DropColumn("dbo.Trips", "Pile");
            DropColumn("dbo.Trips", "PriceChange");
            DropTable("dbo.Drivers");
        }
    }
}
