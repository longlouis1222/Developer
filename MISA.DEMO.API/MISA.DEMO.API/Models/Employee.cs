using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Models
{
    public class Employee
    {
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public Employee()
        {
            EmployeeId = Guid.NewGuid();
        }
        public Guid EmployeeId { get; set; }
        [Required("Mã nhân viên")]
        [CheckDuplicate("Mã nhân viên")]
        [MaxLength("Mã nhân viên", 20)]
        public string EmployeeCode { get; set; }
        [Required("Họ và tên")]
        [MaxLength("Họ và tên", 100)]
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        [Required("CMND/Căn cước công dân")]
        [CheckDuplicate("CMND/Căn cước công dân")]

        public string IdentityCardNumber { get; set; }
        public DateTime? IdentityDate { get; set; }
        public string IdentityPlace { get; set; }
        [Required("Email")]
        public string Email { get; set; }
        [Required("Số điện thoại")]
        [CheckDuplicate("Số điện thoại")]
        [MaxLength("Số điện thoại", 20)]
        public string PhoneNumber { get; set; }
        public Guid PositionId { get; set; }
        //public string PositionName { get; set; }
        public Guid DepartmentId { get; set; }
        //public string DepartmentName { get; set; }
        public string TaxCode { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? DateJoin { get; set; }
        public int? WorkStatus { get; set; }
    }
}
