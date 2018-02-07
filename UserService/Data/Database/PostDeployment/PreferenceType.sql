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


PRINT 'Populating PreferenceType'

DELETE FROM PreferenceType
DBCC CHECKIDENT ('dbo.PreferenceType', RESEED, 0)

INSERT INTO PreferenceType (Name, Description) VALUES ('Food', '')
INSERT INTO PreferenceType (Name, Description) VALUES ('Wine', '')
INSERT INTO PreferenceType (Name, Description) VALUES ('Seating', '')
