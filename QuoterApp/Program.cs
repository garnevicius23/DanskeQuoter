using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoterApp.DataAccess;
using QuoterApp.Endpoints;
using QuoterApp.Services;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddTransient<IQuoter, QuoterService>();
builder.Services.AddSingleton<IMarketOrderSource, HardcodedMarketOrderSource>();
builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();
builder.Services.AddSingleton<IOrderPublisher, OrderPublisher>();

var app = builder.Build();

app.MapDefinedEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var ordersRepository = app.Services.GetService<IOrdersRepository>();
Task.Run(async () => ordersRepository.SubscribeOrders());

app.Run();