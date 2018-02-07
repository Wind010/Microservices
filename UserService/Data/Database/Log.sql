CREATE TABLE [dbo].[Log]
(
	[Id] INT  IDENTITY(1,1) NOT NULL CONSTRAINT PK_Log PRIMARY KEY CLUSTERED (Id), 
	[Application] nvarchar(50) NOT NULL, 
	[Logged] datetime NOT NULL, 
	[Level] nvarchar(50) NOT NULL, 
	[Message] nvarchar(max) NOT NULL, 
	[UserName] nvarchar(250) NULL, 
	[ServerName] nvarchar(max) NULL, 
	[Port] nvarchar(max) NULL, 
	[Url] nvarchar(max) NULL, 
	[Https] bit NULL, 
	[ServerAddress] nvarchar(100) NULL, 
	[RemoteAddress] nvarchar(100) NULL, 
	[Logger] nvarchar(250) NULL, 
	[Callsite] nvarchar(max) NULL,
	[Exception] nvarchar(max) NULL,
	[ClientId] uniqueidentifier NULL
)