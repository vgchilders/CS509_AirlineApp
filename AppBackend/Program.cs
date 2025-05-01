using Microsoft.EntityFrameworkCore;
using AppBackend.Data;
using AutoMapper;
using AppBackend.Mapping;
using AppBackend.Interfaces;
using AppBackend.Repositories;
using AppBackend.Services;
using Stripe;
using DotNetEnv;
using AppBackend.DTOs;
using QuestPDF.Infrastructure;


Env.Load();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AppBackend.Mapping.MappingProfile));

var connectionString= Environment.GetEnvironmentVariable("DefaultConnection");

if(string.IsNullOrEmpty(connectionString)){
    throw new InvalidOperationException("The environment variable  ' DefaultConnection' is not set or empty. ");
}


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy =>
                        {
                            policy.WithOrigins("http://localhost:5173", "http://localhost:5218"); //in lecture 5248, 5218 for my local
                        });
});

builder.Services.AddDbContext<AppDbContext>(options=>
options.UseMySql(connectionString,
new MySqlServerVersion(new Version(8,0,41))
).EnableSensitiveDataLogging(true).LogTo(Console.WriteLine,
LogLevel.Information).EnableDetailedErrors(true));

//ADD Strip
var webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");
var stripeApiKey=Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
if (string.IsNullOrEmpty(stripeApiKey))
    throw new InvalidOperationException("STRIPE_SECRET_KEY is not set. Please configure your environment.");
StripeConfiguration.ApiKey=stripeApiKey;





// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options=>{
    options.JsonSerializerOptions.ReferenceHandler=System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.Configure<SendGridSettings>(options=>{
       options.ApiKey=Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
       options.SenderEmail = "flightapp45@gmail.com";
       options.SenderName = "Flight App";

});

builder.Services.AddScoped<IEMailService,SendEmailSmtpSerivce>();

builder.Services.AddScoped<IFlightRepository,FlightRepository>();
builder.Services.AddScoped<ICitiesRepository,CityRepository>();
builder.Services.AddScoped<IPricecalculate,PriceCalculate>();
builder.Services.AddScoped<ISeatRepository,SeatRepository>();
builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddScoped<ITicketBookingRepository,TicketBookingRepository>();
builder.Services.AddScoped<ITicketBookingFlightRepository, TicketBookingFlightRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IPDFService, PDFService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddHostedService<ReservationCleanUp>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
QuestPDF.Settings.License = LicenseType.Community;
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program{}
