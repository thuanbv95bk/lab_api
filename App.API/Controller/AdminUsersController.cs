﻿using Microsoft.AspNetCore.Mvc;
using App.Common.BaseControllers;
using App.Lab.Service.Interface;
using App.Lab.Model;
namespace App.Admin.Controllers
{
    [ApiController]
    [Route("api/user")]

    /// <summary> Controller API liên quan đến User </summary>
    /// Author: thuanbv
    /// Created: 07/05/2025
    /// Modified: date - user - description
    public class AdminUsersController : BaseController
    {
        private readonly IAdminUsersService _service;
        public AdminUsersController(IAdminUsersService service)
        {
            _service = service;
        }


        /// <summary>API lấy danh sách người dùng</summary>
        /// <param name="filter">Bộ lọc theo User</param>
        /// Author: thuanbv
        /// Created: 22/04/2025
        /// Modified: date - user - description
        [HttpPost]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromBody] Users filter)
        {
            if (filter == null)
            {
                return Failure("Phải nhập tiêu chí tìm kiếm");
            }
            var ret = await _service.GetListAsync(filter);
            return Success(ret);
        }
    }
}