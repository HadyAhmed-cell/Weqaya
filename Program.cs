using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json.Serialization;
using VirtualClinic.Areas.Identity.Data;
using VirtualClinic.Data;
using VirtualClinic.Extensions;
using VirtualClinic.Identity;
using VirtualClinic.Interfaces;
using VirtualClinic.Repositories;
using VirtualClinic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

//builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultUI()
//    .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "205487185629-khae6oa495re762ij3q3ot7rdmork403.apps.googleusercontent.com";
    options.ClientSecret = "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";
});
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("testConnection")));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
//builder.Services.AddScoped(typeof(ILabService), typeof(LabService));
builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));
//builder.Services.AddCors(options => options.AddPolicy("AllowAccess_To_API",
//    policy => policy
//    .AllowAnyOrigin()
//    .AllowAnyHeader()
//    .AllowAnyMethod()
//    ));
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = (actionContext) =>
//    {
//        var errors = actionContext.ModelState.Where(x=>x.Value.Errors.Count>0)
//        .SelectMany(x=>x.Value.Errors)
//        .Select(a=>a.ErrorMessage).ToArray();

//        var errorResponse = new ApiValidationErrorResponse()
//        {
//            Errors = errors
//        };
//        return new BadRequestObjectResult(errorResponse);
//    };
//}
//);
//builder.Services.AddScoped(typeof(DataContext));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

//app.UseCors("AllowAccess_To_API");

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(
        options => options
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .AllowAnyMethod()
    );

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

using ( var scope1 = app.Services.CreateScope() )
{
    var roleManager = scope1.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Doctor", "Patient", "Lab" };
    foreach ( var role in roles )
    {
        if ( !await roleManager.RoleExistsAsync(role) )
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();