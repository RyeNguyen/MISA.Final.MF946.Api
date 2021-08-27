using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.Entity.MISA.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Hàm xử lý phân trang cho nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Dữ liệu phân trang</returns>
        /// Author: NQMinh (27/08/2021)
        public object Pagination(string employeeFilter, int pageIndex, int pageSize)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@EmployeeFilter", employeeFilter ?? string.Empty);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@PageSize", pageSize);

                parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var data = _dbConnection.Query<Employee>($"Proc_EmployeePagingAndFilter", param: parameters, commandType: CommandType.StoredProcedure);

                var totalPage = parameters.Get<int>("@TotalPage");
                var totalRecord = parameters.Get<int>("@TotalRecord");

                var pagingData = new
                {
                    totalPage,
                    totalRecord,
                    data
                };

                return pagingData;
            }
        }
    }
}
