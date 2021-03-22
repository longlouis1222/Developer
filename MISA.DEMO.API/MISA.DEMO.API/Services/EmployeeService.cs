using MISA.DEMO.API.Data;
using MISA.DEMO.API.Enum;
using MISA.DEMO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Services
{
    public class EmployeeService
    {
        DatabaseConnector _databaseConnector;
        ServiceResult _serviceResult;
        public EmployeeService()
        {
            _databaseConnector = new DatabaseConnector();
            _serviceResult = new ServiceResult();
        }
        //public ServiceResult DeleteEmployee(Employee employee)
        //{

        //}

        public ServiceResult UpdateEmployee(Employee employee)
        {
            //Validate dữ liệu
            ValidateObject(employee);
            if (_serviceResult.MISACode == MISAServiceCode.BadRequest)
            {
                return _serviceResult;
            }

            var ServiceResult = new ServiceResult()
            {
                Data = _databaseConnector.Update<Employee>(employee),
                Messenger = new List<string> { Properties.Resources.Msg_Update_Success },
                MISACode = MISAServiceCode.Success
            };
            return ServiceResult;
        }

        public ServiceResult InsertEmployee(Employee employee)
        {
            // Validate dữ liệu
            ValidateObject(employee);
            if (_serviceResult.MISACode == MISAServiceCode.BadRequest)
            {
                return _serviceResult;
            }

            var employeeCode = employee.EmployeeCode;
            var sql = $"SELECT EmployeeCode FROM Employee WHERE EmployeeCode = '{employeeCode}'";
            var employeeDuplicates = _databaseConnector.GetData<Employee>(sql);
            // check Duplicate Ma khach hang
            if (employeeDuplicates.Count() > 0)
            {
                return new ServiceResult()
                {
                    Data = employeeDuplicates,
                    Messenger = new List<string>() { Properties.Resources.Error_Duplicate },
                    MISACode = MISAServiceCode.BadRequest
                };
            }

            return new ServiceResult
            {
                Data = _databaseConnector.Insert<Employee>(employee),
                Messenger = new List<string>() { Properties.Resources.Msg_Success },
                MISACode = MISAServiceCode.Success
            };

        }

        private void ValidateObject(Employee employee)
        {
            var properties = typeof(Employee).GetProperties();
            foreach (var property in properties)
            {
                var propValue = property.GetValue(employee);
                var propName = property.Name;
                // Nếu có attribute là [Required] thì kiểm tra thực hiện bắt buộc nhập
                if (property.IsDefined(typeof(Required),true) && (propValue == null || propValue.ToString() == string.Empty))
                {
                    var requiredAttribute = property.GetCustomAttributes(typeof(Required), true).FirstOrDefault();
                    if (requiredAttribute != null)
                    {
                        var propertyText = (requiredAttribute as Required).PropertyName;
                        var errorMsg = (requiredAttribute as Required).ErrorMessenger;
                        _serviceResult.Messenger.Add(errorMsg == null ? $"{propertyText} Không được phép để trống !" : errorMsg);

                    }
                    _serviceResult.MISACode = MISAServiceCode.BadRequest;
                }

                //Nếu có attribute là [Duplacate] thì thực hiện kiểm tra trùng
                if (property.IsDefined(typeof(CheckDuplicate), true))
                {
                    var requiredAttribute = property.GetCustomAttributes(typeof(CheckDuplicate), true).FirstOrDefault();
                    if (requiredAttribute != null)
                    {
                        var propertyText = (requiredAttribute as CheckDuplicate).PropertyName;
                        var errorMsg = (requiredAttribute as CheckDuplicate).ErrorMessenger;
                        var sql = $"SELECT {propName} FROM {typeof(Employee).Name} WHERE {propName} = '{propValue}'";
                        var entity = _databaseConnector.GetData<Employee>(sql).FirstOrDefault();
                        if (entity != null)
                        {
                            _serviceResult.Messenger.Add(errorMsg == null ? $"{propertyText} đã tồn tại trên hệ thống Tình yêu nhé !" : errorMsg);
                            _serviceResult.MISACode = MISAServiceCode.BadRequest;
                        }
                    }
                }
                
                //Nếu có attribute là [MaxLength] thì thực hiện kiểm độ dài
                if (property.IsDefined(typeof(MaxLength), true))
                {
                    var requiredAttribute = property.GetCustomAttributes(typeof(MaxLength), true).FirstOrDefault();
                    if (requiredAttribute != null)
                    {
                        var propertyText = (requiredAttribute as MaxLength).PropertyName;
                        var errorMsg = (requiredAttribute as MaxLength).ErrorMessenger;
                        var length = (requiredAttribute as MaxLength).Length;
                        if (propValue.ToString().Trim().Length > length)
                        {
                            _serviceResult.Messenger.Add(errorMsg == null ? $"{propertyText} dài quá babe!" : errorMsg);
                            _serviceResult.MISACode = MISAServiceCode.BadRequest;
                        }
                    }
                }
            }
        }
    }
}
