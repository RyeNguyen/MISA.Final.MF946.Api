using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Entity.MISA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Declares
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ServiceResponse _serviceResponse;
        #endregion

        #region Constructor
        public EmployeeService(IBaseRepository<Employee> baseRepository,
            IEmployeeRepository employeeRepository) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResponse = new ServiceResponse();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm xử lý phân trang nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        public ServiceResponse Pagination(string employeeFilter, int pageIndex, int pageSize)
        {
            _serviceResponse.Data = _employeeRepository.Pagination(employeeFilter, pageIndex, pageSize);

            return _serviceResponse;
        }

        /// <summary>
        /// Hàm validate thông tin nhân viên
        /// </summary>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        protected override ServiceResponse ValidateCustom(Employee employee)
        {
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion
    }
}
