CREATE TABLE [dbo].[CatagoryProduct]
(
	[Id] INT NOT NULL PRIMARY KEY

	[CreatedDate] DateTime2 NOT NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime NOT NULL DEFAULT getUtcdate(),

)
