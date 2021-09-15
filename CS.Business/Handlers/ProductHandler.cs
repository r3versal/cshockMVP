using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CS.Business.Handlers
{
    using CS.Common.Models;
    using CS.Common.Utility;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    public class ProductHandler
    {

        public static async Task<Product> InsertProduct(Product product)
        {
            if (product != null)
            {
                Product p = new Product();

                //insert product
                //TODO: handle add photo(s)
                using (var conn = Business.Database.Connection)
                {
                    var newProduct = await conn.QueryAsync<Product>("ProductInsert", new
                    {
                        product
                    },
                     commandType: CommandType.StoredProcedure);
                    if (newProduct.Count() > 0)
                    {
                        return newProduct.AsList()[0];
                    }
                    return null;
                }
            }
            return null;
        }

        public static async Task<Product> UpdateProduct(Product product)
        {
            if (product != null)
            {
                Product p = new Product();

                //insert product
                //TODO: handle add photo(s)
                using (var conn = Business.Database.Connection)
                {
                    var newProduct = await conn.QueryAsync<Product>("ProductUpdate", new
                    {
                        product
                    },
                     commandType: CommandType.StoredProcedure);
                    if (newProduct.Count() > 0)
                    {
                        return newProduct.AsList()[0];
                    }
                    return null;
                }
            }
            return null;
        }

        public static async Task<Product> DeleteProduct(Guid productId)
        {
            if (productId != null)
            {
                using (var conn = Business.Database.Connection)
                {
                    var newProduct = await conn.QueryAsync<Product>("ProductDelete", new
                    {
                        productId
                    },
                     commandType: CommandType.StoredProcedure);
                    if (newProduct.Count() > 0)
                    {
                        return newProduct.AsList()[0];
                    }
                    return null;
                }
            }
            return null;
        }

        public static async Task<List<ProductPhoto>> GetProductPhotosById(Guid productVariantId)
        {
            if (productVariantId != null)
            {
                using (var conn = Business.Database.Connection)
                {
                    var product = await conn.QueryAsync<ProductPhoto>("SELECT * FROM ProductPhoto WHERE productVariantId = '" + productVariantId + "'");
                    return product.AsList();
                }
            }
            return null;
        }

        public static async Task<Product> GetProductById(Guid productId)
        {
            if (productId != null)
            {
                using (var conn = Business.Database.Connection)
                {
                    var product = await conn.QueryAsync<Product>("SELECT * FROM Product WHERE productId = '" + productId + "'");
                    return product.AsList()[0];
                }
            }
            return null;
        }

        public static async Task<Product> GetProductByVariantId(Guid productVariantId)
        {
            if (productVariantId != null)
            {
                using (var conn = Business.Database.Connection)
                {
                    var product = await conn.QueryAsync<Product>("SELECT * FROM Product WHERE productVariantId = '" + productVariantId + "'");
                    return product.AsList()[0];
                }
            }
            return null;
        }

        public static async Task<List<Product>> GetAllProducts()
        {
            using (var conn = Business.Database.Connection)
            {
                var products = await conn.QueryAsync<Product>("SELECT * FROM Product WHERE active = 1");
                return products.AsList();
            }
        }

        public static async Task<List<Product>> UploadPhotos(Guid productVariantId)
        {

            using (var conn = Business.Database.Connection)
            {
                var products = await conn.QueryAsync<Product>("SELECT * FROM Product WHERE active = 1");
                return products.AsList();
            }
        }
    }
}
