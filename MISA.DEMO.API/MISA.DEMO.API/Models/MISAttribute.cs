using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Models
{
    public class MISAttribute
    {
    }
    /// <summary>
    /// Attribute để xác định bắt buộc nhập
    /// </summary>
    /// CreatedBy: NHLONG (28/12/2020)
    [AttributeUsage(AttributeTargets.Property)]
    public class Required : Attribute
    {
        /// <summary>
        /// Tên của property
        /// </summary>
        public string PropertyName;
        /// <summary>
        /// Câu cảnh báo tùy chỉnh
        /// </summary>
        public string ErrorMessenger;
        public Required(string propertyName, string errorMessenger=null)
        {
            this.PropertyName = propertyName;
            this.ErrorMessenger = errorMessenger;
        }
    }

    /// <summary>
    /// Attribute để check TRÙNG
    /// </summary>
    /// CreatedBy: NHLONG (28/12/2020)
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckDuplicate : Attribute
    {
        /// <summary>
        /// Tên của property
        /// </summary>
        public string PropertyName;
        /// <summary>
        /// Câu cảnh báo tùy chỉnh
        /// </summary>
        public string ErrorMessenger;
        public CheckDuplicate(string propertyName, string errorMessenger = null)
        {
            this.PropertyName = propertyName;
            this.ErrorMessenger = errorMessenger;
        }
    }

    /// <summary>
    /// Attribute để check Độ dài chuỗi
    /// </summary>
    /// CreatedBy: NHLONG (28/12/2020)
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength : Attribute
    {
        /// <summary>
        /// Tên của property
        /// </summary>
        public string PropertyName;
        /// <summary>
        /// Câu cảnh báo tùy chỉnh
        /// </summary>
        public string ErrorMessenger;
        /// <summary>
        /// Độ dài tối đa được phép
        /// </summary>
        public int Length { get; set; }
        public MaxLength(string propertyName, int length, string errorMessenger = null)
        {
            this.PropertyName = propertyName;
            this.ErrorMessenger = errorMessenger;
            Length = length;
        }
    }
}
