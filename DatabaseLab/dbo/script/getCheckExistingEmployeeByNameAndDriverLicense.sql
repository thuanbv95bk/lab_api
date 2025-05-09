
-- =============================================
-- Author: thuan.bv
-- Create date: 08/05/2025
-- Description: 
-- =============================================


SELECT PK_EmployeeID,
       DisplayName,
       DriverLicense
FROM dbo.[HRM.Employees]
ORDER BY DisplayName,
         DriverLicense;
GO

DECLARE @jsonIds NVARCHAR(MAX) = N'[455400]';
DECLARE @jsonNames NVARCHAR(MAX) = N'["310100008359"]';
DECLARE @jsonLicenses NVARCHAR(MAX) = N'["VUVANHAI"]';

-- Lấy các cặp Name - License - Id theo chỉ số (index = key)
SELECT E.DisplayName, E.DriverLicense, E.PK_EmployeeID
FROM dbo.[HRM.Employees] E
JOIN (
    SELECT 
        N.[value] AS Name, 
        L.[value] AS DriverLicense,
        CAST(i.[value] AS INT) AS ExcludedId
    FROM OPENJSON(@jsonNames) AS N
    JOIN OPENJSON(@jsonLicenses) AS L ON N.[key] = L.[key]
    JOIN OPENJSON(@jsonIds) AS I ON N.[key] = I.[key]
) AS Pairs
    ON E.Name = Pairs.Name 
    AND E.DriverLicense = Pairs.DriverLicense
    AND E.PK_EmployeeID != Pairs.ExcludedId;



