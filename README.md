<div dir="rtl">

## در صورتی که عملیات با خطا مواجه شود چه راهکارهایی را پیشنهاد می‌دهید؟»

در طراحی این سیستم، مدیریت خطاها به صورت معماری‌شده و در لایه‌های مختلف پیاده‌سازی شده است تا پایداری، امنیت و خوانایی کد در بالاترین سطح قرار گیرد:

### ۱. لایه بیزنس و دامنه (`Domain & Application`) — الگوی `Result Pattern`
* به‌جای استفاده پرهزینه و نامناسب از `throw Exception` برای خطاهای منطقی بیزنس (مانند اعتبارسنجی کد ملی یا تکراری بودن داده)، از **Result Pattern** استفاده شده است تا جریان اجرای برنامه قطع نشود و پاسخ‌ها به‌صورت Strongly-Typed و کاملاً قابل پیش‌بینی مدیریت گردند.

### ۲. لایه پروتکل ارتباطی (`gRPC Status Codes`)
* جلوگیری از تبدیل تمام خطاها به خطاهای عمومی، و نگاشت دقیق وضعیت‌های بیزنس به کدهای استاندارد gRPC:
  * **کد `InvalidArgument`:** ویژه داده‌های نامعتبر ورودی و خطاهای اعتبارسنجی لایه FluentValidation.
  * **کد `NotFound`:** در شرایط عدم وجود شناسه یا رکورد مورد نظر در پایگاه داده.
  * **کد `AlreadyExists`:** برای مدیریت تداخل‌ها و نقض یکتایی (مانند کدهای ملی تکراری).

### ۳. لایه زیرساخت و سراسری (`Global Interceptor & Observability`)
* پیاده‌سازی **gRPC Interceptor** جهت رهگیری و مدیریت متمرکز تمامی خطاهای غیرمنتظره (`Unhandled Exceptions`).
* جلوگیری از افشای اطلاعات حساس سیستم و Stack Trace به سمت کلاینت (تضمین امنیت و Data Sanitization).
* فراهم‌سازی بستر متمرکز جهت لاگین و پایش سیستم با ابزارهایی چون Serilog یا OpenTelemetry.

### ۴. راهکارهای پیشنهادی برای فاز عملیاتی (`Enterprise / Production Level`)
* **مدل خطای غنی (`Rich Error Model`):** بهره‌گیری از `Google RPC Status` (`StatusProto`) در سناریوهایی که نیاز است جزئیات چندین خطای اعتبارسنجی همزمان به شکل ساختاریافته به کلاینت منتقل شوند (`google.rpc.BadRequest.FieldViolation`).
* **الگوهای تاب‌آوری (`Resilience Patterns`):** پیاده‌سازی مکانیزم‌های Retry و Circuit Breaker با استفاده از Polly برای پایداری ارتباطات بین‌سرویسی.

---

##  مشخصات تسک
* **شرکت:** راهکارهای اختصاصی هوشمند (رایرا / Rira)
* **موقعیت:** Senior Back End Specialist
* **مدل داده (`Person`):**
  * شناسه یکتا: `Id` (UUID)
  * نام: `FirstName`
  * نام خانوادگی: `LastName`
  * کد ملی: `NationalCode`
  * تاریخ تولد: `BirthDate`

---

##  معماری و بسترهای فنی
* **پلتفرم:** .NET 10 (C# 14)
* **الگوی معماری:** Clean Architecture (شامل لایه‌های Domain، Application، Infrastructure و Grpc Service)
* **پروتکل ارتباطی:** **gRPC** (proto3)
* **پایگاه داده:** Microsoft SQL Server (با EF Core Code-First و اعمال خودکار Migrations)
* **اعتبارسنجی:** FluentValidation
* **تست‌نویسی:** xUnit v2 + FluentAssertions

</div>
