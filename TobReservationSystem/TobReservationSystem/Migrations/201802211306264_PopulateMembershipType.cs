namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateMembershipType : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO MembershipTypes (Id, Name, SignUpFee, DurationInMonths) VALUES (1, 'No Membership',0 ,0)");
            Sql("INSERT INTO MembershipTypes (Id, Name, SignUpFee, DurationInMonths) VALUES (2, 'Member: Quarterly', 10 , 4)");
            Sql("INSERT INTO MembershipTypes (Id, Name, SignUpFee, DurationInMonths) VALUES (3, 'Member: Annual', 20 , 12)");
        }
        
        public override void Down()
        {
        }
    }
}
