using MalaebBooking.Api;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Register All Dependencies
// ================================
builder.Services.AddApiDependencies(builder.Configuration);

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();