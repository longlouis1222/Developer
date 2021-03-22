using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.DEMO.API.Data;
using MISA.DEMO.API.Models;
using MISA.DEMO.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Controllers
{
    public class EmployeesController : BaseEntityController2<Employee>
    {
        EmployeeService employeeService = new EmployeeService();
        public override IActionResult Get2()
        {
            var sql = $"SELECT * FROM Employee e ORDER BY e.EmployeeCode DESC LIMIT 500";
            DatabaseConnector databaseConnector = new DatabaseConnector();
            return Ok(_dbConnector.GetData<Employee>(sql));
        }

        public override IActionResult Post2([FromBody] Employee employee)
        {
            var res = employeeService.InsertEmployee(employee);
            switch (res.MISACode)
            {
                case Enum.MISAServiceCode.BadRequest:
                    return BadRequest(res);
                case Enum.MISAServiceCode.Success:
                    return Ok(res);
                case Enum.MISAServiceCode.Exception:
                    return StatusCode(500);
                default:
                    return Ok();
            }
            //return Ok(_dbConnector.Insert<Employee>(employee));
        }
    }
}
