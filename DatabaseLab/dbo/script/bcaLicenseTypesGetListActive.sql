

-- =============================================
-- Author: thuan.bv
-- Create date: 26/04/2025
-- Description: Lấy ra danh sách LicenseTypes với điều kiện đang kích hoạt (IsActived) và không bị xóa (IsDeteted)
-- =============================================

SELECT PK_LicenseTypeID,
       Code,
       [Name],
       IsActived,
       IsDeteted
FROM [BCA.LicenseTypes]
WHERE ISNULL(IsDeteted, 0) = 0
      AND IsActived = 1
ORDER BY [Name];