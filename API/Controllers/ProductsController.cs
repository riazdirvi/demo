using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IGenericRepository<Product> _productRepo;
        private IGenericRepository<ProductBrand> _brandRepo;
        private IGenericRepository<ProductType> _typeRepo;
        private IMapper _mapper;
        
        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> brandRepo,
            IGenericRepository<ProductType> typeRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _brandRepo = brandRepo;
            _typeRepo = typeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            // var products = await _productRepo.ListAllAsync();
            var spec = new ProductsWithTypesAndBrandsSpecs();
            var products = await _productRepo.ListAsync(spec);
            // return products.Select(product => new ProductToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name 
            // }).ToList();

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // return await _productRepo.GetByIdAsync(id);
            var spec = new ProductsWithTypesAndBrandsSpecs(id);
            var product = await _productRepo.GetEntityWithSpec(spec);

            // return new ProductToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name    
            // };

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok( await _brandRepo.ListAllAsync());            
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _typeRepo.ListAllAsync());
        }
    }
}