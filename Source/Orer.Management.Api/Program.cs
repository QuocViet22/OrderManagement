using OrderManagement.Common.Helper;
using OrderManagement.Common.Setting;
using OrerManagement.Api;
using OrerManagement.Api.Data;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.BindingConfiguration(builder.Configuration);
builder.Services.AddDBContext<OrderManagementDbContext>(ApplicationOptions.ConnectionStrings.OrderManagementConnection);

builder.Services.CoreRegisterBusinessService();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
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

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
