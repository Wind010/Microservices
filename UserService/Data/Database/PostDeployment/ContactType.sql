/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

PRINT 'Populating ContactType'

DELETE FROM ContactType
DBCC CHECKIDENT ('dbo.ContactType', RESEED, 0)

INSERT INTO ContactType (Name, Description) VALUES ('Cell', 'Cellphone')
INSERT INTO ContactType (Name, Description) VALUES ('Home', 'Home phone.')
INSERT INTO ContactType (Name, Description) VALUES ('Work', 'Work phone.')
INSERT INTO ContactType (Name, Description) VALUES ('Fax', 'Fax machine.')
INSERT INTO ContactType (Name, Description) VALUES ('Email', 'Email Address')
INSERT INTO ContactType (Name, Description) VALUES ('Twitter', 'Twitter')
INSERT INTO ContactType (Name, Description) VALUES ('Facebook', 'Facebook')
INSERT INTO ContactType (Name, Description) VALUES ('LinkedIn', 'LinkedIn')
