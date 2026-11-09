# Rira Person Service — Senior Back End Task (1405-08-18)

پروژه پیاده‌سازی سرویس مدیریت اشخاص (Person CRUD) با استفاده از **.NET 10** و **gRPC (Protocol Buffers)** به همراه معماری تمیز (Clean Architecture).

---


در طراحی این سیستم، مدیریت خطاها به صورت معماری‌شده و در لایه‌های مختلف پیاده‌سازی شده است تا پایداری، امنیت و خوانایی کد در بالاترین سطح قرار گیرد:

1. **لایه بیزنس و دامنه (Domain & Application):** الگوی `Result Pattern`
   * به جای استفاده پرهزینه و نامناسب از `throw Exception` برای خطاهای منطقی بیزنس (مانند اعتبارسنجی کد ملی یا تکراری بودن داده)، از Result Pattern استفاده شده است تا خروجی‌ها به صورت Strongly-Typed و کاملاً قابل پیش‌بینی مدیریت شوند.

2. **لایه پروتکل ارتباطی (`gRPC Status Codes`):**
   * جلوگیری از تبدیل همه خطاها به پاسخ‌های عمومی، و نگاشت دقیق خطاهای دامنه به کدهای استاندارد وضعیت در پروتکل gRPC:
     * `InvalidArgument`: مخصوص داده‌های نامعتبر ورودی و خطاهای اعتبارسنجی `FluentValidation`.
     * `NotFound`: در صورت عدم وجود شناسه یا رکورد مورد نظر در پایگاه داده.
     * `AlreadyExists`: برای مدیریت تداخل‌ها (مثل کدهای ملی تکراری).

3. **لایه زیرساخت و سراسری (`Global Interceptor & Observability`):**
   * پیاده‌سازی gRPC Interceptor جهت رهگیری و مدیریت متمرکز تمام خطاهای هندل‌نشده (`Unhandled Exceptions`) در سطح سیستم.
   * جلوگیری از افشای اطلاعات حساس سیستم و Stack Trace به سمت کلاینت (تضمین امنیت و Sanitization).
   * فراهم‌سازی ساختار مناسب جهت اتصال به ابزارهای لاگینگ و مانیتورینگ متمرکز (مانند Serilog یا OpenTelemetry).

4. **راهکارهای پیشنهادی برای فاز Production (Enterprise Level):**
   * استفاده از `Rich Error Model` (Google RPC Status / StatusProto) در سناریوهایی که نیاز است جزئیات چندین خطای اعتبارسنجی همزمان به صورت ساختاریافته به کلاینت بازگردانده شود (`google.rpc.BadRequest.FieldViolation`).
   * به‌کارگیری مکانیزم‌های `Retry & Circuit Breaker` (با کتابخانه‌هایی نظیر Polly) در لایه ارتباطات بین‌سرویسی (Microservices).

---

## 📋 مشخصات تسک
- **شرکت:** راهکارهای اختصاصی هوشمند (رایرا / Rira)
- **موقعیت:** Senior Back End Specialist
- **مدل داده (`Person`):**
  - `Id` (UUID)
  - `FirstName` (نام)
  - `LastName` (نام خانوادگی)
  - `NationalCode` (کد ملی)
  - `BirthDate` (تاریخ تولد)

---

##  معماری و تکنولوژی‌ها
- **Platform:** .NET 10 (C# 14)
- **Architectural Pattern:** Clean Architecture (Domain, Application, Infrastructure, Grpc Service)
- **Communication Protocol:** **gRPC** (proto3) به‌جای REST API سنتی
- **Database:** Microsoft SQL Server (پشتیبانی از EF Core Code-First و Migrations خودکار)
- **Validation:** FluentValidation
- **Testing:** xUnit v2 + FluentAssertions

---

##  راهنمای اجرا و راه‌اندازی

1. **کلون کردن ریپازیتوری:**
```bash
git clone https://github.com/YOUR_USERNAME/Rira.PersonService.git
cd Rira.PersonService
