CREATE TABLE [dbo].[Address]
(
	[ID] INT NOT NULL,
	[ShippingAddress1] NVARCHAR (100) NOT NULL,
	[ShippingAddress2] NVARCHAR (100) NOT NULL,
	[ShippingCity] NVARCHAR (100) NOT NULL,
	[ShippingState] NVARCHAR (100) NOT NULL,
	[ShippingZip] NVARCHAR (100) NOT NULL,

	[CreatedDate] DATETIME NOT NULL DEFAULT GetUtcDate(),
	[ModifiedDate] DATETIME NOT NULL DEFAULT GetUtcDate(),

	CONSTRAINT [PK_Address] PRIMARY KEY (ID),
)
