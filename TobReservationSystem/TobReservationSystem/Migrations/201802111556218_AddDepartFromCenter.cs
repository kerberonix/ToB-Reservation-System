namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDepartFromCenter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartFromCenters",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CoachJourneys", "DepartFromCenterId", c => c.Byte(nullable: false));
            CreateIndex("dbo.CoachJourneys", "DepartFromCenterId");
            AddForeignKey("dbo.CoachJourneys", "DepartFromCenterId", "dbo.DepartFromCenters", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoachJourneys", "DepartFromCenterId", "dbo.DepartFromCenters");
            DropIndex("dbo.CoachJourneys", new[] { "DepartFromCenterId" });
            DropColumn("dbo.CoachJourneys", "DepartFromCenterId");
            DropTable("dbo.DepartFromCenters");
        }
    }
}
