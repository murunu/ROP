﻿using Murunu.ROP;
using Murunu.ROP.Core;

namespace ROP.Example.Services;

public class PaymentService
{
    public Task<Result<string>> ChargeCreditCard(string customerId, string orderId)
    {
        return Task.FromResult(Result.Success("TransactionId"));
    }
}