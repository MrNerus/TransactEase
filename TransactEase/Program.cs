using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TransactEase.BusinessLayer;
using TransactEase.DataLayer;
using TransactEase.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddTransient<BankHandeler>();
builder.Services.AddTransient<BankDAL>();
builder.Services.AddSingleton<DbConnectionModel>(sp =>
{
    string jsonContent = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "env", "DbConnection.json"));
    return JsonConvert.DeserializeObject<DbConnectionModel>(jsonContent) ?? throw new Exception("DbConnection.json is empty or invalid.");
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();