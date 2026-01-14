using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Net;
using Talabat.APIs.Errors;
using Talabat.Service.DTOs;
using Talabat.Service.Interfaces;

namespace Talabat.APIs.Controllers
{
    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService,IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
        }
        [Authorize] 
        [HttpPost]
        [ProducesResponseType(typeof(PaymentIntentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PaymentIntentResponse>> CreateOrUpdatePaymentIntent(int OrderId)
        {
            var intent = await _paymentService.CreateOrUpdatePaymentIntent(OrderId);
            if (intent == null)
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, "Problem in creating Payment with this order"));
            return Ok(intent);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _configuration["Stripe:WebhookKey"]);
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                }
            }
            catch (StripeException ex)
            {
                return BadRequest();
            }
            return Ok();
        }
        // to test confirming payment intent
        [Authorize]
        [HttpPost("confirm")]
        public IActionResult ConfirmPayment([FromBody] string paymentIntentId)
        {
            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = paymentMethodService.Create(new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Token = "tok_visa"
                }
            });

            var service = new PaymentIntentService();
            var intent = service.Confirm(paymentIntentId, new PaymentIntentConfirmOptions
            {
                PaymentMethod = paymentMethod.Id
            });

            return Ok(new
            {
                intent.Id,
                intent.Status
            });

        }
    }
}
