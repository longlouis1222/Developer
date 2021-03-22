using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Enum
{
    public enum MISAServiceCode
    {
        /// <summary>
        /// Lỗi dữ liệu không hợp lệ !
        /// </summary>
        BadRequest = 400,
        Success = 200,
        Exception = 500
    }
}
