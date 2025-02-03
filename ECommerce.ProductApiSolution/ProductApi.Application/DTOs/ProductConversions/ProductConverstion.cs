using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs.ProductConversions
{
    public static class ProductConverstion
    {
        public static Product ToEntitiy(ProductDTO productDTO) => new()
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Price = productDTO.Price,
            Quantity = productDTO.Quantity,
        };

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product? product, IEnumerable<Product>? products) 
        {
            if (product is not null || products is null) 
            {
                var SingleProduct = new ProductDTO
                (
                    product!.Id,
                    product.Name!,
                    product.Quantity,
                    product.Price
                );
                return (SingleProduct, null);
            }

            if (product is null || products is not null) 
            {
                var _products = products!.Select(p =>
                    new ProductDTO(p.Id,p.Name!,p.Quantity,p.Price)).ToList();
                return(null, _products);
            }
            return (null, null);
        }       
    }
}
