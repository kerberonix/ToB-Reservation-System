namespace TobReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'13595fc6-f6bd-485c-909b-42be7b3a5a2d', N'admin@tob.com', 0, N'ALbkOnJ4ZhlVsV4U3V1OZe22X3h6svgyHP3P35K4Qs87nuN9EUe8QYmcXAFTFy9pWg==', N'a84c1835-43e3-4baa-b884-1bb0fc50b68c', NULL, 0, 0, NULL, 1, 0, N'admin@tob.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'15cf3ba6-c970-43fd-84cb-c6c0c6a9f255', N'guest@tob.com', 0, N'AA5pOBnW3q2r9/bfIY5BTN0K8c0jANNpejig8u3qPjHR0QjJUDYuHFd9+g9YYJlmkw==', N'ee24d988-3790-4129-8f24-73011449a5e7', NULL, 0, 0, NULL, 1, 0, N'guest@tob.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'01fd8fd8-8916-4e1d-97ee-c069940c84d4', N'CanManageCoachJourneys')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'13595fc6-f6bd-485c-909b-42be7b3a5a2d', N'01fd8fd8-8916-4e1d-97ee-c069940c84d4')

");
        }
        
        public override void Down()
        {
        }
    }
}
