using Sportradar.LiveOddsService.Business.Extensions;
using Sportradar.LiveOddsService.Data.InMemoeyCollection.Extensions;
using Sportradar.LiveOddsService.UI.API.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddMatchServices()
    .AddMatchRepositories();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger()
   .UseSwaggerUI();

app.MapControllers();

app.Run();
