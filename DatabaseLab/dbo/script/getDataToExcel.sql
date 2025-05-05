DECLARE @FK_CompanyID INT = 15076;

DECLARE @Name NVARCHAR(100) = N'';
DECLARE @DriverLicense NVARCHAR(32) = N'';
DECLARE @LicenseType INT = NULL;

DECLARE @ListStringEmployeesId NVARCHAR(50) = N'';
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
       B.Name AS LicenseType,
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
          @Name IS NULL
          OR A.Name LIKE '%' + @Name + '%'
      )
      AND
      (
          @DriverLicense IS NULL
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