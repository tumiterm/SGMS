using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using SGMS.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Auth/SignIn";

    option.Cookie.Name = "SGMSAppCookie";
});


builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;

    config.IsDismissable = true;

    config.Position = NotyfPosition.TopLeft;

    config.HasRippleEffect = true;

});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUnitOfWork<Tournament>, TournamentRepository>();

builder.Services.AddScoped<IUnitOfWork<Sponsor>, SponsorRepository>();

builder.Services.AddScoped<IUnitOfWork<Team>, TeamRepository>();

builder.Services.AddScoped<IUnitOfWork<Coach>, CoachRepository>();

builder.Services.AddScoped<IUnitOfWork<Player>, PlayerRepository>();

builder.Services.AddScoped<IUnitOfWork<Referee>, RefereeRepository>();

builder.Services.AddScoped<IUnitOfWork<Attachment>, AttachmentRepository>();

builder.Services.AddScoped<IUnitOfWork<Match>, MatchRepository>();

builder.Services.AddScoped<IUnitOfWork<District>, DistrictRepository>();

builder.Services.AddScoped<IUnitOfWork<Municipality>, MunicipalityRepository>();

builder.Services.AddScoped<IUnitOfWork<PlayerSelection>, PlayerSelectionRepository>();

builder.Services.AddScoped<IUnitOfWork<User>, UserRepository>();

builder.Services.AddScoped<IUnitOfWork<SysUsers>, HelpContactsRepository>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Auth}/{action=SignIn}/{id?}");

app.Run();
