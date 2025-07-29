namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initso : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServiceDetails", "Sort", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServiceDetails", "Sort");
        }
    }
}
