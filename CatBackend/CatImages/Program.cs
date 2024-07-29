using CatImages.Models;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using RabbitMQ.Client;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;



var builder = WebApplication.CreateBuilder(args);

var myCorsPolicy = "myCorsPolicy";

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "myCorsPolicy",
    policy =>
     {
    policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod(); 
    });
});








builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// MySettings'i yapılandırma dosyasından okuyup kaydet
//builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));




// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CatContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CatImagesContext")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();



// Configure the HTTP request pipeline.



app.UseStaticFiles();
app.UseCors(myCorsPolicy);



app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI();


app.Use(async (content, next) =>
{
    if (content.Request.Path == "/")
    {
        content.Response.Redirect("swagger/index.html");
    }
    await next();
});
//Prometheus Monitoring connection

//using var server = new Prometheus.KestrelMetricServer(port: 7146);
//server.Start();
//app.UseHttpMetrics();

app.UseHttpsRedirection();


app.MapControllers();


app.Run();
