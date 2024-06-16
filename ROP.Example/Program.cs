using ROP.Example.Services;
using ROP.Main;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", async (SubmitOrderRequest request,
        EmailService emailService,
        PaymentService paymentService,
        StockService stockService,
        ValidationService validationService,
        OrderService orderService) =>
    await validationService.ValidateLineItems(request)
        .Bind(_ => validationService.ValidateStock(request))
        .Bind(_ => paymentService.ChargeCreditCard(request.OrderId, request.CustomerId))
        .Combine(_ => orderService.SubmitOrder(request.LineItems))
        .Tap(_ => stockService.UpdateInventory(request.LineItems))
        .Tap(o => emailService.SendOrderConfirmation(o.Item1, o.Item2))
        .Match(
            success => Results.Ok(
                new SubmitOrderResponse(success.Item1, success.Item2)),
            failure => Results.BadRequest(failure.Message)));

app.Run();

public abstract record SubmitOrderRequest(string OrderId, string CustomerId, List<string> LineItems);

public record SubmitOrderResponse(string OrderId, string TransactionId);