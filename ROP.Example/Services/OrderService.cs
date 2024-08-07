﻿using Murunu.ROP;
using Murunu.ROP.Core;

namespace ROP.Example.Services;

public class OrderService
{
    // public async Task<Result<(string OrderId, string TransactionId)>> SubmitOrder(string transactionId, List<string> lineItems)
    // {
    //     return ("", transactionId);
    // }
    
    
    public Task<Result<string>> SubmitOrder(List<string> lineItems)
    {
        return Task.FromResult(Result.Success("SubmitOrder"));
    }
}