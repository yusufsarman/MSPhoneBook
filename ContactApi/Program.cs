using ContactApi.Infrastructure;
using ContactApi.Infrastructure.Concretes;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.IoC;
using ContactApi.Mapping;
using ContactDetailApi.Infrastructure.Concretes;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddHealthChecks()
            .AddNpgSql(
        connectionString:builder.Configuration.GetConnectionString("DefaultConnection"),
        name:"Postgresql ContactApi Healtcheck",

         tags: new string[] { "postgreContactApi" }).AddRabbitMQ(
    rabbitConnectionString: "amqp://guest:guest@localhost:15672",
    name: "RabbitMQ HealthCheck",
    failureStatus: HealthStatus.Unhealthy | HealthStatus.Healthy,
    tags: new string[] { "rabbitmq" });
    builder.ConfigureRabbitMQ();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddTransient(typeof(IContactService), typeof(ContactService));
    builder.Services.AddTransient(typeof(IContactDetailService), typeof(ContactDetailService));
}

var app = builder.Build();
{
    app.ConfigureEventBusForSubscription();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHealthChecks("/contact-service-healthcheck", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

