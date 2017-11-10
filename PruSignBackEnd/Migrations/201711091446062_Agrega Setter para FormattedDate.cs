namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregaSetterparaFormattedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceLogs", "FormattedDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceLogs", "FormattedDate");
        }
    }
}
