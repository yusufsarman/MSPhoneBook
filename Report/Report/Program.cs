using ReportApi.Infrastructure;
using ReportApi.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddTransient(typeof(IContactService), typeof(ContactService));
//builder.Services.AddTransient(typeof(IContactDetailService), typeof(ContactDetailService));
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
