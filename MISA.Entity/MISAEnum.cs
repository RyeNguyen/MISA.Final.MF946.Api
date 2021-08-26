using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Entity
{
    /// <summary>
    /// Mã xác định trạng thái validation
    /// </summary>
    public enum MISACode
    {
        /// <summary>
        /// Dữ liệu hợp lệ
        /// </summary>
        IsValid = 111,

        /// <summary>
        /// Dữ liệu chưa hợp lệ
        /// </summary>
        NotValid = 911,

        /// <summary>
        /// Thành công
        /// </summary>
        Success = 200
    }
}
