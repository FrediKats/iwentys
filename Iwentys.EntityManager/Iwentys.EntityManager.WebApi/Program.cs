using System.Text.Json.Serialization;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.WebApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IwentysEntityManagerDbContext>(o => o
    .UseLazyLoadingProxies()
    .UseInMemoryDatabase("InMemoryIwentysEntityManager.db"));

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));
builder.Services.AddScoped<DbContext, IwentysEntityManagerDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using (IServiceScope serviceScope = app.Services.CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<IwentysEntityManagerDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
