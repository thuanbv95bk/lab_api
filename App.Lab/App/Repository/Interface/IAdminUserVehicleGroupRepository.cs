﻿using App.Lab.App.Model;
using App.Lab.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Lab.Repository.Interface
{
    /// <summary>interface các ham liên quan đến nhóm xe đã gán </summary>
    /// Author: thuanbv
    /// Created: 22/04/2025
    /// Modified: date - user - description
    public interface IAdminUserVehicleGroupRepository
    {
        /// <summary>Tạo mới 1 nhóm phương tiện theo user</summary>
        /// <param name="obj">nhóm phương tiện theo user</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        Task CreateAsync(AdminUserVehicleGroup obj);
        /// <summary> Xóa mềm 1 nhóm phương tiện theo user </summary>
        /// <param name="item">nhóm phương tiện theo user</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        /// 
        Task DeleteSoftAsync(AdminUserVehicleGroup item);

        /// <summary> Cập nhật 1 nhóm phương tiện theo user </summary>
        /// <param name="item">nhóm phương tiện theo user</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        /// 
        Task UpdateAsync(AdminUserVehicleGroup item);

        /// <summary> Lấy danh sách nhóm phương tiện theo user </summary>
        /// <param name="filter">Bộ lọc nhóm phương tiện theo user</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        /// 
        List<AdminUserVehicleGroup> GetList(AdminUserVehicleGroupFilter filter);

        /// <summary> Lấy danh sách nhóm phương tiện theo user, bao gồm tên nhóm </summary>
        /// <param name="filter">Bộ lọc nhóm phương tiện theo user</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        ///
        Task<List<VehicleGroups>> GetListViewAsync(AdminUserVehicleGroupFilter filter);


    }
}
