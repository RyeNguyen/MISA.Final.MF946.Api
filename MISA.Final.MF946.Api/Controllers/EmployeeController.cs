using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity.MISA.Attributes;
using MISA.Entity.MISA.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.MF946.Final.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BaseEntityController<Employee>
    {
        #region Declares
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeService employeeService,
            IEmployeeRepository employeeRepository) : base(employeeService, employeeRepository)
        {
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Methods
        #region Hàm xử lý phân trang cho nhân viên
        /// <summary>
        /// Hàm xử lý phân trang cho nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Dữ liệu phân trang</returns>
        /// Author: NQMinh (27/08/2021)
        [HttpGet("paging")]
        public IActionResult EmployeePagination([FromQuery] string employeeFilter, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            try
            {
                var serviceResponse = _employeeService.Pagination(employeeFilter, pageIndex, pageSize, false);

                return StatusCode(200, serviceResponse.Data);
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorGet,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorGet,
                };
                return StatusCode(500, errorObj);
            }

        }
        #endregion

        #region Xuất khẩu dữ liệu
        /// <summary>
        /// Hàm xuất khẩu dữ liệu excel
        /// </summary>
        /// <returns>File để download</returns>
        /// Author: NQMinh (02/09/2021)
        [HttpGet("export")]
        public IActionResult Export([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string employeeFilter)
        {
            try
            {
                var stream = _employeeService.ExportEmployee(employeeFilter, pageIndex, pageSize, true);

                string excelName = $"DanhSachNhanVien.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorExport,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorExport,
                };
                return StatusCode(500, errorObj);
            }
        }
        #endregion
        #endregion
    }
}
