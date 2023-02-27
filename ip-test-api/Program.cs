using ip_test_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ip_test_api.Services;
using ip_test_api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options => options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();



builder.Services.AddDbContext<UsersContext>(
            (options) =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("PGUsersDbConnectionString")
                );
            }
        );
var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
