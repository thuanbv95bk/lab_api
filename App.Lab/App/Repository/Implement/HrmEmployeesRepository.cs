using App.Common.Helper;
using App.Common.Models;
using App.DataAccess;
using App.Lab.Model;
using App.Lab.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text;


namespace App.Lab.Repository.Implement
{
    /// <summary> Các hàm liên quan đến bảng HRM.Employees phải được định nghĩa ở interface để gọi </summary>
    /// Author: thuanbv
    /// Created: 24/04/2025
    /// Modified: date - user - description
    public class HrmEmployeesRepository : Repo, IHrmEmployeesRepository
    {
        public HrmEmployeesRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { Schema = "HRM"; }
        public HrmEmployeesRepository(IHttpContextAccessor accessor, IUnitOfWork unitOfWork) : base(accessor, unitOfWork) { Schema = "HRM"; }


        /// <summary> hàm Lấy danh sách lái xe DisplayName – DriverLicense điều kiện
        /// lái xe thuộc công ty với điều kiện không bị khóa (IsLocked), xóa (IsDeleted)</summary>
        /// <param name="FkCompanyID">Id của công ty</param>
        /// Author: thuanbv
        /// Created: 24/04/2025
        /// Modified: date - user - description
        public List<HrmEmployeesCbx> GetListCbx(int FkCompanyID)
        {
            var listItem = new List<HrmEmployeesCbx>() { };

            listItem = ExecuteReader<HrmEmployeesCbx>
            (
                "SELECT PK_EmployeeID as PkEmployeeID , LTRIM(DisplayName) as DisplayName, DriverLicense " +
                "FROM [HRM.Employees] " +
                "WHERE ISNULL(IsDeleted, 0) = 0 " +
                "AND ISNULL(IsLocked, 0) = 0 " +
                "AND FK_CompanyID = @FK_CompanyID " +
                "ORDER BY DisplayName ,DriverLicense ;",
            CommandType.Text,
                new { FK_CompanyID = FkCompanyID }
            );

            return listItem;
        }

        /// <summary> hàm lấy data của lái xe, tùy chọn có paging hoặc không
        /// lái xe thuộc công ty với điều kiện không bị khóa (IsLocked), xóa (IsDeleted)</summary>
        /// <param name="includePaging">có phân trang hay không?</param>
        /// Author: thuanbv
        /// Created: 07/05/2025
        /// Modified: date - user - description

        private string BuildQueryGetListHrmEmployees(bool includePaging)
        {
            var query = new StringBuilder();
            // Base query
            query.Append(
                   "SELECT A.PK_EmployeeID AS PkEmployeeID" +
                    ",CASE WHEN A.UpdatedDate IS NULL THEN A.CreatedDate ELSE A.UpdatedDate END [UpdatedDate]" +
                    ",LTRIM(DisplayName) as DisplayName" +
                    ",A.Mobile" +
                    ",A.DriverLicense as DriverLicense" +
                    ",A.IssueLicenseDate" +
                    ",A.ExpireLicenseDate" +
                    ",A.IssueLicensePlace" +
                    ",B.Name AS LicenseTypeName " +
                    ",A.LicenseType AS LicenseType " +
                    ",CASE WHEN convert(date, A.ExpireLicenseDate) >= convert(date, GETDATE())  THEN N'Còn hiệu lực' WHEN convert(date, A.ExpireLicenseDate) <= convert(date, GETDATE())  THEN N'Đã hết hạn' ELSE '' END ActiveValue" +
                    ",COUNT(*) OVER () AS TotalCount " +
                "FROM dbo.[HRM.Employees] A " +
                "LEFT JOIN dbo.[BCA.LicenseTypes] B ON A.LicenseType = B.PK_LicenseTypeID " +
                "WHERE A.FK_CompanyID = @FK_CompanyID " +
                      "AND ISNULL(A.IsDeleted, 0) = 0 " +
                      "AND ISNULL(A.IsLocked, 0) = 0 " +
                      "AND (@Name IS NULL OR A.Name LIKE '%' + @Name + '%') " +
                      "AND (@IssueLicensePlace IS NULL OR A.IssueLicensePlace LIKE '%' + @IssueLicensePlace + '%') " +
                      "AND (@DriverLicense IS NULL OR A.DriverLicense LIKE '%' + @DriverLicense + '%') " +
                      "AND (@ListStringLicenseTypesId IS NULL OR A.LicenseType IN (SELECT value FROM STRING_SPLIT(@ListStringLicenseTypesId, ','))) " +
                      "AND (@ListStringEmployeesId IS NULL OR A.PK_EmployeeID IN (SELECT value FROM STRING_SPLIT(@ListStringEmployeesId, ','))) ");

            if (includePaging)
            {
                query.Append(
                    "ORDER BY DisplayName, DriverLicense " +
                    "OFFSET @pageSize * @pageIndex ROWS FETCH NEXT @pageSize ROWS ONLY;");
            }
            else
            {
                query.Append("ORDER BY A.DisplayName, A.DriverLicense");
            }

            return query.ToString();
        }

