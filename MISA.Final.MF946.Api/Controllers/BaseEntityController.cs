using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.MF946.Final.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseEntityController<MISAEntity> : ControllerBase
    {
        #region Declares
        private readonly IBaseService<MISAEntity> _baseService;
        private readonly IBaseRepository<MISAEntity> _baseRepository;
        #endregion

        #region Constructor
        public BaseEntityController(IBaseService<MISAEntity> baseService,
            IBaseRepository<MISAEntity> baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }
        #endregion

        #region Methods
        #region Lấy toàn bộ dữ liệu
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>Danh sách dữ liệu thực thể</returns>
        /// Author: NQMinh (27/08/2021)
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var entities = _baseService.GetAll();
                if (entities.MISACode == MISACode.IsValid)
                {
                    return Ok(entities.Data);
                }
                else
                {
                    return BadRequest(entities.Data);
                }
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorGet,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorGet,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }
        #endregion

        #region Lấy dữ liệu qua ID
        /// <summary>
        /// Lấy dữ liệu qua ID
        /// </summary>
        /// <param name="entityId">ID của thực thể (khóa chính)</param>
        /// <returns>Dữ liệu thực thể</returns>
        /// Author: NQMinh (27/08/2021)
        [HttpGet("{entityId}")]
        public IActionResult GetById(Guid entityId)
        {
            try
            {
                var entity = _baseService.GetById(entityId);

                if (entity.Data != null)
                {
                    return Ok(entity.Data);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorGetById,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorGetById,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }
        #endregion

        #region Sinh mã mới
        /// <summary>
        /// Sinh mã mới
        /// </summary>
        /// <returns>Dữ liệu mã mới</returns>
        /// Author: NQMinh (27/08/2021)
        [HttpGet("newCode")]
        public IActionResult GetNewCode()
        {
            try
            {
                var newCode = _baseService.GetNewCode();

                if (newCode.MISACode == MISACode.IsValid)
                {
                    return Ok(newCode.Data);
                }
                else
                {
                    return NoContent();
                }
            }
            catch
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorGetNewCode,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorGetNewCode,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }
        #endregion

        #region Thêm thông tin vào DB
        /// <summary>
        /// Thêm thông tin vào DB
        /// </summary>
        /// <param name="entity">Thông tin cần thêm mới</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        [HttpPost]
        public IActionResult Insert(MISAEntity entity)
        {
            try
            {
                var insertResult = _baseService.Insert(entity);

                if (insertResult.MISACode == MISACode.NotValid)
                {
                    return BadRequest(insertResult.Data);
                }

                if (insertResult.MISACode == MISACode.IsValid && (int)insertResult.Data > 0)
                {
                    return Created(Entity.Properties.MessageSuccessVN.messageSuccessInsert, insertResult.Data);
                }
                else
                {
                    return NoContent();
                }
            }
            catch
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorInsert,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorInsert,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
            
        }
        #endregion

        #region Cập nhật dữ liệu
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entityId">ID thực thể</param>
        /// <param name="entity">Thông tin mới</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        [HttpPatch("{entityId}")]
        public IActionResult Update(Guid entityId, MISAEntity entity)
        {
            try
            {
                var updateResult = _baseService.Update(entityId, entity);

                if (updateResult.MISACode == MISACode.NotValid)
                {
                    return BadRequest(updateResult.Data);
                }

                if (updateResult.MISACode == MISACode.IsValid && (int)updateResult.Data > 0)
                {
                    return Ok(updateResult.Data);
                }
                else
                {
                    return NoContent();
                }
            }
            catch
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorUpdate,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorUpdate,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }
        #endregion

        #region Xóa thông tin
        /// <summary>
        /// Xóa thông tin 
        /// </summary>
        /// <param name="entityIds">Danh sách ID các thực thể cần xóa</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        [HttpPost("delete")]
        public IActionResult Delete([FromBody] List<Guid> entityIds)
        {
            try
            {
                var deleteResult = _baseService.Delete(entityIds);

                if (deleteResult.MISACode == MISACode.NotValid)
                {
                    return BadRequest(deleteResult);
                }

                if (deleteResult.MISACode == MISACode.IsValid && (int)deleteResult.Data > 0)
                {
                    return StatusCode(200, deleteResult);
                }
                else
                {
                    return NoContent();
                }
            }
            catch
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorDelete,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorDelete,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }
        #endregion
        #endregion
    }
}
