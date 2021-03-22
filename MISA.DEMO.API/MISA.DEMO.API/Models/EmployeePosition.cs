using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Models
{
    public class EmployeePosition
    {
        public Guid PositionId { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
    }
}
