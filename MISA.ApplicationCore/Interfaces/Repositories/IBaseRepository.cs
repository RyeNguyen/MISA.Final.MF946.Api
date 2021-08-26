using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repositories
{
    public interface IBaseRepository<MISAEntity>
    {
        /// <summary>
        /// Lấy toàn bộ thực thể
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: NQMinh (26/08/2021)
        List<MISAEntity> GetAll();

        /// <summary>
        /// Lấy toàn bộ mã thực thể
        /// </summary>
        /// <returns>Danh sách mã thực thể</returns>
        /// Author: NQMinh (26/08/2021)
        List<string> GetAllCode();

        /// <summary>
        /// Lấy thông tin thực thể qua ID
        /// </summary>
        /// <param name="entityId">ID thực thể</param>
        /// <returns>Thực thể cần lấy</returns>
        /// Author: NQMinh (26/08/2021)
        MISAEntity GetById(Guid entityId);

        /// <summary>
        /// Lấy thông tin thực thể qua mã
        /// </summary>
        /// <param name="entityCode">Mã thực thể</param>
        /// <returns>Thực thể cần lấy</returns>
        /// Author: NQMinh (26/08/2021)
        MISAEntity GetByCode(string entityCode);

        /// <summary>
        /// Thêm thông tin thực thể vào DB
        /// </summary>
        /// <param name="entity">Thông tin cần thêm</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (26/08/2021)
        int Insert(MISAEntity entity);

        /// <summary>
        /// Cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entityId">ID thực thể cần sửa</param>
        /// <param name="entity">Thông tin mới</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (26/08/2021)
        int Update(Guid entityId, MISAEntity entity);

        /// <summary>
        /// Xóa thông tin thực thể
        /// </summary>
        /// <param name="entityIds">Danh sách ID các thực thể cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (26/08/2021)
        int Delete(List<Guid> entityIds);

        /// <summary>
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="entityCode">Mã thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        bool CheckDuplicateCode(string entityCode);
    }
}
