using Murunu.ROP;

namespace ROP.Example.Services;

public class OrderService
{
    // public async Task<Result<(string OrderId, string TransactionId)>> SubmitOrder(string transactionId, List<string> lineItems)
    // {
    //     return ("", transactionId);
    // }
    
    
    public async Task<Result<string>> SubmitOrder(List<string> lineItems)
    {
        return "SubmitOrder";
    }
}