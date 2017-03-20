CREATE TABLE [dbo].[Product]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[ProductID] Int NOT NULL,
	[ProductDescription] NTEXT NOT NULL,
	[ProductName] NVARCHAR (500) NOT NULL,
	[ProductPrice] MONEY NOT NULL,
	[Active] BIT NOT NULL DEFAULT(1),
	[Inventory] INT NOT NULL DEFAULT(0),

	[CreatedDate] DateTime2 NOT NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime2 NOT NULL DEFAULT getUtcdate(),

	CONSTRAINT [PK_Product] PRIMARY KEY (ID),
	CONSTRAINT [CK_Product_Inventory] CHECK (Inventory <= 0) ,
)
