CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [DateReleased] DATETIME NOT NULL, 
    [Director] NVARCHAR(50) NOT NULL, 
    [UserId] INT NOT NULL, 
    [PosterId] INT NULL 
)
