using Microsoft.EntityFrameworkCore;
using AppBackend.Data;
using AutoMapper;
using AppBackend.Mapping;



var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AppBackend.Mapping.MappingProfile));


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy =>
                        {
                            policy.WithOrigins("http://localhost:5173", "http://localhost:5218"); //in lecture 5248, 5218 for my local
                        });
});

builder.Services.AddDbContext<AppDbContext>(options=>
options.UseMySql(Environment.GetEnvironmentVariable("DefaultConnection"),
new MySqlServerVersion(new Version(8,0,41))
));



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
