using AppBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
 public class PaymentContoller:ControllerBase
 {
    private readonly IPaymentService _paymentService;
    private readonly ITicketBookingRepository _ticketBookingRepository;
    public PaymentContoller(IPaymentService paymentService, ITicketBookingRepository ticketBookingRepository)
    {
     _paymentService=paymentService;
     _ticketBookingRepository=ticketBookingRepository;   
    }

    [HttpPost("create-session")]
    public async Task<IActionResult> CreateSession([FromBody] Guid sessionId)
    {
        var booking= await _ticketBookingRepository.GetBookingBySessionIdAsync(sessionId);
        if (booking == null)
           return NotFound();
        var url= await _paymentService.CreateCheckoutSessionAsync(booking.Price,sessionId.ToString(),

        "http://localhost:5173/success",
        "http://localhost:5173/cancel"
        );
        return Ok(new{url});
    }
 }   
}