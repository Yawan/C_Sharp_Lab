using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.DataAccess
{
    public class InventoryData : IInventoryData
    {
        private const string ConnectionStringName = "RetailData";
        private readonly IConfiguration _config;
        private readonly ISqlDataAccess _sql;

        public InventoryData(IConfiguration config, ISqlDataAccess sql)
        {
            _config = config;
            _sql = sql;
        }

        public List<InventoryModel> GetInventory()
        {
            var output = _sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, ConnectionStringName);
            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            _sql.SaveData("dbo.spInventory_Insert", item, ConnectionStringName);
        }
    }
}