using BookLendingSystem.Contexts;
using Microsoft.EntityFrameworkCore;
using BookLendingSystem.Interfaces;
using BookLendingSystem.Repositories;
using BookLendingSystem.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

#region repositories
builder.Services.AddTransient<IRepository<int, Book>, BookRepository>();
builder.Services.AddTransient<IRepository<int, User>, UserRepository>();
builder.Services.AddTransient<IRepository<int, LendingRecord>, LendingRecordRepository>();
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
