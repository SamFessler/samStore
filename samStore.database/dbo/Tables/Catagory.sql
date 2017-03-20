CREATE TABLE [dbo].[Catagory]
(
	[ID] INT IDENTITY(1,1) NOT NULL, 
	[CatagoryName] NVARCHAR(100) NOT NULL,
	[CatagoryDescription] NVARCHAR(100) NOT NULL,

	[CreatedDate] DateTime2 NOT NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime2 NOT NULL DEFAULT getUtcdate(),

	CONSTRAINT [PK_CatagoryID] PRIMARY KEY (ID),
	
)
