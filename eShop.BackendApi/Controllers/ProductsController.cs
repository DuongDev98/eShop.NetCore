using Azure.Core;
using eShop.Application.Catalog.Products;
using eShop.ViewModels.Catalog.ProductImage;
using eShop.ViewModels.Catalog.Products.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService publicProductService;
        private readonly IManageProductService manageProductService;

        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            this.publicProductService = publicProductService;
            this.manageProductService = manageProductService;
        }

        [HttpGet("languageId")]
        public async Task<IActionResult> GetAll(string languageId, [FromQuery]GetPublicProductPagingRequest request)
        {
            var products = await publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(products);
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await manageProductService.GetById(productId, languageId);
            if (product == null) return BadRequest("Can not find product");
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var productId = await manageProductService.Create(request);
            if (productId == 0) return BadRequest();

            var product = await manageProductService.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var affectedResult = await manageProductService.Update(request);
            if (affectedResult == 0) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await manageProductService.Delete(productId);
            if (affectedResult == 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("View/{productId}")]
        public async Task<IActionResult> AddViewCount(int productId)
        {
            var affectedResult = await manageProductService.AddViewCount(productId);
            if (affectedResult == 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("Stock/{productId}/{addedQuantity}")]
        public async Task<IActionResult> UpdateStock(int productId, int addedQuantity)
        {
            var affectedResult = await manageProductService.UpdateStock(productId, addedQuantity);
            if (affectedResult == 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("Price/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var affectedResult = await manageProductService.UpdatePrice(productId, newPrice);
            if (affectedResult == 0) return BadRequest();
            return Ok();
        }

        //Images

        [HttpGet("{productId}/images")]
        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await manageProductService.GetListImages(productId);
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<ProductImageViewModel> GetImageById(int productId, int imageId)
        {
            return await manageProductService.GetImageById(imageId);
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> AddImage(int productId, ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var imageId = await manageProductService.AddImage(productId, request);
            if (imageId == 0) return BadRequest();

            var modelView = await manageProductService.GetImageById(imageId);
            return CreatedAtAction(nameof(GetImageById), new { id = productId }, modelView);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int productId, int imageId, ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pi = manageProductService.GetImageById(imageId);
            if (pi == null) return BadRequest();

            var affectedResult = await manageProductService.UpdateImage(imageId, request);
            if (affectedResult == 0) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int productId, int imageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pi = manageProductService.GetImageById(imageId);
            if (pi == null) return BadRequest();

            var affectedResult = await manageProductService.RemoveImage(imageId);
            if (affectedResult == 0) return BadRequest();
            return Ok();
        }
    }
}
