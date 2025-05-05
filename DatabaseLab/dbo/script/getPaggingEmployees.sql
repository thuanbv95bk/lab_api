DECLARE @FK_CompanyID INT = 15076;

DECLARE @Name NVARCHAR(100) = N'DAO XUAN TRUONG';
DECLARE @DriverLicense NVARCHAR(32) = N'';
DECLARE @LicenseType INT = NULL;

DECLARE @ListStringEmployeesId NVARCHAR(50) = N'';
DECLARE @ListStringLicenseTypesId NVARCHAR(50) = N'';

DECLARE @pageSize INT = 50;
DECLARE @pageIndex INT = 1;

SELECT PK_EmployeeID AS PkEmployeeID,
       CASE
           WHEN UpdatedDate IS NULL THEN
               CreatedDate
           ELSE
               UpdatedDate
       END [UpdatedDate],
       DisplayName,
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
          @Name IS NULL
          OR Name LIKE '%' + Name + '%'
      )
      AND
      (
          @DriverLicense IS NULL
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
ORDER BY DisplayName OFFSET @pageSize * (@pageIndex - 1) ROWS FETCH NEXT @pageSize ROWS ONLY;