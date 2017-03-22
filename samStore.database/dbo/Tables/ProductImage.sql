CREATE TABLE [dbo].[ProductImage]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[ImagePath] NVARCHAR(1000) NOT NULL,
	[ImageDescription] NVARCHAR(100) NULL,
	[ProductID] INT NULL,
	
	[CompletedDate] DateTime NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime NULL DEFAULT getUtcdate(), 

    CONSTRAINT [PK_ProductImage] PRIMARY KEY (ID), 
    CONSTRAINT [FK_ProductImage_Product] FOREIGN KEY (ProductID) REFERENCES Product(ID),

)
