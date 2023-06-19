using eCommerce.Framework.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .RegisterServiceCollections(builder.Configuration)
    .AddSwaggerGen();

var app = builder.Build();

app.RegisterApplicationBuilders();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();