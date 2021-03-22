using MISA.DEMO.API.Data;
using MISA.DEMO.API.Enum;
using MISA.DEMO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Services
{
    public class CustomerService
    {
        DatabaseConnector _databaseConnector;
        ServiceResult _serviceResult;
        public CustomerService()
        {
            _databaseConnector = new DatabaseConnector();
            _serviceResult = new ServiceResult();
        }
        public ServiceResult InsertCustomer(Customer customer)
        {
            // Validate dữ liệu
            ValidateObject(customer);
            if (_serviceResult.MISACode == MISAServiceCode.BadRequest)
            {
                return _serviceResult;
            }

            var customerCode = customer.CustomerCode;
            var sql = $"SELECT CustomerCode FROM Customer WHERE CustomerCode = '{customerCode}'";
            var customerDuplicates = _databaseConnector.GetData<Customer>(sql);
            // check Duplicate Ma khach hang
            if (customerDuplicates.Count() > 0)
            {
                return new ServiceResult()
                {
                    Data = customerDuplicates,
                    Messenger = new List<string>() { Properties.Resources.Error_Duplicate },
                    MISACode = MISAServiceCode.BadRequest
                };
            }

            return new ServiceResult
            {
                Data = _databaseConnector.Insert<Customer>(customer),
                Messenger = new List<string>() { Properties.Resources.Msg_Success },
                MISACode = MISAServiceCode.Success
            };
            
        }
        private void ValidateObject(Customer customer)
        {
            var properties = typeof(Customer).GetProperties();
            foreach (var property in properties)
            {
                var propValue = property.GetValue(customer);
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

                if (property.IsDefined(typeof(CheckDuplicate), true))
                {
                    var requiredAttribute = property.GetCustomAttributes(typeof(CheckDuplicate), true).FirstOrDefault();
                    if (requiredAttribute != null)
                    {
                        var propertyText = (requiredAttribute as CheckDuplicate).PropertyName;
                        var errorMsg = (requiredAttribute as CheckDuplicate).ErrorMessenger;
                        var sql = $"SELECT {propName} FROM {typeof(Customer).Name} WHERE {propName} = '{propValue}'";
                        var entity = _databaseConnector.GetData<Customer>(sql).FirstOrDefault();
                        if (entity != null)
                        {
                            _serviceResult.Messenger.Add(errorMsg == null ? $"{propertyText} đã tồn tại trên hệ thống baby nhé !" : errorMsg);
                            _serviceResult.MISACode = MISAServiceCode.BadRequest;
                        }
                    }
                }

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
