using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Models.OrderValues;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService
            , IMapper mapper
            , IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(OrderToReturnDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDTO, ShippingAddress>(orderDto.Address);
            var CreatedOrder = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketID
                , orderDto.DeliveryMethodID, MappedAddress);
            if (CreatedOrder == null)
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, "Problem in creating order"));
            var orderToReturn = _mapper.Map<Order, OrderToReturnDTO>(CreatedOrder);
            return Ok(orderToReturn);
        }
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForCurrentUser()
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUserAsync(UserEmail);
            if (Orders is null)
                return NotFound(new ApiError((int)HttpStatusCode.NotFound, "There is no orders for current user"));
            var ordersToReturn = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>((IReadOnlyList<Order>)Orders);
            return Ok(ordersToReturn);
        }
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderToReturnDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Order>> GetOrderByIDForCurrentUser(int id)
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIDForSepcificUserAsync(UserEmail, id);
            if (Order is null)
                return NotFound(new ApiError((int)HttpStatusCode.NotFound, "There is no order with this ID for current user"));
            var orderToReturn = _mapper.Map<Order, OrderToReturnDTO>(Order);
            return Ok(orderToReturn);
        }
        [HttpGet("DeliveryMethods")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync());
        }
    }
}
