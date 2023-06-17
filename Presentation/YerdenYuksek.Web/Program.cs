using eCommerce.Framework.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .RegisterServiceCollections()
    .AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.StartAsync();