using App.Lab.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Lab.Repository.Interface
{
    /// <summary>interface các ham liên quan đến user</summary>
    /// Author: thuanbv
    /// Created: 22/04/2025
    /// Modified: date - user - description
    public interface IAdminUsersRepository
    {
        /// <summary>Lấy danh sách user</summary>
        /// <param name="filter">Bộ lọc theo user</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        Task<List<Users>> GetListAsync(Users filter);
    }
}
