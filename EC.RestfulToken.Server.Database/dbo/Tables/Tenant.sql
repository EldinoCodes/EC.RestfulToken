CREATE TABLE [dbo].[Tenant] (
    [TenantId]     UNIQUEIDENTIFIER CONSTRAINT [DF.dbo.Tenant.TenantId] DEFAULT (newsequentialid()) NOT NULL,
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedBy]    NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME2 (7)    NOT NULL,
    [ModifiedBy]   NVARCHAR (128)   NOT NULL,
    [Name]         NVARCHAR (1024)  NOT NULL,
    CONSTRAINT [PK.dbo.Tenant] PRIMARY KEY CLUSTERED ([TenantId] ASC)
);

