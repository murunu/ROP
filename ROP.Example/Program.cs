using Microsoft.AspNetCore.Mvc;
using Murunu.ROP.ResultExtensions;
using ROP.Example;
using ROP.Example.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<ValidationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/", ([FromBody]SubmitOrderRequest request,
        EmailService emailService,
        PaymentService paymentService,
        StockService stockService,
        ValidationService validationService,
        OrderService orderService) =>
    validationService.ValidateLineItems(request)
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

namespace ROP.Example
{
    public class SubmitOrderRequest
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<string> LineItems { get; set; }
        public SubmitOrderRequest()
        {
        
        }
    
        public SubmitOrderRequest(string orderId, string customerId, List<string> lineItems)
        {
            OrderId = orderId;
            CustomerId = customerId;
            LineItems = lineItems;
        }
    }

    public record SubmitOrderResponse(string OrderId, string TransactionId);
}