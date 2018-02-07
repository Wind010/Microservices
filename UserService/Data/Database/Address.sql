CREATE TABLE [dbo].[Address]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,  
    [UserId] INT NOT NULL, 
    [AddressLine1] NVARCHAR(100) NOT NULL, 
    [AddressLine2] NVARCHAR(100) NULL,
	[AddressLine3] NVARCHAR(100) NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [StateProvinceId] INT NULL,
    [ZipCode] CHAR(10) NOT NULL, 
    [Created] DATETIME NULL, 
    [Modified] DATETIME NULL, 
    CONSTRAINT [FK_Address_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_Address_StateProvince] FOREIGN KEY ([StateProvinceId]) REFERENCES [StateProvince]([Id])
)