using eCommerce.Application;
using eCommerce.Infrastructure.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddInfrastructureProject(builder.Configuration)
    .AddApplicationProject()
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();