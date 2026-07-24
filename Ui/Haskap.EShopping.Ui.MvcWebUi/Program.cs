using Haskap.DddBase.Presentation;
using Haskap.DddBase.Presentation.Middlewares;
using Haskap.EShopping.Domain.Shared.Consts;
using Haskap.EShopping.Ui.MvcWebUi;
using Haskap.EShopping.Ui.MvcWebUi.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Modules.CustomMessage.Presentation.Middlewares;
using Modules.GlobalExceptionHandling.Presentation;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add services to the container.
builder.Services.AddModules(builder.Configuration);
builder.Services.AddDomainServices();
builder.Services.AddInfra();
builder.Services.AddCustomAuthorization();
builder.Services.AddHostedServices();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddLocalization();

builder.Services.AddIpAddressGlobalRateLimiterPolicy();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    options.CacheProfiles.Add(ResponseCacheProfileNameConsts.Duration1HourProfile, new CacheProfile
    {
        Duration = 60 * 60,
        Location = ResponseCacheLocation.Any
    });
});

builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(365); //.FromMinutes(20);
        options.SlidingExpiration = true;
        //options.AccessDeniedPath = "/Home/AccessDenied";
    });

//builder.Services.AddSignalR()
//    .AddJsonProtocol(options =>
//    {
//        options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//    });

builder.Services.AddHsts(options =>
{
    options.Preload = false;
    options.IncludeSubDomains = false;
    options.MaxAge = TimeSpan.FromMinutes(5);
});

//builder.Services.AddExceptionHandler<LoggingExceptionHandler>();
builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

var app = builder.Build();

//await app.MigrateModulesDatabasesAsync();

app.UseExceptionHandler("/Home/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets().ShortCircuit();
app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();
app.UseCurrentUserIdProvider();
app.UseAuthorization();

app.UseSoftDelete();
app.UseIsActive();
app.UseAnonymousAccountProvider();

app.UseCustomMessage(); // en son middleware olarak eklenmeli

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}")
    .WithStaticAssets();

//app.MapHub<BasketHub>("/basket-hub");


app.Run();
