using Microsoft.EntityFrameworkCore;
using review_microservice.Data;
using review_microservice.Interfaces;
using review_microservice.Repositories;
using review_microservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddHttpClient<IDiscogsService, DiscogsService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var baseUrl = configuration["Discogs:BaseUrl"];
    var userAgent = configuration["Discogs:UserAgent"];

    client.BaseAddress = new Uri(baseUrl!);
    client.DefaultRequestHeaders.Add("User-Agent", userAgent!);
});

var app = builder.Build();

if (args.Length != 0 && args[0].Contains("seeddata"))
{
    Seed.SeedData(app);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();