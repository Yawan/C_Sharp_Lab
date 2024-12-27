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
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;

        public ProductData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<ProductModel> GetProducts()
        {
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", new { }, "RetailData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productId }, "RetailData").FirstOrDefault();

            return output;
        }
    }
}