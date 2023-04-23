using Microsoft.EntityFrameworkCore;
using TodoApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices.Configure(builder);

Console.WriteLine("Connection string: " + builder.Configuration.GetConnectionString("TodoConnection"));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
