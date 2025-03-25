

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopCartApi.DataAccessLayer.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options) =>
    {
        options.AddPolicy("DevCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        options.AddPolicy("ProdCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("https://myProductionSite.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });


// Scoped Connection
 builder.Services.AddScoped<IUserRepository, UserRepository>();


//JWT Auth setup
string? appSettingsTokenKey = builder.Configuration.GetSection("AppSettings:TokenKey")?.Value;


SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(appSettingsTokenKey ?? "")
    );

TokenValidationParameters tokenValidationParameters = new TokenValidationParameters() 
{
    IssuerSigningKey = symmetricSecurityKey,
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateIssuerSigningKey = false,
};
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => 
            {
                //Assign the token validation parameters
                options.TokenValidationParameters = tokenValidationParameters;
            });
//End of JWT setup

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

//UseAuthentication() must always come before UseAuthorization()
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
