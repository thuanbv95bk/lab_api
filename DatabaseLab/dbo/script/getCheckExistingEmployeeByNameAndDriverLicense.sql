
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

DECLARE @jsonIds NVARCHAR(MAX) = N'[455400,458495,464906]';
DECLARE @jsonNames NVARCHAR(MAX) = N'["310100008359","310077000934","BAN VAN THUONG"]';
DECLARE @jsonLicenses NVARCHAR(MAX) = N'["VUVANHAI","TRANMANHHA","AV830213"]';
DECLARE @ INT = 0;
-- =============================================
-- Author: gps3_lab
-- Create date: 13/05/2025
-- Description: Description
-- =============================================

SELECT 
--n.[KEY], l.[key], i.[key],
        N.[value] AS Name, 
        L.[value] AS DriverLicense,
        CAST(i.[value] AS INT) AS ExcludedId
    FROM OPENJSON(@jsonNames) AS N
    JOIN OPENJSON(@jsonLicenses) AS L ON N.[key] = L.[key]
    JOIN OPENJSON(@jsonIds) AS I ON N.[key] = I.[key]

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

	GO 


SELECT DisplayName  , DriverLicense  FROM dbo.[HRM.Employees] FOR JSON AUTO;

SELECT ISJSON('{"xx": "John"}'); -- Trả về 1
SELECT ISJSON('invalid json');     -- Trả về 0

DECLARE @json NVARCHAR(MAX) = '[
    {"id": 1, "name": "Product 1"},
    {"id": 2, "name": "Product 2"}
]';

SELECT * FROM OPENJSON(@json)
WITH (
    id INT,
    name NVARCHAR(100)
);

DECLARE @jsonx NVARCHAR(MAX) = '{
    "orderId": 12345,
    "items": [
        {"productId": 1, "quantity": 2},
        {"productId": 2, "quantity": 1}
    ]
}';

-- Lấy toàn bộ mảng items
SELECT JSON_QUERY(@jsonx, '$.items') AS items_array;