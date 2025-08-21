namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsometb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarServices", "Body", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarServices", "Body");
        }
    }
}
