using Microsoft.EntityFrameworkCore;
using TodoList.Context;
using TodoList.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoAppContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("pgsql"));
});

var mySpecificOrigin = "TodoWeb";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: mySpecificOrigin, policy =>
    {
        policy.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(mySpecificOrigin);

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }