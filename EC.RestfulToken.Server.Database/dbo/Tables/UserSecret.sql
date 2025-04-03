CREATE TABLE [dbo].[UserSecret] (
    [DomainId]     UNIQUEIDENTIFIER NOT NULL,
    [UserSecretId] UNIQUEIDENTIFIER CONSTRAINT [DF.dbo.UserSecret.UserSecretId] DEFAULT (newid()) NOT NULL,
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedBy]    NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME2 (7)    NOT NULL,
    [ModifiedBy]   NVARCHAR (128)   NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [Secret]       NVARCHAR (1024)     COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    CONSTRAINT [PK.dbo.UserSecret] PRIMARY KEY CLUSTERED ([DomainId] ASC, [UserSecretId] ASC),
    CONSTRAINT [FK.dbo.UserSecret.dbo.Domain] FOREIGN KEY ([DomainId]) REFERENCES [dbo].[Domain] ([DomainId]) ON DELETE CASCADE,
    CONSTRAINT [FK.dbo.UserSecret.dbo.User] FOREIGN KEY ([DomainId], [UserId]) REFERENCES [dbo].[User] ([DomainId], [UserId])
);


GO
CREATE NONCLUSTERED INDEX [IDX.dbo.UserSecret.DomainId.UserId]
    ON [dbo].[UserSecret]([DomainId] ASC, [UserId] ASC);
GO

