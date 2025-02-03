using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} already added");

                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Id > 0)
                    return new Response(true, $"{entity.Name} added succsessfully");
                else
                    return new Response(false, $"Error occurred while adding {entity.Name}");
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);

                return new Response(false, "Error occured while creating product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var getProduct = await FindByIdAsync(entity.Id);
                if (getProduct is null)
                    return new Response(false, $"{entity.Name} does not exist");
                context.Products.Remove(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is deleted succsessfully");

            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);

                return new Response(false, "Error occured while deleting product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var getProduct = await context.Products.FindAsync(id);
                return getProduct is not null ? getProduct : null!;

            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);

                throw new Exception("Error occured while retiving product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var getProducts = await context.Products.AsNoTracking().ToListAsync();
                return getProducts is not null ? getProducts : null!;

            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);

                throw new InvalidOperationException("Error occured while retiving products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var getProduct = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return getProduct is not null ? getProduct : null!;

            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);

                throw new Exception("Error occured while retiving product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var getProduct = await FindByIdAsync(entity.Id);

                if (getProduct is null)
                    return new Response(false, $"{entity.Name}not found");

                context.Entry(getProduct).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} added updated succsessfully");
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);

                return new Response(false, "Error occured while updating product");
            }
        }
    }
}
