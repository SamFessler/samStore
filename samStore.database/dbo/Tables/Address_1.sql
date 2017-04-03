CREATE TABLE [dbo].[Address]
(
	[ID] INT Identity(1,1) NOT NULL,
	[ShippingAddress1] NVARCHAR (100) NOT NULL,
	[ShippingAddress2] NVARCHAR (100) NULL,
	[ShippingCity] NVARCHAR (100) NOT NULL,
	[ShippingState] NVARCHAR (100) NOT NULL,
	[ShippingZip] NVARCHAR (100) NOT NULL,

	[CreatedDate] DATETIME2 NOT NULL DEFAULT GetUtcDate(),
	[ModifiedDate] DATETIME2 NOT NULL DEFAULT GetUtcDate(),

	[AspNetUserID] NVARCHAR (128) NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY ([ID] ASC),
    CONSTRAINT [FK_Address_AspNetUsers] FOREIGN KEY (AspNetUserID) REFERENCES [AspNetUsers]([Id]) ON DELETE SET NULL
)
