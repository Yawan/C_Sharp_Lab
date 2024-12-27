using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sql;

        public UserData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<UserModel> GetUserById(string Id)
        {
            var p = new { Id = Id };

            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "RetailData");

            return output;
        }
    }
}