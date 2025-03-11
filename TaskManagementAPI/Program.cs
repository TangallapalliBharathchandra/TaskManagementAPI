
using TaskManagementAPI.Infrastructure;
using TaskManagementAPI.Services;

// Add Swagger


var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddTransient<TaskRepository>();       //  // For database connections and data access
builder.Services.AddTransient<TaskService>();          // For business logic
builder.Services.AddControllers();                    // Enable controllers

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add configuration settings (e.g., appsettings.json)
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Detailed error pages in development

    // Enable Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); // Enable routing for controllers

// Map controllers to handle API requests
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Run the application
app.Run();

