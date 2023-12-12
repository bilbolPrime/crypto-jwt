using BilbolStack.ChainJWT.Common;
using BilbolStack.CryptoJWT.API;
using BilbolStack.CryptoJWT.Chain;
using BilbolStack.CryptoJWT.Repository;
using BilbolStack.CryptoJWT.Security;
using BilbolStack.CryptoJWT.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<NFTActionFilter>();
builder.Services.AddSingleton<IRandomAdapter, CryptoRandomAdaptor>();
builder.Services.AddSingleton<ISecurityManager, SecurityManager>();
builder.Services.AddSingleton<INonceRepository, NonceFileRepository>();
builder.Services.AddSingleton<IChainValidator, ChainValidator>();
builder.Services.AddSingleton<INFTContract, NFTContract>();

builder.Services.AddOptions<ChainSettings>().BindConfiguration(ChainSettings.ConfigKey);


var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
