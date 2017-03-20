CREATE TABLE [dbo].[Category]
(
	[ID] INT IDENTITY(1,1) NOT NULL, 
	[CategoryName] NVARCHAR(100) NOT NULL,
	[CategoryDescription] NVARCHAR(100) NOT NULL,

	[CreatedDate] DateTime NOT NULL DEFAULT getUtcDate(),
	[ModifiedDate] DateTime NOT NULL DEFAULT getUtcdate(),

	CONSTRAINT [PK_CategoryID] PRIMARY KEY (ID),
)
