using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repositories
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        #region Declares
        protected readonly string _connectionString;
        protected IDbConnection _dbConnection;
        protected IConfiguration _configuration;
        private readonly string _className;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MisaFinal");
            _className = typeof(MISAEntity).Name;
        }
        #endregion

        #region Methods
        #region Phương thức lấy tất cả dữ liệu của thực thể
        /// <summary>
        /// Lấy toàn bộ danh sách thực thể từ DB
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: NQMinh(27/08/2021)
        public List<MISAEntity> GetAll()
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var storeName = $"Proc_{_className}GetAll";

                var entities = _dbConnection.Query<MISAEntity>(storeName, commandType: CommandType.StoredProcedure);

                return entities.ToList();
            }
        }
        #endregion

        #region Phương thức lấy thông tin thực thể qua ID
        /// <summary>
        /// Lấy thông tin thực thể theo ID
        /// </summary>
        /// <param name="entityId">ID thực thể</param>
        /// <returns>Thông tin thực thể cần lấy</returns>
        /// Author: NQMinh(27/08/2021)
        public MISAEntity GetById(Guid entityId)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();

                var storeName = $"Proc_{_className}GetById";

                parameters.Add($"@{_className}Id", entityId);

                var entity = _dbConnection.QueryFirstOrDefault<MISAEntity>(storeName, param: parameters, commandType: CommandType.StoredProcedure);

                return entity;
            }
        }
        #endregion

        #region Phương thức lấy thông tin thực thể qua mã
        /// <summary>
        /// Lấy thực thể theo mã
        /// </summary>
        /// <param name="entityCode">Mã thực thể</param>
        /// <returns>Thực thể đầu tiên lấy được</returns>
        /// Author: NQMinh(27/08/2021)
        public MISAEntity GetByCode(string entityCode)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var dynamicParams = new DynamicParameters();

                var storeName = $"Proc_{_className}GetByCode";

                dynamicParams.Add($"@{_className}Code", entityCode);

                var entity = _dbConnection.QueryFirstOrDefault<MISAEntity>(storeName, param: dynamicParams, commandType: CommandType.StoredProcedure);

                return entity;
            }
        }
        #endregion

        #region Phương thức thêm thực thể vào DB
        /// <summary>
        /// Thêm mới thực thể
        /// </summary>
        /// <param name="entity">thông tin thực thể</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh(27/08/2021)
        public int Insert(MISAEntity entity)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                //Khai báo dynamic param:
                var dynamicParams = new DynamicParameters();

                var columnsName = string.Empty;
                var columnsParam = string.Empty;

                //Đọc từng property của object:
                var properties = entity.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //Lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(entity);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;
                    dynamicParams.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                }

                var storeName = $"Proc_{_className}Insert";

                var rowAffects = _dbConnection.Execute(storeName, param: dynamicParams, commandType: CommandType.StoredProcedure);

                //Trả về số bản ghi thêm mới:
                return rowAffects;
            }
        }
        #endregion

        #region Sửa thông tin tin thực thể
        /// <summary>
        /// Sửa thông tin thực thể
        /// </summary>
        /// <param name="entityId">ID của thực thể cần sửa</param>
        /// <param name="entity">Dữ liệu cần sửa của thực thể</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (27/08/2021)
        public int Update(Guid entityId, MISAEntity entity)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                //Khai báo dynamic param:
                DynamicParameters dynamicParams = new();

                //var queryString = string.Empty;

                //Đọc từng property của object:
                var properties = entity.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //Lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(entity);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //Thêm param tương ứng với mỗi property của đối tượng:
                    if (propName != $"{_className}Id")
                    {
                        dynamicParams.Add($"@{propName}", propValue);
                    //    if (propValue != null)
                    //    {
                    //        dynamicParams.Add($"@{propName}", propValue);
                    //    }
                    //    else
                    //    {
                    //        dynamicParams.Add($"@{propName}");
                    //    }
                    }
                }

                dynamicParams.Add($"@{_className}Id", entityId);

                var storeName = $"Proc_{_className}Update";

                var rowAffects = _dbConnection.Execute(storeName, param: dynamicParams, commandType: CommandType.StoredProcedure);

                return rowAffects;
            }
        }
        #endregion

        #region Xóa thực thể khỏi DB
        /// <summary>
        /// Xóa thực thể khỏi DB
        /// </summary>
        /// <param name="entityIds">Danh sách ID của thực thể cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (27/08/2021)
        public int Delete(List<Guid> entityIds)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var idString = string.Empty;

                foreach (var entityId in entityIds)
                {
                    idString += $"'{entityId}',";
                }

                idString = idString.Remove(idString.Length - 1);

                var sqlCommand = $"DELETE FROM {_className} WHERE {_className}Id IN ({idString})";

                var rowAffects = _dbConnection.Execute(sqlCommand);

                return rowAffects;
            }
        }
        #endregion

        #region Lấy tất cả mã thực thể
        /// <summary>
        /// Lấy tất cả mã thực thể
        /// </summary>
        /// <returns>Danh sách mã trả về</returns>
        /// Author: NQMinh (27/08/2021)
        public List<string> GetAllCode()
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var storeName = $"Proc_{_className}GetAllCode";

                var codeList = _dbConnection.Query<string>(storeName, commandType: CommandType.StoredProcedure);

                return codeList.ToList();
            }
        }
        #endregion

        #region Kiểm tra mã thực thể có bị trùng trong hệ thống không
        /// <summary>
        /// Kiểm tra mã thực thể có bị trùng trong hệ thống không
        /// </summary>
        /// <param name="entityCode">Mã thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        public bool CheckDuplicateCode(string entityCode)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();

                var storeName = $"Proc_{_className}CheckDuplicateCode";

                parameters.Add($"@{_className}Code", entityCode);

                parameters.Add("@AlreadyExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                _dbConnection.Execute(storeName, param: parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<bool>("@AlreadyExist");
            }
        }
        #endregion
        #endregion
    }
}
