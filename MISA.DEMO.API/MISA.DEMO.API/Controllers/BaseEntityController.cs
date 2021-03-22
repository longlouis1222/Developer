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
    public class BaseEntityController<TEntity> : ControllerBase
    {
        protected DatabaseConnector _dbConnector;
        public BaseEntityController()
        {
            _dbConnector = new DatabaseConnector();
        }
        /// <summary>
        /// API thực hiện lấy danh sách thông tin
        /// </summary>
        /// <returns>List khách hàng</returns>
        /// CreatedBy: nhlong (16/12/2020)
        [HttpGet]
        public virtual IActionResult Get()
        {
            var customers = _dbConnector.GetData<TEntity>();
            return Ok(customers);
        }

        /// <summary>
        /// API thực hiện lấy danh sách qua Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>List khách hàng</returns>
        /// CreatedBy: nhlong (16/12/2020)
        
        [HttpGet("{customerId}")]
        public IActionResult Get(Guid customerId)
        {
            var customer = _dbConnector.GetById<TEntity>(customerId);
            return Ok(customer);
        }

        /// <summary>
        /// API thực hiện Thêm Mới thông tin
        /// </summary>
        /// <param name="entity">Object thêm mới</param>
        /// <returns></returns>
        /// CreatedBy: nhlong (16/12/2020)
        [HttpPost]
        public virtual IActionResult Post([FromBody] TEntity entity)
        {
            //customer.CustomerId = Guid.NewGuid();
            var effectRows = _dbConnector.Insert<TEntity>(entity);
            return Ok(effectRows);
        }

        /// <summary>
        /// API thực hiện Update thông tin bản ghi được chọn
        /// </summary>
        /// <param name="entity">Object thêm mới</param>
        /// <returns></returns>
        /// CreatedBy: nhlong (16/12/2020)
        [HttpPut]
        public IActionResult Put([FromBody] TEntity entity)
        {
            var effectRows = _dbConnector.Update<TEntity>(entity);
            return Ok(effectRows);
        }

        /// <summary>
        /// API thực hiện Xóa thông tin bản ghi qua Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: nhlong (16/12/2020)
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_dbConnector.Delete<TEntity>(id));
        }
    }
}
