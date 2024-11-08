using Stars.API.Helpers;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPooledDbContextFactory<StarsDbContext>(options =>
{
    var connString = "server=localhost;database=stars;uid=root;password=admin";
    ServerVersion server = ServerVersion.AutoDetect(connString);
    options.UseMySql(connString, server);
    options.EnableServiceProviderCaching(false);
}, 2);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
                                .SetIsOriginAllowed((host) => true)
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .WithExposedHeaders(HeaderNames.AccessControlAllowOrigin));
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ScheduleService>();

var app = builder.Build();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
