using Microsoft.EntityFrameworkCore;
using ApiTest.Data;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Conexión con PostgreSQL
const string conection = "TestDB";
var conectionString = builder.Configuration.GetConnectionString(conection);

// DbContext
builder.Services.AddDbContext<ApiTestContext>(options => options.UseNpgsql(conectionString));

// FastEndpoints + Swagger
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();   // <-- aquí defines el documento Swagger

var app = builder.Build();

app.UseHttpsRedirection();

// Orden correcto: primero FastEndpoints, luego Swagger
app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();
