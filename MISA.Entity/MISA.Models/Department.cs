using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Entity.MISA.Models
{
    public class Department : BaseEntity
    {
        /// <summary>
        /// Id đơn vị
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Mô tả chi tiết
        /// </summary>
        public string Description { get; set; }
    }
}
