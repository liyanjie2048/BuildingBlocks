using Liyanjie.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DelimitedArrayModelBinderProvider());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
