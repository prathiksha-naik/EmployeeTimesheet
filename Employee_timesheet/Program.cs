using Employee_timesheet;
using Employee_timesheet.Models;
using Employee_timesheet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NToastNotify;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CompanyTimesheetContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    {
        option.LoginPath = "/Login/EmployeeLogin";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        option.SlidingExpiration = true;
    }
    );

builder.Services.AddScoped<ITimesheetService, TimesheetsService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});
var app = builder.Build();

//Configure the HTTP request pipeline.
void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error"); 
        app.UseHsts();
    }

   
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=EmployeeLogin}/{id?}");

app.Run();
