using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.DEMO.API.Data;
using MISA.DEMO.API.Models;
using MISA.DEMO.API.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.DEMO.API.Controllers
{
    public class CustomersController : BaseEntityController<Customer>
    {
        public override IActionResult Get()
        {
            var sql = $"SELECT * FROM Customer LIMIT 10";
            DatabaseConnector databaseConnector = new DatabaseConnector();
            return Ok(_dbConnector.GetData<Customer>(sql));
        }

        public override IActionResult Post([FromBody] Customer customer)
        {
            CustomerService customerService = new CustomerService();
            var res = customerService.InsertCustomer(customer);
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
            //return Ok(_dbConnector.Insert<Customer>(customer));
        }
    }
}
