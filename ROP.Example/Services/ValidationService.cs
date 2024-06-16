using ROP.Main;

namespace ROP.Example.Services;

public class ValidationService
{
    public Result<bool> ValidateStock(SubmitOrderRequest request)
    {
        return true;
    }
    
    public Result<bool> ValidateLineItems(SubmitOrderRequest request)
    {
        return true;
    }
}