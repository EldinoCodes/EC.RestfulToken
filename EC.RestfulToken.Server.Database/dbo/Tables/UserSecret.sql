CREATE TABLE [dbo].[UserSecret] (
    [TenantId]     UNIQUEIDENTIFIER NOT NULL,
    [UserSecretId] UNIQUEIDENTIFIER CONSTRAINT [DF.dbo.UserSecret.UserSecretId] DEFAULT (newid()) NOT NULL,
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedBy]    NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME2 (7)    NOT NULL,
    [ModifiedBy]   NVARCHAR (128)   NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [Secret]       NVARCHAR (1024)     COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    CONSTRAINT [PK.dbo.UserSecret] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UserSecretId] ASC),
    CONSTRAINT [FK.dbo.UserSecret.dbo.Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([TenantId]) ON DELETE CASCADE,
    CONSTRAINT [FK.dbo.UserSecret.dbo.User] FOREIGN KEY ([TenantId], [UserId]) REFERENCES [dbo].[User] ([TenantId], [UserId])
);


GO
CREATE NONCLUSTERED INDEX [IDX.dbo.UserSecret.TenantId.UserId]
    ON [dbo].[UserSecret]([TenantId] ASC, [UserId] ASC);
GO

