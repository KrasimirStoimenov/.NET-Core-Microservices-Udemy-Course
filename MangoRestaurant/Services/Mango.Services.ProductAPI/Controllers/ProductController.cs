﻿namespace Mango.Services.ProductAPI.Controllers;

using Mango.Services.ProductAPI.Models.Dtos;
using Mango.Services.ProductAPI.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    protected ResponseDto response;
    private readonly IProductRepository productRepository;

    public ProductController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
        this.response = new ResponseDto();

    }

    [Authorize]
    [HttpGet]
    public async Task<ResponseDto> GetProducts()
    {
        try
        {
            IEnumerable<ProductDto> productDtos = await this.productRepository.GetProducts();
            response.Result = productDtos;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public async Task<ResponseDto> GetProductById(int id)
    {
        try
        {
            ProductDto productDto = await this.productRepository.GetProductById(id);
            response.Result = productDto;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpPost]
    [Authorize]
    public async Task<ResponseDto> PostProduct(
        [FromBody] ProductDto productDto)
    {
        try
        {
            ProductDto productModel = await this.productRepository.CreateUpdateProduct(productDto);
            response.Result = productModel;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpPut]
    [Authorize]
    public async Task<ResponseDto> UpdateProduct(
        [FromBody] ProductDto productDto)
    {
        try
        {
            ProductDto productModel = await this.productRepository.CreateUpdateProduct(productDto);
            response.Result = productModel;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpDelete]
    [Authorize(Roles ="Admin")]
    [Route("{id}")]
    public async Task<ResponseDto> DeleteProduct(int id)
    {
        try
        {
            bool isSuccess = await this.productRepository.DeleteProduct(id);
            response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }
}
