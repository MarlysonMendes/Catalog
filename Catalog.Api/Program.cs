using Catalog.Repositories;
using Catalog.Settings;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

    var settings = builder.Configuration.GetSection(nameof(MongoDbSettings));
    var connection = new MongoDbSettings
    {
        Host = settings["Host"],
        Port = Convert.ToInt16( settings["Port"]),
        User = settings["User"],
        Password = settings["Password"],
    };

builder.Services.AddSingleton<IMongoClient>(ServiceProvider => {
    var client = new MongoClient(connection.ConnectionString);
    return client;
});
builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();

builder.Services.AddControllers(options =>{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddHealthChecks()
    .AddMongoDb(
        connection.ConnectionString, 
        name:"mongodb", 
        timeout: TimeSpan.FromSeconds(3),
        tags: new[] {"ready"});

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

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async(context, report) =>
    {
        var result = JsonSerializer.Serialize(new 
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new {
                name = e.Key,
                status = e.Value.ToString(),
                exception = e.Value.Exception != null ? e.Value.Exception.Message : "none",
                duration = e.Value.Duration.ToString()
            })
        });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

app.Run();
