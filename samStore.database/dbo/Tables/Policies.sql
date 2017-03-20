CREATE TABLE [dbo].[Policies] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [Number]        NVARCHAR (100) NULL,
    [EffectiveDate] DATETIME       NULL,
    [AddressID]     INT            NULL,
    CONSTRAINT [PK_Policies] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [PK_Policies_Addresses] FOREIGN KEY ([AddressID]) REFERENCES [dbo].[Addresses] ([ID])
);

