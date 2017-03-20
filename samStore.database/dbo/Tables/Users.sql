CREATE TABLE [dbo].[Users] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (100) NULL,
    [LastName]  NVARCHAR (100) NULL,
    [Birthday]  DATETIME       NULL,
    [Email]     NVARCHAR (100) NULL,
    [AddressID] INT            NULL,
    [PolicyID]  INT            NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [PK_Users_Addresses] FOREIGN KEY ([AddressID]) REFERENCES [dbo].[Addresses] ([ID]),
    CONSTRAINT [PK_Users_Policies] FOREIGN KEY ([PolicyID]) REFERENCES [dbo].[Policies] ([ID])
);

