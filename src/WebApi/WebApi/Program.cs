using ObjectMapper;
using Persistance.Extensions;
using Queries.Extensions;
using W3TL.Core.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Firebase Authentication
builder.Services.AddFirebaseAuthentication("w37l-a74d5");

// Configure CORS to allow any origin
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAnyOrigin", builder =>
        builder.AllowAnyOrigin() // This allows requests from any origin
            .AllowAnyHeader()
            .AllowAnyMethod());
});


// Add controllers and other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register custom services and repositories
builder.Services.RegisterMappers();
builder.Services.RegisterRepositories();
builder.Services.RegisterCommandDispatcher();
builder.Services.RegisterQueryDispatcher();
builder.Services.RegisterQueries();
builder.Services.RegisterHandlers();

builder.Services.AddHttpClient();

var app = builder.Build();

// Apply CORS policy
app.UseCors("AllowAnyOrigin");

// Map controllers
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Use HTTPS redirection
app.UseHttpsRedirection();

app.Run();