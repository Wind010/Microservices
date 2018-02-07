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

PRINT 'Populating StateProvince'

DELETE FROM dbo.StateProvince

DECLARE @US_Country varchar(20) = 'USA'
DECLARE @CAD_Country varchar(20) = 'CANADA'

SET IDENTITY_INSERT StateProvince ON

INSERT INTO StateProvince(Id, [Name], Code, Country) VALUES
(1, 'Alaska', 'AK', @US_Country),
(2, 'Alabama', 'AL', @US_Country),
(3, 'American Samoa', 'AS', @US_Country),
(4, 'Arizona', 'AZ', @US_Country),
(5, 'Arkansas', 'AR', @US_Country),
(6, 'California', 'CA', @US_Country),
(7, 'Colorado', 'CO', @US_Country),
(8, 'Connecticut', 'CT', @US_Country),
(9, 'Delaware', 'DE', @US_Country),
(10, 'District of Columbia', 'DC', @US_Country),
(11, 'Florida', 'FL', @US_Country),
(12, 'Georgia', 'GA', @US_Country), 
(13, 'Guam', 'GU', @US_Country),
(14, 'Hawaii', 'HI', @US_Country),
(15, 'Idaho', 'ID', @US_Country),
(16, 'Illinois', 'IL', @US_Country),
(17, 'Indiana', 'IN', @US_Country),
(18, 'Iowa', 'IA', @US_Country),
(19, 'Kansas', 'KS', @US_Country),
(20, 'Kentucky', 'KY', @US_Country),
(21, 'Louisiana', 'LA', @US_Country),
(22, 'Maine', 'ME', @US_Country),
(23, 'Maryland', 'MD', @US_Country),
(24, 'Massachusetts', 'MA', @US_Country),
(25, 'Michigan', 'MI', @US_Country),
(26, 'Minnesota', 'MN', @US_Country),
(27, 'Mississippi', 'MS', @US_Country),
(28, 'Missouri', 'MO', @US_Country),
(29, 'Montana', 'MT', @US_Country),
(30, 'Nebraska', 'NE', @US_Country),
(31, 'Nevada', 'NV', @US_Country),
(32, 'New Hampshire', 'NH', @US_Country),
(33, 'New Jersey', 'NJ', @US_Country),
(34, 'New Mexico', 'NM', @US_Country), 
(35, 'New York', 'NY', @US_Country),
(36, 'North Carolina', 'NC', @US_Country),
(37, 'North Dakota', 'ND', @US_Country),
(38, 'Northern Mariana Islands', 'MP', @US_Country),
(39, 'Ohio', 'OH', @US_Country),
(40, 'Oklahoma', 'OK', @US_Country),
(41, 'Oregon', 'OR', @US_Country),
(42, 'Palau', 'PW', @US_Country), 
(43, 'Pennsylvania', 'PA', @US_Country),
(44, 'Puerto Rico', 'PR', @US_Country),
(45, 'Rhode Island', 'RI', @US_Country),
(46, 'South Carolina', 'SC', @US_Country),
(47, 'South Dakota', 'SD', @US_Country),
(48, 'Tennessee', 'TN', @US_Country),
(49, 'Texas', 'TX', @US_Country), 
(50, 'Utah', 'UT', @US_Country),
(51, 'Vermont', 'VT', @US_Country),
(52, 'Virgin Islands', 'VI', @US_Country),
(53, 'Virginia', 'VA', @US_Country),
(54, 'Washington', 'WA', @US_Country),
(55, 'West Virginia', 'WV', @US_Country),
(56, 'Wisconsin', 'WI', @US_Country), 
(57, 'Wyoming', 'WY', @US_Country),
(58, 'Alberta', 'AB', @CAD_Country),
(59, 'British Columbia', 'BC', @CAD_Country),
(60, 'Manitoba', 'MB', @CAD_Country),
(61, 'New Brunswick', 'NB', @CAD_Country),
(62, 'Newfoundland and Labrador', 'NL', @CAD_Country),
(63, 'Northwest Territories', 'NT', @CAD_Country),
(64, 'Nova Scotia', 'NS', @CAD_Country),
(65, 'Nunavut', 'NU', @CAD_Country),
(66, 'Ontario', 'ON', @CAD_Country),
(67, 'Prince Edward Island', 'PE', @CAD_Country),
(68, 'Québec', 'QC', @CAD_Country),
(69, 'Saskatchewan', 'SK', @CAD_Country),
(70, 'Yukon Territory', 'YT', @CAD_Country); 

SET IDENTITY_INSERT StateProvince OFF
