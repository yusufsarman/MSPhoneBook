using ReportApi.Infrastructure;
using ReportApi.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ReportApi.Infrastructure.Concretes;
using ReportApi.Infrastructure.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using ReportApi.IoC;
using MSPhoneBook.Shared.Middlewares.Errors;

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
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddSingleton(typeof(AppDbContextFactory));
    builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddTransient(typeof(IReportService), typeof(ReportService));
    builder.Services.AddTransient(typeof(IReportDetailService), typeof(ReportDetailService));
    builder.ConfigureRabbitMQ();
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
app.ConfigureEventBusForSubscription();
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
