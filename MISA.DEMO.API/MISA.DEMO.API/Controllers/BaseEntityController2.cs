using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.DEMO.API.Data;
using MISA.DEMO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseEntityController2<TEntity> : ControllerBase
    {
        protected DatabaseConnector _dbConnector;
        public BaseEntityController2()
        {
            _dbConnector = new DatabaseConnector();
        }
        /// <summary>
        /// API thực hiện lấy danh sách nhân viên
        /// </summary>
        /// <returns>List nhân viên</returns>
        /// CreatedBy: nhlong (16/12/2020)
        // GET: api/<EmployeesController>
        [HttpGet]
        public virtual IActionResult Get2()
        {
            var employees = _dbConnector.GetData<TEntity>();
            return Ok(employees);
        }

        /// <summary>
        /// API thực hiện lấy danh sách nhân viên qua Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Nhân Viên được chọn</returns>
        /// CreatedBy: nhlong (16/12/2020)
        [HttpGet("{employeeId}")]
        public IActionResult Get2(Guid employeeId)
        {
            var employee = _dbConnector.GetById<TEntity>(employeeId);
            return Ok(employee);
        }

        /// <summary>
        /// API thực hiện lấy danh sách 10 bản ghi cho mỗi trang
        /// </summary>
        /// <param name="infoPerPage"></param>
        /// <returns>List Nhân Viên</returns>
        /// CreatedBy: nhlong (16/12/2020)
        [HttpGet("infoPerPage")]
        public IActionResult Get2(int StartRow, int NumberPage)
        {
            var employees = _dbConnector.GetDataPerPage<TEntity>(StartRow, NumberPage);
            return Ok(employees);
        }

        /// <summary>
        /// API thực hiện lấy danh sách nhân viên qua someInfo
        /// </summary>
        /// <param name="ContainInfo"></param>
        /// <returns></returns>
        /// CreatedBy: NHLONG (30/12/2020)
        [HttpGet("search")]
        public IActionResult Get2([FromQuery] string ContainInfo)
        {
            return Ok(_dbConnector.GetDataBySomeInfo<TEntity>(ContainInfo));
        }

        /// <summary>
        /// API thực hiện lấy danh sách nhân viên qua PositionId và DepartmentId
        /// </summary>
        /// <param name="PositionInfo"></param>
        /// <param name="DepartmentInfo"></param>
        /// <returns></returns>
        [HttpGet("filter")]
        public IActionResult Get2([FromQuery] Guid? PositionInfo, Guid? DepartmentInfo)
        {
            return Ok(_dbConnector.GetDataByPositionAndDepartment<TEntity>(PositionInfo,DepartmentInfo));
        }

        /// <summary>
        /// API thực hiện thêm mới sách nhân viên
        /// </summary>
        /// <param name="entity">Object thêm mới</param>
        /// <returns></returns>
        /// CreatedBy: nhlong (16/12/2020)
        [HttpPost]
        public virtual IActionResult Post2([FromBody] TEntity entity)
        {
            //employee.EmployeeId = Guid.NewGuid();
            var effectRows = _dbConnector.Insert<TEntity>(entity);
            return Ok(effectRows);
        }

        // PUT api/<EmployeesController>/5
        [HttpPut]
        public virtual IActionResult Put2([FromBody] TEntity entity)
        {
            return Ok(_dbConnector.Update<TEntity>(entity));
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        public virtual IActionResult Delete2(Guid id)
        {
            var employee = _dbConnector.Delete<TEntity>(id);
            return Ok(employee);
        }
    }
}
