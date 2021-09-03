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
                var serviceResponse = _employeeService.Pagination(employeeFilter, pageIndex, pageSize);

                return StatusCode(200, serviceResponse.Data);
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorPagingFilter,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorPagingFilter,
                };
                return StatusCode(500, errorObj);
            }

        }
        #endregion

        /// <summary>
        /// Hàm xuất khẩu dữ liệu excel
        /// </summary>
        /// <returns>File để download</returns>
        /// Author: NQMinh (02/09/2021)
        [HttpPost("export")]
        public async Task<IActionResult> ExportExcel()
        {
            await Task.Yield();

            var stream = new MemoryStream();
            var employees = new List<Employee>();

            var properties = typeof(Employee).GetProperties();

            using (var package = new ExcelPackage(stream))
            {

                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(employees, true);
                var column = 1;

                foreach (var prop in properties)
                {
                    var exportProp = prop.GetCustomAttributes(typeof(MISAExported), true);

                    workSheet.Cells.AutoFitColumns();

                    if (!(exportProp.Length == 1))
                    {
                        workSheet.Column(column).Hidden = true;
                    }

                    if (prop.PropertyType.Name.Contains(typeof(Nullable).Name) && prop.PropertyType.GetGenericArguments()[0] == typeof(DateTime))
                    {
                        workSheet.Column(column).Style.Numberformat.Format = "mm/dd/yyyy";
                    }

                    column++;
                }

                package.Save();
            }

            stream.Position = 0;
            string fileName = $"DanhSachNhanVien.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #endregion
    }
}
