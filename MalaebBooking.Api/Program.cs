using Hangfire;
using HangfireBasicAuthenticationFilter;
using MalaebBooking.Api;
using MalaebBooking.Application.Services.HangJobs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Register All Dependencies
// ================================
builder.Services.AddApiDependencies(builder.Configuration);
builder.Services.AddHangfireServer();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// ================================
// Build App
// ================================
var app = builder.Build();

// ================================
// Middleware
// ================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MalaebBooking API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHangfireDashboard("/Jobs", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
    },
    DashboardTitle = "MalaebBooking Jobs Dashboard"
});

app.UseCors("AllowAll");


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();

// ================================
// Hangfire Recurring Jobs ⏳
// ================================
RecurringJob.AddOrUpdate<BookingBackgroundJobs>(
    "auto-complete-bookings",
    job => job.AutoCompleteBookingsAsync(),
    Cron.Hourly()); // تحديث החجوزات المنتهية كل ساعة

RecurringJob.AddOrUpdate<BookingBackgroundJobs>(
    "auto-cancel-expired-pendings",
    job => job.AutoCancelExpiredPendingsAsync(),
    Cron.MinuteInterval(15)); // إلغاء الحجوزات الغير مدفوعة كل 15 دقيقة

RecurringJob.AddOrUpdate<BookingBackgroundJobs>(
    "send-booking-reminders",
    job => job.SendBookingRemindersAsync(),
    Cron.MinuteInterval(30)); // إرسال التذكيرات كل 30 دقيقة

RecurringJob.AddOrUpdate<BookingBackgroundJobs>(
    "cleanup-old-payments-storage",
    job => job.CleanUpOldPaymentsStorageAsync(),
    Cron.Monthly()); // مسح صور الإيصالات القديمة كل شهر


app.Run();
