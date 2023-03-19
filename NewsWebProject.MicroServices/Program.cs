using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var corsBuilder = new CorsPolicyBuilder();
corsBuilder.AllowAnyOrigin();
corsBuilder.AllowAnyMethod();
corsBuilder.AllowAnyHeader();
corsBuilder.AllowCredentials();
builder.Services.AddCors(options => { options.AddPolicy("AllowAll", corsBuilder.Build()); });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapControllers();

app.Run();
