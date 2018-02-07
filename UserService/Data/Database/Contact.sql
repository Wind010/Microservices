CREATE TABLE [dbo].[Contact]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,  
    [ContactTypeId] INT NOT NULL, 
	[UserId] INT NOT NULL, 
    [Name] NVARCHAR(100) NULL, 
    [Description] NVARCHAR(1000) NULL, 
	[Value] NVARCHAR(75) NULL,
    CONSTRAINT [FK_Contact_ContactType] FOREIGN KEY ([ContactTypeId]) REFERENCES [ContactType]([Id]),
	CONSTRAINT [FK_PhoneNumber_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
)
