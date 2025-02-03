using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using ProductApi.Application.Interface;
using ProductApi.Domain.Entities;
using ProductApi.Infrstructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrstructure.Repository
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {

        public async Task<Response> CreateAsync(Product entity)
        {
            try {
                throw new NotImplementedException();

            }
            catch(Exception ex) {
                LogExceptions.LogException(ex);

                return new Response(false,"Error occured while creating product");
            }
        }

        public Task<Response> DeleteAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