        /// <summary>Lấy danh sách lái xe theo điều kiện và theo Paging </summary>
        /// <param name="filter">HrmEmployeesFilter: bộ lọc để lấy dữ liệu</param>
        /// Author: thuanbv
        /// Created: 25/04/2025
        /// Modified: date - user - description
        public PagingResult<HrmEmployees> GetPagingToEdit(HrmEmployeesFilter filter)
        {

            this.ExecuteReader
            (
                out List<HrmEmployees> listItem
                , out int TotalCount
                , BuildQueryGetListHrmEmployees(true)
                , CommandType.Text
                , new
                {
                    FK_CompanyID = filter.FkCompanyId,
                    Name = filter.DisplayName?.Trim(),
                    DriverLicense = filter.DriverLicense?.Trim(),
                    IssueLicensePlace = filter.IssueLicensePlace?.Trim(),
                    ListStringLicenseTypesId = filter.ListStringLicenseTypesId,
                    ListStringEmployeesId = filter.ListStringEmployeesId,

                    pageSize = filter.PageSize,
                    pageIndex = filter.PageIndex
                }

            );

            var ret = new PagingResult<HrmEmployees>()
            {
                TotalCount = TotalCount,
                Data = listItem,
            };
            return ret;
        }
        /// <summary> Lấy danh sách lái xe theo điều kiện => xuất Excel </summary>
        /// <param name="filter">HrmEmployeesFilter: bộ lọc để lấy dữ liệu</param>
        /// Author: thuanbv
        /// Created: 25/04/2025
        /// Modified: date - user - description
        public List<HrmEmployees> GetDataToExcel(HrmEmployeesFilterExcel filter)
        {

            var listItem = this.ExecuteReader<HrmEmployees>
            (
                BuildQueryGetListHrmEmployees(false)
                , CommandType.Text
                , new
                {
                    FK_CompanyID = filter.FkCompanyId,
                    Name = filter.DisplayName?.Trim(),
                    DriverLicense = filter.DriverLicense?.Trim(),
                    IssueLicensePlace = filter.IssueLicensePlace?.Trim(),
                    ListStringLicenseTypesId = filter.ListStringLicenseTypesId,
                    ListStringEmployeesId = filter.ListStringEmployeesId,
                }

            );

            return listItem;
        }

        /// <summary>Updates Thông tin của 1 lái xe.</summary>
        /// <param name="obj">HrmEmployees thông tin của 1 lái xe</param>
        /// Author: thuanbv
        /// Created: 28/04/2025
        /// Modified: date - user - description
        public Task Update(HrmEmployees item)
        {
            var user = "E66E300E-B644-41B0-8124-CE9954434C6F";
            var now = DateTime.Now;

            string sql =
               "UPDATE [HRM.Employees] SET " +
                    " DisplayName = @DisplayName " +
                    ",Name = @Name " +
                    ",Mobile = @Mobile " +
                    ",DriverLicense = @DriverLicense" +
                    ",IssueLicenseDate = @IssueLicenseDate " +
                    ",ExpireLicenseDate = @ExpireLicenseDate " +
                    ",IssueLicensePlace = @IssueLicensePlace " +
                    ",LicenseType = @LicenseType" +
                    ",UpdatedDate = @UpdatedDate" +
                    ",UpdatedByUser = @UpdatedByUser " +
                "WHERE PK_EmployeeID = @PK_EmployeeID;";
            return Task.Run(() => this.ExecuteScalar<int>
            (
                sql
               , CommandType.Text
                , new
                {
                    DisplayName = item.DisplayName,
                    Name = StringHepler.RemoveDiacriticsToUpper(item.DisplayName),
                    Mobile = item.Mobile,
                    DriverLicense = StringHepler.RemoveDiacriticsToUpper(item.DriverLicense),
                    IssueLicenseDate = item.IssueLicenseDate,
                    ExpireLicenseDate = item.ExpireLicenseDate,
                    IssueLicensePlace = item.IssueLicensePlace,
                    LicenseType = item.LicenseType,

                    UpdateByUser = user,
                    UpdatedDate = now,
                    UpdatedByUser = user,
                    PK_EmployeeID = item.PkEmployeeId
                }
            ));
        }


