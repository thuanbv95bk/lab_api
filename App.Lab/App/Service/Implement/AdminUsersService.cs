using App.Common.BaseService;
using App.DataAccess;
using App.Lab.Model;
using App.Lab.Repository.Interface;
using App.Lab.Service.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Lab.Service.Implement
{

    /// <summary> Service liên quan đến người dùng </summary>
    /// Author: thuanbv
    /// Created: 07/05/2025
    /// Modified: date - user - description
    public class AdminUsersService : BaseService<IAdminUsersRepository>, IAdminUsersService
    {
        private readonly IUnitOfWork _uow;

        public AdminUsersService(IHttpContextAccessor accessor, IAdminUsersRepository repo, IUnitOfWork uow) : base(accessor, repo)
        {
            _uow = uow;
        }

        /// <summary>Lấy danh sách user</summary>
        /// <param name="filter">Bộ lọc theo User</param>
        /// Author: thuanbv
        /// Created: 4/22/2025
        /// Modified: date - user - description
        /// 
        public async Task<List<Users>> GetListAsync(Users filter)
        {
            return await _repo.GetListAsync(filter);
        }
    }
}
