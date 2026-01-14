using Microsoft.AspNetCore.Mvc;
using System.Net;
using Talabat.APIs.Errors;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketsController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketsController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet("{basketId}")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            return basket is null ? new CustomerBasket(basketId) : Ok(basket);
        }
        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            if (updatedBasket is null)
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest));
            return Ok(updatedBasket);
        }
        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var isDeleted = await _basketRepository.DeleteBasketAsync(basketId);
            if (!isDeleted)
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest));
            return NoContent();
        }
    }
}
