using GShopping.EmailAPI.Email;
using GShopping.EmailAPI.MessageConsumer;
using GShopping.EmailAPI.Model.Context;
using GShopping.EmailAPI.Repository;
using GShopping.EmailAPI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 30))));

var builderOptions = new DbContextOptionsBuilder<MySQLContext>();

builderOptions.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 30)));

builder.Services.AddSingleton(new EmailRepository(builderOptions.Options));

builder.Services.AddScoped<IEmailRepository, EmailRepository>();

builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "_101SendEmailNotificationDoNetCoreWebAPI", Version = "v1" });
});

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
