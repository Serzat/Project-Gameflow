var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

// Zorgt ervoor dat getallen correct worden weergegeven in OpenAPI
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling =
        System.Text.Json.Serialization.JsonNumberHandling.Strict;
});

builder.Services.AddOpenApi();


// De frontend mag de API aanroepen (ook als die op een andere poort draait)
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Swagger UI beschikbaar maken tijdens ontwikkeling
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Gameflow API v1");
    });
}


app.UseHttpsRedirection();

app.UseCors("Frontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
