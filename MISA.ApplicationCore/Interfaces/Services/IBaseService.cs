using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface IBaseService<MISAEntity>
    {
        /// <summary>
        /// Lấy toàn bộ thực thể
        /// </summary>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        ServiceResponse GetAll();

        /// <summary>
        /// Lấy thông tin thực thể qua ID
        /// </summary>
        /// <param name="entityId">ID thực thể (khóa chính)</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        ServiceResponse GetById(Guid entityId);

        /// <summary>
        /// Sinh mã mới cho thực thể cần thêm mới
        /// </summary>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        ServiceResponse GetNewCode();

        /// <summary>
        /// Thêm thông tin thực thể vào DB
        /// </summary>
        /// <param name="entity">thông tin thực thể cần thêm</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        ServiceResponse Insert(MISAEntity entity);

        /// <summary>
        /// Cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entityId">ID thực thể (khóa chính)</param>
        /// <param name="entity">Thông tin mới</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        ServiceResponse Update(Guid entityId, MISAEntity entity);

        /// <summary>
        /// Xóa thông tin các thực thể
        /// </summary>
        /// <param name="entityIds">Danh sách ID các thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (26/08/2021)
        ServiceResponse Delete(List<Guid> entityIds);
    }
}
