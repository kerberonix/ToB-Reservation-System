namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTicketPropertiesInCoachJourneys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CoachJourneys", "TotalNumberOfTickets", c => c.Int(nullable: false));
            AddColumn("dbo.CoachJourneys", "TicketsAvailable", c => c.Int(nullable: false));
            DropColumn("dbo.CoachJourneys", "SeatsAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CoachJourneys", "SeatsAvailable", c => c.Byte(nullable: false));
            DropColumn("dbo.CoachJourneys", "TicketsAvailable");
            DropColumn("dbo.CoachJourneys", "TotalNumberOfTickets");
        }
    }
}
