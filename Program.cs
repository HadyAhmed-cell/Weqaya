using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VirtualClinic.Areas.Identity.Data;
using VirtualClinic.Data;
using VirtualClinic.Identity;
using VirtualClinic.Interfaces;
using VirtualClinic.Repositories;
using VirtualClinic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "205487185629-khae6oa495re762ij3q3ot7rdmork403.apps.googleusercontent.com";
    options.ClientSecret = "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";
});
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("testConnection")));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped(typeof(ILabService), typeof(LabService));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await DataContextSeed.SeedAsync(context, loggerFactory);
}
catch ( Exception ex )
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();