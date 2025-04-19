namespace AppBackend.Interfaces
{

 public interface IPaymentService
 {
    public Task<string> CreateCheckoutSessionAsync(double amount, string sessionId, string successUrl, string cancelUrl);
 }

}