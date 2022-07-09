using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers;

[Authorize]
public class ProductController:BaseController
{
    private readonly IServiceGeneric<Product, ProductDto> _productService;

    public ProductController(IServiceGeneric<Product, ProductDto> productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return CreateActionResult(await _productService.GetAllAsync());
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveProduct(ProductDto productDto)
    {
        return CreateActionResult(await _productService.AddAsync(productDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductDto productDto)
    {
        return CreateActionResult(await _productService.Update(productDto,productDto.Id));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        return CreateActionResult(await _productService.Remove(id));
    }
}