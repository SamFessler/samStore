CREATE PROCEDURE sp_SearchUsers(@text nvarchar(20)) AS
(

SELECT ID, FirstName + ' ' + LastName AS Name FROM Users WHERE LastName LIKE '%' + @text + '%' OR FirstName LIKE '%' + @text + '%'

)