using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Models
{
    public class EmployeeDepartment
    {
        public EmployeeDepartment()
        {
            DepartmentId = Guid.NewGuid();
        }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
    }
}
