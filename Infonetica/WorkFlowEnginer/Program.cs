using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkFlowEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Controller and Services
builder.Services.AddSingleton<WorkFlowService>();
builder.Services.AddControllers();

var app = builder.Build();

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();
