CREATE TABLE [dbo].[Addresses] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Address1] NVARCHAR (100) NULL,
    [Address2] NVARCHAR (100) NULL,
    [City]     NVARCHAR (100) NULL,
    [State]    NVARCHAR (100) NULL,
    [ZipCode]  NVARCHAR (100) NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([ID] ASC)
);

