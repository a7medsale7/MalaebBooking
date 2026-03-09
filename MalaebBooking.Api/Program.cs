using MalaebBooking.Api;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Register All Dependencies
// ================================
builder.Services.AddApiDependencies(builder.Configuration);

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

app.UseCors("AllowAll");


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();

app.Run();