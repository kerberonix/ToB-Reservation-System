namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerRefCodeProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CustomerRefCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CustomerRefCode");
        }
    }
}
