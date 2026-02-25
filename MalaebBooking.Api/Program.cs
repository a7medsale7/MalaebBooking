using MalaebBooking.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ربط الـ Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// إضافة الـ Controllers و API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// إضافة Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // تفعيل Swagger JSON
    app.UseSwagger();

    // تفعيل Swagger UI على root
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MalaebBooking API V1");
        options.RoutePrefix = string.Empty; // عشان يفتح مباشرة على /
    });
}

app.UseHttpsRedirection();

// ترتيب الـ Middleware مهم: Authentication قبل Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();