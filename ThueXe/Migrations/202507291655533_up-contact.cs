namespace ThueXe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upcontact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Fullname", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Contacts", "TypeCar", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "TypeCar");
            DropColumn("dbo.Contacts", "Fullname");
        }
    }
}
