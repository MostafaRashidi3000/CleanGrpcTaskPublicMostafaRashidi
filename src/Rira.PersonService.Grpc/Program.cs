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

// تنظیم پایگاه داده روی SQL Server (با استفاده از ConnectionString یا مقدار پیش‌فرض لوکال)
builder.Services.AddDbContext<AppDbContext>(o =>
	o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
				   ?? "Server=.;Database=RiraPersonDb;Trusted_Connection=True;TrustServerCertificate=True;"));

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

// اعمال خودکار مایگریشن‌ها هنگام بالا آمدن اپلیکیشن در SQL Server
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	await db.Database.MigrateAsync();
}

// مپ کردن سرویس‌های gRPC و اندپوینت تست
app.MapGrpcService<PersonGrpcService>();
app.MapGet("/", () => "Rira PersonService gRPC endpoint. Use a gRPC client.");

app.Run();

public partial class Program { }
