CREATE TABLE [dbo].[User] (
    [TenantId]     UNIQUEIDENTIFIER NOT NULL,
    [UserId]       UNIQUEIDENTIFIER CONSTRAINT [DF.dbo.User.UserId] DEFAULT (newid()) NOT NULL,
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedBy]    NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME2 (7)    NOT NULL,
    [ModifiedBy]   NVARCHAR (128)   NOT NULL,
    [UserName]     NVARCHAR (512)   NOT NULL,
    [Email]        NVARCHAR (320)   NOT NULL,
    CONSTRAINT [PK.dbo.User] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UserId] ASC),
    CONSTRAINT [FK.dbo.User.Auth.Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([TenantId]) ON DELETE CASCADE
);

