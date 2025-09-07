namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsort : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServices", "Sort", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServices", "Sort");
        }
    }
}
