using App.Common.Helper;
using App.DataAccess;
using App.Lab.App.Model;
using App.Lab.App.Repository.Interface;
using App.Lab.Model;
using App.Lab.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Lab.Repository.Implement
{
    /// <summary>Repository các ham liên quan đến nhóm xe </summary>
    /// Author: thuanbv
    /// Created: 22/04/2025
    /// Modified: date - user - description
    public class VehicleGroupsRepository : Repo, IVehicleGroupsRepository
    {
        public VehicleGroupsRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { Schema = "Vehicle"; }
        public VehicleGroupsRepository(IHttpContextAccessor accessor, IUnitOfWork unitOfWork) : base(accessor, unitOfWork) { Schema = "Vehicle"; }

        /// <summary>lấy thông nhóm phương tiện theo ID của nó</summary>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        public async Task<UserVehicleGroupView> GetViewByIdAsync(int PK_VehicleGroupID)
        {
            string sql = @"select * from [Vehicle.Groups] where PK_VehicleGroupID = @PK_VehicleGroupID";
            var item = await ExecuteReaderSingleAsync<UserVehicleGroupView>
            (
               sql,
                CommandType.Text,
                new { PK_VehicleGroupID = PK_VehicleGroupID }
            );

            return item;
        }

        /// <summary>Lấy danh sách nhóm phương tiện</summary>
        /// <param name="filter">Bộ lọc Nhóm phương tiện</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        public List<VehicleGroups> GetList(VehicleGroupsFilter filter)
        {
            var listOrderOption = new OrderOption[] {
            new OrderOption {
                Column = "Name",
                OrderType = "ASC",
            }};
            var listFilter = MapFilterToOptions(filter);
            this.GetTableData
            (
                out List<VehicleGroups> ret
                , "Groups", null, listFilter
            );
            return ret;

        }
    }
}
