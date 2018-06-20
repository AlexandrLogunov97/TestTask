namespace L3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Brands", "Logo");
            DropColumn("dbo.Makers", "Logo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Makers", "Logo", c => c.String());
            AddColumn("dbo.Brands", "Logo", c => c.String());
        }
    }
}
