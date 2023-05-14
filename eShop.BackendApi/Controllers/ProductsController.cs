using eShop.Application.Catalog.Products;
using eShop.ViewModels.Catalog.ProductImage;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetProductRequest request)
        {
            var result = await _productService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetByCategoryId([FromQuery] GetProductRequest request)
        {
            var result = await _productService.GetByCategoryId(request);
            return Ok(result);
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var result = await _productService.GetById(productId, languageId);
            if (result.data == null) return BadRequest("Can not find product");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var productIdRsesult = await _productService.Create(request);
            if (productIdRsesult.data == 0) return BadRequest();

            var product = await _productService.GetById(productIdRsesult.data, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productIdRsesult.data }, product.data);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var affectedResult = await _productService.Update(request);
            if (affectedResult.data == 0) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _productService.Delete(productId);
            if (affectedResult.data == 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("View/{productId}")]
        public async Task<IActionResult> AddViewCount(int productId)
        {
            var affectedResult = await _productService.AddViewCount(productId);
            if (affectedResult.data == 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("Stock/{productId}/{addedQuantity}")]
        public async Task<IActionResult> UpdateStock(int productId, int addedQuantity)
        {
            var affectedResult = await _productService.UpdateStock(productId, addedQuantity);
            if (affectedResult.data == 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("Price/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var affectedResult = await _productService.UpdatePrice(productId, newPrice);
            if (affectedResult.data == 0) return BadRequest();
            return Ok();
        }

        //Images

        [HttpGet("{productId}/images")]
        public async Task<ApiResult<List<ProductImageVm>>> GetListImages(int productId)
        {
            return await _productService.GetListImages(productId);
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<ApiResult<ProductImageVm>> GetImageById(int productId, int imageId)
        {
            return await _productService.GetImageById(imageId);
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> AddImage(int productId, ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var imageResult = await _productService.AddImage(productId, request);
            if (imageResult.data == 0) return BadRequest();

            var modelView = await _productService.GetImageById(imageResult.data);
            return CreatedAtAction(nameof(GetImageById), new { id = productId }, modelView.data);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int productId, int imageId, ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pi = _productService.GetImageById(imageId);
            if (pi == null) return BadRequest();

            var affectedResult = await _productService.UpdateImage(imageId, request);
            if (affectedResult.data == 0) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int productId, int imageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pi = _productService.GetImageById(imageId);
            if (pi == null) return BadRequest();

            var affectedResult = await _productService.RemoveImage(imageId);
            if (affectedResult.data == 0) return BadRequest();
            return Ok();
        }
    }
}