        /// <summary>Xóa mềm 1 lái xe. isDelete =0 </summary>
        /// <param name="employeeId">id lái xe</param>
        /// Author: thuanbv
        /// Created: 28/04/2025
        /// Modified: date - user - description
        public Task DeleteSoft(int employeeId)
        {
            var user = "E66E300E-B644-41B0-8124-CE9954434C6F";
            var now = DateTime.Now;

            string sql =
               "UPDATE [HRM.Employees] SET " +
                    "IsDeleted = 1 " +
                    ",UpdatedDate = @UpdatedDate" +
                    ",UpdatedByUser = @UpdatedByUser " +
                "WHERE PK_EmployeeID = @PK_EmployeeID;";
            return Task.Run(() => this.ExecuteScalar<int>
            (
                sql
               , CommandType.Text
                , new
                {
                    PK_EmployeeID = employeeId,
                    UpdateByUser = user,
                    UpdatedDate = now,
                    UpdatedByUser = user
                }
            ));

        }

        /// <summary>Kiểm tra sự tồn tại của danh sách PkEmployeeId trong cơ sở dữ liệu </summary>
        /// <param name="employeeIds">Danh sách PkEmployeeId cần kiểm tra</param>
        /// Author: thuanbv
        /// Created: 07/05/2025
        /// Modified: date - user - description
        public IEnumerable<int> GetExistingEmployeeIds(IEnumerable<int> employeeIds)
        {
            if (employeeIds == null || !employeeIds.Any())
            {
                return new List<int>();
            }
        
            // Chuyển danh sách employeeIds thành chuỗi để sử dụng trong câu lệnh SQL
            var idsString = string.Join(",", employeeIds);
        
            // Câu lệnh SQL để kiểm tra sự tồn tại
            var query =
                "SELECT PK_EmployeeID " +
                "FROM [HRM.Employees] " +
                "WHERE PK_EmployeeID IN (SELECT value FROM STRING_SPLIT(@idsString, ',')) " +
                "AND ISNULL(IsDeleted, 0) = 0 " +
                "AND FK_CompanyId = 15076 " +
                "AND ISNULL(IsLocked, 0) = 0;";
        
            // Thực thi truy vấn và trả về danh sách các ID tồn tại
            var existingIds = ExecuteReader<int>(query, CommandType.Text,
            new
            {
                idsString = idsString,
                

            });

            return existingIds;
        }

        /// <summary>Kiểm tra sự tồn tại của danh sách theo tên và giấy phép lái xe trong cơ sở dữ liệu </summary>
        /// Author: thuanbv
        /// Created: 08/05/2025
        /// Modified: date - user - description
        public List<HrmEmployees> GetCheckExistingEmployeeByNameAndDriverLicense(string jsonIds, string jsonNames, string jsonLicenses)
        {
            if (string.IsNullOrEmpty(jsonNames) && string.IsNullOrEmpty(jsonLicenses))
            {
                return new List<HrmEmployees>();
            }

            // Câu lệnh SQL để kiểm tra sự tồn tại
            var query =
                "SELECT E.DisplayName, E.DriverLicense, E.PK_EmployeeID " +
                "FROM dbo.[HRM.Employees] E " +
                "JOIN (" +
                "SELECT  N.[value] AS Name, L.[value] AS DriverLicense, CAST(i.[value] AS INT) AS ExcludedId " +
                "FROM OPENJSON(@jsonNames) AS N " +
                "JOIN OPENJSON(@jsonLicenses) AS L ON N.[key] = L.[key] " +
                "JOIN OPENJSON(@jsonIds) AS I ON N.[key] = I.[key] ) " +
                "AS Pairs ON E.Name = Pairs.Name " +
                "AND E.DriverLicense = Pairs.DriverLicense " +
                "AND E.PK_EmployeeID != Pairs.ExcludedId; ";


            // Thực thi truy vấn và trả về danh sách các ID tồn tại
            var existingIds = ExecuteReader<HrmEmployees>(query, CommandType.Text,
            new
            {
                jsonIds = jsonIds,
                jsonNames = jsonNames,
                jsonLicenses = jsonLicenses

            });

            return existingIds;
        }
    }
}
