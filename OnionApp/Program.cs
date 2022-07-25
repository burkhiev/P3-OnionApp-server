using OnionApp.Utilities.ExtensionMethods;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPresentationLayerServices();
builder.Services.AddInfrastructureLayerServices(builder.Configuration);
builder.Services.AddServiceLayerServices();

var app = builder.Build();


app.UseCustomExceptionHandler();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
