using BookLendingSystem.Contexts;
using Microsoft.EntityFrameworkCore;
using BookLendingSystem.Interfaces;
using BookLendingSystem.Repositories;
using BookLendingSystem.Models;
using BookLendingSystem.Middlewares;
using BookLendingSystem.Services;
using BookLendingSystem.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

#region logger
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");
#endregion

#region repositories
builder.Services.AddTransient<IRepository<int, Book>, BookRepository>();
builder.Services.AddTransient<IRepository<int, User>, UserRepository>();
builder.Services.AddTransient<IRepository<int, LendingRecord>, LendingRecordRepository>();
#endregion

#region services
builder.Services.AddTransient<IBookService, BookService>();
#endregion

#region Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
#endregion


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
