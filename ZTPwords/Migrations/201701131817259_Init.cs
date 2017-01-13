namespace ZTPwords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Language", c => c.String());
            AddColumn("dbo.AspNetUsers", "Level", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Points", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Points");
            DropColumn("dbo.AspNetUsers", "Level");
            DropColumn("dbo.AspNetUsers", "Language");
        }
    }
}
