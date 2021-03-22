using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Models
{
    public class Customer
    {
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public Customer()
        {
            CustomerId = Guid.NewGuid();
        }
        public Guid CustomerId { get; set; }
        [Required("Mã khách hàng")]
        [CheckDuplicate("Mã khách hàng")]
        [MaxLength("Mã khách hàng",20)]
        public string CustomerCode { get; set; }
        [Required("Họ và tên")]
        
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MemberCardCode { get; set; }
        public string Email { get; set; }
        [Required("Số điện thoại")]
        [CheckDuplicate("Số điện thoại")]
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTaxCode { get; set; }
        public string Address { get; set; }
        //public Guid CustomerGroupId { get; set; }
        //public string CustomerGroupName { get; set; }
    }
}
