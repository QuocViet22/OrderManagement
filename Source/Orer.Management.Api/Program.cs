using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using OrderManagement.Common.Helper;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Setting;
using OrerManagement.Api;
using OrerManagement.Api.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.BindingConfiguration(builder.Configuration);
builder.Services.AddDBContext<OrderManagementDbContext>(ApplicationOptions.ConnectionStrings.OrderManagementConnection);

builder.Services.CoreRegisterBusinessService();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Authentication
builder.Services.AddAuthentication("JWTAuth")
     .AddJwtBearer("JWTAuth", options =>
     {

         var keyBytes = Encoding.UTF8.GetBytes(ApplicationOptions.JwtConfig.Secret);
         var key = new SymmetricSecurityKey(keyBytes);

         options.TokenValidationParameters = new TokenValidationParameters()
         {
             ValidIssuer = ApplicationOptions.JwtConfig.Issuer,
             ValidAudience = ApplicationOptions.JwtConfig.Audience,
             IssuerSigningKey = key
         };
     });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
