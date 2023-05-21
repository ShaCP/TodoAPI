using Microsoft.EntityFrameworkCore;
using TodoApi.Configuration;

// args is the command line arguments passed to the application.
var builder = WebApplication.CreateBuilder(args);

// this is my custom class to add services to the container
ConfigureServices.Configure(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("Dev");

app.UseAuthorization();

app.MapControllers();

app.Run();
