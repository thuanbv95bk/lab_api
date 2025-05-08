using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Lab.Model
{

    /// <summary>dùng để lấy dữ liệu cho vào combobox chọn lái xe</summary>
    /// Author: thuanbv
    /// Created: 29/04/2025
    /// Modified: date - user - description
    public class BcaLicenseTypes
    {
        /// <summary>Id giấy phép lái xe</summary
        public int PkLicenseTypeId { get; set; }
        /// <summary>Tên giấy phép lái xe</summary
        public string Name { get; set; }

        /// <summary>Mã giấy phép lái xe</summary
        public string Code { get; set; }

        /// <summary>Có hiệu lưc hay không</summary>
        public bool IsActived { get; set; }
        /// <summary>Có bị xóa hay không</summary>
        public bool? IsDeteted { get; set; }
    }

    /// <summary>dùng để filter dữ liệu cho vào combobox chọn lái xe</summary>
    /// Author: thuanbv
    /// Created: 29/04/2025
    /// Modified: date - user - description
    public class BcaLicenseTypesfilter
    {
        /// <summary>Có hiệu lưc hay không</summary>
        public bool IsActived { get; set; }
        /// <summary>Có bị xóa hay không</summary>
        public bool? IsDeteted { get; set; }
    }
}
