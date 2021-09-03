using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Entity.MISA.Attributes;
using MISA.Entity.MISA.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        /// <param name="dataOnly">Kiểm tra muốn lấy số lượng bản ghi hay không</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        public ServiceResponse Pagination(string employeeFilter, int pageIndex, int pageSize, bool dataOnly)
        {
            _serviceResponse.Data = _employeeRepository.Pagination(employeeFilter, pageIndex, pageSize, dataOnly);

            return _serviceResponse;
        }

        /// <summary>
        /// Hàm xử lý xuất khẩu dữ liệu
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <param name="dataOnly">Kiểm tra muốn lấy số lượng bản ghi hay không</param>
        /// <returns></returns>
        /// Author: NQMinh (03/09/2021)
        public dynamic ExportEmployee(string employeeFilter, int pageIndex, int pageSize, bool dataOnly)
        {
            var stream = new MemoryStream();
            var employees = _employeeRepository.Pagination(employeeFilter, pageIndex, pageSize, dataOnly);

            var genderList = new List<string> { "Nữ", "Nam", "Khác", string.Empty };

            var properties = typeof(Employee).GetProperties();
            using (var package = new ExcelPackage(stream))
            {

                var workSheet = package.Workbook.Worksheets.Add("Danh Sách Nhân Viên");

                // Chỉnh tiêu đề trong bảng.

                // STT
                workSheet.Cells[3, 1].Value = "STT";
                workSheet.Cells[3, 1].Style.Font.Bold = true;
                workSheet.Cells[3, 1].Style.Fill.SetBackground(Color.LightGray);
                workSheet.Cells[3, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);


                var column = 2;

                foreach (var prop in properties)
                {
                    var propMISAExport = prop.GetCustomAttributes(typeof(MISAExported), true);

                    //Xét các trường có được export
                    if (propMISAExport.Length == 1)
                    {
                        // định dạng ngày tháng
                        if (prop.PropertyType.Name.Contains(typeof(Nullable).Name) && prop.PropertyType.GetGenericArguments()[0] == typeof(DateTime))
                        {
                            workSheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            workSheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        workSheet.Cells[3, column].Value = (propMISAExport[0] as MISAExported).Name;
                        workSheet.Cells[3, column].Style.Font.Bold = true;
                        workSheet.Cells[3, column].Style.Fill.SetBackground(Color.LightGray);
                        workSheet.Cells[3, column].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                        column++;
                    }
                }

                // Chỉnh bản ghi vào hàng, cell
                for (int i = 0; i < employees.Count; i++)
                {
                    workSheet.Cells[i + 4, 1].Value = i + 1;
                    workSheet.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                    int col = 2;

                    foreach (var prop in properties)
                    {
                        var propMISAExport = prop.GetCustomAttributes(typeof(MISAExported), true);

                        //Xét các trường có được export
                        if (propMISAExport.Length == 1)
                        {

                            if (prop.PropertyType.Name.Contains(typeof(Nullable).Name) && prop.PropertyType.GetGenericArguments()[0] == typeof(DateTime))
                            {
                                var tmp = employees[i].GetType().GetProperty(prop.Name).GetValue(employees[i], null);
                                workSheet.Cells[i + 4, col].Value = tmp == null ? "" : Convert.ToDateTime(tmp).ToString("dd/MM/yyyy");
                            }
                            else if ((propMISAExport[0] as MISAExported).Name == "Giới tính")
                            {
                                var genderName = employees[i].GetType().GetProperty(prop.Name).GetValue(employees[i], null);
                                workSheet.Cells[i + 4, col].Value = genderList[genderName != null ? (int)genderName : 3];
                            }
                            else
                            {
                                workSheet.Cells[i + 4, col].Value = employees[i].GetType().GetProperty(prop.Name).GetValue(employees[i], null);
                            }

                            workSheet.Cells.AutoFitColumns();
                            workSheet.Cells[i + 4, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                            col++;
                        }
                    }
                }

                // Chỉnh tiêu đề cho workSheet
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A2:I2"].Merge = true;
                workSheet.Cells[1, 1].Value = "DANH SÁCH NHÂN VIÊN";
                workSheet.Cells[1, 1].Style.Font.Size = 16;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                package.Save();
            }

            stream.Position = 0;
            return stream;
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
