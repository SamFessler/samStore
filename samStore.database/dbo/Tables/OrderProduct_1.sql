﻿CREATE TABLE [dbo].[OrderProduct]
(
	[OrderID] INT NOT NULL,
	[ProductID] INT NOT NULL, 
	[Quantity] INT NOT NULL DEFAULT(1),
	[CreatedDate] DATETIME2  NULL DEFAULT GetUtcDate(),
	[ModifiedDate] DATETIME2  NULL DEFAULT GetUtcDate(), 
	[SubTotal] MONEY NOT NULL,
	
	CONSTRAINT [PK_OrderProduct] PRIMARY KEY ([ProductID], [OrderID]), 
    CONSTRAINT [FK_OrderProduct_Order] FOREIGN KEY (OrderID) REFERENCES [Order](ID) ON DELETE CASCADE, 
    CONSTRAINT [FK_OrderProduct_Product] FOREIGN KEY (ProductID) REFERENCES Product([ID]) ON DELETE CASCADE, 
    CONSTRAINT [CK_OrderProduct_Quantity] CHECK (Quantity > 0),
)
