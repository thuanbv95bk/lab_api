
-- =============================================
-- Author: thuan.bv
-- Create date: 26/04/2025
-- Description: Lấy ra danh sách [HRM.Employees] danh sách lái xe thuộc công ty với điều kiện không bị khóa (IsLocked), xóa (IsDeleted)
-- =============================================

DECLARE @FK_CompanyID INT = 15076;

DECLARE @DisplayName NVARCHAR(100) = N'';
DECLARE @DriverLicense NVARCHAR(32) = N'';


SELECT PK_EmployeeID,
       DisplayName,
       DriverLicense,
       IsDeleted,
       IsLocked,
       COUNT(*) OVER () AS TotalCount
FROM dbo.[HRM.Employees]
WHERE FK_CompanyID = @FK_CompanyID
      AND ISNULL(IsDeleted, 0) = 0
      AND ISNULL(IsLocked, 0) = 0
      AND
      (
          ISNULL(@DisplayName, '') = ''
          OR DisplayName LIKE '%' + @DisplayName + '%'
      )
      AND
      (
          ISNULL(@DriverLicense, '') = ''
          OR DriverLicense LIKE '%' + @DriverLicense + '%'
      )
ORDER BY DisplayName;