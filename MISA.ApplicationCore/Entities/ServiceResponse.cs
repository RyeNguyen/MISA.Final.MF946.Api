using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Entity;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResponse
    {
        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Thông báo cho người dùng và lập trình viên
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Mã trả về
        /// </summary>
        public MISACode MISACode { get; set; }
    }
}
