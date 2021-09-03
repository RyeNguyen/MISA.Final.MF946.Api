using MISA.ApplicationCore.Entities;
using MISA.Entity.MISA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        /// <summary>
        /// Phương thức xử lý phân trang cho nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc (có thể là mã, tên nhân viên hoặc sđt)</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <param name="dataOnly">Có muốn lấy số bản ghi hay không</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        public ServiceResponse Pagination(string employeeFilter, int pageIndex, int pageSize, bool dataOnly);

        /// <summary>
        /// Xuất khẩu dữ liệu excel
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc (có thể là mã, tên nhân viên hoặc sđt)</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <param name="dataOnly">Kiểm tra muốn lấy số lượng bản ghi hay không</param>
        /// <returns></returns>
        /// Author: NQMinh (03/09/2021)
        dynamic ExportEmployee(string employeeFilter, int pageIndex, int pageSize, bool dataOnly);
    }
}
