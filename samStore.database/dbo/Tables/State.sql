CREATE TABLE [dbo].[State]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[Abbreviation] NVARCHAR(10),
	[Name] NVARCHAR(100),
	[Created] DATETIME NULL DEFAULT GetUtcDate(),
	[Modified] DATETIME NULL DEFAULT GetUtcDate(), 
)
