using Microsoft.EntityFrameworkCore;
using ApiTest.Data;

var builder = WebApplication.CreateBuilder(args);

// Conexión con PostgreSQL
const string conection = "TestDB";
var conectionString = builder.Configuration.GetConnectionString(conection);

// DbContext
builder.Services.AddDbContext<ApiTestContext>(options => options.UseNpgsql(conectionString));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();   // Necesario para descubrir endpoints
builder.Services.AddSwaggerGen();             // Registra Swagger

var app = builder.Build();

// Middleware de Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                         // Genera el JSON en /swagger/v1/swagger.json
    app.UseSwaggerUI();                       // UI en /swagger
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
