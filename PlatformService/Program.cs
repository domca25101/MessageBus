using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using PlatformService;
using PlatformService.AsyncDataServices;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn")));
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["Commandservice"]}");

//connect to RabbitMQ Broker
var bus = RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("RabbitMQ"), registerServices: s => s.Register<ITypeNameSerializer, TypeNameSerializer>());
builder.Services.AddSingleton(bus);

builder.Services.AddSingleton<IMessageBusPublisher, MessageBusPublisher>();



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

PrepDb.PrepPopulation(app);

app.Run();
