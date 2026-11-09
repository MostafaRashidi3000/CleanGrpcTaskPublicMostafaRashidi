# Rira Person Service — Senior Back End Task (1405-08-18)

پروژه پیاده‌سازی سرویس مدیریت اشخاص (Person CRUD) با استفاده از **.NET 10** و **gRPC (Protocol Buffers)** به همراه معماری تمیز (Clean Architecture).

---

## 📋 مشخصات تسک
- **شرکت:** راهکارهای اختصاصی هوشمند (رایرا / Rira)
- **موقعیت:** Senior Back End Specialist
- **مدل داده:** `Person` شامل:
  - `Id` (UUID)
  - `FirstName` (نام)
  - `LastName` (نام خانوادگی)
  - `NationalCode` (کد ملی)
  - `BirthDate` (تاریخ تولد)

---

## 🏗️ معماری و تکنولوژی‌ها
- **Platform:** .NET 10 (C# 14)
- **Architectural Pattern:** Clean Architecture (Domain, Application, Infrastructure, Grpc Service)
- **Communication Protocol:** **gRPC** (proto3) به‌جای REST API سنتی
- **Database:** Microsoft SQL Server (پشتیبانی از EF Core Code-First و Migrations خودکار)
- **Validation:** FluentValidation
- **Testing:** xUnit v2 + FluentAssertions

---

## 🚀 راهنمای اجرا و راه‌اندازی

1. **کلون کردن ریپازیتوری:**
```bash
   git clone https://github.com/YOUR_USERNAME/Rira.PersonService.git
   cd Rira.PersonService
   
