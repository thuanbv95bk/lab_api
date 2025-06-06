﻿using App.Common.Models;
using App.Lab.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Lab.Repository.Interface
{
    /// <summary> interface các hàm liên quan đến bảng HRM.Employees </summary>
    /// Author: thuanbv
    /// Created: 24/04/2025
    /// Modified: date - user - description
    public interface IHrmEmployeesRepository
    {
        /// <summary> Interface hàm Lấy danh sách lái xe DisplayName – DriverLicense điều kiện
        /// lái xe thuộc công ty với điều kiện không bị khóa (IsLocked), xóa (IsDeleted)</summary>
        /// <param name="FkCompanyID">Id của công ty</param>
        /// Author: thuanbv
        /// Created: 24/04/2025
        /// Modified: date - user - description
        Task<List<HrmEmployeesCbx>>  GetListCbxAsync(int FkCompanyID);


        /// <summary>Interface Lấy danh sách lái xe theo điều kiện và theo Paging </summary>
        /// <param name="filter">HrmEmployeesFilter: bộ lọc để lấy dữ liệu</param>
        /// Author: thuanbv
        /// Created: 25/04/2025
        /// Modified: date - user - description
        Task<PagingResult<HrmEmployees>> GetPagingToEditAsync(HrmEmployeesFilter filter);

        /// <summary>Interface Lấy danh sách lái xe theo điều kiện => xuất Excel </summary>
        /// <param name="filter">HrmEmployeesFilter: bộ lọc để lấy dữ liệu</param>
        /// Author: thuanbv
        /// Created: 28/04/2025
        /// Modified: date - user - description
        Task<List<HrmEmployees>> GetDataToExcelAsync(HrmEmployeesFilterExcel filter);


        /// <summary>Updates Thông tin của 1 lái xe.</summary>
        /// <param name="obj">HrmEmployees thông tin của 1 lái xe</param>
        /// Author: thuanbv
        /// Created: 28/04/2025
        /// Modified: date - user - description
        Task UpdateAsync(HrmEmployees obj);

        /// <summary>Xóa mềm 1 lái xe. isDelete =0 </summary>
        /// <param name="employeeId">id lái xe</param>
        /// Author: thuanbv
        /// Created: 28/04/2025
        /// Modified: date - user - description
        Task DeleteSoftAsync(int employeeId);


        /// <summary>Kiểm tra sự tồn tại của danh sách PkEmployeeId trong cơ sở dữ liệu </summary>
        ///  <param name="employeeIds">Danh sách PkEmployeeId cần kiểm tra</param>
        /// Author: thuanbv
        /// Created: 07/05/2025
        /// Modified: date - user - description

        Task<IEnumerable<int>> GetExistingEmployeeIdsAsync(IEnumerable<int> employeeIds);

        /// <summary>Kiểm tra sự tồn tại của danh sách theo tên và giấy phép lái xe trong cơ sở dữ liệu </summary>
        /// Author: thuanbv
        /// Created: 08/05/2025
        /// Modified: date - user - description
        Task<List<HrmEmployees>> GetCheckExistingEmployeeByNameAndDriverLicenseAsync(string jsonIds, string jsonNames, string jsonLicenses);
    }
}
