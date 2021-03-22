using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Data
{
    public class DatabaseConnector
    {
        public static string connectionString;
        IDbConnection _dbConnection;

        public DatabaseConnector()
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <typeparam name="TEntity">Kiểu đối tượng cần lấy về</typeparam>
        /// <returns>Trả về danh sách các object</returns>
        /// CreatedBy: NHLONG (12/12/2020)
        public IEnumerable<TEntity> GetData<TEntity>()
        {
            string className = typeof(TEntity).Name;
            var sql = $"SELECT * FROM {className}";
            var entities = _dbConnection.Query<TEntity>(sql);
            return entities;
        }
        /// <summary>
        /// Lấy danh sách theo commandText truyền vào
        /// </summary>
        /// <typeparam name="commandText">Mã sql</typeparam>
        /// <returns>Mảng các object lầy được từ db</returns>
        /// CreatedBy: NHLONG (12/12/2020)
        public IEnumerable<TEntity> GetData<TEntity>(string commandText)
        {
            string className = typeof(TEntity).Name;
            var sql = commandText;
            var entities = _dbConnection.Query<TEntity>(sql);
            return entities;
        }
        /// <summary>
        /// Lấy danh sách qua Id
        /// </summary>
        /// <typeparam name="TEntity">Kiểu đối tượng cần lấy về</typeparam>
        /// <returns>Trả về danh sách các object</returns>
        /// CreatedBy: NHLONG (12/12/2020)
        public TEntity GetById<TEntity>(object id)
        {
            string className = typeof(TEntity).Name;
            var sql = $"SELECT * FROM {className} WHERE {className}Id = '{id.ToString()}'";
            var entities = _dbConnection.Query<TEntity>(sql).FirstOrDefault();
            return entities;
        }
        /// <summary>
        /// Lấy danh sách 10 bản ghi cho mỗi trang
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="StartRow"></param>
        /// <param name="NumberPage"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetDataPerPage<TEntity>(int StartRow, int NumberPage)
        {
            string className = typeof(TEntity).Name;
            var storeName = $"Proc_Get{className}PerPage";
            var param = new
            {
                startRow = (StartRow - 1) * NumberPage,
                numberPage = NumberPage
            };
            var entities = _dbConnection.Query<TEntity>(storeName,param,commandType:CommandType.StoredProcedure).ToList();
            return entities;
        } 
        /// <summary>
        /// Lấy thông tin nhân viên qua các thuộc tính
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ContainInfo"></param>
        /// <param name="DepartmentId"></param>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        /// CreatedBy: NHLONG (30/12/2020)
        public IEnumerable<TEntity> getDataBySomeInfo<TEntity>(string ContainInfo, Guid? PositionId, Guid? DepartmentId)
        {
            string className = typeof(TEntity).Name;
            var storeName = $"Proc_Get{className}ByInfo";
            var parameters = new DynamicParameters();
            parameters.Add($"@EmployeeInfoContains", ContainInfo != null ? ContainInfo : string.Empty);
            parameters.Add($"@DepartmentId", DepartmentId != null ? DepartmentId.ToString() : null);
            parameters.Add($"@PositionId", PositionId != null ? PositionId.ToString() : null);
            var selectObject = _dbConnection.Query<TEntity>(storeName, parameters, commandType: CommandType.StoredProcedure);
            return selectObject;
        }
        /// <summary>
        /// Thêm thông tin
        /// </summary>
        /// <typeparam name="TEntity">Kiểu đối tượng cần truyền lên</typeparam>
        /// <returns>Trả về danh sách object</returns>
        /// CreatedBy: NHLONG (12/12/2020)
        public int Insert<TEntity>(TEntity entity)
        {
            string className = typeof(TEntity).Name;
            var properties = typeof(TEntity).GetProperties();
            var parameters = new DynamicParameters();
            var sqlPropetyBuilder = string.Empty;
            var sqlPropetyParamBuilder = string.Empty;
            foreach (var propety in properties)
            {
                var propertyName = propety.Name;
                var propertyValue = propety.GetValue(entity);
                parameters.Add($"@{propertyName}",propertyValue);
                sqlPropetyBuilder += $",{propertyName}";
                sqlPropetyParamBuilder += $",@{propertyName}";
            }
            var sql = $"INSERT INTO {className}({sqlPropetyBuilder.Substring(1)}) VALUE ({sqlPropetyParamBuilder.Substring(1)})";
            var effectRows = _dbConnection.Execute(sql, parameters);
            return effectRows;
        }

        /// <summary>
        /// Xóa thông tin qua Id
        /// </summary>
        /// <typeparam name="TEntity">Generic</typeparam>
        /// <param name="id">Mã Id</param>
        /// <returns></returns>
        /// CreatedBy: NHLONG (12/12/2020)
        public int Delete<TEntity>(Guid id)
        {
            string className = typeof(TEntity).Name;
            var storeName = $"Proc_Delete{className}";
            var parameters = new DynamicParameters();
            parameters.Add($"@{className}Id", id.ToString());
            var affecRows = _dbConnection.Execute(storeName, parameters,commandType: CommandType.StoredProcedure);
            return affecRows;
        }

        /// <summary>
        /// Sửa thông tin nhân viên
        /// </summary>
        /// <typeparam name="TEntity">Generic</typeparam>
        /// <param name="entity">object sửa</param>
        /// <returns></returns>
        /// CreatedBy: NHLONG (12/12/2020)
        /// 
        public int Update<TEntity>(TEntity entity)
        {
            string className = typeof(TEntity).Name;
            var storeName = $"Proc_Update{className}";
            //Đọc các property của TEntity:
            var properties = typeof(TEntity).GetProperties();
            var parameters = new DynamicParameters();
            
            foreach (var propety in properties)
            {
                var propertyName = propety.Name;
                var propertyValue = propety.GetValue(entity);
                var propetyType = propety.PropertyType;

                if (propetyType == typeof(Guid) || propetyType == typeof(Guid?))
                {
                    propertyValue = propertyValue.ToString();
                    parameters.Add($"@{propertyName}", propertyValue);
                }
                else
                {
                    propertyValue = propety.GetValue(entity);
                    parameters.Add($"@{propety.Name}", propertyValue);
                }
            }
            var effectRows = _dbConnection.Execute(storeName, parameters, commandType: CommandType.StoredProcedure);
            return effectRows;
        }
        //public int Update2<TEntity>(TEntity entity)
        //{
        //    var className = typeof(TEntity).Name;
        //    var storeName = $"Proc_Update{className}";
        //    var parameters = new DynamicParameters();
        //    // Lệnh lấy các property
        //    var prorperties = typeof(TEntity).GetProperties();
        //    foreach (var property in prorperties)
        //    {
        //        var propertyName = property.Name;
        //        var propertyValue = property.GetValue(entity);
        //        var propertyType = property.PropertyType.Name;
        //        if (propertyType != "String" && propertyType != "DateTime")
        //            propertyValue = property.GetValue(entity).ToString();
        //        parameters.Add($"@{propertyName}", propertyValue);
        //    }
        //    var affecRows = _dbConnection.Execute(storeName, parameters, commandType: CommandType.StoredProcedure);
        //    return affecRows;
        //}

    }
}
