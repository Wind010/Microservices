﻿CREATE TABLE [dbo].[User]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,  
    [UserId] UNIQUEIDENTIFIER NULL, 
	[TenantId] INT NOT NULL,
    [FirstName] NVARCHAR(1000) NOT NULL, 
    [MiddleName] NVARCHAR(1000) NULL, 
    [LastName] NVARCHAR(1000) NOT NULL, 
    [Title] NCHAR(10) NULL,
	[Created] DATETIME NULL DEFAULT(GETDATE()),
    [Modified] DATETIME NULL
)
