using AuthService.Application.Interface;
using AuthService.Application.IRepositiory;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Repositiory;
using AuthService.Infrastructure.Security;
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
// Program.cs me MediatR ko Assembly scan karke register karne ke liye:
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AuthService.Application.DTO.RegisterDto).Assembly));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
