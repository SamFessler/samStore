CREATE TABLE [dbo].[ProductImage]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[ImagePath] NVARCHAR(1000) NOT NULL,
	[ImageDescription] NVARCHAR(100) NOT NULL,
	[ProductID] INT NOT NULL,
	
	[CompletedDate] DateTime NOT NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime NOT NULL DEFAULT getUtcdate(), 

    CONSTRAINT [PK_ProductImage] PRIMARY KEY (ID), 
    CONSTRAINT [FK_ProductImage_Product] FOREIGN KEY (ProductID) REFERENCES Product(ID),

)
