using ReportApi.Infrastructure;
using ReportApi.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ReportApi.Infrastructure.Concretes;
using ReportApi.Infrastructure.Interfaces;
using EventBus.Base;
using RabbitMQ.Client;
using ReportApi.IntegrationEvents.Handlers;
using EventBus.Factory;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddApiVersioning(_ =>
    {
        _.DefaultApiVersion = new ApiVersion(1, 0);
        _.AssumeDefaultVersionWhenUnspecified = true;
        _.ReportApiVersions = true;
    });
    builder.Services.AddMvc(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    });
    builder.Services.AddDbContext<AppDbContext>(
       options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddHealthChecks()
           .AddNpgSql(
       connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
       name: "Postgresql ReportApi Healtcheck",

        tags: new string[] { "postgreReportApi" }).AddRabbitMQ(
   rabbitConnectionString: "amqp://guest:guest@localhost:15672",
   name: "RabbitMQ HealthCheck",
   failureStatus: HealthStatus.Unhealthy | HealthStatus.Healthy,
   tags: new string[] { "rabbitmq" });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddTransient(typeof(IReportService), typeof(ReportService));
    builder.Services.AddTransient(typeof(IReportDetailService), typeof(ReportDetailService));

    builder.Services.AddSingleton(sp =>
    {
        EventBusConfig config = new()
        {
            ConnectionRetryCount = 5,
            EventNameSuffix = builder.Configuration.GetValue<string>("RabbitMQConfig:EventNameSuffix"),
            SubscriberClientAppName = builder.Configuration.GetValue<string>("RabbitMQConfig:SubscriberClientAppName"),
            Connection = new ConnectionFactory()
            {
                HostName = builder.Configuration.GetValue<string>("RabbitMQConfig:HostName"),
                Port = builder.Configuration.GetValue<int>("RabbitMQConfig:Port")
            },
            EventBusType = EventBusType.RabbitMQ,

        };

        return EventBusFactory.Create(config, sp);
    });


    builder.Services.AddTransient<ReportCreatingEventHandler>();
}

var app = builder.Build();
app.UseHealthChecks("/report-service-healthcheck", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
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
