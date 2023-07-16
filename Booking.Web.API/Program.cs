using System.Reflection;
using Booking;
using Booking.IdGenerator.InMemory;
using Booking.Repositories;
using Booking.Repositories.EntityFramework;
using Booking.Web.API.Authentication;
using Booking.Web.API.Extensions;
using Booking.Web.API.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Description = "For first request against an API HTTP Basic authentication will ask for username and password. " +
                      "For username input 1, 2 or 3. That's a userId you will be mock logged into. Password field is ignored. " +
                      "UserId 1 has staff role, 2 and 3 are customers. " +
                      "To use application with multiple users at the same time, use multiple incognito browser windows. "
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton<BookingContext>();
builder.Services.AddSingleton<IIdsGenerator, InMemoryIdsGenerator>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IDateTime, SystemDateTime>();
builder.Services.AddScoped<IRoomsService, RoomsService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddUserContext();

builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", (o) => {});

var app = builder.Build();

app.UseStaticFiles();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectStylesheet("/swagger-ui/custom.css");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Init();
app.Run();
