namespace HiLToysWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class again2 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Carts", "RecordId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "RecordId");
        }
    }
}
