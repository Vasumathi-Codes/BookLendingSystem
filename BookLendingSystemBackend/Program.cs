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

builder.WebHost.UseUrls("http://localhost:5000");

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
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ILendingRecordService, LendingRecordService>();
#endregion

#region Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAngularApp");

#region middlewares
app.UseMiddleware<RoleInjectionMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
#endregion


app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
