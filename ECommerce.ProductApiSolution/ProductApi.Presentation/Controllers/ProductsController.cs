using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.ProductConversions;
using ProductApi.Application.Interface;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = await productInterface.GetAllAsync();

                if (products == null || !products.Any())
                    return NotFound("No products found from the database");

                var (_, list) = ProductConverstion.FromEntity(null!, products);

                if (list == null || !list.Any())
                    return NotFound("No products found after conversion");

                return Ok(list);
            }
            catch 
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id) 
        {
            var product = await productInterface.FindByIdAsync(id);

            if (product is not null)
                return NotFound("this product not found from the database");

            var (_product, _) = ProductConverstion.FromEntity(product,null!);

            return product is not null ? Ok(product) : NotFound("No product found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO productDTO) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var getEntity = ProductConverstion.ToEntitiy(productDTO);
            var reponse = await productInterface.CreateAsync(getEntity);
            return reponse.flag is true ? Ok(reponse) : NotFound(reponse);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var getEntity = ProductConverstion.ToEntitiy(productDTO);

            var reponse = await productInterface.UpdateAsync(getEntity);
            return reponse.flag is true ? Ok(reponse) : NotFound(reponse);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO productDTO)
        {
            var getEntity = ProductConverstion.ToEntitiy(productDTO);

            var reponse = await productInterface.DeleteAsync(getEntity);
            return reponse.flag is true ? Ok(reponse) : NotFound(reponse);
        }
    }
}