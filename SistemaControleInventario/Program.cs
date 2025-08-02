using SistemaControleInventario.Application.Services;
using SistemaControleInventario.Domain.Repositories;
using SistemaControleInventario.Infrastructure.Repositories;
using SistemaControleInventario.UserInterface.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddScoped<IProdutoRepository>(options => new ProdutoRepository(connectionString));
builder.Services.AddScoped<ProdutoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
