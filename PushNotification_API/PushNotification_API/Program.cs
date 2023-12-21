using DataAccess.Models.PushNotification;
using Push_Service;
using Push_Service.Interface;
using Push_Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;
IConfiguration configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//---------------------- add DbContext connection
var connString = Configuration.GetValue("MYSQL_CONSTR", "");
builder.Services.AddDbContext<SubcriptionContext>(options =>
         options.UseMySql(connString, ServerVersion.AutoDetect(connString)), ServiceLifetime.Transient);

DbContextOptionsBuilder<SubcriptionContext> options_sub = new();
options_sub = options_sub.UseMySql(connString, ServerVersion.AutoDetect(connString));
SubcriptionContext _context = new(options_sub.Options);
//---------------------- add PushNotification Service
builder.Services.AddSingleton<IPushinitialize>(new Pushinitialize(configuration));
builder.Services.AddSingleton<IPushNotificationQueue>(new PushNotificationQueue(_context, configuration));
builder.Services.AddScoped<IPushNotificationService, PushNotificationService>();



var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
