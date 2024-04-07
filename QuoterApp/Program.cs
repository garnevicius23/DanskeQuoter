﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using QuoterApp.DataAccess;
using QuoterApp.Endpoints;
using QuoterApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IQuoter, QuoterService>();
builder.Services.AddSingleton<IMarketOrderSource, HardcodedMarketOrderSource>();
builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();
builder.Services.AddSingleton<IOrderPublisher, OrderPublisher>();
builder.Services.AddHostedService<OrdersSubscriber>();

var app = builder.Build();

app.MapDefinedEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();