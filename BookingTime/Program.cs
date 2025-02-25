var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS to allow requests from your Angular app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://45.59.163.15:4200")  // Allow your frontend
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());  // Allow credentials (if needed)
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in Development & Staging
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Swagger available at root
    });
}

app.UseHttpsRedirection();

// Apply CORS policy before Authorization
app.UseCors("AllowAngularApp");  // Ensure the CORS policy is applied before Authorization

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
