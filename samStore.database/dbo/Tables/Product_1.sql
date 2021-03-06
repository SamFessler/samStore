﻿CREATE TABLE [dbo].[Product]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[ProductName] NVARCHAR (500) NOT NULL,
	[ProductDescription] NTEXT NOT NULL,

	[ProductPrice] MONEY NOT NULL,
	[Active] BIT NOT NULL DEFAULT(1),
	[Inventory] INT NOT NULL DEFAULT(0),

	[TreeSpecies] NVARCHAR(500) NULL,
	[TreeSkill] NVARCHAR(500) NULL,

	[CreatedDate] DateTime2  NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime2  NULL DEFAULT getUtcdate(),

	CONSTRAINT [PK_Product] PRIMARY KEY (ID),
	CONSTRAINT [CK_Product_Inventory] CHECK (Inventory <= 0) ,
)
