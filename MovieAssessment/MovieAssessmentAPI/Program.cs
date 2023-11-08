using Microsoft.AspNetCore.Mvc;
using MovieAssessmentAPI.Models;
using MovieAssessmentAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("MovieDatabase"));
// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddTransient<IMovieService, MovieService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:8888");
