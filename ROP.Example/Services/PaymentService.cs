using Murunu.ROP;

namespace ROP.Example.Services;

public class PaymentService
{
    public async Task<Result<string>> ChargeCreditCard(string customerId, string orderId)
    {
        return "TransactionId";
    }
}