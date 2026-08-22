using AuthService.Application.Common.Behaviors;
using AuthService.Application.Interface;
using AuthService.Application.IRepositiory;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Repositiory;
using AuthService.Infrastructure.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


// Register FluentValidation Validators
builder.Services.AddValidatorsFromAssembly(typeof(AuthService.Application.DTO.RegisterDto).Assembly);

// Register AutoMapper Profiles
builder.Services.AddAutoMapper(typeof(AuthService.Application.DTO.RegisterDto).Assembly);

// Register MediatR with Validation Pipeline Behavior
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AuthService.Application.DTO.RegisterDto).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
