namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removefullname : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contacts", "Fullname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "Fullname", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
