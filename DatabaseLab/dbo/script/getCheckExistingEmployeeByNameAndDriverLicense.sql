
-- =============================================
-- Author: thuan.bv
-- Create date: 08/05/2025
-- Description: 
-- =============================================

DECLARE @ListStringEmployeesName NVARCHAR(50) = N'01:54,00,BAN VAN THUONG,BUI NGOC DUONG';
DECLARE @ListStringDriverLicenseName NVARCHAR(50) = N'105.893379,00,3400473404785';
SELECT LTRIM(DisplayName) AS DisplayName,
       DriverLicense
FROM dbo.[HRM.Employees]
WHERE (
          ISNULL(@ListStringEmployeesName, '') = ''
          OR Name IN
             (
                 SELECT value FROM STRING_SPLIT(@ListStringEmployeesName, ',')
             )
      )
      AND
      (
          ISNULL(@ListStringDriverLicenseName, '') = ''
          OR DriverLicense IN
             (
                 SELECT value FROM STRING_SPLIT(@ListStringDriverLicenseName, ',')
             )
      );