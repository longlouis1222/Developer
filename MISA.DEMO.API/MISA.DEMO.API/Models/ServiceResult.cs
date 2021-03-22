using MISA.DEMO.API.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Models
{
    public class ServiceResult
    {
        public object Data { get; set; }
        public List<string> Messenger { get; set; } = new List<string>();
        /// <summary>
        /// Mã kết quả
        /// </summary>
        public MISAServiceCode MISACode { get; set; }
        //public object Data { get; set; }
    }
}
