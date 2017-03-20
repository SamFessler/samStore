CREATE TABLE [dbo].[Order]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[Inventory] INT NOT NULL,
	[CreatedDate] DateTime2 NOT NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime2 NOT NULL DEFAULT getUtcdate(),

	[PurchaserEmail] NVARCHAR(1000) NULL,
	[Completed] DATETIME NULL,
	[ShipCareOf] NVARCHAR(1000) NULL,
	[ShippingAddressID] INT NULL,
	[BillingAddressID] INT NULL,

	CONSTRAINT [PK_Order] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Order_BillingAddress] FOREIGN KEY (BillingAddressID) REFERENCES [Address](ID),
    CONSTRAINT [FK_Order_ShippingAddress] FOREIGN KEY (ShippingAddressID) REFERENCES [Address](ID)


)
