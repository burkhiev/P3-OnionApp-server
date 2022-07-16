using AppInfrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnionApp.Utilities.ExtensionMethods;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" }));

builder.Services.AddDbContextPool<RepositoryDbContext>(optionsBuilder =>
{
    string connectionString = builder.Configuration.GetConnectionString("Npsql");
    optionsBuilder.UseNpgsql(connectionString, options => options.UseNodaTime());
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDtoValidationServices();
builder.Services.AddRepositories();
builder.Services.AddBusinessServices();
builder.Services.AddExceptionHandlingServices();

var app = builder.Build();


if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingMiddleware();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
