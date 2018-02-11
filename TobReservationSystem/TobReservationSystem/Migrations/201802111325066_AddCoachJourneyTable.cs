namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoachJourneyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoachJourneys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Destination = c.String(nullable: false, maxLength: 255),
                        SeatsAvailable = c.Byte(nullable: false),
                        DateOfJourney = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CoachJourneys");
        }
    }
}
