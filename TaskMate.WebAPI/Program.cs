using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;
using TaskMate.Application.Extenstions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.Options;
using TaskMate.Infrastructure.Extensions;
using TaskMate.Infrastructure.Persistence;
using TaskMate.WebAPI.Middlewares;
using TaskMate.WebAPI.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //It tells the API to treat enums as strings
        // for all serialization and deserialization.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Layers cfg
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

//JWT options
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTOptions>();

builder.Services.Configure<JWTOptions>(
    builder.Configuration.GetSection("JWT"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();


//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("V0", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();

    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DBInitializer.Initialize(serviceProvider);
}

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference(cfg =>
{
    cfg.WithTheme(ScalarTheme.Mars)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Http);
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("V0");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
