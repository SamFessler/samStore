CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[PurchaseEmail] NVARCHAR (100) NOT NULL,
	[Inventory] int NOT NULL,
	[CompletedDate] DateTime2 NOT NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime NOT NULL DEFAULT getUtcdate(),
	CONSTRAINT [PK_Order] PRIMARY KEY ([ID]),
)