using MISA.Entity.MISA.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Entity.MISA.Models
{
    public class Employee
    {
        #region Declares
        /// <summary>
        /// Id của nhân viên
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [MISARequired("Mã nhân viên")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Họ và tên nhân viên
        /// </summary>
        [MISARequired("Họ và tên")]
        public string FullName { get; set; }

        /// <summary>
        /// Ngày tháng năm sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// ID đơn vị
        /// </summary>
        [MISARequired("Đơn vị")]
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Số cmnd/căn cước
        /// </summary>
        /// [MISARequired("Số CMND")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp cmnd
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp cmnd
        /// </summary>
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Email nhân viên
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public string BackAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BackName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string BankBranch { get; set; }
        #endregion
    }
}
