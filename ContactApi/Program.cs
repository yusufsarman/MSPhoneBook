using ContactApi.Infrastructure;
using ContactApi.Infrastructure.Concretes;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Mapping;
using ContactDetailApi.Infrastructure.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddTransient(typeof(IContactService), typeof(ContactService));
builder.Services.AddTransient(typeof(IContactDetailService), typeof(ContactDetailService));
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
