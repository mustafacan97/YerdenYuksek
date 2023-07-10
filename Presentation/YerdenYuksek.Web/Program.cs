using eCommerce.Framework.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .RegisterServiceCollections(builder.Configuration, builder.Environment)
    .AddSwaggerGen();

builder.Host.UseDefaultServiceProvider(options =>
{
    //we don't validate the scopes, since at the app start and the initial configuration we need 
    //to resolve some services (registered as "scoped") through the root container
    options.ValidateScopes = false;
    options.ValidateOnBuild = true;
});

var app = builder.Build();

app.RegisterApplicationBuilders();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();