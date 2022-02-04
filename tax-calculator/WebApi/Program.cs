var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<Infrastructure.Common.IHttpClient, Infrastructure.Common.HttpClient>();
builder.Services.AddTransient<Application.Interfaces.ITaxService, Application.Services.TaxService>();
builder.Services.AddTransient<Application.Interfaces.TaxCalculators.ITaxJarHttpClient, 
    Infrastructure.HttpClients.TaxJar.TaxJarHttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
