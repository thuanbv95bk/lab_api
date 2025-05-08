

-- =============================================
-- Author: thuan.bv
-- Create date: 28/04/2025
-- Description: Lấy ra  danh sách [HRM.Employees]  
-- danh sách lái xe thuộc công ty với điều kiện tìm kiếm theo người dùng
-- =============================================

DECLARE @FK_CompanyID INT = 15076;
-- Tên
DECLARE @Name NVARCHAR(100) = N'';

-- Giấy phép lái xe
DECLARE @DriverLicense NVARCHAR(32) = N'';

-- ID của giấy phép lái xe
DECLARE @LicenseType INT = NULL;

-- Danh sách Id lái xe example (1,2,3)
DECLARE @ListStringEmployeesId NVARCHAR(50) = N'';

-- Danh sách Id số giấy phép lái xe; example (1,2,3)
DECLARE @ListStringLicenseTypesId NVARCHAR(50) = N'';

-- Số dòng dữ liệu mỗi trang 
DECLARE @pageSize INT = 50;
-- Số thứ tự của trang (bắt đầu từ 1, 2, 3,...).
DECLARE @pageIndex INT = 1;

SELECT PK_EmployeeID AS PkEmployeeID,
       CASE
           WHEN UpdatedDate IS NULL THEN
               CreatedDate
           ELSE
               UpdatedDate
       END [UpdatedDate],
       DisplayName,
       [Name],
       Mobile,
       DriverLicense,
       IssueLicenseDate,
       ExpireLicenseDate,
       IssueLicensePlace,
       LicenseType,
       IsDeleted,
       IsLocked,
       COUNT(*) OVER () AS TotalCount
FROM dbo.[HRM.Employees]
WHERE FK_CompanyID = @FK_CompanyID
      AND IsDeleted = 0
      AND ISNULL(IsLocked, 0) = 0
      AND
      (
          ISNULL(@Name, '') = ''
          OR [Name] LIKE '%' + @Name + '%'
      )
      AND
      (
          ISNULL(@DriverLicense, '') = ''
          OR DriverLicense LIKE '%' + @DriverLicense + '%'
      )
      AND
      (
          @LicenseType IS NULL
          OR LicenseType = @LicenseType
      )
      AND
      (
          ISNULL(@ListStringLicenseTypesId, '') = ''
          OR LicenseType IN
             (
                 SELECT value FROM STRING_SPLIT(@ListStringLicenseTypesId, ',')
             )
      )
      AND
      (
          ISNULL(@ListStringEmployeesId, '') = ''
          OR PK_EmployeeID IN
             (
                 SELECT value FROM STRING_SPLIT(@ListStringEmployeesId, ',')
             )
      )
ORDER BY DisplayName OFFSET @pageSize * (@pageIndex - 1) ROWS --Bỏ qua một số lượng dòng nhất định.
FETCH NEXT @pageSize ROWS ONLY; --Lấy tiếp số dòng bằng với @pageSize