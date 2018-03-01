namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateOfBooking = c.DateTime(nullable: false),
                        TicketQuantity = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        CoachJourneyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CoachJourneys", t => t.CoachJourneyId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.CoachJourneyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Bookings", "CoachJourneyId", "dbo.CoachJourneys");
            DropIndex("dbo.Bookings", new[] { "CoachJourneyId" });
            DropIndex("dbo.Bookings", new[] { "CustomerId" });
            DropTable("dbo.Bookings");
        }
    }
}
