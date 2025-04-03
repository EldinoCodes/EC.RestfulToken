USE [ECRestfulTokenServer];
GO

--normally you wouldnt hardcode the Ids in use, but since i am setting up http files to use for testing... meh, why not.

INSERT  INTO [dbo].[Domain] ([DomainId], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [Name])
VALUES                     ('90E70F3A-4A0F-F011-89C2-80B655E9AB1F', GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'TestDomain');
GO

INSERT  INTO [dbo].[User] ([UserId], [DomainId], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [UserName], [Email])
VALUES                   ('A8D9A00A-16BD-4B04-A855-3BD626EBC1DE', '90E70F3A-4A0F-F011-89C2-80B655E9AB1F', GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'TestUser@test.com', 'TestUser@test.com');
GO

INSERT  INTO [dbo].[UserSecret] ([DomainId], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [UserId], [Secret])
VALUES                         ('90E70F3A-4A0F-F011-89C2-80B655E9AB1F', GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', 'A8D9A00A-16BD-4B04-A855-3BD626EBC1DE', 'ThisIsSomeSecureSecretHashGeneratedThroughByteTrickeryAndStuffAndEverything');
GO
