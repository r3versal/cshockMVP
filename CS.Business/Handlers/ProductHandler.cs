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
                //insert product
                using (var conn = Business.Database.Connection)
                {
                    product.createdOn = DateTime.UtcNow;
                    product.updatedOn = DateTime.UtcNow;
                    if (product.ProductId == null || product.ProductId == Guid.Empty)
                    {
                        product.ProductId = Guid.NewGuid();
                    }
                    product.ProductVariantId = Guid.NewGuid();
                    var newProduct = await conn.QueryAsync<Product>("ProductInsert", new {

                        product.ProductId,
                        product.ProductVariantId,
                        product.IsFeatured,
                        product.Active,
                        product.IsDiscount,
                        product.Price,
                        product.StockQuantity,
                        product.IsAlteration,
                        product.IsVirtualFitting,
                        product.IsFitting,
                        product.StripePriceId,
                        product.ProductDescription,
                        product.productTitle,
                        product.ProductCareInstructions,
                        product.productDescriptionShort,
                        product.SKU,
                        product.createdOn,
                        product.updatedOn

                    }, commandType: CommandType.StoredProcedure);
                    var returnedProduct = newProduct.AsList()[0];
                    if (newProduct.Count() > 0)
                    {
                        if(product.productMeasurements != null)
                        {
                            var newMeasurements = await conn.QueryAsync<ProductMeasurements>("ProductMeasurementsInsert", product.productMeasurements, commandType: CommandType.StoredProcedure);
                            returnedProduct.productMeasurements = newMeasurements.AsList()[0];
                        }

                        return returnedProduct;
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
                product.updatedOn = DateTime.UtcNow;
                using (var conn = Business.Database.Connection)
                {
                    var newProduct = await conn.QueryAsync<Product>("ProductUpdate", new
                    {

                        product.ProductId,
                        product.ProductVariantId,
                        product.IsFeatured,
                        product.Active,
                        product.IsDiscount,
                        product.Price,
                        product.StockQuantity,
                        product.IsAlteration,
                        product.IsVirtualFitting,
                        product.IsFitting,
                        product.StripePriceId,
                        product.ProductDescription,
                        product.productTitle,
                        product.ProductCareInstructions,
                        product.productDescriptionShort,
                        product.SKU,
                        product.updatedOn

                    }, commandType: CommandType.StoredProcedure);
                    var returnedProduct = newProduct.AsList()[0];
                    if (newProduct.Count() > 0)
                    {
                        if (product.productMeasurements != null)
                        {
                            if(product.productMeasurements.productMeasurementsId == null || product.productMeasurements.productMeasurementsId == Guid.Empty)
                            {
                                product.productMeasurements.productMeasurementsId = Guid.NewGuid();
                            }
                            var newMeasurements = await conn.QueryAsync<ProductMeasurements>("ProductMeasurementsUpdate", product.productMeasurements, commandType: CommandType.StoredProcedure);
                            if(newMeasurements.AsList().Count() > 0)
                            {
                                returnedProduct.productMeasurements = newMeasurements.AsList()[0];
                            }
                            else
                            {
                                var newMeasurements1 = await conn.QueryAsync<ProductMeasurements>("ProductMeasurementsInsert", product.productMeasurements, commandType: CommandType.StoredProcedure);
                                returnedProduct.productMeasurements = newMeasurements1.AsList()[0];
                            }
                        }

                        return returnedProduct;
                    }
                    return null;
                }
            }
            return null;
        }

        public static async Task<Product> DeleteProduct(Product product)
        {
            if (product != null)
            {
                product.updatedOn = DateTime.UtcNow;
                product.Active = false;
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

        public static async Task<List<ProductPhoto>> GetProductPhotosByProductVariantId(Guid productVariantId)
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

        public static async Task<List<ProductPhoto>> GetProductPhotosByProductId(Guid productId)
        {
            if (productId != null)
            {
                using (var conn = Business.Database.Connection)
                {
                    var product = await conn.QueryAsync<ProductPhoto>("SELECT * FROM ProductPhoto WHERE productId = '" + productId + "'");
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


                    if (product.AsList().Count() > 0)
                    {
                        var prodMeas = await conn.QueryAsync<ProductMeasurements>("SELECT * FROM ProductMeasurements WHERE productId = '" + productId + "'");

                        if (prodMeas.AsList().Count() > 0)
                        {
                            product.AsList()[0].productMeasurements = prodMeas.AsList()[0];
                        }
                        return product.AsList()[0];
                    }
                    else { return null; }
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

        public static async Task<ProductPhoto> CreateProductPhotoMediaFile(ProductPhoto productPhoto)
        {
            using (var conn = Business.Database.Connection)
            {
                productPhoto.createdOn = DateTime.UtcNow;
                productPhoto.productPhotoId = Guid.NewGuid();

                var newProductPhoto = await conn.QueryAsync<ProductPhoto>("ProductPhotoInsert", productPhoto,
                     commandType: CommandType.StoredProcedure);
                if (newProductPhoto.Count() > 0)
                {
                    return newProductPhoto.AsList()[0];
                }
                return null;
            }

        }
    }
}
