using App.Common.BaseService;
using App.Common.Helper;
using App.Common.Models;
using App.DataAccess;
using App.Lab.Model;
using App.Lab.Repository.Interface;
using App.Lab.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Services.Common.CommandLine;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace App.Lab.App.Service.Implement
{

    /// <summary> Các hàm Service liên quan đến bảng HRM.Employees phải được định nghĩa ở interface để gọi </summary>
    /// Author: thuanbv
    /// Created: 24/04/2025
    /// Modified: date - user - description
    public class HrmEmployeesService : BaseService<IHrmEmployeesRepository>, IHrmEmployeesService
    {
        private readonly IUnitOfWork _uow;

        public HrmEmployeesService(IHttpContextAccessor accessor, IHrmEmployeesRepository repo, IUnitOfWork uow) : base(accessor, repo)
        {
            _uow = uow;
        }


        /// <summary> Hàm Lấy danh sách lái xe DisplayName – DriverLicense điều kiện
        /// lái xe thuộc công ty với điều kiện không bị khóa (IsLocked), xóa (IsDeleted)</summary>
        /// <param name="FkCompanyID">Id của công ty</param>
        /// Author: thuanbv
        /// Created: 24/04/2025
        /// Modified: date - user - description
        public List<HrmEmployeesCbx> GetListCbx(int FkCompanyID)
        {
            return _repo.GetListCbx(FkCompanyID);
        }


        public static void SetFilterPropertyFromOption(object filter, Lab.Model.SearchOption option)
        {
            if (filter == null || option == null || string.IsNullOrWhiteSpace(option.Key))
                return;

            var type = filter.GetType();
            var prop = type.GetProperty(option.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (prop != null && prop.CanWrite)
            {
                try
                {
                    string rawValue = option.Value?.Trim();

                    Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    object convertedValue = null;

                    if (targetType == typeof(string))
                    {
                        convertedValue = rawValue;
                    }
                    else if (targetType.IsEnum)
                    {
                        convertedValue = Enum.Parse(targetType, rawValue, ignoreCase: true);
                    }
                    
                    else if (targetType == typeof(DateTime))
                    {
                        convertedValue = DateTime.Parse(rawValue);
                    }
                    else
                    {
                        // Các kiểu phổ biến: int, long, bool, double, ...
                        convertedValue = Convert.ChangeType(rawValue, targetType);
                    }

                    prop.SetValue(filter, convertedValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không thể gán {option.Key} = {option.Value}: {ex.Message}");
                }
            }
        }



        /// <summary>Service Lấy danh sách lái xe theo điều kiện và theo Paging </summary>
        /// <param name="filter">HrmEmployeesFilter: bộ lọc để lấy dữ liệu</param>
        /// Author: thuanbv
        /// Created: 25/04/2025
        /// Modified: date - user - description
        public PagingResult<HrmEmployees> GetPagingToEdit(HrmEmployeesFilter filter)
        {
            SetFilterPropertyFromOption(filter, filter.Option);
            return _repo.GetPagingToEdit(filter);
        }

        /// <summary>Service hàm cập nhât danh sách thông tin của lái xe </summary>
        /// <param name="items">Danh sách lái xe</param>
        /// Author: thuanbv
        /// Created: 28/04/2025
        /// Modified: date - user - description
        public async Task<ServiceStatus> AddOrEditListAsync(List<HrmEmployees> items)
        {

            try
            {
                if (!items.Any())
                {
                    return ServiceStatus.Failure("Danh sách trống!");
                }

                // Lấy danh sách PkEmployeeId từ items
                var employeeIds = items.Select(x => x.PkEmployeeId);

                // Kiểm tra sự tồn tại của các PkEmployeeId trong cơ sở dữ liệu
                if (!employeeIds.Any()) return ServiceStatus.Failure("Danh sách trống!");

                var existingIds =  _repo.GetExistingEmployeeIds(employeeIds);

                // Lấy tất cả các phần tử trong employeeIds mà không tồn tại trong existingIds.
                var invalidIds = employeeIds.Except(existingIds);

                if (invalidIds.Any())
                {
                    return ServiceStatus.Failure($"Lỗi: Các PkEmployeeId sau không tồn tại trong cơ sở dữ liệu: {string.Join(", ", invalidIds)}");
                }


                // lấy ra danh sách tên từ items

                var listName = items.Select(x => x.DisplayName);
                // lấy ra danh sách giấy phép lái xe từ items
                var listDriverLicense = items.Select(x => x.DriverLicense);

                // Chuyển sang chuổi có cấu trúc JSON

                string jsonIds = JsonSerializer.Serialize(employeeIds);
                string jsonNames = JsonSerializer.Serialize(listName);
                string jsonLicenses = JsonSerializer.Serialize(listDriverLicense);

                var listDuplicate = _repo.GetCheckExistingEmployeeByNameAndDriverLicense(jsonIds, jsonNames, jsonLicenses);

                if (listDuplicate.Any())
                {
                    return ServiceStatus.Failure("Tồn tại dòng dữ liệu bị trùng Tên - Giấy phép lái xe",listDuplicate);
                }


                using (_uow.BeginTransaction())
                {
                    foreach (var item in items)
                    {
                        if (item.PkEmployeeId > 0)
                        {
                            await _repo.Update(item);
                        }
                    }

                    _uow.SaveChanges();
                    return ServiceStatus.Success("Cập nhật thành công");
                }
            }
            catch (Exception ex)
            {
                return ServiceStatus.Failure("Đã xảy ra lỗi trong quá trình cập nhật!");
            }
        }


        /// <summary>service hàm Xóa mềm 1 lái xe </summary>
        /// <param name="employeeId"> Id của lái xe</param>
        /// Author: thuanbv
        /// Created: 28/04/2025
        /// Modified: date - user - description
        public async Task<ServiceStatus> DeleteSoft(int employeeId)
        {
            try
            {
                using (_uow.BeginTransaction())
                {
                    await _repo.DeleteSoft(employeeId);
                    _uow.SaveChanges();
                    return ServiceStatus.Success("Xóa thành công");
                }
            }
            catch (Exception ex)
            {
                return ServiceStatus.Failure("Đã xảy ra lỗi trong quá trình xóa!");
            }
        }


        /// <summary>Service hamf Export Excel danh sách lái xe theo bộ lọc </summary>
        /// <param name="filter"> Bộ lọc </param>
        /// Author: thuanbv
        /// Created: 29/04/2025
        /// Modified: date - user - description
        public MemoryStream ExportExcel(HrmEmployeesFilterExcel filter)
        {
            try
            {
                MemoryStream stream;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("DATA");

                    SetFilterPropertyFromOption(filter, filter.Option);
                    
                    string title = "THÔNG TIN LÁI XE";
                    // lay du lieu

                    var listData = _repo.GetDataToExcel(filter);

                    // lưu danh sách bộ lọc
                    var listFilter = new List<Lab.Model.SearchOption>() { };

                    if (!string.IsNullOrEmpty(filter.Option.Value))
                    {
                        listFilter.Add(new Lab.Model.SearchOption
                        {
                            Key = filter.Option.Name,
                            Value = filter.Option.Value
                        });
                    }

                    if (!string.IsNullOrEmpty(filter.ListStringEmployeesId))
                    {
                        listFilter.Add(new Lab.Model.SearchOption
                        {
                            Key = "Danh sách lái xe",
                            Value = filter.ListStringEmployeesName
                        });
                    }
                    if (!string.IsNullOrEmpty(filter.ListStringLicenseTypesName))
                    {
                        listFilter.Add(new Lab.Model.SearchOption
                        {
                            Key = "Loại bằng",
                            Value = filter.ListStringLicenseTypesName
                        });
                    }
                    
              
                    EmployessReportExcel.FillExcell(ws, title, listFilter, 1, listData, EmployessReportExcel.HeaderRows());

                    stream = new MemoryStream(package.GetAsByteArray());
                }

                return stream;
            }
            catch (Exception ex)
            {
                return new MemoryStream(); 
            }
        }
    }
}
