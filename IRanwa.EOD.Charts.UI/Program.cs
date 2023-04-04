using IRanwa.EOD.Chart.Business;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Principal;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
var services = builder.Services;

services.AddControllersWithViews();

//Service classes dependency inject
var businessProjectClasses = typeof(ExchangeService).Assembly.GetTypes()
             .Where(service => service.Name.EndsWith("Service")).ToList();
var interfaces = businessProjectClasses.Where(x => x.IsInterface);
var serviceClasses = businessProjectClasses.Where(x => !x.IsInterface);
foreach (var serviceClass in serviceClasses)
{
    var interfaceClass = interfaces.Where(x => x.Name.Equals($"I{serviceClass.Name}")).FirstOrDefault();
    if (interfaceClass != null)
        services.AddTransient(interfaceClass, serviceClass);
}


services.AddSingleton<IEODHttpClient, EODHttpClient>();
services.AddTransient<IUnitOfWorkAsync, UnitOfWorkAsync>();
services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);


//Swagger set up
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EOD Charts", Version = "v1" });
});

//Database connection string set up.
var connectionString = Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<EODDBContext>(options => options.UseSqlServer(connectionString));

services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<EODDBContext>()
    .AddDefaultTokenProviders();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = Configuration["JWT:ValidAudience"],
        ValidIssuer = Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
    };
});

services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

services.Configure<MailSettingsModel>(Configuration.GetSection("MailSettings"));

//Logs
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    "logs/portal_.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();
Log.Information("started");

var app = builder.Build();

try
{
    //Migrate when application starts.
    using (var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<EODDBContext>();
        dataContext.Database.Migrate();
    }
}
catch (Exception ex)
{
    Log.Error("Database migration error : ", ex);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Swagger set up.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EOD Charts API V1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
