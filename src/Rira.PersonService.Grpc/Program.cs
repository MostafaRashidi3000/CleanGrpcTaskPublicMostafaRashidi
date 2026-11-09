using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Application.Features.Persons;
using Rira.PersonService.Application.Features.Persons.Validators;
using Rira.PersonService.Grpc.Interceptors;
using Rira.PersonService.Grpc.Services;
using Rira.PersonService.Infrastructure.Persistence;
using Rira.PersonService.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// تنظیم gRPC و اینترسپتور مدیریت خطاها
builder.Services.AddGrpc(o => o.Interceptors.Add<ExceptionHandlingInterceptor>());
builder.Services.AddGrpcReflection();

// تنظیم پایگاه داده SQL Server
// تنظیم پایگاه داده SQL Server از طریق appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString));

// ثبت وابستگی‌ها (DI)
builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// ثبت Handler های CQRS
builder.Services.AddScoped<CreatePerson.Handler>();
builder.Services.AddScoped<GetPersonById.Handler>();
builder.Services.AddScoped<ListPersons.Handler>();
builder.Services.AddScoped<UpdatePerson.Handler>();
builder.Services.AddScoped<DeletePerson.Handler>();

// ثبت FluentValidation ها
builder.Services.AddScoped<IValidator<CreatePerson.Command>, CreatePersonValidator>();
builder.Services.AddScoped<IValidator<UpdatePerson.Command>, UpdatePersonValidator>();

var app = builder.Build();

// اعمال خودکار مایگریشن‌ها در استارتاپ
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// مپ کردن سرویس‌ها
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGrpcService<PersonGrpcService>();
app.MapGet("/", () => "Rira PersonService gRPC endpoint is running. Use a gRPC client.");

app.Run();

public partial class Program { }
