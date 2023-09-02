using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//cors policy configuration 
builder.Services.AddCors(cors =>
{
    cors.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });
});

// SeriLog Configuration 
Log.Logger = new LoggerConfiguration().WriteTo.File(path: "c:\\hotellistings\\logs\\log-.txt",
                                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lg}{NewLine}{Exception}",
                                                    rollingInterval: RollingInterval.Minute,
                                                    restrictedToMinimumLevel: LogEventLevel.Information).CreateLogger();
try
{
    Log.Information("Application is started ..");
}
catch (Exception ex)
{
    Log.Fatal("Application Failed to start");
}
finally
{
    Log.CloseAndFlush();
}

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
