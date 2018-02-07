CREATE TABLE [Preference]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,  
    [UserId] INT NOT NULL, 
	[PreferenceTypeId] INT NOT NULL,
    [Name] NVARCHAR(200) NOT NULL, 
    [Details] NVARCHAR(2000) NOT NULL,
	[Like] BIT NOT NULL,
    CONSTRAINT [FK_Preferences_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_PreferenceType_Preference] FOREIGN KEY ([PreferenceTypeId]) REFERENCES [PreferenceType]([Id])
)
