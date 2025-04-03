CREATE TABLE [dbo].[Domain] (
    [DomainId]     UNIQUEIDENTIFIER CONSTRAINT [DF.dbo.Domain.DomainId] DEFAULT (newsequentialid()) NOT NULL,
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedBy]    NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME2 (7)    NOT NULL,
    [ModifiedBy]   NVARCHAR (128)   NOT NULL,
    [Name]         NVARCHAR (1024)  NOT NULL,
    CONSTRAINT [PK.dbo.Domain] PRIMARY KEY CLUSTERED ([DomainId] ASC)
);

