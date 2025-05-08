

-- =============================================
-- Author: thuan.bv
-- Create date: 29/04/2025
-- Description: Lấy ra data để export file excel danh sách [HRM.Employees]  
-- danh sách lái xe thuộc công ty với điều kiện tìm kiếm theo người dùng
-- =============================================

DECLARE @FK_CompanyID INT = 15076;

-- Tên
DECLARE @Name NVARCHAR(100) = N'DAO XUAN TRUONG';
-- Giấy phép lái xe
DECLARE @DriverLicense NVARCHAR(32) = N''; -- 
-- ID của giấy phép lái xe
DECLARE @LicenseType INT = NULL;
-- Danh sách Id lái xe example (1,2,3)
DECLARE @ListStringEmployeesId NVARCHAR(50) = N'';
-- Danh sách Id số giấy phép lái xe; example (1,2,3)
DECLARE @ListStringLicenseTypesId NVARCHAR(50) = N'';

SELECT A.PK_EmployeeID AS PkEmployeeID,
       CASE
           WHEN A.UpdatedDate IS NULL THEN
               A.CreatedDate
           ELSE
               A.UpdatedDate
       END AS [UpdatedDate],
       A.DisplayName,
       A.Mobile,
       A.DriverLicense,
       A.IssueLicenseDate,
       A.ExpireLicenseDate,
       A.IssueLicensePlace,
       B.[Name] AS LicenseType,
       A.IsDeleted,
       A.IsLocked
FROM dbo.[HRM.Employees] A
    LEFT JOIN dbo.[BCA.LicenseTypes] B
        ON A.LicenseType = B.PK_LicenseTypeID
WHERE A.FK_CompanyID = @FK_CompanyID
      AND A.IsDeleted = 0
      AND ISNULL(A.IsLocked, 0) = 0
      AND
      (
          ISNULL(@Name, '') = ''
          OR A.[Name] LIKE '%' + @Name + '%'
      )
      AND
      (
          ISNULL(@DriverLicense, '') = ''
          OR A.DriverLicense LIKE '%' + @DriverLicense + '%'
      )
      AND
      (
          @LicenseType IS NULL
          OR A.LicenseType = @LicenseType
      )
      AND
      (
          ISNULL(@ListStringLicenseTypesId, '') = ''
          OR A.LicenseType IN
             (
                 SELECT value FROM STRING_SPLIT(@ListStringLicenseTypesId, ',')
             )
      )
      AND
      (
          ISNULL(@ListStringEmployeesId, '') = ''
          OR A.PK_EmployeeID IN
             (
                 SELECT value FROM STRING_SPLIT(@ListStringEmployeesId, ',')
             )
      )
ORDER BY A.DisplayName;