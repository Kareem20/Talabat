using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Specifications.Paramters;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(Pagination<ProductDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecificationParamters paramters)
        {
            var specification = new ProductsWithBrandAndTypeSpecification(paramters);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationAsync(specification);
            var productsDTO = _mapper.Map<IReadOnlyList<Product>, IEnumerable<ProductDTO>>(products);
            return Ok(new Pagination<ProductDTO>()
            {
                PageIndex = paramters.PageIndex,
                PageSize = paramters.PageSize,
                Count = await _unitOfWork.Repository<Product>().GetCountWithSpecificationAsync(new ProductsWithFiltersForCountSpecification(paramters)),
                Data = productsDTO.ToList()
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var specification = new ProductByIdWithBrandAndTypeSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecificationAsync(specification);
            if (product == null)
                return NotFound(new ApiError((int)HttpStatusCode.NotFound));
            var productDTO = _mapper.Map<Product, ProductDTO>(product);
            return Ok(productDTO);
        }

        [HttpGet("types")]
        [ProducesResponseType(typeof(IReadOnlyList<ProductType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(productTypes);
        }

        [HttpGet("brands")]
        [ProducesResponseType(typeof(IReadOnlyList<ProductBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(productBrands);
        }
    }
}
