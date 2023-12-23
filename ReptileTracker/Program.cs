using Microsoft.EntityFrameworkCore;
using ReptileTracker.EntityFramework;
using ReptileTracker.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddDbContext<ReptileContext>();

var app = builder.Build();


app.Run();