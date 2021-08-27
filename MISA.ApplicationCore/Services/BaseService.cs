using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Entity.MISA.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region Declares
        private readonly IBaseRepository<MISAEntity> _baseRepository;
        private readonly ServiceResponse _serviceResponse;
        private readonly string _className;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResponse = new ServiceResponse();
            _className = typeof(MISAEntity).Name;
        }
        #endregion

        #region Methods
        #region Lấy danh sách thực thể từ DB
        /// <summary>
        /// Lấy danh sách thực thể từ DB
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: NQMinh (26/08/2021)
        public ServiceResponse GetAll()
        {
            _serviceResponse.Data = _baseRepository.GetAll();

            if (_serviceResponse.Data != null)
            {
                _serviceResponse.MISACode = MISACode.IsValid;
            }
            else
            {
                var errorMsg = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorGet,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorGet,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorMsg;
                _serviceResponse.Message = Entity.Properties.MessageErrorVN.messageErrorGet;
                _serviceResponse.MISACode = MISACode.NotValid;
            }
            return _serviceResponse;
        }
        #endregion

        #region Lấy thông tin thực thể qua ID
        /// <summary>
        /// Lấy thông tin thực thể qua ID
        /// </summary>
        /// <param name="entityId">ID thực thể</param>
        /// <returns>Thông tin thực thể cần lấy</returns>
        /// Author: NQMinh (27/08/2021)
        public ServiceResponse GetById(Guid entityId)
        {
            _serviceResponse.Data = _baseRepository.GetById(entityId);

            if (_serviceResponse.Data != null)
            {
                _serviceResponse.MISACode = MISACode.IsValid;
            }
            else
            {
                var errorMsg = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorGetById,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorGetById,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorMsg;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Message = Entity.Properties.MessageErrorVN.messageErrorGetById;
            }

            return _serviceResponse;
        }
        #endregion

        #region Sinh mã mới
        /// <summary>
        /// Sinh mã mới
        /// </summary>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        public ServiceResponse GetNewCode()
        {
            var codeList = _baseRepository.GetAllCode();

            if (codeList is null)
            {
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Data = codeList;
                _serviceResponse.Message = Entity.Properties.MessageErrorVN.messageErrorGetAllCode;
                return _serviceResponse;
            }

            var codeDigitList = new List<int>();
            foreach (var code in codeList)
            {
                var codeDigit = int.Parse(code[2..]);
                codeDigitList.Add(codeDigit);
            }

            codeDigitList.Sort((x, y) => y.CompareTo(x));

            _serviceResponse.Data = codeList[0].Substring(0, 2) + (codeDigitList[0] + 1).ToString();
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Thêm mới thực thể vào cơ sở dữ liệu
        /// <summary>
        /// Thêm mới thực thể vào cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Thông tin thực thể cần thêm</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        public ServiceResponse Insert(MISAEntity entity)
        {
            var commonValidate = ValidateCommon(entity);
            if (commonValidate.MISACode == MISACode.NotValid)
            {
                return commonValidate;
            }

            var customValidate = ValidateCustom(entity);
            if (customValidate.MISACode == MISACode.NotValid)
            {
                return customValidate;
            }

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = _baseRepository.Insert(entity);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Entity.Properties.MessageSuccessVN.messageSuccessInsert;
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Cập nhật thông tin thực thể
        /// <summary>
        /// Cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entityId">ID thực thể cần sửa</param>
        /// <param name="entity">Thông tin mới</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        public ServiceResponse Update(Guid entityId, MISAEntity entity)
        {
            var commonValidate = ValidateCommon(entity);
            if (commonValidate.MISACode == MISACode.NotValid)
            {
                return commonValidate;
            }

            var customValidate = ValidateCustom(entity);
            if (customValidate.MISACode == MISACode.NotValid)
            {
                return customValidate;
            }

            //Sửa thông tin khi dữ liệu hợp lệ:
            var rowAffects = _baseRepository.Update(entityId, entity);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Entity.Properties.MessageSuccessVN.messageSuccessUpdate;
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Xóa thông tin (các) thực thể khỏi cơ sở dữ liệu
        /// <summary>
        /// Xóa thông tin (các) thực thể khỏi cơ sở dữ liệu
        /// </summary>
        /// <param name="entityIds">Danh sách ID thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        public ServiceResponse Delete(List<Guid> entityIds)
        {
            var rowAffects = _baseRepository.Delete(entityIds);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Entity.Properties.MessageSuccessVN.messageSuccessDelete;
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Phương thức kiểm tra dữ liệu chung
        /// <summary>
        /// Phương thức kiểm tra dữ liệu chung
        /// </summary>
        /// <param name="entity">Dữ liệu thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        private ServiceResponse ValidateCommon(MISAEntity entity)
        {
            //Thực hiện validate:
            //1. Bắt buộc nhập:
            var checkRequired = CheckRequired(entity);
            if (checkRequired.MISACode == MISACode.NotValid)
            {
                return checkRequired;
            }

            //2. Check trùng mã:
            var checkDuplicateCode = CheckDuplicateCode(entity);
            if (checkDuplicateCode.MISACode == MISACode.NotValid)
            {
                return checkDuplicateCode;
            }

            //3. Check định dạng email:
            var checkEmailFormat = CheckEmailFormat(entity);
            if (checkEmailFormat.MISACode == MISACode.NotValid)
            {
                return checkEmailFormat;
            }

            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Phương thức kiểm tra các trường bắt buộc
        /// <summary>
        /// Phương thức kiểm tra các trường bắt buộc
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        private ServiceResponse CheckRequired(MISAEntity entity)
        {
            //1. Lấy thông tin các property:
            var properties = typeof(MISAEntity).GetProperties();

            //2. Xác định việc validate dựa trên attribute: (MISARequired - check thông tin không được phép null hoặc trống)
            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(entity);

                //Kiểm tra prop hiện tại có bắt buộc nhập hay không
                var propMISARequired = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (propMISARequired.Length > 0)
                {
                    var errorMessage = (propMISARequired[0] as MISARequired)._message;
                    if (prop.PropertyType == typeof(string) && (propValue == null || propValue.ToString() == string.Empty))
                    {
                        _serviceResponse.MISACode = MISACode.NotValid;
                        _serviceResponse.Message = errorMessage;
                        _serviceResponse.Data = errorMessage;
                        return _serviceResponse;
                    }
                }
            }
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Phương thức kiểm tra mã trùng
        /// <summary>
        /// Phương thức kiểm tra mã trùng
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        private ServiceResponse CheckDuplicateCode(MISAEntity entity)
        {
            var entityId = (Guid)entity.GetType().GetProperty($"{_className}Id").GetValue(entity);
            var checkedEntity = _baseRepository.GetById(entityId);

            var checkCode = _baseRepository.CheckDuplicateCode(entity.GetType().GetProperty($"{_className}Code").GetValue(entity).ToString());
            if (checkCode == true && checkedEntity == null)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorDuplicateCode,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorDuplicateCode,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorObj;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Message = Entity.Properties.MessageErrorVN.messageErrorDuplicateCode;
                return _serviceResponse;
            }
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Phương thức kiểm tra định dạng email
        /// <summary>
        /// Phương thức kiểm tra định dạng email
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        private ServiceResponse CheckEmailFormat(MISAEntity entity)
        {
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            var isMatch = Regex.IsMatch(entity.GetType().GetProperty("Email").GetValue(entity).ToString(), emailFormat, RegexOptions.IgnoreCase);

            if (isMatch == false)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.MessageErrorVN.messageErrorEmailFormat,
                    userMsg = Entity.Properties.MessageErrorVN.messageErrorEmailFormat,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Message = Entity.Properties.MessageErrorVN.messageErrorEmailFormat;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Data = errorObj;
                return _serviceResponse;
            }
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion

        #region Phương thức kiểm tra dữ liệu riêng
        /// <summary>
        /// Phương thức kiểm tra dữ liệu riêng
        /// </summary>
        /// <param name="entity">Dữ liệu thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (27/08/2021)
        protected virtual ServiceResponse ValidateCustom(MISAEntity entity)
        {
            _serviceResponse.MISACode = MISACode.IsValid;
            return _serviceResponse;
        }
        #endregion
        #endregion
    }
}
