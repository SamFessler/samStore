CREATE TABLE [dbo].[Address]
(
	[ID] INT NOT NULL PRIMARY KEY,
	[ShippingAddress1] NVARCHAR (100) NOT NULL,
	[ShippingAddress2] NVARCHAR (100) NOT NULL,
	[ShippingCity] NVARCHAR (100) NOT NULL,
	[ShippingState] NVARCHAR (100) NOT NULL,
	[ShippingZip] NVARCHAR (100) NOT NULL,

	CONSTRAINT [PK_Address] PRIMARY KEY (ID),
)
