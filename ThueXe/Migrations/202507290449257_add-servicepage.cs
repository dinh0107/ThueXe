namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addservicepage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarServiceDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarServiceId = c.Int(nullable: false),
                        Image = c.String(),
                        Name = c.String(nullable: false),
                        Desciption = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarServices", t => t.CarServiceId, cascadeDelete: true)
                .Index(t => t.CarServiceId);
            
            CreateTable(
                "dbo.CarServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Slug = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CarServicePrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarServiceId = c.Int(nullable: false),
                        RouteDescription = c.String(nullable: false),
                        Price = c.String(nullable: false),
                        Hot = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarServices", t => t.CarServiceId, cascadeDelete: true)
                .Index(t => t.CarServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarServicePrices", "CarServiceId", "dbo.CarServices");
            DropForeignKey("dbo.CarServiceDetails", "CarServiceId", "dbo.CarServices");
            DropIndex("dbo.CarServicePrices", new[] { "CarServiceId" });
            DropIndex("dbo.CarServiceDetails", new[] { "CarServiceId" });
            DropTable("dbo.CarServicePrices");
            DropTable("dbo.CarServices");
            DropTable("dbo.CarServiceDetails");
        }
    }
}
