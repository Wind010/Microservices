CREATE TABLE [dbo].[UserToInterface]
(
    [UserId] INT NOT NULL, 
    [InterfaceId] INT NOT NULL,
	PRIMARY KEY (UserId, InterfaceId), -- Add index to InterfaceId if needed.
	CONSTRAINT [FK_UserInterface_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_UserInterface_Interface] FOREIGN KEY ([InterfaceId]) REFERENCES [Interface]([Id])
)
