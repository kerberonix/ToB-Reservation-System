namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateDepartFromCenters : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO DepartFromCenters (Id, Name) VALUES (1, 'Victoria Coach Station - London')");
            Sql("INSERT INTO DepartFromCenters (Id, Name) VALUES (2, 'Newcastle Coach Station - Newcastle')");
            Sql("INSERT INTO DepartFromCenters (Id, Name) VALUES (3, 'Southampton Coach Station - Southampton')");
            Sql("INSERT INTO DepartFromCenters (Id, Name) VALUES (4, 'Pool Meadow Bus Station - Coventry')");
            Sql("INSERT INTO DepartFromCenters (Id, Name) VALUES (5, 'Victoria Bus Station - Nottingham')");
        }
        
        public override void Down()
        {
        }
    }
}
