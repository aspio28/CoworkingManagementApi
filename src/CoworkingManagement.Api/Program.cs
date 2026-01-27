using CoworkingManagement;
using CoworkingManagement.Api.Middleware;
using CoworkingManagement.Application;
using CoworkingManagement.Infrastructure;
using CoworkingManagement.Infrastructure.Extensions;
using CoworkingManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation()
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddMemoryCache();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();

    await app.InitialiseDatabaseAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();